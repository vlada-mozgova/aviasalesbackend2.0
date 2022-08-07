using Aviasales.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public ResponseModel(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
