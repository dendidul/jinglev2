using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmCategoryController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmCategoryController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }


        public IActionResult Index()
        {
            var model = IMasterManager.GetAllCategory();

            return View(model);
        }


        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryModel model)
        {
            model.CreatedBy = HelperController.GetCookie("UserId");
            model.IsActive = true;
            IMasterManager.CreateCategoryData(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {

            CategoryModel model = new CategoryModel();
            model.Id = id;
            model = IMasterManager.GetDataCategory(model);


            return View(model);
        }

        public IActionResult Details(int id)
        {

            CategoryModel model = new CategoryModel();
            model.Id = id;
            model = IMasterManager.GetDataCategory(model);


            return View(model);
        }

        public IActionResult Delete(int id)
        {

            CategoryModel model = new CategoryModel();
            model.Id = id;
            model = IMasterManager.GetDataCategory(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(CategoryModel model)
        {

           
           IMasterManager.DeleteCategoryData(model.Id);


            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryModel model)
        {
            try
            {
                // TODO: Add update logic here
                model.UpdatedBy = HelperController.GetCookie("UserId");
                IMasterManager.UpdateCategoryData(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}