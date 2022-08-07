using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username not specified")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password confirmation not specified")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Invalid username or password")]
        public string ConfirmPassword { get; set; }
    }
}
