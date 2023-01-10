using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.WebApi.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActivityController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}