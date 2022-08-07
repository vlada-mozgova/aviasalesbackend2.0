using Aviasales.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviasales.Web.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public User Userbase => (User)HttpContext.Items["Account"];
        protected int GetUserId()
        {
            return Userbase.Id;
        }
    }
}
