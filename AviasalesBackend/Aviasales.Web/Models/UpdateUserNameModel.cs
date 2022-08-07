using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class UpdateUserNameModel
    {
        [Required(ErrorMessage = "Username not specified")]
        public string NewUserName { get; set; }
    }
}
