using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class ApproveModel
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
    }
}
