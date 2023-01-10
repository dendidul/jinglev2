using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmRptDailyPaymentController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmRptDailyPaymentController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            DailyPaymentModel paymentModel = new DailyPaymentModel();
            paymentModel.BeginDate = DateTime.Now;
            paymentModel.EndDate = DateTime.Now;
            BookModel model = new BookModel();
            model.BeginDate = DateTime.Now;
            model.EndDate = DateTime.Now;
            var data = ITransactionManager.GetDailyPayment(model);
            paymentModel.ListPaymentData = data;
            return View(paymentModel);
        }

        public IActionResult SearchData(DateTime begindate, DateTime enddate)
        {
            DailyPaymentModel paymentModel = new DailyPaymentModel();
            BookModel model = new BookModel();
            model.BeginDate = Convert.ToDateTime(begindate);
            model.EndDate = Convert.ToDateTime(enddate);
            var data = ITransactionManager.GetDailyPayment(model);
            paymentModel.ListPaymentData = data;
          
            return View("~/Views/AdmRptDailyPayment/Index.cshtml", paymentModel);
        }

    }
}