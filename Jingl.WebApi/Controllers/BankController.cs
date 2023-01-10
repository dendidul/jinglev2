using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Model.Admin.Master;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.WebApi.Controllers
{
    public class BankController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;

        private readonly HelperController HelperController;


        public BankController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }


        [HttpGet]
        [Route("~/api/Account/GetBank")]
        public IActionResult GetBank()
        {
            try
            {
                var model = IMasterManager.GetAllBank().ToList();
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
            }
            catch (Exception ex)
            {
                List<BankModel> BModel = new List<BankModel>();
                HelperController.InsertLog(0, "Support", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = BModel });
                throw ex;
            }
            //var model = IMasterManager.GetAllBank().ToList();

            //return new JsonResult(model)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }
    }
}