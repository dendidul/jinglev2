using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmSupportController : AdmMenuController
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;
        private readonly IUserManagementManager IUserManagementManager;

        public AdmSupportController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            var model = ITransactionManager.GetAllSupport();

            return View(model);
        }

        public IActionResult Create()
        {
          
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUserForDropdown(), "Id", "UserName", "");
            ViewBag.ListRegStat = new SelectList(HelperController.SupportStatusList, "value", "text", "");
            return View();
        }

        public IActionResult Edit(int id)
        {

            SupportModel model = new SupportModel();
            model.Id = id;
            model = ITransactionManager.GetAllSupport().Where(x=>x.Id == id).FirstOrDefault();
            ViewBag.ListRegStat = new SelectList(HelperController.SupportStatusList, "value", "text", model.Status);
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUserForDropdown(), "Id", "UserName", model.CreatedBy);
            return View(model);
        }

        public IActionResult Details(int id)
        {
           
            SupportModel model = new SupportModel();
            model.Id = id;
            model = ITransactionManager.GetAllSupport().Where(x => x.Id == id).FirstOrDefault();
            ViewBag.ListRegStat = new SelectList(HelperController.SupportStatusList, "value", "text", model.Status);
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUserForDropdown(), "Id", "UserName", model.CreatedBy);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupportModel model)
        {
            try
            {
                model.CreatedBy = HelperController.GetCookie("UserId");
                // TODO: Add insert logic here
                ITransactionManager.CreateSupport(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupportModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.UpdatedBy = HelperController.GetCookie("UserId");
                ITransactionManager.UpdateSupport(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {

            SupportModel model = new SupportModel();
            model.Id = id;
            model = ITransactionManager.GetSupport(model);
            ViewBag.ListRegStat = new SelectList(HelperController.SupportStatusList, "value", "text", model.Status);
            ViewBag.ListUser = new SelectList(IUserManagementManager.GetAllUserForDropdown(), "Id", "UserName", model.CreatedBy);

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(SupportModel model)
        {


            ITransactionManager.DeleteSupport(model.Id);


            return RedirectToAction("Index");
        }
    }
}