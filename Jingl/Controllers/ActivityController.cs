using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ActivityController : Controller
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;

        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public ActivityController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.IUserManagementManager = new UserManagementManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }

        [HttpGet]
        [Route("~/api/Activity/Activity")]
        public IActionResult Activity(int userid)
        {
            NotificationViewModel model = new NotificationViewModel();
            var talentMdl = new TalentModel();
            try
            {
                var userId = userid;
                talentMdl = IMasterManager.GetTalentProfiles(userId);

                model.ListActiveBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book=>book.Status > 1 && book.Status < 5).ToList();
                model.ListFinishBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book => book.Status < 0 || book.Status >= 5).ToList();
                model.CountActiveBook = model.ListActiveBook.Count();
                model.CountFinishBook = model.ListFinishBook.Count();

                model.RoleId = Convert.ToInt32(talentMdl.RoleId);
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Activity", ex.Message);

                throw ex;
            }

            return new JsonResult(model)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        public IActionResult Workspace()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                var userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                data = ITransactionManager.GetBookingByUserId(Convert.ToInt32(userId));
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Workspace", ex.Message);

                throw ex;
            }

            return View(data);
        }

        [HttpGet]
        [Route("~/api/Activity/Notification")]
        public IActionResult Notification(string UserId)
        {
            var model = new NotificationViewModel();
            var talentMdl = new TalentModel();
            try
            {
                var userId = Convert.ToInt32(UserId);
                talentMdl = IMasterManager.GetTalentProfiles(userId);

                //model.ListActiveBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book=>book.Status > 1 && book.Status < 5).ToList();
                //model.ListFinishBook = ITransactionManager.GetBookingByTalentId(talentMdl.Id).Where(book => book.Status < 0 || book.Status >= 5).ToList();
                //model.CountActiveBook = model.ListActiveBook.Count();
                //model.CountFinishBook = model.ListFinishBook.Count();




                model.UserNotificationList = ITransactionManager.GetNotificationForUser(userId);
                model.TalentNotificationList = ITransactionManager.GetNotificationForTalent(userId);

                foreach (var i in model.TalentNotificationList)
                {
                    NotificationModel Notif = new NotificationModel();
                    Notif = i;
                    model.UserNotificationList.Add(Notif);

                }

                model.CountUserNotification = model.UserNotificationList.Where(x => x.IsReaded == 0).Count();
                model.CountTalentNotification = model.TalentNotificationList.Where(x => x.IsReaded == 0).Count();

                int? xx = IUserManagementManager.GetAllUser().Where(m => m.Id == userId).Select(m => m.RoleId).FirstOrDefault();

                model.RoleId = Convert.ToInt32(xx);
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(UserId), "Activity", ex.Message);

                throw ex;
            }

            return new JsonResult(model)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

       


    }
}