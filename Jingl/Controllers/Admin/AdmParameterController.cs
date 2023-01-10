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
    public class AdmParameterController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;    
        private readonly HelperController HelperController;


        public AdmParameterController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var AdmParam = IMasterManager.AdmGetAllParameter();
            return View(AdmParam);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ParameterModel model)
        {
            model.IsActive = true;
            IMasterManager.CreateParam(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {

            ParameterModel model = new ParameterModel();
            model.Id = id;
            model = IMasterManager.GetParameter(model.Id);

           
            return View(model);
        }

        public IActionResult Details(int id)
        {

            ParameterModel model = new ParameterModel();
            model.Id = id;
            model = IMasterManager.GetParameter(model.Id);


            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParameterModel model)
        {
            try
            {
                model.IsActive = true;
                // TODO: Add update logic here
                IMasterManager.UpdateParam(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {

            ParameterModel model = new ParameterModel();
            model.Id = id;
            model = IMasterManager.GetParameter(model.Id);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ParameterModel model)
        {


            IMasterManager.DeleteParameter(model.Id);


            return RedirectToAction("Index");
        }
    }
}