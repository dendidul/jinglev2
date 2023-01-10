using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Jingl.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        private readonly FilesController FilesController;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration config;

        public WishlistController(IConfiguration config, ICookie cookie, SignInManager<ApplicationUser> signInManager)
        {
            this.IMasterManager = new MasterManager(config);
            this.IUserManagementManager = new UserManagementManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
            this.FilesController = new FilesController(config, cookie);
            _signInManager = signInManager;
            this.config = config;
        }
        public IActionResult Wishlist()
        {
            int UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
            try
            {
                List<TalentCategoryViewModel> listWishlist = new List<TalentCategoryViewModel>();
                listWishlist = ITransactionManager.GetWishListByUserId(UserId).ToList();
                foreach (var z in listWishlist)
                {
                    z.CategoryNm = HelperController.CategoryTalentData(z.TalentId);
                }
                return View(listWishlist);
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(UserId), "WishlistList", ex.Message);
                return RedirectToAction("NotFound", "NoAccess");
            }

        }
    }
}