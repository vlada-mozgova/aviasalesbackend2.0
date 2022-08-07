using Aviasales.DAL.DataAccess;
using Aviasales.DAL.Models;
using Aviasales.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Services
{
    public class TicketService : ITicketService
    {
        private UserContext _context;
        public TicketService(UserContext context)
        {
            _context = context;
        }
        public List<TicketResponse> AllTickets(int userId)
        {
            var user = _context.Users
                .Include(t => t.ticketsList)
                .SingleOrDefault(u => u.Id == userId);
            var tickets = user.ticketsList;

            if (tickets.Count == 0)
                throw new Exception("You don't have any tickets");

            List<TicketResponse> allTickets = new List<TicketResponse>();

            foreach (var item in tickets)
                allTickets.Add(new TicketResponse(item));

            return allTickets;
        }
        public List<TicketResponse> ActiveTickets(int userId)
        {
            var user = _context.Users
                .Include(t => t.ticketsList)
                .SingleOrDefault(u => u.Id == userId);
            var tickets = user.ticketsList;

            if (tickets.Count == 0)
                throw new Exception("You don't have any tickets");

            List<TicketResponse> activeTickets = new List<TicketResponse>();

            foreach (var item in tickets)
                if (item.arrival_date >= DateTime.UtcNow)
                    activeTickets.Add(new TicketResponse(item));

            if (activeTickets.Count == 0)
                throw new Exception("You have no active tickets");

            return activeTickets;
        }
        public void Add(int userId, int ticketId)
        {
            var user = _context.Users
                .Include(t => t.ticketsList)
                .SingleOrDefault(u => u.Id == userId);
            var ticket = _context.Tickets.SingleOrDefault(u => u.Id == ticketId);

            if (user == null)
                throw new Exception("User not found");
            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.count == 0)
                throw new Exception("That ticket is not available");

            if (user.ticketsList.Contains(ticket))
                throw new Exception("You've already have that ticket");
            else
            {
                user.ticketsList.Add(ticket);
                ticket.count -= 1;

                _context.Users.Update(user);
                _context.Tickets.Update(ticket);
                _context.SaveChanges();
            }
        }
        public void Cancel(int userId, int ticketId)
        {
            var user = _context.Users
                .Include(t => t.ticketsList)
                .SingleOrDefault(u => u.Id == userId);
            var ticket = _context.Tickets.SingleOrDefault(u => u.Id == ticketId);

            if (ticket != null)
                if (!user.ticketsList.Contains(ticket))
                    throw new Exception("You don't have that ticket");

            TimeSpan intervalOfDays = ticket.departure_date - DateTime.UtcNow;
            TimeSpan intervalOfHours = ticket.departure_time - DateTime.UtcNow;

            if (user != null && intervalOfDays.TotalDays >= 1 && Math.Abs(intervalOfHours.TotalHours) >= 24)
            {
                user.ticketsList.Remove(ticket);
                ticket.count += 1;

                _context.Users.Update(user);
                _context.Tickets.Update(ticket);
                _context.SaveChanges();
            }
            else
                throw new Exception("You can't cancel that ticket");
        }
        public Ticket Create(Ticket ticket, int price)
        {
            if (ticket == null)
                throw new Exception("Invalid ticket");
            if (price <= 0)
                throw new Exception("Invalid price");


            if (getTicket(ticket))
                throw new Exception("That ticket is already taken");

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return ticket;
        }
        private bool getTicket(Ticket ticket)
        {
            var getTicket = _context.Tickets.All(t => t.origin == ticket.origin &&
                                          t.destination == ticket.destination &&
                                          t.carrier == ticket.carrier &&
                                          t.departure_date == ticket.departure_date &&
                                          t.departure_time == ticket.departure_time &&
                                          t.arrival_date == ticket.arrival_date &&
                                          t.arrival_time == ticket.arrival_time &&
                                          t.stops == ticket.stops &&
                                          t.price == ticket.price);
            if (getTicket) return true;
            return false;
        }
        public void Update(UpdateTicketModel model)
        {
            var ticket = _context.Tickets.Find(model.Id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            if (!string.IsNullOrWhiteSpace(model.Origin))
                ticket.origin = model.Origin;
            if (!string.IsNullOrWhiteSpace(model.Origin_name))
                ticket.origin_name = model.Origin_name;
            if (!string.IsNullOrWhiteSpace(model.Destination))
                ticket.destination = model.Destination;
            if (!string.IsNullOrWhiteSpace(model.Destination_name))
                ticket.destination_name = model.Destination_name;
            if (!string.IsNullOrWhiteSpace(model.Departure_date.ToString()))
                ticket.departure_date = model.Departure_date;
            if (!string.IsNullOrWhiteSpace(model.Departure_time.ToString()))
                ticket.departure_time = model.Departure_time;
            if (!string.IsNullOrWhiteSpace(model.Arrival_date.ToString()))
                ticket.arrival_date = model.Arrival_date;
            if (!string.IsNullOrWhiteSpace(model.Arrival_time.ToString()))
                ticket.arrival_time = model.Arrival_time;
            if (!string.IsNullOrWhiteSpace(model.Carrier))
                ticket.carrier = model.Carrier;
            if (!string.IsNullOrWhiteSpace(model.Stops.ToString()))
                ticket.stops = model.Stops;
            if (!string.IsNullOrWhiteSpace(model.Price.ToString()))
                ticket.price = model.Price;

            if (getTicket(ticket))
                throw new Exception("That ticket is already taken");

            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
            }
        }
        public List<Ticket> ShowTickets()
        {
            return _context.Tickets
               //.Where(t => t.arrival_date > DateTime.UtcNow)
               //.Where(t => t.count > 0)
               .ToList();
        }
    }
}
