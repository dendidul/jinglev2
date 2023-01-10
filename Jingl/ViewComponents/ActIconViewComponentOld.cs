using CookieManager;
using Jingl.General.Model.Admin.Master;
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
    public class ActIconViewComponentOld : ViewComponent
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;

        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ActIconViewComponentOld(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }

        public IViewComponentResult Invoke()
        {
            var model = new NotificationViewModel();
            var talentModel = new TalentModel();
            var countNotif = 0;
            try
            {
                var userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                talentModel = IMasterManager.GetTalentProfiles(userId);

                model.UserNotificationList = ITransactionManager.GetNotificationForUser(userId);
                model.TalentNotificationList = ITransactionManager.GetNotificationForTalent(userId);
                model.CountUserNotification = model.UserNotificationList.Where(x => x.IsReaded == 0).Count();
                model.CountTalentNotification = model.TalentNotificationList.Where(x => x.IsReaded == 0).Count();

                countNotif = model.CountTalentNotification + model.CountUserNotification;
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
