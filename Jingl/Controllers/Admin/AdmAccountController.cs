using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Jingl.General.Utility;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Enum;

namespace Jingl.Web.Controllers.CRM
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AdmAccountController : Controller
    {
     
        private readonly IUserManagementManager IUserManagementManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public AdmAccountController(IConfiguration _config,ICookie cookie)
        {
            this.IMasterManager = new MasterManager(_config);
            this.IUserManagementManager = new UserManagementManager(_config);
            this._cookie = cookie;
            this.HelperController = new HelperController(_config, cookie);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("~/api/AdmAccount/Login")]
        public IActionResult Login([FromBody]UserModel model)
        {
            var Password = Encryptor.Encrypt(model.Password);
            model.Password = Password;
            var checkdata = IUserManagementManager.CheckValidUser(model);

            if(checkdata != null )
            {
                //CookieOptions options = new CookieOptions();
                //options.Expires = DateTime.Now.AddDays(1);
                //Response.Cookies.Append("Role_ID", checkdata != null ? checkdata.RoleId.ToString():"", options);
                //Response.Cookies.Append("UserName", model.UserName, options);

                string FirstName = checkdata.FirstName != null ? checkdata.FirstName : "";
                string LastName = checkdata.LastName != null ? checkdata.LastName : "";
                string UserName = FirstName + " " + LastName;
                //HelperController.SetCookie("UserId", checkdata != null ? checkdata.Id.ToString() : "");
                //HelperController.SetCookie("UserName", UserName);
                //HelperController.SetCookie("Role_ID", checkdata != null ? checkdata.RoleId.ToString() : "");

                return Json(new { User = "Valid", UserId = checkdata.Id, RoleId = checkdata.RoleId });
            }
            else
            {
                return Json(new { User = "NotValid", UserId = checkdata != null ? checkdata.Id.ToString() : "", RoleId = checkdata != null ? checkdata.RoleId.ToString() : "" });
            }

        }

        [HttpPost]
        [Route("~/api/AdmAccount/CheckValidUserExternal")]
        public IActionResult CheckValidUserExternal([FromBody]UserModel model)
        {

            var checkdata = IUserManagementManager.CheckValidUserExternal(model);

            //var checkemail = IUserManagementManager.GetUserByEmail(model).FirstOrDefault();

            if (checkdata != null)
            {

                string FirstName = checkdata.FirstName != null ? checkdata.FirstName : "";
                string LastName = checkdata.LastName != null ? checkdata.LastName : "";
                string UserName = FirstName + " " + LastName;

                if (checkdata.Email == model.Email && checkdata.SignUpType.IndexOf(model.SignUpType, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Json(new { User = "Valid", UserId = checkdata.Id, RoleId = checkdata.RoleId });
                }

                else
                {
                    return Json(new { User = "NotValid", UserId = checkdata != null ? checkdata.Id.ToString() : "", RoleId = checkdata != null ? checkdata.RoleId.ToString() : "" });
                }         
            }
            //else if (checkemail != null)
            //{
            //    UserModel data = new UserModel();
            //    data.Id = checkemail.Id;
            //    data.SignUpType = model.SignUpType;
            //    IUserManagementManager.SignUpTypeUpdate(data);
            //    return Json(new { User = "Valid", UserId = checkemail.Id, RoleId = checkemail.RoleId });
            //}
            else
            {
                var getparam = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "DEFPWD").FirstOrDefault();
                var _password = getparam != null ?getparam.ParamValue:"";
                model.Password = Encryptor.Encrypt(_password);
                UserModel data = new UserModel();
                Random random = new Random();
                var email = model.Email;
                var firstname = model.FirstName;
                var last_name = model.LastName;
                var signuptype = model.SignUpType;

                data.Email = email;
                data.FirstName = firstname;
                data.LastName = last_name;
                data.SignUpType = signuptype;
                data.RoleId = (int)Role.User;
                data.UserName = firstname + random.Next(0, 9999);
                data.DefaultPassword = model.Password;
                data.IsActive = 1;
                data.IsVerified = "0";
                data.Password = model.Password;
                data.DefaultUsername = firstname;

                //var checkvaliddata = IUserManagementManager.GetAllUser().Where(x => x.Email.ToLower() == data.Email.ToLower() && x.IsActive == 1 && string.IsNullOrEmpty(x.SignUpType)).FirstOrDefault();
                var checkvaliddata = IUserManagementManager.GetUserByEmail(data).Where(x=> string.IsNullOrEmpty(x.SignUpType)).FirstOrDefault();

                if (checkvaliddata != null)
                {
                    if (string.IsNullOrEmpty(checkvaliddata.SignUpType))
                    {
                        //  return Json(new { User = "Invalid Login", Status = "2" });
                        var sendData = IUserManagementManager.CreateUser(data);
                        var getdata = IUserManagementManager.GetUser(sendData);
                        return Json(new { getdata, Status = "4" });

                    }
                    else
                    {
                        return Json(new { Email = "Email Already Taken", Status = "3" });
                    }
                }
                else
                {
                    var sendData = IUserManagementManager.CreateUser(data);
                    var getdata = IUserManagementManager.GetUser(sendData);
                    return Json(new { getdata, Status = "4" });
                }
            }
        }

        public IActionResult LogOut()
        {
            return View();
        }

    }
}