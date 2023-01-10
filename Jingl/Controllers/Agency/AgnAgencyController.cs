using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AgnAgencyController : AgnMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;
        private readonly ITransactionManager ITransactionManager;


        public AgnAgencyController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.ITransactionManager = new TransactionManager(config);

        }

        public IActionResult Index()
        {
            IList<AgencyModel> model = new List<AgencyModel>();
            int UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
            //model = IMasterManager.GetAllAgency().Where(x => x.UserId == test);
            var data = IMasterManager.GetAllAgency().Where(x => x.UserId == UserId);
            return View(data);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.UserAgentList = new SelectList(HelperController.UserAgentList, "value", "text");
            AgencyModel model = new AgencyModel();
            model.Id = id;
            model = IMasterManager.GetAgency(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgencyModel model)
        {
            try
            {
                model.UpdatedBy = HelperController.GetCookie("UserId");
                // TODO: Add update logic here
                IMasterManager.UpdateAgency(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.UserAgentList = new SelectList(HelperController.UserAgentList, "value", "text");
            AgencyModel model = new AgencyModel();
            model.Id = id;
            model = IMasterManager.GetAgency(model);
            return View(model);
        }
    }
}