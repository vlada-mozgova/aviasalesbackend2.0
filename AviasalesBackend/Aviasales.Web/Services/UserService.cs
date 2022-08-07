using Aviasales.DAL.DataAccess;
using Aviasales.DAL.Models;
using Aviasales.Web.Helpers;
using Aviasales.Web.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Aviasales.Web.Services
{
    public class UserService : IUserService
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        public UserService(UserContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public ResponseModel Authenticate(LoginModel model, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == model.UserName);

            if (user == null) return null;

            if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            _context.SaveChanges();

            return new ResponseModel(user, jwtToken, refreshToken.Token);
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

            using (var temp = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computerHash = temp.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computerHash.Length; i++)
                    if (computerHash[i] != storedHash[i])
                        return false;
            }

            return true;
        }
        public void Logout(int userId, string ipAddress)
        {
            var user = _context.Users.Find(userId);
            int num = user.RefreshTokens.Count - 1;
            var token = user.RefreshTokens[num].ToString();
            RevokeToken(token, ipAddress);
        }
        public void ApproveRole(ApproveModel model, int userId)
        {
            var user = _context.Users.Find(userId);

            if (model.Code != "smile")
                throw new Exception("You can't approve your role. Code is not right");

            user.Role = Role.Admin;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public User CreateUser(User user, string password, string code)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new Exception($"Username {user.UserName} is already taken");
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (!string.IsNullOrWhiteSpace(code) && code == "smile")
                user.Role = Role.Admin;

            user.Role = Role.User;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
        public void UpdateUserName(UpdateUserNameModel userParam, int userId)
        {
            var user = _context.Users.Find(userId);

            if (!string.IsNullOrWhiteSpace(userParam.NewUserName) && userParam.NewUserName != user.UserName)
            {
                if (_context.Users.Any(x => x.UserName == userParam.NewUserName))
                    throw new Exception("Username " + userParam.NewUserName + " is already taken");

                user.UserName = userParam.NewUserName;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void UpdatePassword(UpdatePasswordModel userParam, int userId)
        {
            var user = _context.Users.Find(userId);

            if (!string.IsNullOrWhiteSpace(userParam.OldPassword))
            {
                if (!VerifyPasswordHash(userParam.OldPassword, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("Invalid current password");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userParam.NewPassword, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty");

            using (var temp = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = temp.Key;
                passwordHash = temp.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var temp = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                temp.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public ResponseModel RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null) return null;
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive) return null;

            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            _context.Update(user);
            _context.SaveChanges();

            var jwtToken = generateJwtToken(user);

            return new ResponseModel(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null) return false;
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.Expires = DateTime.UtcNow;

            _context.Update(user);
            _context.SaveChanges();

            return true;
        }
    }
}
