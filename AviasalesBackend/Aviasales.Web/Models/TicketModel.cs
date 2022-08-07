using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class TicketModel
    {
        [Required(ErrorMessage = "Origin not specified")]
        public string origin { get; set; }

        [Required(ErrorMessage = "Name of origin not specified")]
        public string origin_name { get; set; }

        [Required(ErrorMessage = "Destination not specified")]
        public string destination { get; set; }

        [Required(ErrorMessage = "Name of destination not specified")]
        public string destination_name { get; set; }

        [Required(ErrorMessage = "Departure date not specified")]
        [DataType(DataType.DateTime)]
        public DateTime departure_date { get; set; }

        [Required(ErrorMessage = "Departure time not specified")]
        [DataType(DataType.DateTime)]
        public DateTime departure_time { get; set; }

        [Required(ErrorMessage = "Arrival date not specified")]
        [DataType(DataType.DateTime)]
        public DateTime arrival_date { get; set; }

        [Required(ErrorMessage = "Arrival time not specified")]
        [DataType(DataType.DateTime)]
        public DateTime arrival_time { get; set; }

        [Required(ErrorMessage = "Name of carrier not specified")]
        public string carrier { get; set; }

        [Required(ErrorMessage = "Number of stops not specified")]
        public int stops { get; set; }

        [Required(ErrorMessage = "Price not specified")]
        public int price { get; set; }
        [Required(ErrorMessage = "Count of available tickets not specified")]
        public int count { get; set; }
    }
}
