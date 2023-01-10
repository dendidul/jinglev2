using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.WebApi.Authentication;
using Jingl.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QAController : Controller
    {

        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
      //  private readonly ICookie _cookie;
        private readonly HelperController HelperController;


        public QAController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestion")]
        public IActionResult PostQuestion([FromBody]QuestionModel model)
        {
            QuestionModel data = new QuestionModel();
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
                data.IsActive = true;
                data.TalentId = model.TalentId;
                data.Question = model.Question;
                data.IsAnswered = false;
                var getCurrentData = ITransactionManager.CreateQuestion(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "QA", ex.Message);
                //return Json(new { QuestionId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestionComment")]
        public IActionResult PostQuestionComment([FromBody]QuestionComment model)
        {
            QuestionComment data = new QuestionComment();
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
                data.QuestionId = model.QuestionId;
                var getCurrentData = ITransactionManager.CreateQuestionComment(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "QA", ex.Message);
                // return Json(new { QuestionId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostAnswer")]
        public IActionResult PostAnswer([FromBody]AnswerModel model)
        {
            var question = new QuestionModel();
            try
            {
               
                AnswerModel data = new AnswerModel();
                data.QuestionId = model.QuestionId;
                if (string.IsNullOrEmpty(model.Answer))
                {
                    return Json(new { Status = "Empty Answer" });
                }
                else
                {
                    data.Answer = model.Answer;
                }
                var getCurrentData = ITransactionManager.CreateAnswer(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }

            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(0), "QA", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = question });
                throw ex;
            }
        }

        [HttpGet]
        [Route("~/api/QA/GetAllQuestion")]
        public IActionResult GetAllQuestion(bool answered = false, string TalentName = null, string UserName = null)
        {
            try
            {
                var getCurrentData = ITransactionManager.GetAllQuestion().ToList();

                if (answered == false)
                {
                    if (!string.IsNullOrEmpty(TalentName))
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == false && m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else if (!string.IsNullOrEmpty(UserName))
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == false && m.UserName.IndexOf(UserName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == false).ToList();
                    }
                }
                else if (answered == true)
                {
                    if (!string.IsNullOrEmpty(TalentName))
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == true && m.TalentName.IndexOf(TalentName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else if (!string.IsNullOrEmpty(UserName))
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == true && m.UserName.IndexOf(UserName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else
                    {
                        getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == true).ToList();
                    }
                }
                else
                {
                    getCurrentData = ITransactionManager.GetAllQuestion().Where(m => m.IsAnswered == true).ToList();
                }

                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                List<QuestionModel> failmodel = new List<QuestionModel>();
                HelperController.InsertLog(Convert.ToInt32(0), "GetAllQuestion", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = failmodel });
                throw ex;
            }
           
          
        }

        [HttpGet]
        [Route("~/api/QA/GetDetailQuestion")]
        public IActionResult GetDetailQuestion(int? TalentId, string sortby = null, string username = null)
        {
            try
            {
                var getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId).ToList();

                if (!string.IsNullOrEmpty(sortby))
                {
                    if (sortby == "date")
                    {
                        if (!string.IsNullOrEmpty(username))
                        {
                            getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId
                            && m.UserName.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.QuestionDate).ToList();
                        }
                        else
                        {
                            getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.QuestionDate).ToList();

                        }
                    }
                    else if (sortby == "like")
                    {
                        if (!string.IsNullOrEmpty(username))
                        {
                            getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId
                            && m.UserName.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(m => m.TotalLike).ToList();
                        }
                        else
                        {
                            getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.TotalLike).ToList();
                        }
                    }
                    else
                    {
                        getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId).OrderByDescending(m => m.QuestionDate).ToList();
                    }
                }
                else
                {
                    getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.TalentId == TalentId).ToList();
                }
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });

                // return Json(new { getCurrentData, Status = "OK" });
            }
            catch (Exception ex)
            {
                List<QuestionModel> failmodel = new List<QuestionModel>();
                HelperController.InsertLog(Convert.ToInt32(0), "GetDetailQuestion", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = failmodel });
                throw ex;
            }
          
        }

        [HttpGet]
        [Route("~/api/QA/GetSubDetailQuestion")]
        public IActionResult GetSubDetailQuestion(int? QuestionId)
        {
            try
            {
                var getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.QuestionId == QuestionId).ToList();

                //  return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                List<QuestionModel> failmodel = new List<QuestionModel>();
                HelperController.InsertLog(Convert.ToInt32(0), "GetSubDetailQuestion", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = failmodel });
                throw ex;
            }
           
        }


        [HttpGet]
        [Route("~/api/QA/GetQuestionComment")]
        public IActionResult GetQuestionComment(int? QuestionId)
        {
            try
            {
                var getCurrentData = ITransactionManager.GetQuestionComment().Where(m => m.QuestionId == QuestionId).ToList();

                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                List<GetQuestionComment> failmodel = new List<GetQuestionComment>();
                HelperController.InsertLog(Convert.ToInt32(0), "GetQuestionComment", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = failmodel });
                throw ex;
            }
           
        }

        [HttpPost]
        [Route("~/api/QA/AddQuestionLike")]
        public IActionResult AddQuestionLike([FromBody] QuestionLikeModel model)
        {
            QuestionLikeModel data = new QuestionLikeModel();
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
                var getCurrentData = ITransactionManager.CreateQuestionLike(model);

                // return Json(new { Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "AddQuestionLike", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });

                //return Json("Error");
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/RemoveQuestionLike")]
        public IActionResult RemoveQuestionLike([FromBody]QuestionLikeModel model)
        {
            QuestionLikeModel data = new QuestionLikeModel();
            try
            {
               // QuestionLikeModel data = new QuestionLikeModel();

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
                ITransactionManager.RemoveQuestionLike(model);
                // return Json("OK");
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "RemoveQuestionLike", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message});
                //return Json("Error");
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestionVideo")]
        public IActionResult PostQuestionVideo([FromBody]QuestionVideoModel model)
        {
            QuestionVideoModel data = new QuestionVideoModel();
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
                data.IsActive = true;
                data.FileId = model.FileId;
                data.TalentId = model.TalentId;
                data.Question = model.Question;
                data.IsAnswered = false;
                var getCurrentData = ITransactionManager.CreateQuestionVideo(data);
                // return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "PostQuestionVideo", ex.Message);
                // return Json(new { QuestionVideoId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = data });

                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostAnswerQuestionVideo")]
        public IActionResult PostAnswerQuestionVideo([FromBody]AnswerQuestionVideoModel model)
        {
            var question = new QuestionVideoModel();
            try
            {
                
                AnswerQuestionVideoModel data = new AnswerQuestionVideoModel();
                data.QuestionVideoId = model.QuestionVideoId;
                if (string.IsNullOrEmpty(model.Answer))
                {
                    return Json(new { Status = "Empty Answer" });
                }
                else
                {
                    data.Answer = model.Answer;
                }
                var getCurrentData = ITransactionManager.CreateAnswerQuestionVideo(data);
                //return Json(new { getCurrentData, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });
            }

            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(0), "QA", ex.Message);
                // return Json(new { AnswerId = 0, Status = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = question });

                throw ex;
            }
        }
    }
}