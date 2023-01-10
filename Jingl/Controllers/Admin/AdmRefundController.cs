using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Model.Admin.UserManagement;
using CookieManager;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Jingl.General.Enum;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmRefundController : AdmMenuController
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;
        private readonly IUserManagementManager IUserManagementManager;

        public AdmRefundController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }
        public IActionResult Index()
        {
            RefundFormModel RefundModel = new RefundFormModel();
            IList<RefundModel> model = new List<RefundModel>();
            model = ITransactionManager.GetAllRefund();
            RefundModel.ListRefundModel = model;
            ViewBag.ListStatus = new SelectList(HelperController.RegistrationStatusList, "value", "text", 1);

            return View(RefundModel);
        }

        [HttpPost]
        public IActionResult SetStatus(string[] SelectedRefund, RefundFormModel model)
        {
            foreach (var i in SelectedRefund)
            {
                var getRefunddata = ITransactionManager.GetRefund(int.Parse(i));
                getRefunddata.Status = Convert.ToInt32(model.Status);
                getRefunddata.UpdatedBy = HelperController.GetCookie("UserId");
                if (model.Status == "3")
                {
                    UserModel UserModel = new UserModel();
                    UserModel.Id = Convert.ToInt32(getRefunddata.UserId);
                    UserModel = IUserManagementManager.GetUser(UserModel);
                    getRefunddata.PaidDate = DateTime.Now;
                   
                    var temp = ITransactionManager.GetDataBookByOrderId(getRefunddata.OrderNo);
                    var getBookingData = ITransactionManager.GetDataBook(temp);

                    if (getBookingData != null)
                    {
                        getBookingData.Status = (int)BookingFlow.RefundCompleted;
                        ITransactionManager.UpdateBookData(getBookingData);


                    }
                    HelperController.EmailCompletedTransfer(getBookingData.Email, "Refund", UserModel.Name, getRefunddata.RefundNumber).Wait();

                }

                ITransactionManager.UpdateRefund(getRefunddata);


            }
            return Json("OK");
        }

        public IActionResult SearchData(string Status)
        {
            RefundFormModel RefundModel = new RefundFormModel();
            IList<RefundModel> model = new List<RefundModel>();
            if (Status != null)
            {
                model = ITransactionManager.GetAllRefund().Where(x => x.Status == Convert.ToInt32(Status)).ToList();
            }
            else
            {
                model = ITransactionManager.GetAllRefund();
            }

            RefundModel.ListRefundModel = model;

            //ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text", Period);
            ViewBag.ListStatus = new SelectList(HelperController.RegistrationStatusList, "value", "text", Status);
            return View("~/Views/AdmRefund/Index.cshtml", RefundModel);

        }
    }
}