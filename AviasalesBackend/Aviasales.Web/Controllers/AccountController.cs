using AutoMapper;
using Aviasales.DAL.Models;
using Aviasales.Web.Helpers;
using Aviasales.Web.Models;
using Aviasales.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Controllers
{
    [ApiController]
    [Route("account-controller")]
    public class AccountController : BaseController
    {
        private IUserService _userService;
        private IMapper _mapper;
        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                _userService.CreateUser(user, model.Password, model.Code);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("approve")]
        public IActionResult Approve([FromBody] ApproveModel model)
        {
            try
            {
                _userService.ApproveRole(model, GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("update-username")]
        public IActionResult UpdateUser([FromBody] UpdateUserNameModel model)
        {
            try
            {
                _userService.UpdateUserName(model, GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update-password")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            try
            {
                _userService.UpdatePassword(model, GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("authenticate")]
        public IActionResult AuthenticateUser([FromBody] LoginModel model)
        {
            var response = _userService.Authenticate(model, ipAddress()); //"192.168.1.11");//

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        [Authorize]
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            _userService.Logout(GetUserId(), ipAddress());
            return Ok();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        [Authorize(Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
