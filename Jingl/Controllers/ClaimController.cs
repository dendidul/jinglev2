using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ClaimController : Controller
    {


        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;

        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ClaimController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }

        [HttpGet]
        [Route("~/api/Claim/Detail")]
        public IActionResult Detail(string ClaimId)
        {
            var ClaimModel = ITransactionManager.GetClaim(Convert.ToInt32(ClaimId));
            var StatusName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "UClaimStat" && x.ParamCode == ClaimModel.Status.Value.ToString()).FirstOrDefault();
            ClaimModel.StatusNm = StatusName != null ? StatusName.ParamValue : "";

            return new JsonResult(ClaimModel)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}