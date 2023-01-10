using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
using Microsoft.Extensions.Configuration;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
       // private readonly ICookie _cookie;
        private readonly HelperController HelperController;


        public CommentController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }

        [HttpPost]
        [Route("~/api/Comment/PostComment")]
        public IActionResult PostComment([FromBody]CommentModel model)
        {
            try
            {
                CommentModel data = new CommentModel();
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
                data.TalentId = model.TalentId;
                data.Message = model.Message;
                data.ObjectId = model.ObjectId;
                var getCurrentData = ITransactionManager.CreateComment(data);
                //return Json(new { getCurrentData, Status = "OK" });

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                CommentModel data = new CommentModel();
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "Comment", ex.Message);
                // return Json(new { QuestionId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Comment/PostReplyComment")]
        public IActionResult PostReplyComment([FromBody]PostCommentModel model)
        {
            PostCommentModel data = new PostCommentModel();
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
                data.CommentMsg = model.CommentMsg;
                data.PostId = model.PostId;
                var getCurrentData = ITransactionManager.CreatePostComment(data);
                //return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "Comment", ex.Message);
                //return Json(new { QuestionId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Comment/PostSubComment")]
        public IActionResult PostSubComment([FromBody]SubCommentModel model)
        {
            SubCommentModel data = new SubCommentModel();
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
                data.CommentMsg = model.CommentMsg;
                data.ComId = model.ComId;
                var getCurrentData = ITransactionManager.CreatePostSubComment(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "Comment", ex.Message);
                //return Json(new { QuestionId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Comment/PostCommentVideo")]
        public IActionResult PostQuestion([FromBody]PostCommentVideoModel model)
        {
            PostCommentVideoModel data = new PostCommentVideoModel();
            try
            {
               
                //var cookieuser = HelperController.GetCookie("UserId");
                //if (cookieuser == "0")
                //{
                //    data.UserId = model.UserId;
                //}
                //else
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}
                data.UserId = model.UserId;
                data.Message = model.Message;
                data.FileId = model.FileId;
                data.IsActive = true;
                var getCurrentData = ITransactionManager.CreatePostCommentVideo(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "PostCommentVideo", ex.Message);
                // return Json(new { PostId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Comment/PostReplyCommentVideo")]
        public IActionResult PostReplyCommentVideo([FromBody]CommentVideoModel model)
        {
            CommentVideoModel data = new CommentVideoModel();
            try
            {
               
                //var cookieuser = HelperController.GetCookie("UserId");
                //if (cookieuser == "0")
                //{
                //    data.UserId = model.UserId;
                //}
                //else
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}

                data.UserId = model.UserId;
                data.CommentMsg = model.CommentMsg;
                data.PostId = model.PostId;
                data.IsActive = true;
                var getCurrentData = ITransactionManager.CreateCommentVideo(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "PostReplyCommentVideo", ex.Message);
                //return Json(new { PostId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Comment/PostSubCommentVideo")]
        public IActionResult PostSubCommentVideo([FromBody]SubCommentVideoModel model)
        {
            SubCommentVideoModel data = new SubCommentVideoModel();
            try
            {
              
                //var cookieuser = HelperController.GetCookie("UserId");
                //if (cookieuser == "0")
                //{
                //    data.UserId = model.UserId;
                //}
                //else
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}

                data.UserId = model.UserId;
                data.CommentMsg = model.CommentMsg;
                data.ComId = model.ComId;
                data.IsActive = true;
                var getCurrentData = ITransactionManager.CreatePostSubComment(data);
                //  return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "PostSubCommentVideo", ex.Message);
                // return Json(new { PostId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }
    }
}