using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.WebApi.Authentication;
using Jingl.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupportController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;

        private readonly HelperController HelperController;


        public SupportController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        [Route("~/api/Support/CreateSupport")]
        public IActionResult CreateSupport([FromBody]SupportModel model)
        {
            try
            {

                model.CreatedBy = model.CreatedBy;
                var data = ITransactionManager.CreateSupport(model);
                HelperController.EmailSupport(model.Subject, model.Details, model.CreatedBy, model.EmailAddress).Wait();


                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });

                //return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "Support", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
                //return Json("Error");
                throw ex;
            }
        }
    }
}