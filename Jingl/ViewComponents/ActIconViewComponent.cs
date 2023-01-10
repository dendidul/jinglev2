using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jingl.Web.ViewComponents
{
    public class ActIconViewComponent : ViewComponent
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;

        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ActIconViewComponent(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }

        public IViewComponentResult Invoke()
        {
            List<BookModel> listBook = new List<BookModel>();
            var talentModel = new TalentModel();
            var countNotif = 0;
            try
            {
                if (!string.IsNullOrEmpty(HelperController.GetCookie("UserId")))
                {
                    var userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                    talentModel = IMasterManager.GetTalentProfiles(userId);
                    if (talentModel != null)
                    {
                        listBook = ITransactionManager.GetBookingByTalentId(talentModel.Id).ToList();
                        countNotif = listBook.Where(book => book.Status < 5 && book.Status > 1).Count();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Activity", ex.Message);

                throw ex;
            }
            return View(countNotif);
        }
    }
}
