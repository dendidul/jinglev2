using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmAccessRoleController : AdmMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmAccessRoleController(IConfiguration config, ICookie cookie) : base(config, cookie)
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

        public IActionResult RoleAccess(int id)
        {
            RoleAccessMenuViewModel model = new RoleAccessMenuViewModel();
            RoleModel roleModel = new RoleModel();
            IList<MenuModel> SelectedList = new List<MenuModel>();
            roleModel.Id = id;
            var getRole = IUserManagementManager.GetRole(roleModel);
            model.RoleId = getRole.Id;
            model.RoleNm = getRole.RoleNm;
            model.ListMenu = IUserManagementManager.GetAllMenu();
            var selectedMenu = IUserManagementManager.GetAllRoleAccess().Where(x => x.RoleId == id).ToList();

            foreach(var i in selectedMenu)
            {
                MenuModel menuModel = new MenuModel();
                menuModel.Id = i.MenuId;
                SelectedList.Add(menuModel);
            }
            model.ListSelectedMenu = SelectedList;

            

            return View(model);
        }

        [HttpPost]
        public ActionResult RoleAccess(RoleAccessMenuViewModel model, string[] SelectedMenu, string[] SelectedChildMenu)
        {

            var getdata = IUserManagementManager.GetAllRoleAccess().Where(x => x.RoleId == model.RoleId).ToList();
            foreach (var i in getdata)
            {
                IUserManagementManager.DeleteAccessMenu(i.Id);
            }

            if (SelectedMenu != null)
            {
                foreach (var j in SelectedMenu)
                {
                    RoleAccessMenuModel form = new RoleAccessMenuModel();
                    form.MenuId = int.Parse(j);
                    form.RoleId = model.RoleId;
                    IUserManagementManager.CreateRoleAccessMenu(form);
                   
                }
            }


            if (SelectedChildMenu != null)
            {
                foreach (var k in SelectedChildMenu)
                {
                    RoleAccessMenuModel form = new RoleAccessMenuModel();
                    form.MenuId = int.Parse(k);
                    form.RoleId = model.RoleId;
                    IUserManagementManager.CreateRoleAccessMenu(form);

                }
            }



            //var data = RolesMenuLogic.ReadRoleMenuByRoleID(id);
            //return View(data);
            return RedirectToAction("Index");
        }
    }
}