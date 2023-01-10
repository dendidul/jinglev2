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
    public class AdmAgencyController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;
        private readonly ITransactionManager ITransactionManager;


        public AdmAgencyController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.ITransactionManager = new TransactionManager(config);

        }

        public IActionResult Index()
        {
            IList<AgencyModel> model = new List<AgencyModel>();
            model = IMasterManager.GetAllAgency();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.UserAgentList = new SelectList(HelperController.UserAgentList, "value", "text");
            return View();
        }

        [HttpPost]
        public IActionResult Create(AgencyModel model)
        {
            model.CreatedBy = HelperController.GetCookie("UserId");
            IMasterManager.CreateAgency(model);
            return RedirectToAction("Index");
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

        public IActionResult Delete(int id)
        {

            AgencyModel model = new AgencyModel();
            model.Id = id;
            model = IMasterManager.GetAgency(model);
            ViewBag.UserAgentList = new SelectList(HelperController.UserAgentList, "value", "text");
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(AgencyModel model)
        {

            model.UpdatedBy = HelperController.GetCookie("UserId");
            IMasterManager.DeleteAgency(model.Id);


            return RedirectToAction("Index");
        }

    }
}