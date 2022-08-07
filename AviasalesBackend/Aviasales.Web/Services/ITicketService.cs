using Aviasales.DAL.Models;
using Aviasales.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Services
{
    public interface ITicketService
    {
        Ticket Create(Ticket ticket, int price);
        public List<TicketResponse> AllTickets(int userId);
        public List<TicketResponse> ActiveTickets(int userId);
        void Delete(int id);
        void Update(UpdateTicketModel model);
        void Add(int userId, int ticketId);
        void Cancel(int userId, int ticketId);
        public List<Ticket> ShowTickets();
    }
}
