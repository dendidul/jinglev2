using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class ExploreController : Controller
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ExploreController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Explore(int? CategoryId)
        {
            ExploreViewModel model = new ExploreViewModel();
            var Explore = ITransactionManager.GetAllVideoByCategory(CategoryId);
            model.ListVideo = Explore;
            model.ListCategoryModel = IMasterManager.GetCategoryByType("Book");
            return View(model);
        }

        [HttpPost]
        public IActionResult ExploreContent(string CategoryId)
        {
            int? temp= null;
            if(CategoryId  !=null)
            {
                temp = Convert.ToInt32(CategoryId);
            }
            ExploreViewModel model = new ExploreViewModel();
            var Explore = ITransactionManager.GetAllVideoByCategory(temp);
            model.ListVideo = Explore;
            return PartialView("~/Views/Explore/ExploreContent.cshtml", model);
        }

        public IActionResult ExploreDetail()
        {
            return View();
        }
    }
}