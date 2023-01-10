using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jingl.Web.Controllers
{
    public class NoAccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult NotAuthorizedLogin()
        {
            return View();
        }
    }
}