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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Controllers
{
    public class DiscoverController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;


        public DiscoverController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Discover()
        {
            //IList<TalentModel> data = new List<TalentModel>();
            //try
            //{
            //    data = IMasterManager.GetAllTalent();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //return View(data);

            IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();
            try
            {
                var UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                data = ITransactionManager.GetWishListByUserId(UserId);
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Discover", ex.Message);

                throw;
            }
            return View(data);

        }
       
    }
}