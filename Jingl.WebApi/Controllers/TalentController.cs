using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.WebApi.Authentication;
using Jingl.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;


namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class TalentController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
       
        private readonly HelperController HelperController;


        public TalentController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("~/api/Talent/TalentDetail")]
        public IActionResult TalentDetail(string TalentId)
        {
            #region validasi get booking first

            int max_booking_general = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault() != null ? Convert.ToInt32(IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "General" && x.ParamName == "MaxBooking").FirstOrDefault().ParamValue) : 0;
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
                    _data.QuestionVideoId = i.b.QuestionVideoId;
                    _data.UserId = i.b.UserId;
                    _data.FileId = i.b.FileId;
                    _data.TalentId = i.b.TalentId;
                    _data.Question = i.b.Question;
                    _data.IsActive = i.b.IsActive;
                    _data.IsAnswered = i.b.IsAnswered;
                    _data.UserProfPicLink = i.b.UserProfPicLink;
                    _data.TalentProfPicLink = i.b.TalentProfPicLink;
                    _data.UserName = i.b.UserName;
                    _data.TalentName = i.b.TalentName;
                    _data.Answer = i.b.Answer;
                    _data.QuestionDate = i.b.QuestionDate;
                    _data.AnswerDate = i.b.AnswerDate;
                    _data.LinkVideo = i.b.LinkVideo;
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

                if (data.IsAvailable == false)
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

               // ViewBag.LinkType = "video";
                var getvintro = ITransactionManager.GetTalentVideos(data.Id).Where(x => x.BookCategory == 0).Take(1).OrderByDescending(m => m.FileId).FirstOrDefault();
                data.VideoIntro = getvintro != null ? getvintro.Link.ToString() : "";
                data.VideoIntroId = getvintro != null ? getvintro.FileId.ToString() : "";
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

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = data });
            }
            catch (Exception ex)
            {

                //HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "TalentDetail", ex.Message);
                HelperController.InsertLog(Convert.ToInt32(data.UserId), "TalentDetail", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
            //var result = data;

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpGet]
        [Route("~/api/Talent/ListTalentVideo")]
        public IActionResult ListTalentVideo(int TalentIdData = 0, string TalentName = "", int type = 0, int limitvideo = 0, int limitreaction = 0, int limitquestion = 0, int limitall = 0, int Offset = 0, int Fetch = 0)
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
                        else if (!string.IsNullOrEmpty(TalentName))
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
                else if (type == 2)
                {
                    if (limitreaction > 0)
                    {
                        if (TalentId > 0)
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
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = data });
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(0), "TalentDetail", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
            //var result = data;

            //return new JsonResult(result)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpPost]
        [Route("~/api/Talent/AddTalentVideoLike")]
        public IActionResult AddTalentVideoLike([FromBody] TalentLikeVideoModel model)
        {
            TalentLikeVideoModel data = new TalentLikeVideoModel();
            try
            {
               

                //var cookieuser = HelperController.GetCookie("UserId");

                //if (!string.IsNullOrEmpty(cookieuser))
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}
                //else
                //{
                //    data.UserId = model.UserId;
                //}

                data.UserId = model.UserId;

                var getCurrentData = ITransactionManager.CreateTalentVideoLike(model);

                // return Json(new { Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "AddQuestionLike", ex.Message);

                //return Json("Error");
                // return Json(new { Status = "OK" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message =ex.Message, result = data });
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

                //var cookieuser = HelperController.GetCookie("UserId");

                //if (!string.IsNullOrEmpty(cookieuser))
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}
                //else
                //{
                //    data.UserId = model.UserId;
                //}

                data.UserId = model.UserId;


                ITransactionManager.RemoveTalentVideoLike(model);
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });
                // return Json(new { Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "RemoveTalentVideoLike", ex.Message);

                // return Json(new { Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
                throw ex;
            }
        }


    }
}