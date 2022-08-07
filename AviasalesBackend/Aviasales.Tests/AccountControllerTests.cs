using AutoMapper;
using Aviasales.DAL.Models;
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
    public class AccountControllerTests //: ControllerBase
    {
        private static IMapper _mapper;
        public AccountControllerTests()
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
        public void Register_ThrowsException()
        {
            //Arrange
            var mockService = new Mock<IUserService>();
            var controller = new AccountController(mockService.Object, _mapper);

            //Act
            var rezult = controller.Register(null);

            //Assert
            Xunit.Assert.IsType<BadRequestObjectResult>(rezult);
        }
        [Fact]
        public void Register_DoesNotThrowException()
        {
            var mockService = new Mock<IUserService>();
            var controller = new AccountController(mockService.Object, _mapper);
            var newModel = new RegisterModel() { UserName = "Tom", Password = "1234567890" };

            var rezult = controller.Register(newModel);

            Xunit.Assert.IsType<OkObjectResult>(rezult);
        }
        [Fact] //if change method call ipAddress() to a string then test passes
        public void Authenticate_ThrowsException()
        {
            var mockService = new Mock<IUserService>();
            var controller = new AccountController(mockService.Object, _mapper);

            var rez = controller.AuthenticateUser(null);

            Xunit.Assert.IsType<BadRequestObjectResult>(rez);
        }
        [Fact]
        public void Authenticate_DoesNotThrowException()
        {
            var mockService = new Mock<IUserService>();
            var controller = new AccountController(mockService.Object, _mapper);
            var newModel = new LoginModel() { UserName = "mrosenauer0", Password = "f8Ev2mmV9pB" };

            var rez = controller.AuthenticateUser(newModel);

            Xunit.Assert.IsType<OkObjectResult>(rez);
        }
        [Fact]
        public void GetAllUsers()
        {
            var mockService = new Mock<IUserService>();
            var mockMapper = new Mock<IMapper>();
            mockService
                .Setup(x => x.GetAll())
                .Returns(GetTestUsers());
            var controller = new AccountController(mockService.Object, _mapper);

            var rez = controller.GetAll();

            var rezult = Xunit.Assert.IsType<OkObjectResult>(rez);
            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<User>>(rezult.Value);
            Xunit.Assert.Equal(GetTestUsers().Count, model.Count());
        }
        private List<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User{Id=1, UserName="Tom", Email="email1@gmail.com", Password="1234567890", Role = Role.Admin},
                new User{Id=2, UserName="Katty", Email="email2@gmail.com", Password="1234567890", Role = Role.User},
                new User{Id=3, UserName="Ben", Email="email3@gmail.com", Password="1234567890", Role = Role.User},
                new User{Id=4, UserName="Lisa", Email="email4@gmail.com", Password="1234567890", Role = Role.User},
                new User{Id=5, UserName="Alice", Email="email5@gmail.com", Password="1234567890", Role = Role.Admin},
                new User{Id=6, UserName="Piter", Email="email6@gmail.com", Password="1234567890", Role = Role.Admin},
                new User{Id=7, UserName="Sam", Email="email7@gmail.com", Password="1234567890", Role = Role.User},
                new User{Id=8, UserName="Met", Email="email8@gmail.com", Password="1234567890", Role = Role.User}
            };
            return users;
        }
        [Fact]
        public void GetUserByIdNotFound()
        {
            int testId = 1;
            var mockService = new Mock<IUserService>();
            mockService
                .Setup(x => x.GetById(testId))
                .Returns(null as User);
            var controller = new AccountController(mockService.Object, _mapper);

            var rezult = controller.GetUserById(testId);

            Xunit.Assert.IsType<NotFoundResult>(rezult);
        }
        [Fact]
        public void GetUserById()
        {
            int testId = 1;
            var mockService = new Mock<IUserService>();
            mockService
                .Setup(x => x.GetById(testId))
                .Returns(GetTestUsers().FirstOrDefault(x => x.Id == testId));
            var controller = new AccountController(mockService.Object, _mapper);

            var rezult = controller.GetUserById(testId);

            var rez = Xunit.Assert.IsType<OkObjectResult>(rezult);
            var model = Xunit.Assert.IsType<User>(rez.Value);
            Xunit.Assert.Equal("Tom", model.UserName);
            Xunit.Assert.Equal("email1@gmail.com", model.Email);
            Xunit.Assert.Equal(testId, model.Id);

        }
    }
}
