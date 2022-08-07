using Aviasales.DAL.Models;
using Aviasales.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Services
{
    public interface IUserService
    {
        ResponseModel Authenticate(LoginModel model, string ipAddress);
        User CreateUser(User user, string password, string code);
        void ApproveRole(ApproveModel model, int userId);
        void UpdateUserName(UpdateUserNameModel user, int userId);
        void UpdatePassword(UpdatePasswordModel userParam, int userId);
        void Logout(int userId, string ipAddress);
        ResponseModel RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
