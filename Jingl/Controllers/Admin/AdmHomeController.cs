using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jingl.General.Model.Admin.UserManagement;
using Microsoft.Extensions.Configuration;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Controllers;
using Jingl.Models;
using Microsoft.AspNetCore.Http;
using CookieManager;

namespace Jingl.Web.Controllers
{
    public class AdmHomeController : AdmMenuController
    {
        private readonly IUserManagementManager _IUserManagementManager;
        //private readonly IConfiguration _config;

        public AdmHomeController(IConfiguration config, ICookie cookie) :base(config, cookie)
        {
            this._IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult DashboardDataStudio()
        {
            return View();
        }
    }
}
