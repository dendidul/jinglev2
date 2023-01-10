using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CookieManager;
using Jingl.General.Model.User.Notification;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public NotificationController(IConfiguration config, ICookie cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this._cookie = cookie;
            this.HelperController = new HelperController(config, cookie);
        }
        public IActionResult Index()
        {
            return View();

        }

        //public IActionResult SendNotification()
        //{
        //    //string vapidPublicKey = config.GetSection("VapidKeys")["PublicKey"];
        //    //string vapidPrivateKey = config.GetSection("VapidKeys")["PrivateKey"];

        //    //var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);
        //    //var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

        //    //var webPushClient = new WebPushClient();
        //    //webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
        //}

        public IActionResult ActiveNotification()
        {
            return Json(true);
        }

        public IActionResult AddDevice(DeviceModel model)
        {
            try
            {
                model.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                IMasterManager.CreateDevice(model);

                return Json(true);
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "AddDevice", ex.Message);
                return Json(false);
            }
        }

        public IActionResult DeleteDevice(DeviceModel model)
        {
            //model.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
            //IMasterManager.CreateDevice(model);
            try
            {
                var getDeviceData = IMasterManager.GetDeviceByUserId(Convert.ToInt32(HelperController.GetCookie("UserId")));
                var checkData = getDeviceData.Where(x => x.PushEndpoint == model.PushEndpoint && x.PushAuth == model.PushAuth && x.PushP256DH == model.PushP256DH).FirstOrDefault();
                if (checkData != null)
                {
                    IMasterManager.DeleteDevice(checkData.Id);
                }

                return Json(true);
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "DeleteDevice", ex.Message);
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult CheckActiveNotificationOld(DeviceModel model)
        {
            try
            {
                var getDeviceData = IMasterManager.GetDeviceByUserId(Convert.ToInt32(HelperController.GetCookie("UserId")));
                var checkData = getDeviceData.Where(x => x.PushEndpoint == model.PushEndpoint && x.PushAuth == model.PushAuth && x.PushP256DH == model.PushP256DH).Any();

                if (checkData == true)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "CheckActiveNotification", ex.Message);
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult CheckActiveNotification(DeviceModel model)
        {
            try
            {
                var getDeviceData = IMasterManager.GetDeviceByUserId(Convert.ToInt32(HelperController.GetCookie("UserId"))).Any();
                

                if (getDeviceData == true)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "CheckActiveNotification", ex.Message);
                return Json(false);
            }
        }


    }
}