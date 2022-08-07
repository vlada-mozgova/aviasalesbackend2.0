using Aviasales.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Models
{
    public class TicketResponse
    {
        public string origin { get; set; }
        public string origin_name { get; set; }
        public string destination { get; set; }
        public string destination_name { get; set; }
        public DateTime departure_date { get; set; }
        public DateTime departure_time { get; set; }
        public DateTime arrival_date { get; set; }
        public DateTime arrival_time { get; set; }
        public string carrier { get; set; }
        public int stops { get; set; }
        public int price { get; set; }
        public TicketResponse(Ticket ticket)
        {
            origin = ticket.origin;
            origin_name = ticket.origin_name;
            destination = ticket.destination;
            destination_name = ticket.destination_name;
            departure_date = ticket.departure_date;
            departure_time = ticket.departure_time;
            arrival_date = ticket.arrival_date;
            arrival_time = ticket.arrival_time;
            carrier = ticket.carrier;
            stops = ticket.stops;
            price = ticket.price;
        }
    }
}
