using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
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
    public class ClaimController : Controller
    {

        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;

      
        private readonly HelperController HelperController;

        public ClaimController(IConfiguration config)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
         
            this.HelperController = new HelperController(config);
        }

        [HttpGet]
        [Route("~/api/Claim/Detail")]
        public IActionResult Detail(string ClaimId)
        {
            var ClaimModel = new ClaimModel();
            try
            {
                ClaimModel = ITransactionManager.GetClaim(Convert.ToInt32(ClaimId));
                var StatusName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "UClaimStat" && x.ParamCode == ClaimModel.Status.Value.ToString()).FirstOrDefault();
                ClaimModel.StatusNm = StatusName != null ? StatusName.ParamValue : "";
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = ClaimModel });
            }
            catch (Exception ex )
            {
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = ClaimModel });
                throw ex;
            }
            
            //return new JsonResult(ClaimModel)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }
    }
}