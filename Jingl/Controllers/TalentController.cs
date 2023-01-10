using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TalentController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
   

        public TalentController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            return View();
        }
      

        public ActionResult RegisterTalent(string UserId)
        {
            var model = new TalentRegModel();
            try
            {
                var UserData = HelperController.GetUserData();
                model.UserId = Convert.ToInt32(UserId);
                if(UserData != null)
                {
                    model.TalentNm = UserData.Name;
                    model.Email = UserData.Email;
                }
                var categorymodel = IMasterManager.GetCategoryByType("Talent");
                List<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
                foreach (var i in categorymodel)
                {
                    TalentCategoryViewModel data = new TalentCategoryViewModel();
                    data.CategoryId = i.Id;
                    data.CategoryNm = i.CategoryNm;
                    ListCategory.Add(data);
                }
                model.TalentCategory = ListCategory;

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(UserId), "NoPostRegisterTalent", ex.Message);
                throw ex;
            }

            return View(model);
        }

        
        public ActionResult Video(string FileId,string TalentId)
        {
            var model = new TalentVideoModel();
            try
            {
                int? TempCategoryId = null;
                var VideoList = ITransactionManager.GetAllVideoByCategory(TempCategoryId);
                if(VideoList != null)
                {
                    model = VideoList.Where(x => x.FileId == Convert.ToInt32(FileId)).FirstOrDefault();
                    if(model != null)
                    {
                        model.TalentNm = model.TalentNm;
                        model.ProjectNm = model.ProjectNm;
                    }
                    else
                    {
                        return RedirectToAction("NotFound","NoAccess");
                    }
                    
                }


                //var talentmodel = new TalentModel();
                //talentmodel.Id = Convert.ToInt32(TalentId);
                //var talent = IMasterManager.GetTalent(talentmodel);
                
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Video", ex.Message);
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterTalent(TalentRegModel model,string[] TalentCategory)
        {
            try
            {
                //TalentRegModel Tmodel = new TalentRegModel();
                //Tmodel.UserId = model.UserId;
                //Tmode

                model.CreatedBy = model.UserId.ToString();
                foreach (var i in TalentCategory)
                {
                    model.CategoryId = int.Parse(i);
                }
                var data=  ITransactionManager.CreateTalentRegistration(model);
              
                //var gettalentdata = IMasterManager.CreateTalent(model);

                //if (data != null)
                //{
                //    IMasterManager.DeleteTalentCategoryById(data.id);
                //    foreach (var i in TalentCategory)
                //    {
                //        var categoryModel = new TalentCategoryViewModel();
                //        categoryModel.TalentId = data.id;
                //        categoryModel.CategoryId = int.Parse(i);
                //        var temp = IMasterManager.CreateTalentCategory(categoryModel);
                //    }
                //}

                UserModel UserModel = new UserModel();
                UserModel.Id = model.UserId.Value;
                UserModel = IUserManagementManager.GetUser(UserModel);
                var getlastdata = ITransactionManager.GetAllTalentRegistration().LastOrDefault();

                HelperController.EmailApprovalTalent(UserModel.Email,"1",model.TalentNm, getlastdata.RegNum,"").Wait();

                return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "RegisterTalent", ex.Message);

                return Json("Error");
               
            }
        }

        public IActionResult UpdateLevel()
        {
            return View();

        }

        public IActionResult BestTalent()
        {
            var data = new List<TalentModel>();
            try
            {
                data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x=>x.CompletedBook).Take(5).ToList();
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "BestTalent", ex.Message);
                throw ex;
            }

            return View(data);
        }

        public IActionResult GetBestTalentData()
        {
            var data = new List<TalentModel>();
            var model = new DashboardViewModel();
            try
            {
                var itemdata = new List<Itemdata>();

                data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x => x.CompletedBook).Take(5).ToList();
                foreach (var i in data)
                {
                    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                }

                model.items = itemdata.ToList();

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetBestTalentData", ex.Message);
                throw ex;
            }

            return Json(model);
        }



        public IActionResult OurTalent(string CategoryId)
        {
            var data = new TalentViewModel();
            ViewBag.CategoryId = 0;
            try
            {
                data = IMasterManager.GetAllTalentCategory();
                data.ListTalentViewModel = IMasterManager.GetAllTalentByCategory(Convert.ToInt32(CategoryId)).ToList();
                if (!String.IsNullOrEmpty(CategoryId)) ViewBag.CategoryId = CategoryId;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "OurTalent", ex.Message);
                throw ex;
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult OurTalentcontent(string CategoryId)
        {
            
            var data = new TalentViewModel();
            try
            {
                data = IMasterManager.GetAllTalentCategory();
                var Ourtalent = IMasterManager.GetAllTalentByCategory(Convert.ToInt32(CategoryId)).OrderByDescending(x => x.IsAvailable).ToList();
                data.ListTalentViewModel = Ourtalent;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "OurTalentContent", ex.Message);
                throw ex;
            }

            return PartialView("~/Views/Talent/OurTalentcontent.cshtml", data); ;

            //int? temp = null;
            //if (CategoryId != null)
            //{
            //    temp = Convert.ToInt32(CategoryId);
            //}
            //TalentViewModel model = new TalentViewModel();
            //model = IMasterManager.GetAllTalentCategory();
            //var Ourtalent = IMasterManager.GetAllTalentByCategory(temp).ToList();
            //model.ListTalentViewModel = Ourtalent;
            //return PartialView("~/Views/Explore/ExploreContent.cshtml", model);
        }

        [HttpGet("{VideoId}")]
        [Route("~/api/Talent/TalentDetailVideo")]
        public IActionResult TalentDetailVideo(int VideoId)
        {
            #region validasi get booking first

            //int max_booking_general = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault() != null ? Convert.ToInt32(IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault().ParamValue) : 0;
            //int max_booking_underagency = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault() != null ? Convert.ToInt32(IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "UnderAgency" && x.ParamName == "MaxBooking").FirstOrDefault().ParamValue) : 0;

            //var statusbooking = ITransactionManager.GetAllBook().Where(a => (a.Status == (int)BookingFlow.Paid ||
            //a.Status == (int)BookingFlow.ProjectAccepted ||
            //a.Status == (int)BookingFlow.RecordingProcess) && a.TalentId == Convert.ToInt32(TalentId)).ToList();

            #endregion
            //var question = new QuestionModel();
            //IList<BookModel> data = new List<BookModel>();
            //IList<LandingModel> data = new List<LandingModel>();
            var data = new LandingModel();
            //var wishlist = new WishlistModel();
            //var questionVideoModel = new TalentVideoModel();

            try
            {
                //data.Id = Convert.ToInt32(VideoId);
                //data = IMasterManager.GetTalent(data);
                //data.ListTalentVideo = ITransactionManager.GetTalentVideosPertanyaan(VideoId).ToList();
                //data = IMasterManager.GetTalent(data);
                data = ITransactionManager.GetBookingByVideoId(VideoId);
                //data.ReactionVideoList = ITransactionManager.GetReactionByTalentId(data.Id);
                //data.QuestionList = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == data.Id).ToList();
                //data.TalentCategory = IMasterManager.GetTalentCategoryData(data.Id);
                //data.CategoryName = HelperController.CategoryTalentData(data.Id);
                //data.ListPostCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                //data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                //data.ListSubReplyCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                //data.BookingCount = statusbooking.Count();
                //#region child all question video inside TalentVideoModel


                //var nn = from a in ITransactionManager.GetTalentVideosPertanyaan(data.Id)
                //         join b in ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentId == data.Id)
                //         on a.FileId equals b.FileId
                //         select new { b };

                //var ListCategory = new TalentVideoModel();

                //var list = ListCategory.QuestionVideoList;

                //List<QuestionVideoModel> _ListCategory = new List<QuestionVideoModel>();


                //ViewBag.LinkType = "video";
               // var getvintro = ITransactionManager.GetTalentVideosPertanyaan(data.Id).Take(1).OrderByDescending(m => m.FileId).FirstOrDefault();
                //data.VideoIntro = getvintro != null ? getvintro.Link.ToString() : "";
                //data.VideoIntroId = getvintro != null ? getvintro.FileId.ToString() : "";
                //data.VideoIntroTotalLikes = getvintro != null ? getvintro.TotalLikes : 0;

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(VideoId), "TalentDetailPertanyaan", ex.Message);
                throw ex;
            }

            var result = new JsonResult(data);
            result.StatusCode = StatusCodes.Status200OK;

            //return Json(result);
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("{TalentId}")]
        [Route("~/api/Talent/TalentDetail")]
        public IActionResult TalentDetail(string TalentId)
        {
            #region validasi get booking first

            int max_booking_general = IMasterManager.AdmGetAllParameter().Where(x=>x.ParamCode=="General" && x.ParamName == "MaxBooking").FirstOrDefault() != null? Convert.ToInt32(IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault().ParamValue):0;
            int max_booking_underagency = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault() != null ? Convert.ToInt32(IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "UnderAgency" && x.ParamName == "MaxBooking").FirstOrDefault().ParamValue) : 0; 
      

            //cek total jumlah status (2,3,4)
            var statusbooking = ITransactionManager.GetAllBook().Where(a => (a.Status == (int)BookingFlow.Paid ||
            a.Status == (int)BookingFlow.ProjectAccepted ||
            a.Status == (int)BookingFlow.RecordingProcess) && a.TalentId == Convert.ToInt32(TalentId)).ToList();

            //cek kalau talent undergency untuk status limitasi order
            //var underagency = IMasterManager.GetAllTalent().Where(a => a.Id == Convert.ToInt32(TalentId) && a.IsUnderAgency ==  true).FirstOrDefault();

            #endregion
            var question = new QuestionModel();
            var data = new TalentModel();
            var wishlist = new WishlistModel();
            var questionVideoModel = new TalentVideoModel();
            
            try
            {
                
                data.Id = Convert.ToInt32(TalentId);
                data = IMasterManager.GetTalent(data);
                data.ListTalentVideo = ITransactionManager.GetTalentVideos(data.Id).Where(m => m.BookCategory != 0).ToList();
                data.TalentBookList = ITransactionManager.GetBookingByTalentId(data.Id);
                data.ReactionVideoList = ITransactionManager.GetReactionByTalentId(data.Id);
                data.QuestionList = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == data.Id).ToList();
                data.TalentCategory = IMasterManager.GetTalentCategoryData(data.Id);
                data.CategoryName = HelperController.CategoryTalentData(data.Id);
                data.ListPostCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                data.ListSubReplyCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == data.Id).ToList();
                data.BookingCount = statusbooking.Count();
                #region child all question video inside TalentVideoModel


                var nn = from a in ITransactionManager.GetTalentVideos(data.Id)
                         join b in ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentId == data.Id)
                         on a.FileId equals b.FileId
                         select new { b };

                var ListCategory = new TalentVideoModel();

                var list = ListCategory.QuestionVideoList;

                List<QuestionVideoModel> _ListCategory = new List<QuestionVideoModel>();

                foreach (var i in nn)
                {

                    var _data = new QuestionVideoModel();
                  _data.QuestionVideoId    = i.b.QuestionVideoId;
                  _data.UserId             = i.b.UserId;
                  _data.FileId             = i.b.FileId;
                  _data.TalentId           = i.b.TalentId;
                  _data.Question           = i.b.Question;
                  _data.IsActive           = i.b.IsActive;
                  _data.IsAnswered         = i.b.IsAnswered;
                  _data.UserProfPicLink    = i.b.UserProfPicLink;
                  _data.TalentProfPicLink  = i.b.TalentProfPicLink;
                  _data.UserName           = i.b.UserName;
                  _data.TalentName         = i.b.TalentName;
                  _data.Answer             = i.b.Answer;
                  _data.QuestionDate       = i.b.QuestionDate;
                  _data.AnswerDate         = i.b.AnswerDate;
                  _data.LinkVideo          = i.b.LinkVideo;
                   _data.Thumbnails = i.b.Thumbnails;
                  
                _ListCategory.Add(_data);
                }

                list = _ListCategory;

                data.ListQuestionVideo = _ListCategory;

                #endregion
                ViewBag.Email = "";
                ViewBag.Code = "";
                ViewBag.FirstName = "";
                ViewBag.PhoneNo = "";
                ViewBag.IsVerified = "";
                ViewBag.OverBooking = false;
                data.IsTalentAcceptOrder = true;

                //wishlist.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
              //  wishlist.UserId = data.UserId;
             //   wishlist.TalentId = data.Id;

               // data.WishlistId = ITransactionManager.GetWishlistIdByUserTalent(wishlist);

                //if (status2 >= 10 || status3 >= 10 || status4 >= 10)
                //{
                //    ViewBag.OverBooking = true;
                //}


                //if (Convert.ToInt32(TalentId)== 155)
                //{
                //    if (statusbooking.Count() >= max_booking_marion)
                //    {
                //        //ViewBag.OverBooking = true;
                //        //return Json(new { Status = 0 , Message = "OverBooking For Marion Zola when maximum 5 booking" });
                //    }
                //}

                if (data.IsUnderAgency == true)
                {
                    if (statusbooking.Count() >= max_booking_general)
                    {
                        //ViewBag.OverBooking = true;
                        //  return Json(new { Status = 0, Message = "OverBooking general when maximum 10 booking" });
                        data.IsTalentAcceptOrder = false;
                    }
                }

                if (data.IsUnderAgency == false)
                {
                    if (statusbooking.Count() >= max_booking_underagency)
                    {
                        //ViewBag.OverBooking = true;
                        //  return Json(new { Status = 0, Message = "OverBooking underagency when maximum 10 booking" });
                        data.IsTalentAcceptOrder = false;
                    }
                }

                if(data.IsAvailable == false)
                {
                    data.IsTalentAcceptOrder = false;   
                }



                //if (Convert.ToInt32(HelperController.GetCookie("UserId")) > 0)
                if (data.UserId > 0)
                {
                    UserModel user = new UserModel();
                    //user.Id = Convert.ToInt32(HelperController.GetCookie("UserId"));
                    user.Id = Convert.ToInt32(data.UserId);
                    user = IUserManagementManager.GetUser(user);
                    data.Booker = user;
                    ViewBag.Email = user.Email;
                    ViewBag.Code = user.VerificationCode;
                    ViewBag.FirstName = user.FirstName;
                    ViewBag.IsVerified = user.IsVerified;
                    ViewBag.PhoneNo = user.PhoneNumber;
                }

                ViewBag.LinkType = "video";
                var getvintro = ITransactionManager.GetTalentVideos(data.Id).Where(x => x.BookCategory == 0).Take(1).OrderByDescending(m => m.FileId).FirstOrDefault();
                data.VideoIntro = getvintro != null ? getvintro.Link.ToString() : "";
                data.VideoIntroId = getvintro != null ? getvintro.FileId.ToString():"";
                data.VideoIntroTotalLikes = getvintro != null ? getvintro.TotalLikes : 0;
                //if (data.ListTalentVideo.Count() > 0)
                //{
                //    var getlink = ITransactionManager.GetTalentVideos(data.Id).Where(x => x.BookCategory == 0).Take(1).OrderByDescending(m => m.FileId).FirstOrDefault();
                //    ViewBag.Link = getlink != null ? getlink.Link : "";
                //    ViewBag.ThumbLink = getlink != null ? getlink.Thumbnails:"";
                //    //data.VideoIntro = getlink.Link;
                //}
                //else
                //{
                //    ViewBag.LinkType = "photos";
                //    var getUserData = data.LinkImg;
                //    ViewBag.Link = data != null ? data.LinkImg: "";
                //}

                return Json(new { Status = "OK", Message = "OK", result = data });

            }
            catch (Exception ex)
            {
                TalentModel tm = new TalentModel();
                //HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                HelperController.InsertLog(Convert.ToInt32(0), "TalentDetail", ex.Message);
                return Json(new { Status = "Error", Message = ex.Message, result = tm });
                throw ex;
            }
            //var result = data;

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        public IActionResult TalentDetailOld(string TalentId)
        {
            var data = new TalentModel();
            try
            {
                data.Id = Convert.ToInt32(TalentId);
                data = IMasterManager.GetTalent(data);
                data.ListTalentVideo = ITransactionManager.GetTalentVideos(data.Id);
               
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                throw ex;
            }
            return View(data);
        }

        public IActionResult TalentVideo(string TalentId)
        {
            IList<TalentVideoModel> VideoList = new List<TalentVideoModel>();
            try
            {

                VideoList = ITransactionManager.GetTalentVideos(Convert.ToInt32(TalentId));
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentVideo", ex.Message);
                throw ex;
            }

            return View(VideoList);
        }

        public IActionResult TalentWorkspace(string TalentId, int? Status)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                var Id = Convert.ToInt32(TalentId);
                if (Status != null)
                {
                    if (Status > 3 && Status < 5)
                    {
                        data = ITransactionManager.GetBookingByTalentId(Id).Where(book => book.Status < 5 && book.Status > 1).ToList();
                    }
                    else
                    {
                        data = ITransactionManager.GetBookingByTalentId(Id).Where(book => book.Status == Status && book.Status > 1).ToList();
                    }
                }
                else
                {
                    data = ITransactionManager.GetBookingByTalentId(Id);
                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentWorkspace", ex.Message);
                throw ex;
            }
            return View(data);
        }

        public IActionResult Level()
        {
            return View();
        }


        [HttpGet]
        [Route("~/api/Talent/ListTalentVideo")]
        public IActionResult ListTalentVideo(int TalentIdData = 0,string TalentName = "", int type = 0, int limitvideo = 0, int limitreaction = 0, int limitquestion = 0, int limitall= 0, int Offset = 0 , int Fetch = 0 )
        {
            #region param variable Store Proc
            var xmodel = new TalentVideoModel();
            xmodel.TalentNm = TalentName;
            xmodel.offset = Offset;
            xmodel.fetch = Fetch;

            var xmodelbook = new BookModel();
            xmodelbook.TalentNm = TalentName;
            xmodelbook.TalentId = 0;
            xmodelbook.offset = Offset;
            xmodelbook.fetch = Fetch;

            var xmodelquestion = new QuestionVideoModel();
            xmodelquestion.TalentName = TalentName;
            xmodelquestion.TalentId = 0;
            xmodelquestion.offset = Offset;
            xmodelquestion.fetch = Fetch;

            var xmodelpostcommentvideo = new ViewCommentVideoModel();
            xmodelpostcommentvideo.TalentId = 0;
            xmodelpostcommentvideo.offset = Offset;
            xmodelpostcommentvideo.fetch = Fetch;

            var xmodelallreplycommentvideo = new ViewReplyCommentVideoModel();
            xmodelallreplycommentvideo.TalentId = 0;
            xmodelallreplycommentvideo.offset = Offset;
            xmodelallreplycommentvideo.fetch = Fetch;

            var xmodelsubreplycommentvideo = new ViewSubCommentVideoModel();
            xmodelsubreplycommentvideo.TalentId = 0;
            xmodelsubreplycommentvideo.offset = Offset;
            xmodelsubreplycommentvideo.fetch = Fetch;
            #endregion



            var data = new AllTalentVideo();
            int TalentId = 0;
            if (TalentIdData == 0 && TalentName != "")
            {
                 TalentId = IMasterManager.GetAllTalent().Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).Select(m => m.Id).FirstOrDefault();
            }
            else
            {
                TalentId = TalentIdData;
            }
           
            try
            {
                if (type == 1)
                {
                    if (limitvideo > 0)
                    {
                        if (TalentId > 0)
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.ViewsCount).Take(limitvideo).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo().Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.ViewsCount).Take(limitvideo).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo().OrderByDescending(m => m.ViewsCount).Take(limitvideo).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                    else
                    {
                        if (TalentId > 0)
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.TotalLikes).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else  if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo2(xmodel).Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.TotalLikes).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo2(xmodel).OrderByDescending(m => m.TotalLikes).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                }
                else if(type == 2)
                {
                    if (limitreaction > 0)
                    {
                        if(TalentId > 0)
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(TalentId).OrderByDescending(m => m.ViewsCount).Take(limitreaction).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(0).Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.ViewsCount).Take(limitreaction).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(0).OrderByDescending(m => m.ViewsCount).Take(limitreaction).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                    else
                    {
                        if (TalentId > 0)
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(TalentId).OrderByDescending(m => m.ViewsCount).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }

                        else if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo2(xmodelbook).Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.ViewsCount).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo2(xmodelbook).OrderByDescending(m => m.ViewsCount).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                }
                else if (type == 3)
                {
                    if (limitquestion > 0)
                    {

                        if (TalentId > 0)
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).Take(limitquestion).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }

                        else if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).Take(limitquestion).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().OrderByDescending(m => m.QuestionDate).Take(limitquestion).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                    else
                    {
                        if (TalentId > 0)
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).Take(limitquestion).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else if (!string.IsNullOrEmpty(TalentName))
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo2(xmodelquestion).Where(m => m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo2(xmodelquestion).Where(m => m.IsAnswered == true).OrderByDescending(m => m.QuestionDate).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }
                }
                else
                {
                    if (TalentId > 0)
                    {
                        data.ListTalentVideo = ITransactionManager.GetAllVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.TotalLikes).ToList();
                        data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(0).Where(m => m.TalentId == TalentId).OrderByDescending(m => m.ViewsCount).ToList();
                        data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.QuestionDate).Take(limitquestion).ToList();
                        data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                        data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(TalentName))
                    {
                        data.ListTalentVideo = ITransactionManager.GetAllVideo().Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.TotalLikes).ToList();
                        data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(0).Where(m => m.TalentNm.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.ViewsCount).ToList();
                        data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Where(m => m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).ToList();
                        data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                        data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                    }
                    else
                    {
                        if (limitall > 0)
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo().Take(limitall).OrderByDescending(m => m.TotalLikes).ToList();
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo(0).Take(limitall).OrderByDescending(m => m.ViewsCount).ToList();
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo().Take(limitall).OrderByDescending(m => m.QuestionDate).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                        else
                        {
                            data.ListTalentVideo = ITransactionManager.GetAllVideo2(xmodel).OrderByDescending(m => m.TotalLikes).ToList();
                            data.ReactionVideoList = ITransactionManager.GetAllReactionVideo2(xmodelbook).OrderByDescending(m => m.ViewsCount).ToList();
                            data.QuestionVideoList = ITransactionManager.GetAllQuestionVideo2(xmodelquestion).OrderByDescending(m => m.QuestionDate).ToList();
                            data.ListCommentVideo = ITransactionManager.GetAllPostCommentVideo2(xmodelpostcommentvideo).OrderByDescending(m => m.PostedDate).ToList();
                            data.ListReplyCommentVideo = ITransactionManager.GetAllReplyCommentVideo2(xmodelallreplycommentvideo).OrderByDescending(m => m.CommentedDate).ToList();
                            data.ListSubCommentVideo = ITransactionManager.GetSubReplyCommentVideo2(xmodelsubreplycommentvideo).OrderByDescending(m => m.CommentedDate).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                throw ex;
            }
            var result = data;

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost]
        [Route("~/api/Talent/AddTalentVideoLike")]
        public IActionResult AddTalentVideoLike([FromBody] TalentLikeVideoModel model)
        {
            try
            {
                TalentLikeVideoModel data = new TalentLikeVideoModel();

                var cookieuser = HelperController.GetCookie("UserId");

                if (!string.IsNullOrEmpty(cookieuser))
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                else
                {
                    data.UserId = model.UserId;
                }

                var getCurrentData = ITransactionManager.CreateTalentVideoLike(model);

                return Json(new { Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "AddQuestionLike", ex.Message);

                return Json("Error");
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Talent/RemoveTalentVideoLike")]
        public IActionResult RemoveTalentVideoLike([FromBody]TalentLikeVideoModel model)
        {
            try
            {
                TalentLikeVideoModel data = new TalentLikeVideoModel();

                var cookieuser = HelperController.GetCookie("UserId");

                if (!string.IsNullOrEmpty(cookieuser))
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                else
                {
                    data.UserId = model.UserId;
                }


                ITransactionManager.RemoveTalentVideoLike(model);
                return Json(new { Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "RemoveTalentVideoLike", ex.Message);

                return Json(new { Status = "Error" });
                throw ex;
            }
        }

    }
}