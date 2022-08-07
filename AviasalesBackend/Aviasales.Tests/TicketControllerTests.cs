using AutoMapper;
using Aviasales.Web.Controllers;
using Aviasales.Web.Helpers;
using Aviasales.Web.Models;
using Aviasales.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aviasales.Tests
{
    public class TicketControllerTests
    {
        private static IMapper _mapper;
        public TicketControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        [Fact]
        public void AddNewTicket_ThrowsException()
        {
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.AddNewTicket(null);

            Xunit.Assert.IsType<BadRequestObjectResult>(rezult);
        }
        [Fact]
        public void AddNewTicket_DoesNotThrowException()
        {
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.AddNewTicket(ticketSample());

            Xunit.Assert.IsType<OkObjectResult>(rezult);
        }
        private TicketModel ticketSample()
        {
            return new TicketModel
            {
                origin = "VVO1",
                origin_name = "Владивосток",
                destination = "TLV",
                destination_name = "Тель-Авив",
                departure_date = DateTime.Parse("13.11.21"),
                departure_time = DateTime.Parse("17:20"),
                arrival_date = DateTime.Parse("13.11.21"),
                arrival_time = DateTime.Parse("23:50"),
                carrier = "S7",
                stops = 1,
                price = 13200
            };
        }
        [Fact]
        public void Delete_DoesNotThrowException()
        {
            int testId = 1;
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.Delete(testId);

            Xunit.Assert.IsType<OkResult>(rezult);
        }
        [Fact]
        public void BookTicket_DoesNotThrowExeption()
        {
            int ticketId = 1;
            int userId = 1;
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.BookTicket(userId, ticketId);

            Xunit.Assert.IsType<OkResult>(rezult);
        }
        [Fact]
        public void CancelTicket_DoesNotThrowException()
        {
            int ticketId = 1;
            int userId = 1;
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.CancelTicket(userId, ticketId);

            Xunit.Assert.IsType<OkResult>(rezult);
        }
        [Fact]
        public void ShowActiveTickets_DoesNotThrowException()
        {
            int userId = 1;
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.ShowActiveTickets_OfUser(userId);

            Xunit.Assert.IsType<OkObjectResult>(rezult);
        }
        [Fact]
        public void ShowAllTickets_DoesNotThrowException()
        {
            int userId = 1;
            var mockService = new Mock<ITicketService>();
            var controller = new TicketController(mockService.Object, _mapper);

            var rezult = controller.ShowAllTickets_OfUser(userId);

            Xunit.Assert.IsType<OkObjectResult>(rezult);
        }
    }
}
