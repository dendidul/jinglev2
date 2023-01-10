using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NReco.VideoConverter;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmTalentCommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}