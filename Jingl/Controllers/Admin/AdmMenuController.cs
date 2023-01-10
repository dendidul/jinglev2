using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.General.Model.Admin.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Jingl.Web.Helper;
using CookieManager;

namespace Jingl.Web.Controllers
{
    public class AdmMenuController : Controller
    {
        private readonly IUserManagementManager _IUserManagementManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public AdmMenuController(IConfiguration _config, ICookie cookie)
        {
           
            this._IUserManagementManager = new UserManagementManager(_config);
            this._cookie = cookie;
            this.HelperController = new HelperController(_config, cookie);
            // ViewBag.Menu = BuildRoleMenu();

            // ViewBag.ListProject = BuildListProject();



        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            ValidateUserRole(ctx);
            //  ViewBagItem(ctx);
        }

        public List<RoleMenuViewModel> BuildRoleMenu()
        {
            //HttpContext.Session.SetString("Role_ID", "1");           
            //var data = HttpContext.Session.GetString("Role_ID");
            //var RolesID = Convert.ToInt32(data);
            //var UserName = _cookie["UserName"];
            //var RolesID = _cookie["Role_ID"];
            var RolesID = HelperController.GetCookie("Role_ID");

             //var RolesID1 = _cookie["Role_ID"];
            var datalist = _IUserManagementManager.BuildRoleMenu(Convert.ToInt32(RolesID));
            return datalist.ToList();

        }

        public IActionResult SetMenu()
        {
            //var UserName = _cookie["UserName"];
            //var RolesID = _cookie["Role_ID"];

            //var UserName = HelperController.GetCookie("UserName");
            var RolesID = HelperController.GetCookie("Role_ID");
            //var RolesID1 = _cookie["Role_ID"];
            var datalist = _IUserManagementManager.BuildRoleMenu(Convert.ToInt32(RolesID));
            ViewBag.Menu = datalist;
            return PartialView("Menu");
        }

        public ActionResult ValidateUserRole(ActionExecutingContext ctx)
        {

            if (HelperController.GetCookie("Role_ID") != null)
            {
                var session_role_id = HelperController.GetCookie("Role_ID");
                if (string.IsNullOrEmpty(session_role_id))
                {
                    ctx.Result = new RedirectResult(Url.Action("Login", "AdmAccount"));
                    return RedirectToAction("Login", "AdmAccount");
                }

                var role_id = Convert.ToInt32(session_role_id);

                var URL = HttpContext.Request.GetDisplayUrl();
                var fullUrl = HttpContext.Request.GetDisplayUrl().ToString();

                var questionMarkIndex = fullUrl.IndexOf('?');
                string queryString = null;
                string url = fullUrl;
                if (questionMarkIndex != -1) // There is a QueryString
                {
                    url = fullUrl.Substring(0, questionMarkIndex);
                    queryString = fullUrl.Substring(questionMarkIndex + 1);
                }

                //var request = new HttpRequest(null, url, queryString);
                //var response = new HttpResponse(new StringWriter());
                //var httpContext = new HttpContext(request, response);
                //var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
                var routeData = ControllerContext.ActionDescriptor.ControllerName;
                //var controllerName = routeData.Values["controller"] == null ? null : routeData.Values["controller"].ToString();

                ByPassController ByPassController = new ByPassController();
                var AccessingController = ByPassController.ControllerLists();
                var count = AccessingController.Where(x => x.ToLower() == routeData.ToString().ToLower()).Count();

                if (count < 1)
                {
                    var dataCount = _IUserManagementManager.CheckMenuForFoles(role_id, routeData);
                    if (dataCount == false)
                    {
                        ctx.Result = new RedirectResult(Url.Action("Index", "NoAccess"));
                        return RedirectToAction("Index", "NoAccess");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                return null;

            }
            else
            {
                return RedirectToAction("Login", "AdmAccount");
            }
        }
    }
}