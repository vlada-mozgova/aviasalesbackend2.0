using AutoMapper;
using Aviasales.DAL.Models;
using Aviasales.Web.Models;
using Aviasales.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aviasales.Web.Helpers;

namespace Aviasales.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ticket-controller")]
    public class TicketController : BaseController
    {
        private ITicketService _ticketService;
        private IMapper _mapper;
        public TicketController(ITicketService ticketService, IMapper mapper)
        {
            _ticketService = ticketService;
            _mapper = mapper;
        }
        [Authorize(Role.Admin)]
        [HttpPost("add-new-ticket")]
        public IActionResult AddNewTicket([FromBody] TicketModel model)
        {
            var ticket = _mapper.Map<Ticket>(model);

            try
            {
                _ticketService.Create(ticket, model.price);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete("delete-ticket/{id}")]
        public IActionResult Delete(int id)
        {
            _ticketService.Delete(id);
            return Ok();
        }
        [Authorize(Role.Admin)]
        [HttpPut("update")]
        public IActionResult UpdateTicket([FromBody] UpdateTicketModel model)
        {
            try
            {
                _ticketService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("book-ticket/{ticketId}")]
        public IActionResult BookTicket(int ticketId)
        {
            try
            {
                _ticketService.Add(GetUserId(), ticketId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("cancel-ticket/{ticketId}")]
        public IActionResult CancelTicket(int ticketId)
        {
            _ticketService.Cancel(GetUserId(), ticketId);
            return Ok();
        }
        [HttpGet("active-tickets-of-user")]
        public IActionResult ShowActiveTickets_OfUser()
        {
            try
            {
                var listOfTickets = _ticketService.ActiveTickets(GetUserId());
                return Ok(listOfTickets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("all-tickets-of-user")]
        public IActionResult ShowAllTickets_OfUser()
        {
            try
            {
                var listOfTickets = _ticketService.AllTickets(GetUserId());
                return Ok(listOfTickets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Role.Admin)]
        [HttpGet("all-tickets")]
        public IActionResult ShowAllTickets()
        {
            var allTickets = _ticketService.ShowTickets();
            return Ok(allTickets);
        }
    }
}
