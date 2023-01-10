using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
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
    public class RefundController : Controller
    {
        private readonly FilesController FilesController;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public RefundController(IConfiguration config)
        {
            this.FilesController = new FilesController(config);
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        [HttpPost]
        [Route("~/api/Refund/RequestRefund")]
        public IActionResult RequestRefund([FromBody] BookModel bmodel)
        {
            try
            {

                var getBookData = ITransactionManager.GetDataBookByOrderId(bmodel.OrderNo);

                if (getBookData != null)
                {
                    if (getBookData.Status == Convert.ToInt32(BookingFlow.Refund))
                    {
                        if (getBookData.BookedBy == bmodel.UserId)
                        {
                            var refundData = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == bmodel.OrderNo).FirstOrDefault();


                            if (refundData != null)
                            {
                                RefundModel model = new RefundModel();
                                // return Json(new { result = model, Status = "Error", Message = "Order sudah di Refund sebelumnya" });
                                return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Order sudah di Refund sebelumnya", result = model });

                            }
                            else
                            {
                                RefundModel model = new RefundModel();
                                UserModel userModel = new UserModel();
                                userModel.Id = getBookData.BookedBy.Value;
                                var getUser = IUserManagementManager.GetUser(userModel);

                                model.OrderNo = getBookData.OrderNo;
                                model.UserCode = getUser.UserCode;
                                model.CreatedBy = getBookData.CreatedBy;
                                model.Amount = getBookData.TotalPay.Value;
                                model.BookId = Convert.ToInt32(getBookData.Id);
                                model.AccountNumber = getUser.AccountNumber;
                                model.CustomerName = getUser.UserName;
                                model.BeneficiaryName = getUser.BeneficiaryName;
                                model.BankName = getUser.BankName;
                                model.BankCode = getUser.Bank;
                                model.UserId = getUser.Id.ToString();

                                return Json(new { Status = StatusCodes.Status200OK, Message = "OK", result = model });



                                // return Json(new { result = model, Status = "OK", Message = "OK" });
                            }
                        }
                        else
                        {
                            RefundModel model = new RefundModel();
                            // return Json(new { result = model, Status = "Error", Message = "User Id tidak memiliki Akses" });
                            return Json(new { Status = StatusCodes.Status400BadRequest, Message = "User Id tidak memiliki Akses", result = model });

                        }
                    }
                    else
                    {
                        RefundModel model = new RefundModel();
                        //return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
                        return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Order tidak valid", result = model });

                    }



                }
                else
                {
                    RefundModel model = new RefundModel();
                    // return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
                    return Json(new { Status = StatusCodes.Status400BadRequest, Message = "Order tidak valid", result = model });

                }





            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(bmodel.UserId), "RequestRefund", ex.Message);
                RefundModel model = new RefundModel();
                // return Json(new { result = model, Status = "Error", Message = ex.Message });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = model });


                // return RedirectToAction("NotFound", "NoAccess");
            }

        }

        [HttpPost]
        [Route("~/api/Refund/PostRequestRefund")]
        public IActionResult PostRequestRefund([FromBody]RefundModel rmodel)
        {
            try
            {

                var getBookData = ITransactionManager.GetDataBookByOrderId(rmodel.OrderNo);

                if (getBookData != null)
                {
                    if (getBookData.Status == Convert.ToInt32(BookingFlow.Refund))
                    {
                        if (getBookData.BookedBy.Value.ToString() == rmodel.UserId)
                        {
                            var refundData = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == rmodel.OrderNo).FirstOrDefault();


                            if (refundData != null)
                            {
                                ClaimModel model = new ClaimModel();
                                return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Order sudah di Refund sebelumnya" });

                            }
                            else
                            {
                                if (rmodel.BankCode == null || rmodel.AccountNumber == null || rmodel.BeneficiaryName == null)
                                {

                                    if (rmodel.BankCode == null)
                                    {
                                        ClaimModel model = new ClaimModel();
                                        return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Bank Code kosong" });
                                    }
                                    else if (rmodel.AccountNumber == null)
                                    {
                                        ClaimModel model = new ClaimModel();
                                        return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Account Number kosong" });
                                    }
                                    else if (rmodel.BeneficiaryName == null)
                                    {
                                        ClaimModel model = new ClaimModel();
                                        return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "BeneficiaryName kosong" });
                                    }


                                }
                                else
                                {
                                    var getdatabank = IMasterManager.GetAllBank().Where(x => x.BankCode == rmodel.BankCode).FirstOrDefault();

                                    if (getdatabank == null)
                                    {
                                        ClaimModel model = new ClaimModel();
                                        return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Bank Code yang dimasukkan tidak tersedia" });
                                    }
                                    else
                                    {
                                        UserModel userModel = new UserModel();
                                        userModel.Id = Convert.ToInt32(rmodel.UserId);
                                        var getUser = IUserManagementManager.GetUser(userModel);


                                        rmodel.CustomerName = getUser.UserName;
                                        rmodel.BankName = rmodel.BankCode;
                                        rmodel.RequestDate = DateTime.Now;
                                        rmodel.Amount = getBookData.TotalPay.HasValue ? getBookData.TotalPay.Value : 0;
                                        rmodel.Status = (int)Registration.Submit;
                                        rmodel.CreatedBy = rmodel.UserId;
                                        ITransactionManager.CreateRefund(rmodel);

                                        var getClaim = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == rmodel.OrderNo).FirstOrDefault();

                                        getUser.AccountNumber = rmodel.AccountNumber;
                                        getUser.Bank = rmodel.BankCode;
                                        getUser.BeneficiaryName = rmodel.BeneficiaryName;
                                        IUserManagementManager.UpdateUser(getUser);

                                        BookModel BModel = new BookModel();

                                        var temp = ITransactionManager.GetDataBookByOrderId(rmodel.OrderNo);
                                        var getBookingData = ITransactionManager.GetDataBook(temp);

                                        if (getBookingData != null)
                                        {
                                            getBookingData.Status = (int)BookingFlow.RefundSubmitted;
                                            ITransactionManager.UpdateBookData(getBookingData);

                                            HelperController.NotificationEmail(getBookingData.Email, getBookingData.Status.ToString(), EmailTargetType.User, getBookingData.Id, getBookingData.OrderNo, getUser.FirstName).Wait();

                                        }
                                        return Json(new { result = getClaim, Status = StatusCodes.Status200OK, Message = "OK" });
                                    }




                                }


                                //  return Json(new { result = model, Status = "OK",Message = "OK" });
                            }
                            ClaimModel models = new ClaimModel();
                            return Json(new { result = models, Status = StatusCodes.Status400BadRequest, Message = " Data yang dimasukkan salah" });
                        }
                        else
                        {
                            ClaimModel model = new ClaimModel();
                            return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "User Id tidak memiliki Akses" });
                        }
                    }
                    else
                    {
                        ClaimModel model = new ClaimModel();
                        return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Order tidak valid" });
                    }
                }
                else
                {
                    ClaimModel model = new ClaimModel();
                    return Json(new { result = model, Status = StatusCodes.Status400BadRequest, Message = "Order tidak valid" });
                }


            }
            catch (Exception ex)
            {
                BookModel bm = new BookModel();
                HelperController.InsertLog(Convert.ToInt32(rmodel.UserId), "PostRequestRefund", ex.Message);
                //return Json(new { result = bm, Status = "Error", Message = "Error" });
                return Json(new { Status = StatusCodes.Status400BadRequest, Message = ex.Message, result = bm });

                throw ex;
            }
        }

        [HttpPost]
        [Route("~/api/Refund/GetDetailRefund")]
        public IActionResult GetDetailRefund([FromBody]RefundModel rmodel)
        {
            try
            {
                var getClaimData = ITransactionManager.GetAllRefund().Where(x => x.OrderNo == rmodel.OrderNo).FirstOrDefault();

                if (getClaimData != null)
                {
                    if (getClaimData.CreatedBy != rmodel.UserId)
                    {
                        ClaimModel model = new ClaimModel();
                        return Json(new { result = model, Status = "Error", Message = "User Id tidak memiliki Akses" });

                    }
                    else
                    {
                        return Json(new { result = getClaimData, Status = "OK", Message = "OK" });
                    }
                }
                else
                {
                    ClaimModel model = new ClaimModel();
                    return Json(new { result = model, Status = "Error", Message = "Order tidak valid" });
                }



            }
            catch (Exception ex)
            {

                ClaimModel model = new ClaimModel();
                HelperController.InsertLog(Convert.ToInt32(rmodel.UserId), "GetDetailRefund", ex.Message);
                return Json(new { result = model, Status = "Error", Message = "Error" });
                throw ex;
            }
        }
    }
}