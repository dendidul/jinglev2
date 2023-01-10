using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmRoleController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmRoleController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            IList<RoleModel> model = new List<RoleModel>();
            model = IUserManagementManager.GetAllRole();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoleModel model)
        {
            IUserManagementManager.CreateRoleData(model);
            return RedirectToAction("Index");
        } 

        public IActionResult Edit(int id)
        {
            RoleModel model = new RoleModel();
            model.Id = id;
            model = IUserManagementManager.GetRole(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleModel model)
        {
            try
            {
             
                // TODO: Add update logic here
                IUserManagementManager.UpdateRoleData(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            RoleModel model = new RoleModel();
            model.Id = id;
            model = IUserManagementManager.GetRole(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            RoleModel model = new RoleModel();
            model.Id = id;
            model = IUserManagementManager.GetRole(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(RoleModel model)
        {


            IUserManagementManager.DeleteRole(model.Id);


            return RedirectToAction("Index");
        }


    }
}