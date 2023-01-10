using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jingl.Models;
using Jingl.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Jingl.Service.Interface;
using CookieManager;
using Jingl.Web.Helper;
using Jingl.Service.Manager;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using WebPush;
using Jingl.General.Model.User.Notification;
using Newtonsoft.Json;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;
using Microsoft.AspNetCore.Http;

namespace Jingl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        private readonly GmailAuthServices GAuth;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;

        public HomeController(IConfiguration config, ICookie cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.GAuth = new GmailAuthServices();
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        [HttpGet]
        [Route("~/api/Home/Index")]
        public IActionResult Index(string message = "" ,string talentname="", string CategoryId = "" , int? limit = 0)
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
               // listTv = ITransactionManager.GetTalentVideos(35).ToList();
                listCat = IMasterManager.GetCategoryByType("Talent").ToList();
                talentname = talentname != null ? talentname : "";
                CategoryId = CategoryId != null ? CategoryId : "";
               // limit = limit.HasValue ? limit.Value : 0;
                if (CategoryId == "")
                {
                    if(limit > 0)
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

               
                
                adVideo = listTv.Where(x => x.BookCategory == 0).FirstOrDefault();
                //if (adVideo != null)
                //{
                //    ViewBag.Link = adVideo.Link;
                //}
                //else
                //{
                //    ViewBag.Link = "";
                //}
                var talentModel = new TalentViewModel();
                int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                user.Id = userId;
               // talentModel = IMasterManager.GetAllTalentCategory();
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

            }
            catch (Exception ex)
            {
               
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                throw ex;
            }

            var result = model;

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
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
                int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
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
                ViewBag.Categories = listCat.OrderByDescending(i => i.Id != 25).ThenByDescending(i => i.Id == 52).Where(i => i.Id != 25);
                ViewBag.Message = message;

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                throw ex;
            }
            var result = model;

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public IActionResult IndexOld2()
        {

            var model = new HomeViewModel();
            try
            {
                var talentModel = new TalentViewModel();
                int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                talentModel = IMasterManager.GetAllTalentCategory();
                //talentModel.ListTalentModel = IMasterManager.GetAllTalent().Where(x=>x.UserId != userId).ToList();

                int talentSequence = 0;
                IList<TalentModel> TalentList = new List<TalentModel>();
                //foreach (var i in talentModel.ListTalentModel.Where(x => x.UserId != userId && x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                foreach (var i in talentModel.ListTalentModel.Where(x => x.Status == (int)Registration.Completed && x.RoleId == 3 && x.Id != 35).ToList())
                {
                    TalentModel talent = new TalentModel();
                    talent = i;
                    talent.CategoryName = HelperController.CategoryTalentData(i.Id);
                    talent.sequence = talentSequence;
                    TalentList.Add(talent);
                    talentSequence++;
                }

                talentModel.ListTalentModel = TalentList;

                var listTalentVideo = ITransactionManager.GetAllVideo();
                var listTalentVideoForMostWatch = ITransactionManager.GetAllVideo();

                IList<TalentVideoModel> ListVideo = new List<TalentVideoModel>();
                IList<TalentVideoModel> ListMostWatchVideo = new List<TalentVideoModel>();

                int videoSequence = 1;
                int videoTrending = 1;

                foreach (var i in listTalentVideo.ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = i;
                    tvm.Sequence = videoSequence;
                    ListVideo.Add(tvm);
                    videoSequence++;

                }

                foreach (var j in listTalentVideoForMostWatch.OrderByDescending(x => x.ViewsCount).ToList())
                {
                    TalentVideoModel tvm = new TalentVideoModel();
                    tvm = j;
                    tvm.Sequence = videoTrending;
                    ListMostWatchVideo.Add(tvm);
                    videoTrending++;

                }
                var getBannerData = IMasterManager.GetAllBanner().Where(x => x.BannerCategory == "HomeScr").OrderBy(x => x.Sequence).ToList();
                //talentModel.ListVideo = ListVideo;
                model.ListBanner = getBannerData;
                model.ListTalentModel = TalentList;
                ListVideo = ListVideo.OrderBy(x => x.Sequence).ToList();
                model.ListBestVideo = ListVideo.OrderBy(x => x.Sequence).Take(4).ToList();
                model.ListMostWatchVideo = ListMostWatchVideo.Take(4).ToList();

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                throw ex;
            }

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult welcome()
        {

            var RoleId = HelperController.GetCookie("Role_ID");
            if (RoleId != "")
            {
                if (RoleId == "2" || RoleId == "3")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("DashBoardDataStudio", "AdmHome");

                }


            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Carapesan()
        {
            return View();
        }
        public IActionResult LoginUser()
        {
            return View();
        }

        public async Task<IActionResult> SignInGmail(string returnUrl)
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync("External");
                var authenticateResul1 = await HttpContext.AuthenticateAsync("Application");
                //  var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                var principal = authenticateResult.Principal;
                var providerKey = authenticateResult.Principal.Claims.FirstOrDefault();

                if (!authenticateResult.Succeeded)
                    return BadRequest(); // TODO: Handle this better.

                var claimsIdentity = new ClaimsIdentity("Application");

                claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier));
                claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Email));

                await HttpContext.SignInAsync(
                    "Application",
                    new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "SignInGmail", ex.Message);
                return BadRequest();
            }
        }

        public IActionResult SamsungPrize()
        {
            return View();
        }

        public IActionResult TipsVideo()
        {
            return View();
        }

        public IActionResult BenefitLevel()
        {
            return View();
        }

        public IActionResult NotificationAgreement()
        {
            return View();
        }

        [HttpGet]
        [Route("~/api/Home/GetTalentCategory")]
        public IActionResult GetTalentCategory()
        {
            var result = IMasterManager.GetCategoryByType("Talent").ToList();

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        //[HttpGet]
        //[Route("~/api/Home/Sort/")]
        //public IActionResult Sort(int? SortCategoryId, string SortCategoryName )
        //{
        //    var model = new HomeViewModel();
        //    try
        //    {
        //        UserModel user = new UserModel();
        //        List<CategoryModel> listCat = new List<CategoryModel>();
        //        List<TalentVideoModel> listTv = new List<TalentVideoModel>();
        //        List<TalentCategoryViewModel> listTc = new List<TalentCategoryViewModel>();
        //        var adVideo = new TalentVideoModel();
        //        string defUser = "";
        //        string defPass = "";
        //        listTv = ITransactionManager.GetTalentVideos(35).ToList();
        //        listCat = IMasterManager.GetCategoryByType("Talent").ToList();
        //        listTc = IMasterManager.GetTalentCategoryAllData().ToList();
        //        adVideo = listTv.Where(x => x.BookCategory == 0).FirstOrDefault();
        //        if (adVideo != null)
        //        {
        //            ViewBag.Link = adVideo.Link;
        //        }
        //        else
        //        {
        //            ViewBag.Link = "";
        //        }
        //        var talentModel = new TalentViewModel();
        //        int userId = Convert.ToInt32(HelperController.GetCookie("UserId"));
        //        user.Id = userId;
        //        talentModel = IMasterManager.GetAllTalentCategory();
        //        model.ListTalentCategoryModel = listTc;
        //        ViewBag.Categories = listCat.OrderByDescending(i => i.Id != 25).ThenByDescending(i => i.Id == 52).Where(i => i.Id != 25);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

        //        throw ex;
        //    }

        //    var result = model.ListTalentModel.Where(m => m.CategoryName == SortCategoryName).Take(2);

        //    return new JsonResult(result)
        //    {
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //}
    }
}
