using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.DAL.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        public string origin { get; set; }
        [Required]
        public string origin_name { get; set; }
        [Required]
        public string destination { get; set; }
        [Required]
        public string destination_name { get; set; }
        [Required]
        public DateTime departure_date { get; set; }
        [Required]
        public DateTime departure_time { get; set; }
        [Required]
        public DateTime arrival_date { get; set; }
        [Required]
        public DateTime arrival_time { get; set; }
        [Required]
        public string carrier { get; set; }
        [Required]
        public int stops { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int count { get; set; }
    }
}
