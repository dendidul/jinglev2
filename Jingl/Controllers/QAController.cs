using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace Jingl.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class QAController : Controller
    {
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;


        public QAController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestion")]
        public IActionResult PostQuestion([FromBody]QuestionModel model)
        {
            try
            {
                QuestionModel data = new QuestionModel();
                var cookieuser = HelperController.GetCookie("UserId");
                if (cookieuser == "0")
                {
                    data.UserId = model.UserId;
                }
                else
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                data.IsActive = true;
                data.TalentId = model.TalentId;
                data.Question = model.Question;
                data.IsAnswered = false;
                var getCurrentData = ITransactionManager.CreateQuestion(data);
                return Json(new { getCurrentData, Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "QA", ex.Message);
                return Json(new { QuestionId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestionComment")]
        public IActionResult PostQuestionComment([FromBody]QuestionComment model)
        {
            try
            {
                QuestionComment data = new QuestionComment();
                var cookieuser = HelperController.GetCookie("UserId");
                if (cookieuser == "0")
                {
                    data.UserId = model.UserId;
                }
                else
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                data.CommentMsg = model.CommentMsg;
                data.QuestionId = model.QuestionId;
                var getCurrentData = ITransactionManager.CreateQuestionComment(data);
                return Json(new { getCurrentData, Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "QA", ex.Message);
                return Json(new { QuestionId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostAnswer")]
        public IActionResult PostAnswer([FromBody]AnswerModel model)
        {
            try
            {
                var question = new QuestionModel();
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
                return Json(new { getCurrentData, Status = "OK" });
            }

            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "QA", ex.Message);
                return Json(new { AnswerId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpGet]
        [Route("~/api/QA/GetAllQuestion")]
        public IActionResult GetAllQuestion(bool answered = false, string TalentName = null, string UserName = null)
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
            return Json(new { getCurrentData, Status = "OK" });
        }

        [HttpGet]
        [Route("~/api/QA/GetDetailQuestion")]
        public IActionResult GetDetailQuestion(int? TalentId , string sortby = null, string username= null)
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


            return Json(new { getCurrentData, Status = "OK" });
        }

        [HttpGet]
        [Route("~/api/QA/GetSubDetailQuestion")]
        public IActionResult GetSubDetailQuestion(int? QuestionId)
        {
            var getCurrentData = ITransactionManager.GetDetailQuestion().Where(m => m.QuestionId == QuestionId).ToList();
          
            return Json(new { getCurrentData, Status = "OK" });
        }


        [HttpGet]
        [Route("~/api/QA/GetQuestionComment")]
        public IActionResult GetQuestionComment(int? QuestionId)
        {
            var getCurrentData = ITransactionManager.GetQuestionComment().Where(m => m.QuestionId == QuestionId).ToList();

            return Json(new { getCurrentData, Status = "OK" });
        }

        [HttpPost]
        [Route("~/api/QA/AddQuestionLike")]
        public IActionResult AddQuestionLike([FromBody] QuestionLikeModel model)
        {
            try
            {
                QuestionLikeModel data = new QuestionLikeModel();

                var cookieuser = HelperController.GetCookie("UserId");

                if (!string.IsNullOrEmpty(cookieuser))
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                else
                {
                    data.UserId = model.UserId;
                }

                var getCurrentData = ITransactionManager.CreateQuestionLike(model);

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
        [Route("~/api/QA/RemoveQuestionLike")]
        public IActionResult RemoveQuestionLike([FromBody]QuestionLikeModel model)
        {
            try
            {
                QuestionLikeModel data = new QuestionLikeModel();

                var cookieuser = HelperController.GetCookie("UserId");

                if (!string.IsNullOrEmpty(cookieuser))
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                else
                {
                    data.UserId = model.UserId;
                }


                ITransactionManager.RemoveQuestionLike(model);
                return Json("OK");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "RemoveQuestionLike", ex.Message);

                return Json("Error");
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostQuestionVideo")]
        public IActionResult PostQuestionVideo([FromBody]QuestionVideoModel model)
        {
            try
            {
                QuestionVideoModel data = new QuestionVideoModel();
                var cookieuser = HelperController.GetCookie("UserId");
                if (cookieuser == "0")
                {
                    data.UserId = model.UserId;
                }
                else
                {
                    data.UserId = Convert.ToInt32(cookieuser);
                }
                data.IsActive = true;
                data.FileId = model.FileId;
                data.TalentId = model.TalentId;
                data.Question = model.Question;
                data.IsAnswered = false;
                var getCurrentData = ITransactionManager.CreateQuestionVideo(data);
                return Json(new { getCurrentData, Status = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "QA", ex.Message);
                return Json(new { QuestionVideoId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/QA/PostAnswerQuestionVideo")]
        public IActionResult PostAnswerQuestionVideo([FromBody]AnswerQuestionVideoModel model)
        {
            try
            {
                var question = new QuestionVideoModel();
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
                return Json(new { getCurrentData, Status = "OK" });
            }

            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "QA", ex.Message);
                return Json(new { AnswerId = 0, Status = "Error" });
                throw ex;
            }
        }
    }
}