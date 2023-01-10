using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmVoucherController : AdmMenuController
    {
        private readonly FilesController FilesController;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public AdmVoucherController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.FilesController = new FilesController(config, cookie);
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            var AdmVoucher = IMasterManager.GetAllVoucher();
            return View(AdmVoucher);
        }

        public IActionResult Create()
        {
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            ViewBag.ListTalent = IMasterManager.GetAllTalent().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherModel model, string[] SelectedTalent)
        {
            try
            {                
                model.VoucherCd = model.VoucherCd;
                model.CreatedBy = HelperController.GetCookie("UserId");
                CultureInfo provider = CultureInfo.InvariantCulture;
                // It throws Argument null exception  
                //model.StartDate = DateTime.ParseExact(model.StartDateTemp, "MM/dd/yyyy", provider);
                ////model.StartDate = DateTime.Parse(model.StartDateTemp);
                //model.EndDate = DateTime.ParseExact(model.EndDateTemp, "MM/dd/yyyy", provider);
                VoucherModel vm = IMasterManager.CreateVoucher(model);

                IMasterManager.DeleteVoucherTalentByVoucherId(vm.Id);

                foreach (var i in SelectedTalent)
                {
                    var talentModel = new VoucherTalentViewModel();
                    talentModel.VoucherId = vm.Id;
                    talentModel.TalentId = int.Parse(i);
                    var temp = IMasterManager.CreateVoucherTalent(talentModel);
                }

                return Json("OK");
            }
            catch (Exception ex)
            {
                return View();

            }
        }

        public IActionResult Edit(int id)
        {
            VoucherModel model = new VoucherModel();
            model.Id = id;
            model = IMasterManager.GetVoucher(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListTalent = IMasterManager.GetAllTalent().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3);
            return View(model);
        }

        [HttpPost]
      
        public async Task<IActionResult> Edit(VoucherModel model, string[] SelectedTalent)
        {
            try
            {
                model.UpdatedBy = HelperController.GetCookie("UserId");
                CultureInfo provider = CultureInfo.InvariantCulture;
                // It throws Argument null exception  
                //model.StartDate = DateTime.ParseExact(model.StartDateTemp, "mm/dd/yyyy", provider);
                ////model.StartDate = DateTime.Parse(model.StartDateTemp);
                //model.EndDate = DateTime.ParseExact(model.EndDateTemp, "mm/dd/yyyy", provider);
                VoucherModel vm = IMasterManager.UpdateVoucher(model);

                IMasterManager.DeleteVoucherTalentByVoucherId(vm.Id);

                foreach (var i in SelectedTalent)
                {
                    var talentModel = new VoucherTalentViewModel();
                    talentModel.VoucherId = vm.Id;
                    talentModel.TalentId = int.Parse(i);
                    var temp = IMasterManager.CreateVoucherTalent(talentModel);
                }
                //return RedirectToAction("Index");
                return Json("OK");
            }
            catch (Exception ex)
            {
                return View();

            }
        }

        public IActionResult Details(int id)
        {
            VoucherModel model = new VoucherModel();
            model.Id = id;
            model = IMasterManager.GetVoucher(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListTalent = IMasterManager.GetAllTalent().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            VoucherModel model = new VoucherModel();
            model.Id = id;
            model = IMasterManager.GetVoucher(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListTalent = IMasterManager.GetAllTalent().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(VoucherModel model)
        {
            IMasterManager.DeleteVoucherTalentByVoucherId(model.Id);
            IMasterManager.DeleteVoucher(model.Id);

            return RedirectToAction("Index");
        }
    }
}