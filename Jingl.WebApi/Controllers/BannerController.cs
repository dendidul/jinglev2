using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
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
    public class BannerController : Controller
    {

        private readonly FilesController FilesController;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public BannerController(IConfiguration config)
        {
            this.FilesController = new FilesController(config);
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        [HttpPost]
        [Route("~/api/Banner/GetBanner")]
        public IActionResult GetBanner([FromBody]BannerModel model)
        {
            try
            {
                var getCurrentData = IMasterManager.GetAllBanner().Where(x => x.BannerCategory == model.BannerCategory && x.IsVisible == 1).OrderBy(x => x.Sequence).ToList();
                //return Json(new { Result = getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "GetBanner", ex.Message);
                List<BannerModel> Blist = new List<BannerModel>();
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = Blist });
                //return Json(new { QuestionId = 0, Status = "Error" });

            }
        }
    }
}