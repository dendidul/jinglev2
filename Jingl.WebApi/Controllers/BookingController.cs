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
using Microsoft.VisualStudio.Web.CodeGeneration;
//using Microsoft.AspNetCore.Mvc;

namespace Jingl.WebApi.Controllers
{
    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : Controller
    {

        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;     
        private readonly HelperController HelperController;


        public BookingController(IConfiguration config)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config);
        }

        [HttpPost]
        [Route("~/api/Booking/PostHire")]
        public IActionResult PostHire([FromBody]BookModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ProjectNm))
                {
                    CategoryModel cat = new CategoryModel();
                    cat.Id = Convert.ToInt32(model.BookCategory);
                    cat = IMasterManager.GetDataCategory(cat);
                    model.ProjectNm = cat.CategoryNm;
                }

                //var _Email = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.Email).FirstOrDefault();

                UserModel usm = new UserModel();
                usm.Id = model.UserId;
                var Getuser = IUserManagementManager.GetUser(usm);
                model.Email = Getuser != null ? Getuser.Email : "";

                //model.CreatedBy = HelperController.GetCookie("UserId");
                model.CreatedBy = Convert.ToString(model.UserId);
                var _userid = Convert.ToInt32(model.CreatedBy);
                //model.BookedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
                model.BookedBy = _userid;
                var periodData = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault();
                model.Period = periodData != null ? periodData.ParamValue : "";
                var getCurrentData = ITransactionManager.CreateBookData(model);
                //return Json(new { BookId = getCurrentData.Id, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK",result= getCurrentData.Id });
            }
            catch (Exception ex)
            {
                var user_id = Convert.ToInt32(model.CreatedBy);
                HelperController.InsertLog(Convert.ToInt32(user_id), "Hire", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });

                // return Json(new { BookId = 0, Status = "Error" });
                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Booking/GetMidTransToken")]
        public async Task<IActionResult> GetMidTransToken([FromBody]BookModel model)
        {

            try
            {

                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if (model.UserId == getcurrentdata.BookedBy)
                {

                    transaction_details transdetails = new transaction_details();
                    transdetails.order_id = getcurrentdata.OrderNo;
                    transdetails.gross_amount = getcurrentdata.TotalPay.Value;
                    TransactionOrder Order = new TransactionOrder()
                    {
                        transaction_details = transdetails
                    }
                        ;
                    var data = await HelperController.SnapApiPost<TokenResultModel, TransactionOrder>("snap/v1/transactions", Order);


                    // var getcurrentdata = ITransactionManager.GetDataBookByOrderId(model.OrderNo);
                    //getcurrentdata.PriceAmount = model.PriceAmount;
                    //getcurrentdata.SalePrice = model.SalePrice;
                    //getcurrentdata.PaymentShare = model.PaymentShare;
                    //getcurrentdata.Potongan = model.Potongan;
                    //getcurrentdata.TotalPay = model.TotalPay;
                    //getcurrentdata.VoucherCode = model.VoucherCode;
                    getcurrentdata.PayMethod = "gopay";
                    ITransactionManager.UpdateBookData(getcurrentdata);
                    //return Json(data);
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = data });

                }
                else
                {
                    // return Json("Error");
                    BookModel bm = new BookModel();
                    //return Json(new { BookId = getcurrentdata.Id, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    // return Json(new { Status = "Error", Message = "User Tidak memiliki akses untuk order ini", result = bm });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
                }


                //transaction_details transdetails = new transaction_details();
                //transdetails.order_id = model.OrderNo;
                //transdetails.gross_amount = model.TotalPay.Value;
                //TransactionOrder Order = new TransactionOrder()
                //{
                //    transaction_details = transdetails
                //}
                //    ;
                //var data = await HelperController.SnapApiPost<TokenResultModel, TransactionOrder>("snap/v1/transactions", Order);


                //var getcurrentdata = ITransactionManager.GetDataBookByOrderId(model.OrderNo);
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.SalePrice = model.SalePrice;
                //getcurrentdata.PaymentShare = model.PaymentShare;
                //getcurrentdata.Potongan = model.Potongan;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.VoucherCode = model.VoucherCode;
                //ITransactionManager.UpdateBookData(getcurrentdata);


            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "GetMidTransToken", ex.Message);
                TokenResultModel failmodel = new TokenResultModel();
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Error", result = failmodel });
                throw ex;
            }

            //try
            //{

            //    transaction_details transdetails = new transaction_details();
            //    transdetails.order_id = model.OrderNo;
            //    transdetails.gross_amount = model.TotalPay.Value;
            //    TransactionOrder Order = new TransactionOrder()
            //    {
            //        transaction_details = transdetails
            //    }
            //        ;
            //    var data = await HelperController.SnapApiPost<TokenResultModel, TransactionOrder>("snap/v1/transactions", Order);
            //    //ITransactionManager.CreateWishlistData(model);

            //    var getcurrentdata = ITransactionManager.GetDataBookByOrderId(model.OrderNo);
            //    getcurrentdata.PriceAmount = model.PriceAmount;
            //    getcurrentdata.SalePrice = model.SalePrice;
            //    getcurrentdata.PaymentShare = model.PaymentShare;
            //    getcurrentdata.Potongan = model.Potongan;
            //    getcurrentdata.TotalPay = model.TotalPay;
            //    getcurrentdata.VoucherCode = model.VoucherCode;
            //    ITransactionManager.UpdateBookData(getcurrentdata);

            //    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = data });

            //    //return Json(data);
            //}
            //catch (Exception ex)
            //{
            //    HelperController.InsertLog(Convert.ToInt32(model.UserId), "GetMidTransToken", ex.Message);
            //    TokenResultModel failmodel = new TokenResultModel();
            //    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Error", result = failmodel });
            //    throw ex;
            //}
        }


        [HttpPost]
        [Route("~/api/Booking/OrderDetail")]
        public async Task<IActionResult> OrderDetail([FromBody] BookModel bookModel)

        {
            var model = new BookModel();

            try
            {
                var transactionModel = new TransactionResultModel();
                var data = new BookModel();
                // data.Id = Convert.ToInt32(bookModel.Id);
                // model.Id = bookModel.Id;

                //   data = ITransactionManager.GetDataBook(data);
                //transactionModel = await HelperController.GetTransactionStatus(data.SnapToken);
                model = ITransactionManager.GetBookConfirmation(bookModel);
                if (model.UserOfTalentId == bookModel.UserId || model.BookedBy == bookModel.UserId)
                {

                    model.VaNumber = model.SnapToken;

                    if (model.PayMethod == "bank_transfer")
                    {
                        if (model.PaymentChannel == "802")
                        {
                            model.BankName = "Mandiri";
                        }
                        else if (model.PaymentChannel == "801")
                        {
                            model.BankName = "BNI";
                        }
                        else if (model.PaymentChannel == "800")
                        {
                            model.BankName = "BRI";
                        }
                        else if (model.PaymentChannel == "702")
                        {
                            model.BankName = "BCA";
                        }
                    }

                    model.ClientKeyId = HelperController.MidtransClientKey();
                    // model.IsEmail = IsEmail;
                    //  return Json(new { Status = "OK", Message = "OK", result = model });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });

                }
                else
                {
                    BookModel bm = new BookModel();
                    //return Json(new { BookId = getcurrentdata.Id, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    // return Json(new { Status = "Error", Message = "User Tidak memiliki akses untuk order ini", result = bm });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
                }

            }
            catch (Exception ex)
            {
                BookModel failmodel = new BookModel();
                HelperController.InsertLog(0, "OrderDetail", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = failmodel });
                //return Json(new { Status = "Error", Message = ex.Message, result = bm });
                throw ex;
            }

            //return new JsonResult(model)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};


        }

        [HttpGet]
        [Route("~/api/Booking/GetCategory")]
        public IActionResult GetCategory()
        {
            try
            {
                var model = IMasterManager.GetAllCategory().Where(m => m.CategoryType == "Book").ToList();
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });
                //return new JsonResult(model)
                //{
                //    StatusCode = StatusCodes.Status200OK
                //};


            }
            catch (Exception ex)
            {
                List<CategoryModel> failmodel = new List<CategoryModel>();
                return Json(new { Status = StatusCodes.Status200OK, Message = ex.Message, result = failmodel });
                throw ex;
            }
            
        }

        //[HttpGet]
        //[Route("~/api/Booking/DataConfirm")]
        //public IActionResult DataConfirm(int BookId)
        [HttpPost]
        [Route("~/api/Booking/DataConfirm")]
        public IActionResult DataConfirm([FromBody] BookModel data)
        {
            try
            {
                var model = new BookModel();
                model.Id = data.Id;
                model.ClientKeyId = HelperController.MidtransClientKey();
                model = ITransactionManager.GetBookConfirmation(model);



                if (model.UserOfTalentId == data.UserId || model.BookedBy == data.UserId)
                {
                    if (model.Status == Convert.ToInt32(BookingFlow.Submit))
                    {
                        var result = model;
                        //return Json(new { Status = "OK", Message = "OK", result = model });
                        return Json(new { Status = StatusCodes.Status200OK, result = model, Message = "OK" });
                    }
                    else
                    {
                        BookModel bm = new BookModel();
                        //return Json(new { Status = "Error", Message = "Status Order sudah tidak submit", result = bm });
                        return Json(new { Status = StatusCodes.Status400BadRequest, result = bm, Message = "Status Order sudah tidak submit" });
                    }

                }
                else
                {
                    BookModel bm = new BookModel();
                    //return Json(new { BookId = getcurrentdata.Id, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    //return Json(new { Status = "Error", Message = "User Tidak memiliki akses untuk order ini", result = bm });
                    return Json(new { Status = StatusCodes.Status400BadRequest, result = bm, Message = "User Tidak memiliki akses untuk order ini" });
                }


            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(0, "DataConfirm", ex.Message);
                //return Json(new { Status = "Error", Message = ex.Message, result = bm });
                return Json(new { Status = StatusCodes.Status400BadRequest, result = bm, Message = "Error" });
                throw ex;

            }

        }

        [HttpPost]
        [Route("~/api/Booking/UpdateMaterialSent")]
        public IActionResult UpdateMaterialSent([FromBody] BookModel model/*string BookId, string status, string FileId*/)
        {
            try
            {
                //model = new BookModel();
              //  model.Id = Convert.ToInt32(model.BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                getcurrentdata.UpdatedBy = Convert.ToString(model.UserId);
                var sendTo = "";
                var body = "";
                var subject = "";
                var firstName = "";

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                Userdata = IUserManagementManager.GetUser(Userdata);
                firstName = Userdata.FirstName;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.Potongan = model.Potongan;

                int Statusdata = Convert.ToInt32(model.Status);

                if (Statusdata == 2)
                {
                    getcurrentdata.Status = (int)BookingFlow.Paid;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                }
                else if (Statusdata == 3)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                }
                else if (Statusdata == 4)
                {
                    getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                }
                else if (Statusdata == 5)
                {
                    var periodData = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault();

                    getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    getcurrentdata.Period = Convert.ToString(periodData.ParamValue);
                }
                else if (Statusdata == 6)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                }
                else if (Statusdata == 7)
                {
                    getcurrentdata.Status = (int)BookingFlow.RateTalent;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                getcurrentdata.IsActive = (int)Status.Active;
                //   getcurrentdata.PayMethod = model.PayMethod;
                getcurrentdata.FileId = Convert.ToInt32(model.FileId);
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                //send notifikasi
                if ((Convert.ToInt32(model.Status) == 2))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, Convert.ToString(model.Status), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                }
                else
                {
                    HelperController.NotificationEmail(getcurrentdata.Email, Convert.ToString(model.Status), EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                }



                //return Json(new { BookId = getCurrentData.Id, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" ,result = getCurrentData.Id });


            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "UpdateMaterialSent", ex.Message);

                // return Json(new { BookId = 0, Status = "Error" });

                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });
                throw ex;
            }

        }

        [HttpPost]
        [Route("~/api/Booking/UpdateBookingMaterialVideo")]
        public IActionResult UpdateBookingMaterialVideo([FromBody] BookModel model)
        {
            try
            {
                var Bookmodel = new BookModel();
                Bookmodel.Id = Convert.ToInt32(model.Id);
                var getBookData = ITransactionManager.GetDataBook(model);
                getBookData.FileId = model.FileId;

                var getCurrentData = ITransactionManager.UpdateBookData(getBookData);
                // return Json(new { BookId = getCurrentData.Id, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData.Id });
            }
            catch (Exception ex)
            {
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });
                throw;
            }
        }

        [HttpPost]
        [Route("~/api/Booking/UpdateBookingReactionVideo")]
        public IActionResult UpdateBookingReactionVideo([FromBody] BookModel model)
        {
            try
            {
                var Bookmodel = new BookModel();
                Bookmodel.Id = Convert.ToInt32(model.Id);
                var getBookData = ITransactionManager.GetDataBook(model);
                getBookData.ReactionFileId = model.ReactionFileId;

                var getCurrentData = ITransactionManager.UpdateBookData(getBookData);
                //return Json(new { BookId = getCurrentData.Id, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData.Id });
            }
            catch (Exception ex)
            {
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });

                throw ex;
            }
        }


        [HttpPost]
        [Route("~/api/Booking/UpdateReactionVideo")]
        //public IActionResult UpdatReactionVideo(string BookId, string status, string FileId)
        public IActionResult UpdateReactionVideo([FromBody] BookModel data)
        {
            try
            {
                var model = new BookModel();
                model.Id = Convert.ToInt32(data.Id);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                //getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                getcurrentdata.UpdatedBy = data.UserId.ToString() ;

                var sendTo = "";
                var body = "";
                var subject = "";
                var firstName = "";

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                Userdata = IUserManagementManager.GetUser(Userdata);
                firstName = Userdata.FirstName;
                //getcurrentdata.TotalPay = model.TotalPay;
                //getcurrentdata.PriceAmount = model.PriceAmount;
                //getcurrentdata.Potongan = model.Potongan;

                int Statusdata = Convert.ToInt32(data.Status);

                if (Statusdata == 2)
                {
                    getcurrentdata.Status = (int)BookingFlow.Paid;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                }
                else if (Statusdata == 3)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                }
                else if (Statusdata == 4)
                {
                    getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                }
                else if (Statusdata == 5)
                {
                    var periodData = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault();

                    getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    getcurrentdata.Period = Convert.ToString(periodData.ParamValue);
                }
                else if (Statusdata == 6)
                {
                    getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                }
                else if (Statusdata == 7)
                {
                    getcurrentdata.Status = (int)BookingFlow.RateTalent;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                else if (Statusdata == 8)
                {
                    getcurrentdata.Status = (int)BookingFlow.ReactionVideo;
                    sendTo = Userdata.Email;
                    body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                }

                getcurrentdata.IsActive = (int)Status.Active;
                //   getcurrentdata.PayMethod = model.PayMethod;
                getcurrentdata.ReactionFileId = Convert.ToInt32(data.FileId);
                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                //send notifikasi
                if ((Convert.ToInt32(data.Status) == 2))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, data.Status.ToString(), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                }
                else if ((Convert.ToInt32(data.Status) == 8))
                {
                    HelperController.NotificationEmail(TalentUserdata.Email, data.Status.ToString(), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                }
                else
                {
                    HelperController.NotificationEmail(getcurrentdata.Email, data.Status.ToString(), EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                }



                // return Json(new { BookId = getCurrentData.Id, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData.Id });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(data.UserId), "UpdatReactionVideo", ex.Message);

                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });
                throw ex;
            }

        }


        [HttpPost]
        [Route("~/api/booking/DeclineOrderBook")]
        public IActionResult DeclineOrderBook([FromBody] BookModel model)
        {
            try
            {
                //var model = new BookModel();

                //model.Id = Convert.ToInt32(data.Id);
                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
                {
                    getcurrentdata.UpdatedBy = model.UserId.ToString();




                    model = getcurrentdata;
                    model.Reason = Regex.Replace(model.Reason != null ? model.Reason : "", @"\t|\n|\r", "");
                    model.Status = (int)BookingFlow.Refund;
                    ITransactionManager.UpdateBookData(model);

                    //  return Json(new { result = getcurrentdata, Message = "OK", Status = "OK" });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getcurrentdata });
                }

                else
                {
                    BookModel bm = new BookModel();
                    // return Json(new { result = bm, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
                }




            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(Convert.ToInt32(0), "DeclineOrderBook", ex.Message);

                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });
                throw ex;
            }

            //try
            //{
            //    //var model = new BookModel();

            //    //model.Id = Convert.ToInt32(data.Id);
            //    var getcurrentdata = ITransactionManager.GetDataBook(model);

            //    if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
            //    {
            //        getcurrentdata.UpdatedBy = model.UserId.ToString();




            //        model = getcurrentdata;
            //        model.Reason = Regex.Replace(model.Reason, @"\t|\n|\r", "");
            //        model.Status = (int)BookingFlow.Refund;
            //        ITransactionManager.UpdateBookData(model);

            //        //return Json(new { result = getcurrentdata, Message = "OK", Status = "OK" });
            //        return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getcurrentdata });

            //    }

            //    else
            //    {
            //        BookModel bm = new BookModel();
            //        // return Json(new { result = bm, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
            //        return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
            //    }




            //}
            //catch (Exception ex)
            //{
            //    BookModel bm = new BookModel();
            //    HelperController.InsertLog(Convert.ToInt32(model.UserId), "UpdateBookingStatus", ex.Message);

            //    // return Json(new { result = bm, Status = "Error", Message = ex.Message });
            //    return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });
            //    throw ex;
            //}
        }



      


        [HttpPost]
        [Route("~/api/booking/UpdateBookingAmount")]
        public IActionResult UpdateBookingAmount([FromBody] BookModel model)
        {
            try
            {

                //model = new BookModel();
                // model.Id = Convert.ToInt32(model.BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
                {
                    //getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                    getcurrentdata.UpdatedBy = Convert.ToString(model.UserId);
                    //getcurrentdata.TotalPay = model.TotalPay - model.Potongan;
                    getcurrentdata.TotalPay = model.TotalPay;
                    getcurrentdata.Potongan = model.Potongan;
                    getcurrentdata.PriceAmount = model.PriceAmount;
                    getcurrentdata.VoucherCode = model.VoucherCode;

                    var sendTo = "";
                    var body = "";
                    var subject = "";
                    var firstName = "";

                    var Userdata = new UserModel();
                    var Talentdata = new TalentModel();
                    var TalentUserdata = new UserModel();

                    Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                    Talentdata.Id = getcurrentdata.TalentId.Value;
                    Talentdata = IMasterManager.GetTalent(Talentdata);

                    TalentUserdata.Id = Talentdata.UserId.Value;
                    TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                    Userdata = IUserManagementManager.GetUser(Userdata);
                    firstName = Userdata.FirstName;
                    //getcurrentdata.TotalPay = model.TotalPay;
                    //getcurrentdata.PriceAmount = model.PriceAmount;
                    //getcurrentdata.Potongan = model.Potongan;




                    getcurrentdata.Status = (int)BookingFlow.Submit;
                    getcurrentdata.IsActive = (int)Status.Active;
                    getcurrentdata.SalePrice = Talentdata.SalePrice;

                    var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);

                    // return Json(new { result = getCurrentData, Status = "OK", Message = "OK" });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });

                }
                else
                {
                    BookModel bm = new BookModel();
                    // return Json(new { result = bm, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
                }



            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "UpdateBookingAmount", ex.Message);
                //  return Json(new { result = bm, Status = "Error", Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });
                throw ex;
            }


        }


        [HttpPost]
        [Route("~/api/booking/UpdateSuccessPaymentBooking")]
        public IActionResult UpdateSuccessPaymentBooking([FromBody] BookModel model)
        {
            try
            {

                //model = new BookModel();
                //  model.Id = Convert.ToInt32(model.BookId);
                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
                {
                    //getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                    getcurrentdata.UpdatedBy = Convert.ToString(model.UserId);
                    //getcurrentdata.TotalPay = model.TotalPay - model.Potongan;
                    getcurrentdata.TotalPay = model.TotalPay;
                    getcurrentdata.Potongan = model.Potongan;
                    getcurrentdata.PriceAmount = model.PriceAmount;
                    getcurrentdata.VoucherCode = model.VoucherCode;

                    var sendTo = "";
                    var body = "";
                    var subject = "";
                    var firstName = "";

                    var Userdata = new UserModel();
                    var Talentdata = new TalentModel();
                    var TalentUserdata = new UserModel();

                    Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                    Talentdata.Id = getcurrentdata.TalentId.Value;
                    Talentdata = IMasterManager.GetTalent(Talentdata);

                    TalentUserdata.Id = Talentdata.UserId.Value;
                    TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                    Userdata = IUserManagementManager.GetUser(Userdata);
                    firstName = Userdata.FirstName;
                    //getcurrentdata.TotalPay = model.TotalPay;
                    //getcurrentdata.PriceAmount = model.PriceAmount;
                    //getcurrentdata.Potongan = model.Potongan;

                    int Statusdata = Convert.ToInt32(model.Status);

                    if (Statusdata == 2)
                    {
                        getcurrentdata.Status = (int)BookingFlow.Paid;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    }
                    else if (Statusdata == 3)
                    {
                        getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    }
                    else if (Statusdata == 4)
                    {
                        getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    }
                    else if (Statusdata == 5)
                    {
                        getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    }
                    else if (Statusdata == 6)
                    {
                        getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    }
                    else if (Statusdata == 7)
                    {
                        getcurrentdata.Status = (int)BookingFlow.RateTalent;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    }

                    getcurrentdata.IsActive = (int)Status.Active;
                    getcurrentdata.SalePrice = Talentdata.SalePrice;

                    //VoucherTransactionModel VOM = new VoucherTransactionModel();
                    //VOM.BookId = getcurrentdata.Id;
                    //VOM.TalentId = Talentdata.Id;
                    //VOM.VoucherCd = getcurrentdata.VoucherCode;
                    //var GetVoucher = IMasterManager.GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                    //VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                    //VOM.qty = 1;
                    ////VOM.UserId = Convert.ToInt32(HelperController.GetCookie("UserId"));
                    //VOM.UserId = Convert.ToInt32(model.UserId);
                    //VOM.Amount = getcurrentdata.Potongan.Value;

                    //ITransactionManager.CreateVoucherTransaction(VOM);

                    //if (GetVoucher != null)
                    //{
                    //    VoucherModel VM = new VoucherModel();
                    //    VM = GetVoucher;
                    //    VM.RemainingCount = VM.RemainingCount - 1;
                    //    VM.Budget = VM.Budget - VOM.Amount;
                    //    IMasterManager.UpdateVoucher(VM);

                    //}

                    // getcurrentdata.PayMethod = model.PayMethod;
                    var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);





                    //send notifikasi
                    if ((Convert.ToInt32(model.Status) == 2))
                    {
                        HelperController.NotificationEmail(TalentUserdata.Email, Convert.ToString(model.Status), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                        //if(Talentdata.IsPriority)
                        //{
                        //    SaldoModel saldoData = new SaldoModel();
                        //    saldoData.TalentId = Talentdata.Id;
                        //    saldoData = ITransactionManager.GetSaldoByTalentId(saldoData);
                        //    if(saldoData != null)
                        //    {
                        //        saldoData.SaldoUsedAmt += getcurrentdata.TotalPay;
                        //        saldoData = ITransactionManager.UpdateSaldo(saldoData);
                        //    }
                        //}

                    }
                    else
                    {

                        HelperController.NotificationEmail(getcurrentdata.Email, Convert.ToString(model.Status), EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                        if (model.Status != 0)
                        {
                            HelperController.NotificationEmail(TalentUserdata.Email, Convert.ToString(model.Status), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, TalentUserdata.FirstName).Wait();
                        }

                    }


                    // return Json(new { result = getCurrentData, Status = "OK", Message = "OK" });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });

                }
                else
                {
                    BookModel bm = new BookModel();
                    //return Json(new { result = bm, Status = "Error", Message = "User tidak memiliki akses untuk order ini" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User tidak memiliki akses untuk order ini", result = bm });
                }




            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "UpdateBookingStatus", ex.Message);

                // return Json(new { result = bm, Status = "Error", Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });
                throw ex;
            }


        }

        [HttpPost]
        [Route("~/api/booking/UpdateCompletedBookingStatus")]
        public IActionResult UpdateCompletedBookingStatus([FromBody]BookModel model)
        {
            try
            {
                //model = new BookModel();
                model.Id = Convert.ToInt32(model.Id);
                var getcurrentdata = ITransactionManager.GetDataBook(model);
                if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
                {

                    string _userid = Convert.ToString(model.UserId);
                    getcurrentdata.UpdatedBy = Convert.ToString(model.UserId);
                    getcurrentdata.Review = model.Review;
                    var sendTo = "";
                    var body = "";
                    var subject = "";
                    var firstName = "";

                    var Userdata = new UserModel();
                    var Talentdata = new TalentModel();
                    var TalentUserdata = new UserModel();

                    Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                    Talentdata.Id = getcurrentdata.TalentId.Value;
                    Talentdata = IMasterManager.GetTalent(Talentdata);

                    TalentUserdata.Id = Talentdata.UserId.Value;
                    TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                    Userdata = IUserManagementManager.GetUser(Userdata);
                    firstName = Userdata.FirstName;
                    //getcurrentdata.TotalPay = model.TotalPay;
                    //getcurrentdata.PriceAmount = model.PriceAmount;
                    //getcurrentdata.Potongan = model.Potongan;

                    int Statusdata = Convert.ToInt32(model.Status);

                    if (Statusdata == 2)
                    {
                        getcurrentdata.Status = (int)BookingFlow.Paid;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.Paid.ToString();
                    }
                    else if (Statusdata == 3)
                    {
                        getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectAccepted.ToString();
                    }
                    else if (Statusdata == 4)
                    {
                        getcurrentdata.Status = (int)BookingFlow.RecordingProcess;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RecordingProcess.ToString();
                    }
                    else if (Statusdata == 5)
                    {
                        getcurrentdata.Status = (int)BookingFlow.MaterialAccepted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.MaterialAccepted.ToString();
                    }
                    else if (Statusdata == 6)
                    {
                        getcurrentdata.Status = (int)BookingFlow.ProjectCompleted;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.ProjectCompleted.ToString();
                    }
                    else if (Statusdata == 7)
                    {
                        getcurrentdata.Status = (int)BookingFlow.RateTalent;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    }
                    else if (Statusdata == 8)
                    {
                        getcurrentdata.Status = (int)BookingFlow.ReactionVideo;
                        sendTo = Userdata.Email;
                        body = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                        subject = getcurrentdata.OrderNo + ' ' + BookingFlow.RateTalent.ToString();
                    }

                    getcurrentdata.IsActive = (int)Status.Active;
                    //  getcurrentdata.PayMethod = model.PayMethod;
                    var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);




                    //send notifikasi
                    if ((Convert.ToInt32(model.Status) == 2))
                    {
                        HelperController.NotificationEmail(TalentUserdata.Email, Convert.ToString(model.Status), EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();


                    }
                    else
                    {
                        HelperController.NotificationEmail(getcurrentdata.Email, Convert.ToString(model.Status), EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();
                    }

                    RatingModel RateModel = new RatingModel();
                    RateModel.UserId = model.UserId;
                    RateModel.FileId = getcurrentdata.FileId;
                    RateModel.Rate = Convert.ToInt32(model.Rate);
                    RateModel.CreatedBy = Convert.ToString(model.UserId);
                    ITransactionManager.CreateRatingFiles(RateModel);


                    // return Json(new { result = getCurrentData, Status = "OK", Message = "OK" });
                    return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData });

                }
                else
                {
                    BookModel bm = new BookModel();
                    // return Json(new { result = bm, Status = "Error", Message = "User Id tidak memiliki Akses" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User Id tidak memiliki Akses", result = bm });
                }
            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "UpdateCompletedBookingStatus", ex.Message);

                //return Json(new { BookId = 0, Status = "Error", Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });
                throw ex;
            }



        }


        [HttpPost]
        [Route("~/api/Booking/BookingHistory")]
        public IActionResult BookingHistory([FromBody] BookModel data)
        {
            var model = new BookModel();
            try
            {
                model.Id = Convert.ToInt32(model.Id);

                model = ITransactionManager.GetDataBook(model);
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });


            }
            catch (Exception ex)
            {

                // HelperController.InsertLog(Convert.ToInt32(data.UserId), "BookingHistory", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });
                throw ex;
            }
            //return new JsonResult(model)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }

        [HttpPost]
        [Route("~/api/Booking/AddToWishlist")]
        public IActionResult AddToWishlist([FromBody] WishlistModel model)
        {
            try
            {
                WishlistModel data = new WishlistModel();
                data.UserId = model.UserId;

                //var cookieuser = HelperController.GetCookie("UserId");

                //if (!string.IsNullOrEmpty(cookieuser))
                //{
                //    data.UserId = Convert.ToInt32(cookieuser);
                //}
                //else
                //{
                //    data.UserId = model.UserId;
                //}

                var getCurrentData = ITransactionManager.CreateWishlistData(model);




                // return Json(new { FavouriteCount = getCurrentData.FavouriteCount, Status = "OK" });
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = getCurrentData.FavouriteCount });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "AddToWishlist", ex.Message);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = 0 });
                //return Json("Error");
                throw ex;
            }
        }


        [HttpPost]
        [Route("~/api/Booking/RemoveFromWishlist")]
        public IActionResult RemoveFromWishlist([FromBody]WishlistModel model)
        {
            try
            {
                WishlistModel data = new WishlistModel();
                            

                data.UserId = model.UserId;


                ITransactionManager.RemoveWishlistData(model);
                //return Json("OK");
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "RemoveWishlistData", ex.Message);

                //return Json("Error");
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
                throw ex;
            }
        }

        [HttpGet]
        [Route("~/api/Booking/GetVoucherCodeByTalent")]
        public IActionResult GetVoucherCodeByTalent(int TalentId, string Code)
        {
            try
            {
                var voucher = IMasterManager.GetAllVoucherByTalent().ToList();

                if (TalentId > 0)
                {
                    if (!string.IsNullOrEmpty(Code))
                    {
                        voucher = IMasterManager.GetAllVoucherByTalent().Where(m => m.TalentId == TalentId && m.VoucherCd.IndexOf(Code, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else
                    {
                        voucher = IMasterManager.GetAllVoucherByTalent().Where(m => m.TalentId == TalentId).ToList();
                    }
                }
                else
                {
                    voucher = IMasterManager.GetAllVoucherByTalent().ToList();
                }

                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = voucher });

                //return new JsonResult(voucher)
                //{
                //    StatusCode = StatusCodes.Status200OK
                //};
            }
            catch (Exception ex)
            {
                List<VoucherAllTalentModel> list = new List<VoucherAllTalentModel>();
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = list });

                throw;
            }
           
        }




        [HttpPost]
        [Route("~/api/Booking/CancelledBooking")]
        public IActionResult CancelledBooking([FromBody] BookModel model)
        {
            try
            {


                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if (getcurrentdata.UserOfTalentId == model.UserId || getcurrentdata.BookedBy == model.UserId)
                {

                    if (getcurrentdata.Status == Convert.ToInt32(BookingFlow.Submit))
                    {
                        getcurrentdata.Status = Convert.ToInt32(BookingFlow.Expired);
                        ITransactionManager.UpdateBookData(getcurrentdata);
                        ITransactionManager.DeleteVoucherTransactionByBookId(model.Id);
                        // return Json(new { Status = "OK", Message = "Success" });
                        return Json(new { Status = StatusCodes.Status200OK, Message = "OK" });
                    }
                    else
                    {
                        // return Json(new { Status = "Error", Message = "Status Order Invalid untuk Cancel Order" });
                        return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Status Order Invalid untuk Cancel Order" });
                    }


                }
                else
                {
                    // return Json(new { Status = "Error", Message = "User Id tidak memiliki Akses" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User Id tidak memiliki Akses" });
                }




            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(model.UserId), "CancelledBooking", ex.Message);
                //return Json(new { Status = "Error", Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("~/api/Booking/PostVoucherCode")]
        public IActionResult PostVoucherCode(int BookId, string VoucherCd, decimal PriceAmount)
        {
            decimal voucherAmt = 0;
            try
            {
                var getdata = IMasterManager.CheckVoucherCode(BookId, VoucherCd, PriceAmount);


                if (getdata != null)
                {
                    voucherAmt = getdata != null ? getdata.Discount : 0;
                    if (voucherAmt > 0)
                    {
                        ITransactionManager.DeleteVoucherTransactionByBookId(BookId);

                        VoucherTransactionModel VOM = new VoucherTransactionModel();
                        BookModel Bmodel = new BookModel();
                        Bmodel.Id = BookId;
                        var getcurrentdata = ITransactionManager.GetDataBook(Bmodel);

                        VOM.BookId = getcurrentdata.Id;
                        VOM.TalentId = getcurrentdata.TalentId.Value;
                        VOM.VoucherCd = VoucherCd;
                        var GetVoucher = IMasterManager.GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                        VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                        VOM.qty = 1;
                        VOM.UserId = Convert.ToInt32(getcurrentdata.BookedBy);
                        VOM.Amount = voucherAmt;

                        ITransactionManager.CreateVoucherTransaction(VOM);

                        if (GetVoucher != null)
                        {
                            VoucherModel VM = new VoucherModel();
                            VM = GetVoucher;
                            VM.RemainingCount = VM.RemainingCount - 1;
                            VM.Budget = VM.Budget - VOM.Amount;
                            IMasterManager.UpdateVoucher(VM);

                        }
                        //return Json(new { Status = "OK", Result = voucherAmt, Message = "Success" });
                        return Json(new { Status = StatusCodes.Status200OK, Message = "OK" ,result= voucherAmt });
                    }
                    else
                    {
                        //return Json(new { Status = "Error", Result = voucherAmt, Message = "Quota untuk penggunakan voucher ini sudah habis" });
                        return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Quota untuk penggunakan voucher ini sudah habis", result = voucherAmt });
                    }



                }
                else
                {
                    //return Json(new { Status = "Error", Result = voucherAmt, Message = "Voucher tidak valid atau sudah tidak berlaku lagi" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Voucher tidak valid atau sudah tidak berlaku lagi", result = voucherAmt });
                }
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "PostVoucherCode", ex.Message);
                // return Json(new { Status = "Error", Result = voucherAmt, Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = voucherAmt });

            }

        }

        [HttpGet]
        [Route("~/api/Booking/VerifyVoucherCode")]
        public IActionResult VerifyVoucherCode(int BookId, string VoucherCd, decimal PriceAmount)
        {

            decimal voucherAmt = 0;
            try
            {
                var getdata = IMasterManager.CheckVoucherCode(BookId, VoucherCd, PriceAmount);
                voucherAmt = getdata.Discount;
                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = voucherAmt });
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(0, "VerifyVoucherCode", ex.Message);
                //return Json(voucherAmt);
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = voucherAmt });

                throw ex;
            }
            //return new JsonResult(voucherAmt)
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }







    }
}