using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Jingl.Models;
//using Jingl.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Jingl.Service.Interface;
//using CookieManager;
using Jingl.WebApi.Helper;
using Jingl.Service.Manager;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
//using WebPush;
using Jingl.General.Model.User.Notification;
using Newtonsoft.Json;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;
using Microsoft.AspNetCore.Http;
using Jingl.WebApi.Authentication;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {

        private readonly IMasterManager IMasterManager;
      //  private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        //private readonly GmailAuthServices GAuth;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;

        public HomeController(IConfiguration config)
        {
            this.ITransactionManager = new TransactionManager(config);
          //  this.GAuth = new GmailAuthServices();
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        [HttpGet]
        [Route("~/api/Home/Index")]
        public IActionResult Index(string message = "", string talentname = "", string CategoryId = "", int? limit = 0)
        {
            var model = new HomeViewModel();
            try
            {
                UserModel user = new UserModel();
                List<CategoryModel> listCat = new List<CategoryModel>();
                List<TalentVideoModel> listTv = new List<TalentVideoModel>();
                List<TalentCategoryViewModel> listTc = new List<TalentCategoryViewModel>();
                var adVideo = new TalentVideoModel();
                string defUser = "";
                string defPass = "";
                listTv = ITransactionManager.GetTalentVideos(35).ToList();
                listCat = IMasterManager.GetCategoryByType("Talent").ToList();

                talentname = talentname != null ? talentname : "";
                CategoryId = CategoryId != null ? CategoryId : "";
                // limit = limit.HasValue ? limit.Value : 0;
                if (CategoryId == "")
                {
                    if (limit > 0)
                    {
                        if (talentname == "")
                        {
                            listTc = IMasterManager.GetDistinctAllTalentByCategory().Take(limit.Value).ToList();
                        }
                        else
                        {
                            listTc = IMasterManager.GetDistinctAllTalentByCategory().Where(x => x.TalentNm.ToLower().Contains(talentname.ToLower())).Take(limit.Value).ToList();
                        }
                    }
                    else
                    {

                        if (talentname == "")
                        {
                            listTc = IMasterManager.GetDistinctAllTalentByCategory().ToList();
                        }
                        else
                        {
                            listTc = IMasterManager.GetDistinctAllTalentByCategory().Where(x => x.TalentNm.ToLower().Contains(talentname.ToLower())).ToList();
                        }
                    }

                }
                else
                {
                    int CatId = CategoryId != "" ? Convert.ToInt32(CategoryId) : 0;
                    if (CatId > 0 && limit <= 0 && talentname == "")
                    {
                        //listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId && m.TalentNm ).ToList();

                        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId).ToList();
                    }
                    else if (CatId > 0 && limit <= 0 && talentname != "")
                    {
                        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId && m.TalentNm.ToLower().Contains(talentname.ToLower())).ToList();
                    }

                    else if (CatId > 0 && limit > 0 && talentname == "")
                    {
                        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId).Take(limit.Value).ToList();
                    }


                    else if (CatId > 0 && limit > 0 && talentname != "")
                    {
                        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId && m.TalentNm.ToLower().Contains(talentname.ToLower())).Take(limit.Value).ToList();
                    }


                    else
                    {
                        listTc = IMasterManager.GetDistinctAllTalentByCategory().Take(limit.Value).ToList();
                    }
                }

                //if (CategoryId == "")
                //{
                //    if (talentname == "")
                //    {
                //        listTc = IMasterManager.GetDistinctAllTalentByCategory().Take(8).ToList();
                //    }
                //    else
                //    {
                //        listTc = IMasterManager.GetDistinctAllTalentByCategory().Where(x => x.TalentNm.ToLower().Contains(talentname.ToLower())).ToList();
                //    }
                //}
                //else
                //{
                //    int CatId = CategoryId != "" ? Convert.ToInt32(CategoryId) : 0;
                //    if (CatId > 0 && limit <= 0 && talentname == "")
                //    {
                //        //listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId && m.TalentNm ).ToList();

                //        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId).ToList();
                //    }
                //    else if (CatId > 0 && limit <= 0 && talentname != "")
                //    {
                //        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId && m.TalentNm.ToLower().Contains(talentname.ToLower())).ToList();
                //    }

                //    else if (CatId > 0 && limit > 0 && talentname == "")
                //    {
                //        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId).Take(limit.Value).ToList();
                //    }


                //    else if (CatId > 0 && limit > 0 && talentname != "")
                //    {
                //        listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CatId && m.TalentNm.ToLower().Contains(talentname.ToLower())).Take(limit.Value).ToList();
                //    }


                //    else
                //    {
                //        listTc = IMasterManager.GetDistinctAllTalentByCategory().Take(8).ToList();
                //    }
                //}

                ////if (CategoryId > 0 && limit <= 0 && talentname == "")
                ////{
                ////    //listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId && m.TalentNm ).ToList();

                ////    listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId).ToList();
                ////}
                ////else if (CategoryId > 0 && limit <= 0 && talentname != "")
                ////{
                ////    listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId && m.TalentNm.ToLower().Contains(talentname.ToLower())).ToList();
                ////}

                ////else if (CategoryId > 0 && limit > 0 && talentname == "")
                ////{
                ////    listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId).Take(limit.Value).ToList();
                ////}


                ////else if (CategoryId > 0 && limit > 0 && talentname != "")
                ////{
                ////    listTc = IMasterManager.GetTalentCategoryAllData().Where(m => m.CategoryId == CategoryId && m.TalentNm.ToLower().Contains(talentname.ToLower())).Take(limit.Value).ToList();
                ////}

                ////else
                ////{
                ////    listTc = IMasterManager.GetTalentCategoryAllData().Take(8).ToList();
                ////}

                adVideo = listTv.Where(x => x.BookCategory == 0).FirstOrDefault();
                if (adVideo != null)
                {
                    ViewBag.Link = adVideo.Link;
                }
                else
                {
                    ViewBag.Link = "";
                }
                var talentModel = new TalentViewModel();
                //int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                int userId =0;
                user.Id = userId;
                talentModel = IMasterManager.GetAllTalentCategory();
                user = IUserManagementManager.GetUser(user);

                if (user != null)
                {
                    if (user.UserName == user.DefaultUsername)
                    {
                        defUser = "Username";
                    }
                    if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.DefaultPassword))
                    {
                        if (Encryptor.Decrypt(user.Password) == Encryptor.Decrypt(user.DefaultPassword))
                        {
                            defPass = "Password";
                        }
                    }
                    if (!string.IsNullOrEmpty(defPass) || !string.IsNullOrEmpty(defUser))
                    {
                        message = string.Format("Hi {0}.\\nSilahkan ubah {1} {2} mu di Menu Profle/Setting/Edit Profile", user.UserName, defUser, defPass);
                    }
                }

                model.ListTalentCategoryModel = listTc;
                //ViewBag.Categories = listCat.OrderByDescending(i => i.Id != 25).ThenByDescending(i => i.Id == 52).Where(i => i.Id != 25);
                //ViewBag.Message = message;
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
            
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(0), "Index", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });
                throw ex;
            }

           // var result = model;

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpGet]
        [Route("~/api/Home/AllTalent")]
        public IActionResult AllTalent(string message = "", string talentname = "", int limit = 0)
        {
            var model = new HomeViewModel();
            try
            {
                UserModel user = new UserModel();
                List<CategoryModel> listCat = new List<CategoryModel>();
                List<TalentVideoModel> listTv = new List<TalentVideoModel>();
                List<TalentCategoryViewModel> listTc = new List<TalentCategoryViewModel>();
                if (!string.IsNullOrEmpty(talentname))
                {
                    listTc = IMasterManager.GetTalentCategoryAllDataNew().Where(fi => fi.TalentNm.IndexOf(talentname, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else
                {
                    if (limit > 0)
                    {
                        listTc = IMasterManager.GetTalentCategoryAllDataNew().Take(limit).ToList();
                    }
                    else
                    {
                        listTc = IMasterManager.GetTalentCategoryAllDataNew().ToList();
                    }
                }
                var adVideo = new TalentVideoModel();
                string defUser = "";
                string defPass = "";
                listTv = ITransactionManager.GetTalentVideos(35).ToList();
                listCat = IMasterManager.GetCategoryByType("Talent").ToList();
                adVideo = listTv.Where(x => x.BookCategory == 0).FirstOrDefault();
                if (adVideo != null)
                {
                    ViewBag.Link = adVideo.Link;
                }
                else
                {
                    ViewBag.Link = "";
                }
                var talentModel = new TalentViewModel();
                //  int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                int userId = 0;
                user.Id = userId;
                talentModel = IMasterManager.GetAllTalentCategory();
                user = IUserManagementManager.GetUser(user);

                if (user != null)
                {
                    if (user.UserName == user.DefaultUsername)
                    {
                        defUser = "Username";
                    }
                    if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.DefaultPassword))
                    {
                        if (Encryptor.Decrypt(user.Password) == Encryptor.Decrypt(user.DefaultPassword))
                        {
                            defPass = "Password";
                        }
                    }
                    if (!string.IsNullOrEmpty(defPass) || !string.IsNullOrEmpty(defUser))
                    {
                        message = string.Format("Hi {0}.\\nSilahkan ubah {1} {2} mu di Menu Profle/Setting/Edit Profile", user.UserName, defUser, defPass);
                    }
                }
                model.ListTalentCategoryModel = listTc;
                //ViewBag.Categories = listCat.OrderByDescending(i => i.Id != 25).ThenByDescending(i => i.Id == 52).Where(i => i.Id != 25);
                //ViewBag.Message = message;
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(0), "AllTalent", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });
                throw ex;
            }
           // var result = model;

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpGet]
        [Route("~/api/Home/GetTalentCategory")]
        public IActionResult GetTalentCategory()
        {
            var model = new List<CategoryModel>();
            try
            {
                 model = IMasterManager.GetCategoryByType("Talent").ToList();
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(0), "GetTalentCategory", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });
                throw;
            }
           

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }
    }
}