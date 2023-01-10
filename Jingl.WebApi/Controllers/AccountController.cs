using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using CookieManager;
using Jingl.WebApi.Helper;
using Jingl.General.Model.Admin.Transaction;
using System.IO;
using Jingl.General.Model.User.ViewModel;
//using Jingl.Web.Controllers;
using Newtonsoft.Json;
using Jingl.General.Model.Admin.Master;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Jingl.Web.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
//using WebPush;
using Microsoft.AspNetCore.Mvc.Rendering;
using Jingl.General.Enum;
using Jingl.General.Utility;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Jingl.WebApi.Authentication;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;
        private readonly FilesController FilesController;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration config;


        public AccountController(IConfiguration config)
        {
            this.IMasterManager = new MasterManager(config);
            this.IUserManagementManager = new UserManagementManager(config);
            this.ITransactionManager = new TransactionManager(config);
            //this._cookie = cookie;
            this.HelperController = new HelperController(config);
           this.FilesController = new FilesController(config);
           // _signInManager = signInManager;
            this.config = config;
        }

        [HttpGet]
        [Route("~/api/Account/Profile")]
        public IActionResult Profile(string userid       //, string BookId
            )
        {

            var model = new ProfileViewModel();
            IList<BookModel> listBooking = new List<BookModel>();
            IList<BookModel> listBookingTalent = new List<BookModel>();
            IList<BookModel> listBookingPaid = new List<BookModel>();
            IList<TalentCategoryViewModel> listWishlist = new List<TalentCategoryViewModel>();
            IList<QuestionModel> listquestion = new List<QuestionModel>();

            var userModel = new UserModel();
            userModel.Name = "Guest";
            userModel.FirstName = "Guest";
            userModel.RoleId = 0;

            var Saldo = new SaldoModel();

            model.ListBooking = listBooking.ToList();
            model.ListTalentWishlist = listWishlist.ToList();
            model.UserModel = userModel;
            model.ActBookCount = "0";
            model.CompBookCount = "0";

            try
            {
                int UserId = Convert.ToInt32(userid);

                if (UserId != null && UserId != 0)
                {
                    model.UserModel = IUserManagementManager.GetUserProfiles(UserId);
                    model.TalentModel = IMasterManager.GetTalentProfiles(UserId);
                    var TalentData = IMasterManager.GetTalentProfiles(UserId);
                    IList<TalentVideoModel> TalentVideoList = new List<TalentVideoModel>();
                    IList<TalentVideoModel> UserVideoList = new List<TalentVideoModel>();
                    listWishlist = new List<TalentCategoryViewModel>();
                    listBooking = new List<BookModel>();
                    listBookingTalent = new List<BookModel>();
                    listWishlist = ITransactionManager.GetWishListByUserId(Convert.ToInt32(UserId));
                    listBooking = ITransactionManager.GetBookingByUserId(UserId);
                    model.QuestionList = ITransactionManager.GetAllQuestion().Where(m => m.UserId == UserId).ToList();
                    if (listBooking != null)
                    {
                        if (listBooking.Where(book => book.Status == 5).Count() > 99)
                        {
                            model.CompBookCount = "99+";
                        }
                        else
                        {
                            model.CompBookCount = Convert.ToString(listBooking.Where(book => book.Status == 5).Count());
                        }
                    }
                    if (TalentData != null)
                    {
                        TalentVideoList = ITransactionManager.GetTalentVideos(TalentData.Id);
                        listBookingTalent = ITransactionManager.GetBookingByTalentId(TalentData.Id).Where(book => book.Status > 1 && book.Status < 5).ToList();
                        listBookingPaid = ITransactionManager.GetBookingPaidByTalentId(TalentData.Id);
                        listquestion = ITransactionManager.GetAllQuestion().Where(m => m.TalentId == TalentData.Id).ToList();
                        model.TalentBookList = listBookingTalent.ToList();
                        model.AllBookListByTalent = ITransactionManager.GetBookingByTalentId(TalentData.Id).ToList();
                        model.TalentReactionVideoList = ITransactionManager.GetReactionByTalentId(TalentData.Id).ToList();
                        model.BookPaidList = listBookingPaid.ToList();

                        model.QuestionList = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentData.Id).ToList();

                        model.QuestionList = listquestion.ToList();
                        if (listBookingTalent.Where(book => book.Status > 1 && book.Status < 5).Count() > 99)
                        {
                            model.ActBookCount = "99+";
                        }
                        else
                        {
                            model.ActBookCount = Convert.ToString(listBookingTalent.Where(book => book.Status > 1 && book.Status < 5).Count());
                        }

                        var initSaldo = new SaldoModel();
                        initSaldo.TalentId = TalentData.Id;
                        Saldo = ITransactionManager.GetSaldoByTalentId(initSaldo);
                        if (Saldo != null)
                        {
                            var n = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "SaldoLimit").FirstOrDefault();
                            if (n != null) { model.LimitSaldo = (Convert.ToInt32(n.ParamCode) * TalentData.PriceAmount); }
                            else { model.LimitSaldo = (2 * TalentData.PriceAmount); }
                            model.TalentSaldo = Saldo;
                        }
                    }
                    UserVideoList = ITransactionManager.GetUserVideos(Convert.ToInt32(UserId));
                    model.UserVideoList = UserVideoList;
                    model.TalentVideoList = TalentVideoList;

                    if (!string.IsNullOrEmpty(model.UserModel.PhoneNumber))
                    {
                        if (model.UserModel.PhoneNumber.Substring(0, 1) == "0")
                        {
                            //string codeArea = "+62";
                            var theString = model.UserModel.PhoneNumber;
                            var aStringBuilder = new StringBuilder(theString);
                            aStringBuilder.Remove(0, 1);
                            aStringBuilder.Insert(0, "+62 ");
                            theString = aStringBuilder.ToString();
                            model.UserModel.PhoneNumberArea = theString;
                        }
                    }
                    // model.UserModel.Password = Encryptor.Decrypt(model.UserModel.Password);
                    model.UserModel.Password = model.UserModel.Password;
                    foreach (var x in listWishlist)
                    {
                        x.CategoryNm = HelperController.CategoryTalentData(x.TalentId);
                    }

                    var listTalentVideo = ITransactionManager.GetAllVideo();
                    IList<TalentVideoModel> ListVideo = new List<TalentVideoModel>();
                    int videoSequence = 1;
                    foreach (var i in listTalentVideo.ToList())
                    {
                        TalentVideoModel tvm = new TalentVideoModel();
                        tvm = i;
                        tvm.Sequence = videoSequence;
                        ListVideo.Add(tvm);
                        videoSequence++;

                    }
                    model.ListTalentWishlist = listWishlist.ToList();
                    //if (!string.IsNullOrEmpty(BookId))
                    //{
                    //    model.ListBooking = listBooking.Where(m => m.BookId == BookId && m.BookedBy == UserId).ToList();
                    //}
                    //else
                    //{
                    //    model.ListBooking = listBooking.ToList();
                    //}
                    model.ListBooking = listBooking.ToList();
                    var getParameter = IMasterManager.AdmGetAllParameter().ToList();
                    var getAppVersion = getParameter.Where(x => x.ParamName == "AppVersion" && x.ParamCode == "AppVersion").FirstOrDefault();
                    var getPeriod = getParameter.Where(x => x.ParamName == "ClmPeriod" && x.ParamCode == "ClmPeriod").FirstOrDefault();
                    model.AppVersion = getAppVersion != null ? getAppVersion.ParamValue : "";
                    model.Period = getPeriod != null ? getPeriod.ParamValue : "";
                    model.AllVideoList = ListVideo;
                   
                }
                else
                {
                    model.UserModel = IUserManagementManager.GetUserProfiles(UserId);
                    model.TalentModel = IMasterManager.GetTalentProfiles(UserId);
                    var TalentData = IMasterManager.GetTalentProfiles(UserId);
                    IList<TalentVideoModel> TalentVideoList = new List<TalentVideoModel>();
                    IList<TalentVideoModel> UserVideoList = new List<TalentVideoModel>();
                    listWishlist = new List<TalentCategoryViewModel>();
                    listBooking = new List<BookModel>();
                    listBookingTalent = new List<BookModel>();
                    listWishlist = ITransactionManager.GetWishListByUserId(Convert.ToInt32(userid));
                    listBooking = ITransactionManager.GetBookingByUserId(UserId);
                    if (listBooking != null)
                    {
                        if (listBooking.Where(book => book.Status == 5).Count() > 99)
                        {
                            model.CompBookCount = "99+";
                        }
                        else
                        {
                            model.CompBookCount = Convert.ToString(listBooking.Where(book => book.Status == 5).Count());
                        }
                    }
                    if (TalentData != null)
                    {
                        TalentVideoList = ITransactionManager.GetTalentVideos(TalentData.Id);
                        listBookingTalent = ITransactionManager.GetBookingByTalentId(TalentData.Id);
                        listBookingPaid = ITransactionManager.GetBookingPaidByTalentId(TalentData.Id);
                        model.TalentBookList = listBookingTalent.ToList();
                        model.BookPaidList = listBookingPaid.ToList();
                        if (listBookingTalent.Where(book => book.Status > 1 && book.Status < 5).Count() > 99)
                        {
                            model.ActBookCount = "99+";
                        }
                        else
                        {
                            model.ActBookCount = Convert.ToString(listBookingTalent.Where(book => book.Status > 1 && book.Status < 5).Count());
                        }

                        var initSaldo = new SaldoModel();
                        initSaldo.TalentId = TalentData.Id;
                        Saldo = ITransactionManager.GetSaldoByTalentId(initSaldo);
                        if (Saldo != null)
                        {
                            var n = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "SaldoLimit").FirstOrDefault();
                            if (n != null) { model.LimitSaldo = (Convert.ToInt32(n.ParamCode) * TalentData.PriceAmount); }
                            else { model.LimitSaldo = (2 * TalentData.PriceAmount); }
                            model.TalentSaldo = Saldo;
                        }
                    }
                    UserVideoList = ITransactionManager.GetUserVideos(Convert.ToInt32(userid));
                    model.UserVideoList = UserVideoList;
                    model.TalentVideoList = TalentVideoList;

                    if (!string.IsNullOrEmpty(model.UserModel.PhoneNumber))
                    {
                        if (model.UserModel.PhoneNumber.Substring(0, 1) == "0")
                        {
                            //string codeArea = "+62";
                            var theString = model.UserModel.PhoneNumber;
                            var aStringBuilder = new StringBuilder(theString);
                            aStringBuilder.Remove(0, 1);
                            aStringBuilder.Insert(0, "+62 ");
                            theString = aStringBuilder.ToString();
                            model.UserModel.PhoneNumberArea = theString;
                        }
                    }
                    model.UserModel.Password = Encryptor.Decrypt(model.UserModel.Password);
                    foreach (var x in listWishlist)
                    {
                        x.CategoryNm = HelperController.CategoryTalentData(x.TalentId);
                    }

                    var listTalentVideo = ITransactionManager.GetAllVideo();
                    IList<TalentVideoModel> ListVideo = new List<TalentVideoModel>();
                    int videoSequence = 1;
                    foreach (var i in listTalentVideo.ToList())
                    {
                        TalentVideoModel tvm = new TalentVideoModel();
                        tvm = i;
                        tvm.Sequence = videoSequence;
                        ListVideo.Add(tvm);
                        videoSequence++;

                    }
                    model.ListTalentWishlist = listWishlist.ToList();
                    //if (!string.IsNullOrEmpty(BookId))
                    //{
                    //    model.ListBooking = listBooking.Where(m => m.BookId == BookId && m.BookedBy == userid).ToList();
                    //}
                    //else
                    //{
                    //    model.ListBooking = listBooking.ToList();
                    //}
                    model.ListBooking = listBooking.ToList();
                    var getParameter = IMasterManager.AdmGetAllParameter().ToList();
                    var getAppVersion = getParameter.Where(x => x.ParamName == "AppVersion" && x.ParamCode == "AppVersion").FirstOrDefault();
                    var getPeriod = getParameter.Where(x => x.ParamName == "ClmPeriod" && x.ParamCode == "ClmPeriod").FirstOrDefault();
                    model.AppVersion = getAppVersion != null ? getAppVersion.ParamValue : "";
                    model.Period = getPeriod != null ? getPeriod.ParamValue : "";
                    model.AllVideoList = ListVideo;

                  

                }

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
            }
            catch (Exception ex)
            {
                var pmodel = new ProfileViewModel();
                HelperController.InsertLog(Convert.ToInt32(userid), "Profile", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = pmodel });
                throw ex;
            }
            //return new JsonResult(model)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpPost]
        [Route("~/api/Account/ChangePassword")]
        public IActionResult ChangePassword([FromBody] UserModel model)
        {
            try
            {

                UserModel usModel = new UserModel();
                usModel.Id = model.Id;

                usModel = IUserManagementManager.GetUser(usModel);

                //if (Encryptor.Decrypt(usModel.Password) == model.OldPassword)
                //{
                usModel.Password = Encryptor.Encrypt(model.NewPassword);
                IUserManagementManager.UpdateUser(usModel);

                //return Json(new { Status = "1", Message = "Success mengganti password" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "Success mengganti password" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(model.Id, "ChangePassword", ex.Message);

                //return Json(new { Status = "-2", Message = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message});

            }
        }

        [HttpGet]
        [Route("~/api/Account/Workspace")]
        public IActionResult Workspace(string UserId, string TalentName)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                var Id = Convert.ToInt32(UserId);

                if (!string.IsNullOrEmpty(TalentName))
                {
                    data = ITransactionManager.GetBookingByUserId(Id).Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else
                {
                    data = ITransactionManager.GetBookingByUserId(Id);
                }

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = data });

            }
            catch (Exception ex)
            {
                IList<BookModel> pdata = new List<BookModel>();
                HelperController.InsertLog(Convert.ToInt32(UserId), "Workspace", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message,result= pdata });
                throw ex;
            }
            //return new JsonResult(data)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }


        //[HttpGet]
        //[AllowAnonymous]
        //[Route("~/api/Account/ExternalLoginCallback")]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    try
        //    {
        //        if (remoteError != null)
        //        {
        //            //ErrorMessage = $"Error from external provider: {remoteError}";
        //            return RedirectToAction(nameof(Login));
        //        }
        //        var info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return RedirectToAction(nameof(Login));
        //        }

        //        // Sign in the user with this external login provider if the user already has a login.
        //        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //        var GetAllUser = IUserManagementManager.GetAllUser();
        //        var isValid = GetAllUser.Where(x => x.Email.ToLower() == info.Principal.FindFirstValue(ClaimTypes.Email).ToLower()).Any();

        //        if (result.Succeeded || isValid == true)
        //        {
        //            var checkdata = GetAllUser.Where(x => x.Email.ToLower() == info.Principal.FindFirstValue(ClaimTypes.Email).ToLower()).FirstOrDefault();
        //            var checkdata1 = GetAllUser.Where(x => x.Email.ToLower() == info.Principal.FindFirstValue(ClaimTypes.Email).ToLower()).Count();
        //            if (checkdata != null)
        //            {
        //                HelperController.SetCookie("UserId", checkdata != null ? checkdata.Id.ToString() : "");
        //                HelperController.SetCookie("Role_ID", checkdata != null ? checkdata.RoleId.ToString() : "");
        //                //return RedirectToAction("Index", "Home");
        //                return Json(new
        //                {
        //                    User = "Valid",
        //                    RoleId = checkdata.RoleId,
        //                    Message = "Ok"

        //                });
        //            }
        //            else
        //            {
        //                //return RedirectToAction("ErrorPage", "NoAccess");
        //                return Json(new
        //                {
        //                    Message = "NoAccess"

        //                });
        //            }

        //            //_logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
        //            //return RedirectToLocal(returnUrl);
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            //return RedirectToAction("ErrorPage", "NoAccess");
        //            //return RedirectToAction(nameof(Lockout));
        //            return Json(new
        //            {
        //                Message = "Lockout"

        //            });
        //        }
        //        else
        //        {
        //            UserModel model = new UserModel();
        //            Random random = new Random();
        //            string defaultPassword = IMasterManager.GetParameterByCode("DEFPWD").ParamValue;

        //            // If the user does not have an account, then ask the user to create an account.
        //            //ViewData["ReturnUrl"] = returnUrl;
        //            //ViewData["LoginProvider"] = info.LoginProvider;
        //            var data = info.Principal;
        //            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //            var gender = info.Principal.FindFirstValue(ClaimTypes.Gender);
        //            var MobilePhone = info.Principal.FindFirstValue(ClaimTypes.MobilePhone);
        //            var DateOfBirth = info.Principal.FindFirstValue(ClaimTypes.DateOfBirth);
        //            var LastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
        //            var FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);

        //            model.Email = email;
        //            model.FirstName = FirstName;
        //            model.LastName = LastName;
        //            model.PhoneNumber = MobilePhone;
        //            model.SignUpType = info.LoginProvider;
        //            model.Password = Encryptor.Encrypt(defaultPassword);
        //            model.UserName = FirstName + random.Next(0, 9999);
        //            model.DefaultPassword = model.Password;
        //            model.DefaultUsername = model.UserName;
        //            model.RoleId = (int)Role.User;
        //            //model.IsVerified = "0";
        //            if (model.SignUpType == "Google")
        //            {
        //                model.IsVerified = "1";
        //                model.IsActiveCode = false;
        //            }

        //            if (model.SignUpType != "Google")
        //            {
        //                model.IsVerified = "0";
        //                model.IsActiveCode = false;
        //            }
        //            if (!string.IsNullOrEmpty(DateOfBirth))
        //            {
        //                model.BirthDate = Convert.ToDateTime(DateOfBirth);
        //            }

        //            Console.WriteLine(model);

        //            var checkdata = IUserManagementManager.GetAllUser().Where(x => x.Email.ToLower() == model.Email.ToLower() && x.IsActive == 1).FirstOrDefault();
        //            if (checkdata != null)
        //            {
        //                string NamaDepan = checkdata.FirstName != null ? checkdata.FirstName : "";
        //                string NamaBelakang = checkdata.LastName != null ? checkdata.LastName : "";
        //                string UserName = NamaDepan + " " + NamaBelakang;

        //                HelperController.SetCookie("UserId", checkdata != null ? checkdata.Id.ToString() : "");
        //                HelperController.SetCookie("UserName", UserName);
        //                HelperController.SetCookie("Role_ID", checkdata != null ? checkdata.RoleId.ToString() : "");

        //                //return RedirectToAction("Index", "Home");
        //                return Json(new
        //                {
        //                    User = "Valid",
        //                    RoleId = checkdata.RoleId,
        //                    Message = "Ok"

        //                });
        //            }
        //            else
        //            {
        //                model.IsActive = 1;
        //                var sendData = IUserManagementManager.CreateUser(model);
        //                var getdata = IUserManagementManager.GetUser(sendData);

        //                string NamaDepan = getdata.FirstName != null ? getdata.FirstName : "";
        //                string NamaBelakang = getdata.LastName != null ? getdata.LastName : "";
        //                string UserName = NamaDepan + " " + NamaBelakang;

        //                HelperController.SetCookie("UserId", getdata != null ? getdata.Id.ToString() : "");
        //                HelperController.SetCookie("UserName", UserName);
        //                HelperController.SetCookie("Role_ID", getdata != null ? getdata.RoleId.ToString() : "");

        //                string message = string.Format("Hi {0}.\\nSilahkan ubah Username dan Password mu di Menu Profle/Setting/Edit Profile", model.UserName);
        //                return RedirectToAction("Index", "Home", new { message = message });
        //            }


        //            //return RedirectToAction("SignUpPost", model);
        //            //return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        HelperController.InsertLog(0, "ExternalLoginCallback", ex.Message);
        //        //return RedirectToAction("ErrorPage", "NoAccess");
        //        return Json(new
        //        {
        //            Message = "NoAccess"

        //        });
        //    }
        //}

        [HttpPost]
        [Route("~/api/Account/SendEmailForgetPassword")]
        public IActionResult SendEmailForgetPassword([FromBody]UserModel model/*string email*/)
        {
            bool IsValid = false;
            try
            {
                var getdata = IUserManagementManager.GetAllUser();
                IsValid = getdata.Where(x => x.Email.ToLower() == model.Email.ToLower()).Any();

                if (IsValid == true)
                {
                    //kirim email
                    var data = getdata.Where(x => x.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

                    RequestChangeModel rcm = new RequestChangeModel();
                    rcm.RequestType = "ResetPassword";
                    rcm.UserId = data.Id;

                    var newRequestChange = ITransactionManager.CreateRequestChange(rcm);

                    HelperController.SendEmailForgotPassword(rcm.UserId.ToString(), model.Email, newRequestChange.RequestCode).Wait();
                    return Json(new { Status = StatusCodes.Status200OK, Message = "Email Sent" });
                }
                else
                {
                    //return Json(new { Status = "Error", Message = "Email is not exist" });
                    return Json(new { Status = StatusCodes.Status204NoContent, Message = "Email is not exist" });
                }


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "SendEmailForgetPassword", ex.Message);
                //return Json(false);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
                throw ex;
            }
            //return Json(IsValid);
        }

        [HttpGet]
        [Route("~/api/Account/VerifyEmailUsed")]
        public IActionResult VerifyEmailUsed([FromBody] UserModel model)
        {
            bool IsValid = false;
            try
            {
                var getdata = IUserManagementManager.GetAllUser();
                IsValid = getdata.Where(x => x.Email.ToLower() == model.Email.ToLower()).Any();
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = IsValid });
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "VerifyEmailUsed", ex.Message);
                // return Json(false);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = IsValid });
                throw ex;
            }
            //return Json(IsValid);
        }


        [HttpGet]
        [Route("~/api/Account/GetEditProfile")]
        public IActionResult GetEditProfile(string UserId)
        {
            var model = new UserModel();
            try
            {
                model.Id = Convert.ToInt32(UserId);
                model = IUserManagementManager.GetUser(model);
                var talentdata = IMasterManager.GetAllTalent().Where(x => x.UserId == model.Id).FirstOrDefault();
                //  model.TalentCategory = IMasterManager.getta();
                var categorymodel = IMasterManager.GetCategoryByType("Talent");

               // model.Password = Encryptor.Decrypt(model.Password);

                IList<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
                foreach (var i in categorymodel)
                {
                    TalentCategoryViewModel data = new TalentCategoryViewModel();
                    data.CategoryId = i.Id;
                    data.CategoryNm = i.CategoryNm;
                    ListCategory.Add(data);
                }

                model.TalentCategory = ListCategory;

                if (talentdata != null)
                {
                    model.TalentSelectedCategory = IMasterManager.GetTalentCategoryData(talentdata.Id);
                    model.TalentId = talentdata.Id;
                    model.Profesi = talentdata.Profesion;
                    model.VideoProfile = ITransactionManager.GetTalentVideos(talentdata.Id).Where(vid => vid.BookCategory == 0).FirstOrDefault();
                }
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(UserId), "EditProfile", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });
                throw ex;
            }
            //return new JsonResult(model)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }


        //[HttpPost]
        //[Route("~/api/Account/Support")]
        //public IActionResult Support([FromBody]SupportModel model)
        //{
        //    try
        //    {

        //        model.CreatedBy = model.CreatedBy;
        //        var data = ITransactionManager.CreateSupport(model);
        //        HelperController.EmailSupport(model.Subject, model.Details, model.CreatedBy, model.EmailAddress).Wait();


        //        return Json("OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperController.InsertLog(0, "Support", ex.Message);

        //        return Json("Error");
        //        throw ex;
        //    }
        //}


       



        [HttpPost]
        [Route("~/api/Account/PostEditProfile")]
        public async Task<IActionResult> PostEditProfile(
            // [FromRoute]int id ,
            [FromForm]UserModel model, IFormFile ProfImg, IFormFile AccountNumberImg, IFormFile BgrImg
            , IFormFile IdCardImgData, 
            
            IFormFile NpwpImgData, IFormFile ProfileVideo, string[] SelectedCategory)
        {
            try
            {
                int ProfFileId = 0;
                int BgrImgFileId = 0;
                int IdCardFileId = 0;
                int NpwpFileId = 0;
                int VideoProfileId = 0;
                int AccountNumberId = 0;
                if (ProfImg != null)
                {
                    var ProfFileIds = await FilesController.UploadPhotosFile(ProfImg);
                    ProfFileId = ProfFileIds.Id;
                    // ProfFileId = ProfFileIds

                    //   ProfFileId = Convert.ToInt32(ProfFileIds.ExecuteResultAsync);
                    //var llala = JsonConvert.DeserializeObject<FilesModel>(ProfFileIds);

                }

                if (AccountNumberImg != null)
                {
                    var AccountNumberIds = await FilesController.UploadPhotosFile(AccountNumberImg);
                    AccountNumberId = AccountNumberIds.Id;
                }

                if (BgrImg != null)
                {
                    var BgrImgFileIds = await FilesController.UploadPhotosFile(BgrImg);
                    BgrImgFileId = BgrImgFileIds.Id;
                }

                if (IdCardImgData != null)
                {
                    var IdCardFileIds = await FilesController.UploadPhotosFile(IdCardImgData);
                    IdCardFileId = IdCardFileIds.Id;
                }
                if (NpwpImgData != null)
                {
                    var NpwpImgFileIds = await FilesController.UploadPhotosFile(NpwpImgData);
                    NpwpFileId = NpwpImgFileIds.Id;
                }
                if (ProfileVideo != null)
                {
                    var ProfileVideoIds = await FilesController.UploadVideoFilesData1(ProfileVideo, model.Id);
                    VideoProfileId = ProfileVideoIds.Id;
                }

                var currentdata = IUserManagementManager.GetUser(model);
                currentdata.FirstName = model.FirstName;
                currentdata.LastName = model.LastName;
                currentdata.Bio = model.Bio;
                currentdata.PhoneNumber = model.PhoneNumber;
                currentdata.UpdatedBy = model.Id.ToString();
                currentdata.UserName = model.UserName;
                currentdata.AccountNumber = model.AccountNumber;
                currentdata.Bank = model.Bank;
                currentdata.BeneficiaryName = model.BeneficiaryName;
                //if (model.Password != null)
                //{
                //    currentdata.Password = Encryptor.Encrypt(model.Password);
                //}
                currentdata.Email = model.Email;
                if (ProfFileId != 0)
                {
                    currentdata.ImgProfFileId = ProfFileId;
                }

                if (BgrImgFileId != 0)
                {
                    currentdata.BgrFileId = BgrImgFileId;
                }
                IUserManagementManager.UpdateUser(currentdata);

                model.TalentId = IMasterManager.GetAllTalent().Where(m => m.UserId == Convert.ToInt32(model.Id)).Select(m => m.Id).FirstOrDefault();

                if (model.TalentId > 0)
                {
                    var talentmodel = new TalentModel();
                    talentmodel.Id = model.TalentId;
                    var gettalentdata = IMasterManager.GetTalent(talentmodel);

                    if (IdCardFileId != 0)
                    {
                        gettalentdata.IdCardFileId = IdCardFileId;
                    }
                    if (NpwpFileId != 0)
                    {
                        gettalentdata.NPWPFileId = NpwpFileId;
                    }
                    if (AccountNumberId != 0)
                    {
                        gettalentdata.AccountNumberFileId = AccountNumberId;
                    }

                    IMasterManager.UpdateTalent(gettalentdata);

                    if (gettalentdata != null)
                    {
                        //gettalentdata.Profesion = model.Profesi;
                        IMasterManager.UpdateTalent(gettalentdata);
                        //IMasterManager.DeleteTalentCategoryById(gettalentdata.Id);

                        if (VideoProfileId != 0)
                        {
                            TalentVideoModel videomodel = new TalentVideoModel();
                            TalentVideoModel oldTalentVideo = new TalentVideoModel();

                            oldTalentVideo = ITransactionManager.GetTalentVideos(gettalentdata.Id).Where(cond => cond.BookCategory == 0).FirstOrDefault();

                            videomodel.FileId = VideoProfileId;
                            videomodel.TalentId = gettalentdata.Id;
                            videomodel.VideoNm = "Profile Video";
                            videomodel.BookCategory = 0;
                            if (oldTalentVideo != null)
                            { ITransactionManager.DeleteTalentVideo(oldTalentVideo.Id); }
                            ITransactionManager.CreateTalentVideo(videomodel);
                        }

                        //foreach (var i in SelectedCategory)
                        //{
                        //    var categoryModel = new TalentCategoryViewModel();
                        //    categoryModel.TalentId = gettalentdata.Id;
                        //    categoryModel.CategoryId = int.Parse(i);
                        //    var temp = IMasterManager.CreateTalentCategory(categoryModel);
                        //}


                    };
                }

                // var getCurrentData = IUserManagementManager.GetUser(model);

                //return Json("OK");
                //if (model.TalentId > 0)
                //{
                //    var ProfileVideoIdss = await FilesController.UploadVideoFilesData1(ProfileVideo, model.Id);
                //    return Json(new { getCurrentData, ProfileVideo = ProfileVideoIdss.Link, Status = "OK" });
                //}
                //else
                //{
                //    return Json(new { getCurrentData, Status = "OK" });
                //}

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.Id), "EditProfile", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
                //return Json("Error");
                throw ex;
            }


        }






        //[HttpGet]
        //[Route("~/api/Account/GetOnboarding")]
        //public IActionResult Onboarding()
        //{
        //    var GetUserId = HelperController.GetCookie("UserId");
        //    var GetUserName = HelperController.GetCookie("UserName");
        //    UserModel model = new UserModel();
        //    //ar payload = Request.Form["payload"];
        //    //var device = await _context.Devices.SingleOrDefaultAsync(m => m.Id == id);

        //    //string vapidPublicKey = config.GetSection("VapidKeys")["PublicKey"];
        //    //string vapidPrivateKey = config.GetSection("VapidKeys")["PrivateKey"];

        //    //var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);
        //    //var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

        //    //var webPushClient = new WebPushClient();
        //    //webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
        //    try
        //    {
        //        if (GetUserId != "")
        //        {
        //            var checkUserData = HelperController.ConfigUserDataToCookies(GetUserId);
        //            var GetRolesId = HelperController.GetCookie("Role_ID");
        //            if (checkUserData == true)
        //            {
        //                if (GetRolesId == "1")
        //                {
        //                    return RedirectToAction("Dashboard", "AdmHome");
        //                }
        //                else
        //                {
        //                    return RedirectToAction("welcome", "Home");
        //                }

        //            }
        //            else
        //            {
        //                //return View(model);
        //                return new JsonResult(model)
        //                {
        //                    StatusCode = StatusCodes.Status200OK
        //                };
        //            }

        //        }
        //        else
        //        {
        //            //return View(model);
        //            return new JsonResult(model)
        //            {
        //                StatusCode = StatusCodes.Status200OK
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //HelperController.InsertLog(0, "Onboarding", ex.Message);

        //        //return View(model);
        //        //throw ex;
        //        return new JsonResult(model)
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest
        //        };
        //    }
        //}


        //[HttpPut]
        //[Route("~/api/Account/VerifyCode")]
        //public IActionResult VerifyCode([FromBody] UserModel data/*string code, string UserId*/)
        //{
        //    try
        //    {
        //        //int _userid = Convert.ToInt32(HelperController.GetCookie("UserId"));
        //        UserModel model = new UserModel();
        //        model.Id = Convert.ToInt32(data.Id);
        //        model.VerificationCode = data.VerificationCode;
        //        var IsValid = IUserManagementManager.VerifyCodeUser(model);

        //        if (IsValid == true)
        //        {
        //            var getUserData = IUserManagementManager.GetUser(model);
        //            getUserData.IsActive = 1;
        //            getUserData.IsActiveCode = false;
        //            getUserData.IsVerified = "1";
        //            IUserManagementManager.UpdateUser(getUserData);
        //            string FirstName = getUserData.FirstName != null ? getUserData.FirstName : "";
        //            string LastName = getUserData.LastName != null ? getUserData.LastName : "";
        //            string UserName = FirstName + " " + LastName;

        //            HelperController.SetCookie("UserId", getUserData != null ? getUserData.Id.ToString() : "");
        //            HelperController.SetCookie("Role_ID", getUserData != null ? getUserData.RoleId.ToString() : "");
        //            HelperController.SetCookie("UserName", UserName);

        //            HelperController.EmailNewUserSignUp(getUserData.Email).Wait();
        //        }


        //        return Json(IsValid);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "VerifyCode", ex.Message);

        //        return Json("Error");
        //        throw ex;
        //    }
        //}

        [HttpPut]
        [Route("~/api/Account/VerifyCodeSignUp")]
        public IActionResult VerifyCodeSignUp([FromBody] UserModel data/*string code, string UserId*/)
        {
            var IsValid = false;
            try
            {
                //int _userid = Convert.ToInt32(HelperController.GetCookie("UserId"));
                UserModel model = new UserModel();
                model.Id = Convert.ToInt32(data.Id);
                model.VerificationCode = data.VerificationCode;
                 IsValid = IUserManagementManager.VerifyCodeUser(model);

                if (IsValid == true)
                {
                    var getUserData = IUserManagementManager.GetUser(model);
                    getUserData.IsActive = 1;
                    getUserData.IsActiveCode = false;
                    getUserData.IsVerified = "1";
                    IUserManagementManager.UpdateUser(getUserData);
                    string FirstName = getUserData.FirstName != null ? getUserData.FirstName : "";
                    string LastName = getUserData.LastName != null ? getUserData.LastName : "";
                    string UserName = FirstName + " " + LastName;

                    //HelperController.SetCookie("UserId", getUserData != null ? getUserData.Id.ToString() : "");
                    //HelperController.SetCookie("Role_ID", getUserData != null ? getUserData.RoleId.ToString() : "");
                    //HelperController.SetCookie("UserName", UserName);

                    HelperController.EmailNewUserSignUp(getUserData.Email).Wait();
                }


                //return Json(IsValid);
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = IsValid });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(data.Id), "VerifyCode", ex.Message);

                // return Json("Error");
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = IsValid });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Account/PostClaim")]
        public IActionResult PostClaim([FromBody]ProfileViewModel model)
        {
            ClaimModel ClaimModel = new ClaimModel();
            try
            {
                
                //model.Id = id;
                //model = IUserManagementManager.GetUser(model);  
                //UserModel UserModel = new UserModel();
                //UserModel = IUserManagementManager.GetUser(model.UserModel);
                var periodData = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault();
                var _bank = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.Bank).FirstOrDefault();
                var _AccountNumber = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.AccountNumber).FirstOrDefault();
                ClaimModel.Period = periodData != null ? periodData.ParamValue : "";
                ClaimModel.Status = (int)Registration.Submit;
                ClaimModel.UserId = model.UserId;
                ClaimModel.AccountNumber = _AccountNumber;
                ClaimModel.BankName = _bank;
                ClaimModel.Amount = model.NominalTransfer;
                ClaimModel.Message = model.ClaimMessage;
                ClaimModel.CreatedBy = Convert.ToString(model.UserId);

                if (model.NominalTransfer < 100000)
                {
                    //return Json(new { Status = "Error : Minimum penarikan adalah Rp. 100.000" });
                    // return Json(new { Status = "0" });

                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Error : Minimum penarikan adalah Rp. 100.000", result = ClaimModel });
                }

                else
                {

                    var Talent = IMasterManager.GetTalentProfiles(model.UserId);


                    if (model.NominalTransfer.Value > Talent.Income)
                    {
                        //return Json(new { Status = "Error : Jumlah penarikanmu melebihi dari saldo" });
                        //return Json(new { Status = "1" });

                        return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Error : Jumlah penarikanmu melebihi dari saldo", result = ClaimModel });
                    }
                    else
                    {
                        ITransactionManager.CreateClaim(ClaimModel);
                    }

                    var getParam = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "NotifClaim" && x.ParamCode == Convert.ToInt32(Registration.Submit).ToString()).FirstOrDefault();
                    var message = getParam != null ? getParam.ParamValue : "";
                    var JinglUrl = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Url" && x.ParamName == "Jingl").FirstOrDefault().ParamValue;

                    var getClaim = ITransactionManager.GetAllClaim().Where(x => x.UserId == model.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                    ClaimModel.ClmNumber = getClaim != null ? getClaim.ClmNumber : "";
                    ClaimModel.Id = getClaim != null ? getClaim.Id : 0;

                    NotificationModel NmModel = new NotificationModel();
                    NmModel.Message = ClaimModel.ClmNumber + " " + message;
                    NmModel.CreatedBy = model.UserId.ToString();
                    NmModel.To = model.UserId;
                    NmModel.Link = JinglUrl + "\\Claim\\Detail?ClaimId=" + ClaimModel.Id.ToString();
                    NmModel.NotifCategory = "Claim";
                    NmModel.IsReaded = 0;
                    NmModel.NotifType = "T";
                    NmModel.IsActive = true;
                    ITransactionManager.InsertNotification(NmModel);

                    HelperController.EmailClaimRequest(model.UserId.ToString(), (int)Registration.Submit, ClaimModel.ClmNumber, model.NominalTransfer.Value).Wait();
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK" ,result = ClaimModel });
                }


            

               // return Json(new { ClaimModel, Status = "2" });

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "PostClaim", ex.Message);
                // return Json(model);

                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = ClaimModel });
                throw ex;
            }
        }


        //[HttpPost]
        //[Route("~/api/Account/RequestRefund")]
        //public IActionResult RequestRefund([FromBody] BookModel bmodel)
        //{
        //    try
        //    {

        //        var getBookData = ITransactionManager.GetDataBookByOrderId(bmodel.OrderNo);

        //        if (getBookData != null)
        //        {
        //            if (getBookData.Status != Convert.ToInt32(BookingFlow.Refund))
        //            {
        //                if (getBookData.BookedBy == bmodel.UserId)
        //                {
        //                    var refundData = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == bmodel.OrderNo).FirstOrDefault();


        //                    if (refundData != null)
        //                    {
        //                        RefundModel model = new RefundModel();
        //                        return Json(new { result = model, Status = "Error", Message = "Order sudah di Refund sebelumnya" });

        //                    }
        //                    else
        //                    {
        //                        RefundModel model = new RefundModel();
        //                        UserModel userModel = new UserModel();
        //                        userModel.Id = getBookData.BookedBy.Value;
        //                        var getUser = IUserManagementManager.GetUser(userModel);

        //                        model.OrderNo = model.OrderNo;
        //                        model.UserCode = getUser.UserCode;
        //                        model.CreatedBy = getBookData.CreatedBy;
        //                        model.Amount = getBookData.TotalPay.Value;
        //                        model.BookId = Convert.ToInt32(getBookData.Id);
        //                        model.AccountNumber = getUser.AccountNumber;
        //                        model.CustomerName = getUser.UserName;
        //                        model.BeneficiaryName = getUser.BeneficiaryName;
        //                        model.BankName = getUser.Bank;
        //                        model.UserId = getUser.Id.ToString();


        //                        return Json(new { result = model, Status = "OK", Message = "OK" });
        //                    }
        //                }
        //                else
        //                {
        //                    RefundModel model = new RefundModel();
        //                    return Json(new { result = model, Status = "Error", Message = "User Id tidak memiliki Akses" });
        //                }
        //            }
        //            else
        //            {
        //                RefundModel model = new RefundModel();
        //                return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
        //            }



        //        }
        //        else
        //        {
        //            RefundModel model = new RefundModel();
        //            return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
        //        }





        //    }
        //    catch (Exception ex)
        //    {
        //        HelperController.InsertLog(Convert.ToInt32(bmodel.UserId), "RequestRefund", ex.Message);
        //        RefundModel model = new RefundModel();
        //        return Json(new { result = model, Status = "Error", Message = ex.Message });
        //        // return RedirectToAction("NotFound", "NoAccess");
        //    }

        //}

        //[HttpPost]
        //[Route("~/api/Account/PostRequestRefund")]
        //public IActionResult PostRequestRefund([FromBody]RefundModel rmodel)
        //{
        //    try
        //    {

        //        var getBookData = ITransactionManager.GetDataBookByOrderId(rmodel.OrderNo);

        //        if (getBookData != null)
        //        {
        //            if (getBookData.Status != Convert.ToInt32(BookingFlow.Refund))
        //            {
        //                if (getBookData.BookedBy.Value.ToString() == rmodel.UserId)
        //                {
        //                    var refundData = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == rmodel.OrderNo).FirstOrDefault();


        //                    if (refundData != null)
        //                    {
        //                        BookModel model = new BookModel();
        //                        return Json(new { result = model, Status = "Error", Message = "Order sudah di Refund sebelumnya" });

        //                    }
        //                    else
        //                    {
        //                        rmodel.Status = (int)Registration.Submit;
        //                        rmodel.CreatedBy = rmodel.UserId;
        //                        ITransactionManager.CreateRefund(rmodel);
        //                        UserModel userModel = new UserModel();
        //                        userModel.Id = Convert.ToInt32(rmodel.UserId);
        //                        var getUser = IUserManagementManager.GetUser(userModel);
        //                        getUser.AccountNumber = rmodel.AccountNumber;
        //                        getUser.Bank = rmodel.BankName;
        //                        getUser.BeneficiaryName = rmodel.BeneficiaryName;
        //                        IUserManagementManager.UpdateUser(getUser);

        //                        BookModel BModel = new BookModel();

        //                        var temp = ITransactionManager.GetDataBookByOrderId(rmodel.OrderNo);
        //                        var getBookingData = ITransactionManager.GetDataBook(temp);

        //                        if (getBookingData != null)
        //                        {
        //                            getBookingData.Status = (int)BookingFlow.RefundSubmitted;
        //                            ITransactionManager.UpdateBookData(getBookingData);

        //                            HelperController.NotificationEmail(getBookingData.Email, getBookingData.Status.ToString(), EmailTargetType.User, getBookingData.Id, getBookingData.OrderNo, getUser.FirstName).Wait();

        //                        }
        //                        return Json(new { result = getBookingData, Status = "OK", Message = "OK" });

        //                        //  return Json(new { result = model, Status = "OK",Message = "OK" });
        //                    }
        //                }
        //                else
        //                {
        //                    BookModel model = new BookModel();
        //                    return Json(new { result = model, Status = "Error", Message = "User Id tidak memiliki Akses" });
        //                }
        //            }
        //            else
        //            {
        //                BookModel model = new BookModel();
        //                return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
        //            }
        //        }
        //        else
        //        {
        //            BookModel bmodel = new BookModel();
        //            return Json(new { result = bmodel, Status = "Error", Message = "Order tidak valid" });
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        BookModel bm = new BookModel();
        //        HelperController.InsertLog(Convert.ToInt32(rmodel.UserId), "Profile", ex.Message);
        //        return Json(new { result = bm, Status = "Error", Message = "Error" });
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        //[Route("~/api/Account/Setting")]
        //public IActionResult Setting([FromBody]ProfileViewModel model)
        //{
        //    //var model = new ProfileViewModel();

        //    try
        //    {
        //        int UserId = model.UserId;
        //        model.AppVersion = IMasterManager.GetParameterByCode("AppVersion").ParamValue;
        //        model.UserModel = IUserManagementManager.GetUserProfiles(UserId);
        //        model.TalentModel = IMasterManager.GetTalentProfiles(UserId);
        //        var TalentData = IMasterManager.GetTalentProfiles(UserId);
        //        IList<TalentVideoModel> TalentVideoList = new List<TalentVideoModel>();
        //        IList<TalentVideoModel> UserVideoList = new List<TalentVideoModel>();
        //        if (TalentData != null)
        //        {
        //            TalentVideoList = ITransactionManager.GetTalentVideos(TalentData.Id);

        //        }
        //        UserVideoList = ITransactionManager.GetUserVideos(Convert.ToInt32(UserId));
        //        model.UserVideoList = UserVideoList;
        //        model.TalentVideoList = TalentVideoList;
        //        //model.TalentModel.TalentBookList = ITransactionManager.

        //    }
        //    catch (Exception ex)
        //    {

        //        HelperController.InsertLog((model.UserId), "Profile", ex.Message);
        //        throw ex;
        //    }
        //    return new JsonResult(model)
        //    {
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //}

        [HttpPost]
        [Route("~/api/Account/ForgotPassword")]
        public IActionResult ForgotPassword([FromBody]RequestChangeModel model)
        {
            try
            {
                RequestChangeModel rcm = new RequestChangeModel();
                rcm.RequestCode = model.RequestCode;
                var getrequest = ITransactionManager.GetRequestChange(rcm);

                if (getrequest != null)
                {
                    UserModel Usm = new UserModel();
                    Usm.Id = getrequest.UserId.Value;

                    var getdata = IUserManagementManager.GetUser(Usm);
                    getdata.Password = Encryptor.Encrypt(model.Password);
                    IUserManagementManager.UpdateUser(getdata);
                    //return Json(new { Result = getdata, Status = "OK", Message = "Password Berhasil diubah" });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = getdata });
                }
                else
                {
                    RequestChangeModel rcmodels = new RequestChangeModel();
                    return Json(new { result = rcmodels, Status = StatusCodes.Status400BadRequest, Message = "Request Tidak Ditemukan" });
                }


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(model.Id), "ForgotPassword", ex.Message);
                RequestChangeModel rcmodels = new RequestChangeModel();
                return Json(new { result = rcmodels, Status = StatusCodes.Status400BadRequest, Message = "Password Tidak Berhasil di Ubah" });
                //return RedirectToAction("NotFound", "NoAccess");
            }
        }

        [HttpGet]
        [Route("~/api/Account/WishlistList")]
        public IActionResult WishlistList(string UserId )
        {
            try
            {
                //if (UserId == 0)
                //{
                //    UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                //}

                WishlistViewModel wishlistView = new WishlistViewModel();
                List<TalentCategoryViewModel> listWishlist = new List<TalentCategoryViewModel>();
                List<TalentVideoModel> videos = ITransactionManager.GetAllVideo().ToList();

                listWishlist = ITransactionManager.GetWishListByUserId(Convert.ToInt32(UserId)).ToList();
                foreach (var z in listWishlist)
                {
                    z.CategoryNm = HelperController.CategoryTalentData(z.TalentId);
                }

                wishlistView.ListTalent = listWishlist;
                wishlistView.ListVideo = videos;

                //return new JsonResult(listWishlist)
                //{
                //    StatusCode = StatusCodes.Status200OK
                //};

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result = listWishlist });

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(UserId), "WishlistList", ex.Message);
                return Json( new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
            }

        }

        //[HttpGet]
        //[Route("~/api/Account/GetAllUser")]
        //public IActionResult GetAllUser(int Id)
        //{
        //    var result = IUserManagementManager.GetAllUser().Where(m => m.RoleId == 2).ToList();

        //    if (Id > 0)
        //    {
        //        result = IUserManagementManager.GetAllUser().Where(m => m.RoleId == 2 && m.Id == Id).ToList();
        //    }
        //    else
        //    {
        //        result = IUserManagementManager.GetAllUser().Where(m => m.RoleId == 2).ToList();
        //    }

        //    return new JsonResult(result)
        //    {
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //}


        [HttpPost]
        [Route("~/api/Account/PreSignUpPost")]
        //[EnableCors("AllowOrigin")]
        public IActionResult PreSignUpPost([FromBody]UserModel model)
        {
            try
            {
                UserModel data = new UserModel();
                Random random = new Random();

                data.Email = model.Email;
                data.PhoneNumber = model.PhoneNumber;
                data.Password = Encryptor.Encrypt(model.Password);
                data.IsActive = 1;
                data.UserName = "USERDEF" + random.Next(0, 999999);
                data.RoleId = (int)Role.User;
                data.IsVerified = "0";
                if (!string.IsNullOrEmpty(model.Email))
                {
                    var createData = IUserManagementManager.CreateUser(data);
                    IUserManagementManager.SetVerificationCode(createData);
                    var getdata = IUserManagementManager.GetUser(createData);

                    //HelperController.SetCookie("UserId", data != null ? data.Id.ToString() : "");
                    HelperController.SendVerificationTokenAfterSignupData(getdata.Id, getdata.VerificationCode, getdata.Email, getdata.FirstName).Wait();
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getdata });
                    //return new JsonResult(getdata)
                    //{
                    //    StatusCode = StatusCodes.Status200OK
                    //};
                }
                else if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    var createData = IUserManagementManager.CreateUser(data);
                    IUserManagementManager.SetVerificationCode(createData);
                    var getdata = IUserManagementManager.GetUser(createData);

                    string FirstName = getdata.FirstName != null ? getdata.FirstName : "";
                    string LastName = getdata.LastName != null ? getdata.LastName : "";
                    string UserName = FirstName + " " + LastName;
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getdata });


                    //return new JsonResult(getdata)
                    //{
                    //    StatusCode = StatusCodes.Status200OK
                    //};
                }
                else
                {
                    UserModel userModel = new UserModel();
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Error", result = userModel });
                }


            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.Id), "PreSignUpPost", ex.Message);

                UserModel userModel = new UserModel();
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = userModel });
                throw ex;
            }
        }




    }
}