using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.General.Model.Admin.Transaction.API.FasPay;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jingl.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;
        private readonly BookingController BookingController;
        private IConfiguration _config;


        public PaymentController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.BookingController = new BookingController(config, cookie);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
            this._config = config;
        }

        [HttpPost]
        [Route("~/api/Payment/PaymentNotification")]
        public ActionResult PaymentNotification([FromBody] TransactionResultModel body)
        {
            try
            {
                var model = new TransactionResultModel();
                var userdata = new UserModel();
                //HelperController.ApiPost<,>
                // sample 
                //va_numbers vadata = new va_numbers();
                //payment_amounts payment_amounts = new payment_amounts();

                //vadata.bank = "bni";
                //vadata.va_number = "9881118886068330";
                //model.transaction_time = "2019-03-26 17:11:14";
                //model.gross_amount = 5000;
                //model.currency = "IDR";
                //model.order_id = "ORD-2019000090";
                //model.payment_type = "bank_transfer";
                //model.signature_key = "d78a87f2a1d1443f0e557cbbd140f392f8f5b9d51cc8a3e8f73e9ef9a6ba820e70bf34412c3ddf694b482dd63d798bba3d15f27539ca62b382c389e7edf00a22";
                //model.status_code = "200";
                //model.transaction_id = "565af41c-2328-4377-aa16-04105976cfec";
                //model.transaction_status = "settlement";
                //model.fraud_status = "accept";
                //model.status_message = "midtrans payment notification";

                //body = model;


                //

                 var IsValid = HelperController.VerifyPaymentNotificationOrder(body).Result;
               // var IsValid =true;

                if (body.status_code == "202")
                {
                    IsValid = true;
                }

                if (IsValid == true)
                {
                    var getcurrentdata = ITransactionManager.GetDataBookByOrderId(body.order_id);
                    var firstName = "";
                    userdata.Id = getcurrentdata.Id;
                    userdata = IUserManagementManager.GetUser(userdata);
                    firstName = userdata.FirstName;

                    PaymentBookLogModel paymodel = new PaymentBookLogModel();
                    paymodel.BookId = getcurrentdata.Id;
                    paymodel.OrderId = body.order_id;
                    paymodel.SnapToken = body.transaction_id;
                    paymodel.StatusCode = body.status_code;
                    paymodel.TransactionStatus = body.transaction_status;
                    paymodel.CreatedDate = DateTime.Now;
                    ITransactionManager.CreatePaymentBookLog(paymodel);

                    if (body.status_code == "200")
                    {

                        if (getcurrentdata != null)
                        {
                            if (getcurrentdata.Status < 3 && getcurrentdata.Status >= 0)
                            {
                                TalentModel Talent = new TalentModel();
                                Talent.Id = Convert.ToInt32(getcurrentdata.TalentId);
                                Talent = IMasterManager.GetTalent(Talent);

                                getcurrentdata.TotalPay = body.gross_amount;
                                getcurrentdata.SnapToken = body.transaction_id;
                                getcurrentdata.PriceAmount = body.gross_amount;
                                getcurrentdata.PayMethod = body.payment_type;
                                getcurrentdata.PaymentChannel = body.payment_type;
                                getcurrentdata.UpdatedBy = "GOPAY";
                                //  getcurrentdata.Status = (int)BookingFlow.Paid;
                                getcurrentdata.PaymentShare = Talent != null ? Talent.PaymentShare : 0;
                                getcurrentdata.SalePrice = Talent != null ? Talent.SalePrice : 0;
                                getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                                getcurrentdata = ITransactionManager.UpdateBookData(getcurrentdata);

                            }


                           // COMMENT By Erdendi Because Remaining Voucher has decreased when user using Promo Code
                            // if ((getcurrentdata.VoucherCode).Length > 0)
                            //if (!string.IsNullOrEmpty(getcurrentdata.VoucherCode))
                            //{
                            //    VoucherTransactionModel VOM = new VoucherTransactionModel();
                            //    VOM.BookId = paymodel.BookId.Value;
                            //    VOM.TalentId = getcurrentdata.TalentId.Value;
                            //    VOM.VoucherCd = getcurrentdata.VoucherCode;
                            //    var GetVoucher = IMasterManager.GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                            //    VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                            //    VOM.Amount = getcurrentdata.Potongan.Value;
                            //    ITransactionManager.CreateVoucherTransaction(VOM);

                                //    if (GetVoucher != null)
                                //    {
                                //        VoucherModel VM = new VoucherModel();
                                //        VM = GetVoucher;
                                //        VM.RemainingCount = VM.RemainingCount - 1;
                                //        VM.Budget = VM.Budget - VOM.Amount;
                                //        IMasterManager.UpdateVoucher(VM);

                                //    }

                                //}

                            var Userdata = new UserModel();
                            var Talentdata = new TalentModel();
                            var TalentUserdata = new UserModel();

                            Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                            Talentdata.Id = getcurrentdata.TalentId.Value;
                            Talentdata = IMasterManager.GetTalent(Talentdata);

                            TalentUserdata.Id = Talentdata.UserId.Value;
                            TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                            HelperController.NotificationEmail(TalentUserdata.Email, "2", EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, firstName).Wait();

                        }




                        // ITransactionManager.UpdateBookData(getcurrentdata);

                    }
                    else if (body.status_code == "202")
                    {
                        if (getcurrentdata != null)
                        {
                            getcurrentdata.TotalPay = body.gross_amount;
                            getcurrentdata.SnapToken = body.transaction_id;
                            getcurrentdata.PriceAmount = body.gross_amount;
                            getcurrentdata.PayMethod = body.payment_type;
                            getcurrentdata.Status = (int)BookingFlow.Expired;
                            ITransactionManager.UpdateBookData(getcurrentdata);


                        }
                    }
                    else if (body.status_code == "201")
                    {
                        if (getcurrentdata != null)
                        {


                            getcurrentdata.TotalPay = body.gross_amount;
                            getcurrentdata.SnapToken = body.transaction_id;
                            getcurrentdata.PriceAmount = body.gross_amount;
                            getcurrentdata.PayMethod = body.payment_type;
                            getcurrentdata.Status = (int)BookingFlow.WaitingPayment;
                            ITransactionManager.UpdateBookData(getcurrentdata);


                        }
                    }
                    //else 
                    //{

                    //}
                }

            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "PaymentNotification", ex.Message);

            }
            return StatusCode(200);
        }


        [HttpPost]
        [Route("~/api/Payment/PayNotif")]
        public ActionResult PayNotif([FromBody] PaymentNotificationRequest body)
        {
            try
            {

                var getcurrentdata = ITransactionManager.GetDataBookByOrderId(body.bill_no);

                if (getcurrentdata.Status < 3 && getcurrentdata.Status >= 0)
                {
                    PaymentBookLogModel paymodel = new PaymentBookLogModel();
                    paymodel.BookId = getcurrentdata.Id;
                    paymodel.OrderId = body.bill_no;
                    paymodel.SnapToken = body.trx_id;
                    paymodel.StatusCode = body.payment_status_code;
                    paymodel.TransactionStatus = body.payment_status_code;
                    paymodel.CreatedDate = DateTime.Now;
                    ITransactionManager.CreatePaymentBookLog(paymodel);


                    getcurrentdata.TotalPay = body.bill_total;
                    getcurrentdata.SnapToken = body.trx_id;
                    getcurrentdata.PriceAmount = body.bill_total;
                    // getcurrentdata.PayMethod = body.payment_channel;
                    //  getcurrentdata.Status = (int)BookingFlow.Paid;
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;
                    ITransactionManager.UpdateBookData(getcurrentdata);
                }

                var Userdata = new UserModel();
                var Talentdata = new TalentModel();
                var TalentUserdata = new UserModel();

                Userdata.Id = Convert.ToInt32(getcurrentdata.BookedBy);
                Talentdata.Id = getcurrentdata.TalentId.Value;
                Talentdata = IMasterManager.GetTalent(Talentdata);

                TalentUserdata.Id = Talentdata.UserId.Value;
                TalentUserdata = IUserManagementManager.GetUser(TalentUserdata);

                HelperController.NotificationEmail(TalentUserdata.Email, "2", EmailTargetType.Talent, getcurrentdata.Id, getcurrentdata.OrderNo, Userdata.FirstName).Wait();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "PayNotif", ex.Message);
            }

            return Json(new { 
                response = "Payment Notification",
                trx_id = body.trx_id,
                merchant_id = body.merchant_id,
                merchant = body.merchant,
                bill_no = body.bill_no,
                response_code = "00",
                response_desc = "Sukses",
                response_date = DateTime.Now,
                Status = "OK"
            });
        }

        public IActionResult PaymentFinish()
        {
            return StatusCode(200);
        }
        public IActionResult PaymentUnFinish()
        {
            return StatusCode(200);
        }

        public IActionResult PaymentError()
        {
            return StatusCode(200);
        }

        public IActionResult Pembayaran()
        {

            return View();
        }

        public IActionResult PayResponse(string trx_id, string merchant_id, string bill_no, string bill_ref,
                                             decimal bill_total, string bank_user_name, string status, string signature)


        {
            if (status == "1")
            {
                return RedirectToAction("PaymentSuccess", "Booking");
            }
            else
            {
                return RedirectToAction("PaymentSuccess", "Booking");
            }

        }

        public IActionResult FasPayPembayaran(BookModel model)
        {
            return PartialView("~/Views/Payment/FasPayPembayaran.cshtml", model);
        }


        [HttpPost]
        [Route("~/api/Payment/PayFasPay")]
        public async Task<IActionResult> PayFasPay([FromBody]BookModel model)
        {
            var response = new TransactionPostResponse();
            try
            {
                var getcurrentdata = ITransactionManager.GetDataBook(model);

                if(getcurrentdata.BookedBy == model.UserId)
                {
                    var config = HelperController.GetFasPayConfig();
                    var firstname = "";
                    var Userdata = new UserModel();

                    Userdata.Id = Convert.ToInt32(model.UserId);
                    Userdata = IUserManagementManager.GetUser(Userdata);

                    firstname = Userdata.FirstName;

                    TransactionPostModel postModel = new TransactionPostModel();
                    List<item> item = new List<item>();

                    string PaymentChannel = "";

                    if (model.BankSelected == "Mandiri")
                    {
                        PaymentChannel = "802";
                    }
                    else if (model.BankSelected == "BNI")
                    {
                        PaymentChannel = "801";
                    }
                    else if (model.BankSelected == "BRI")
                    {
                        PaymentChannel = "800";
                        //PaymentChannel = "802029";

                    }

                    else if (model.BankSelected == "ovo")
                    {
                        PaymentChannel = "812";
                    }
                    else if (model.BankSelected == "BCA")
                    {
                        PaymentChannel = "702";
                    }


                    item itemdata = new item();
                    itemdata.product = getcurrentdata.ProjectNm;
                    itemdata.qty = "1";
                    itemdata.amount = Convert.ToInt32(getcurrentdata.TotalPay.Value * 100).ToString();
                    itemdata.payment_plan = "01";
                    itemdata.merchant_id = PaymentChannel;
                    itemdata.tenor = "00";

                    var getParameter = IMasterManager.AdmGetAllParameter().ToList();
                    var getPaymentDeadline = getParameter.Where(x => x.ParamName == "Deadline" && x.ParamCode == "Payment").FirstOrDefault();
                    var getFaspayPaymentDeadline = getParameter.Where(x => x.ParamName == "Deadline" && x.ParamCode == "FasPayPayment").FirstOrDefault();
                    UserModel usm = new UserModel();
                    usm.Id = model.UserId;
                    usm = IUserManagementManager.GetUser(usm);
                    var _UserCode = usm != null ? usm.UserCode : "";
                    var _PhoneNumber = usm != null ? usm.PhoneNumber : "";
                    var _Email = usm != null ? usm.Email : "";
                    //var _UserCode = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.UserCode).FirstOrDefault();
                    //var _PhoneNumber = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.PhoneNumber).FirstOrDefault();
                    //var _Email = IUserManagementManager.GetAllUser().Where(m => m.Id == model.UserId).Select(m => m.Email).FirstOrDefault();
                    item.Add(itemdata);
                    postModel.item = item;
                    postModel.request = getcurrentdata.OrderNo;
                    postModel.merchant_id = config.MerchantId;
                    postModel.merchant = config.MerchantName;
                    postModel.bill_no = getcurrentdata.OrderNo;
                    postModel.bill_desc = "Pembayaran " + getcurrentdata.OrderNo;
                    postModel.pay_type = "1";
                    postModel.bill_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    postModel.bill_expired = DateTime.Now.AddDays(Convert.ToInt32(getFaspayPaymentDeadline.ParamValue)).ToString("yyyy-MM-dd HH:mm:ss");
                    postModel.bill_gross = Convert.ToInt32(getcurrentdata.TotalPay.Value * 100).ToString();
                    postModel.bill_total = Convert.ToInt32(getcurrentdata.TotalPay.Value * 100).ToString();
                    postModel.bill_currency = "IDR";
                    //postModel.cust_no = model.UserCode;
                    postModel.cust_no = _UserCode;
                    postModel.cust_name = model.CustomerName;
                    //postModel.msisdn = model.PhoneNumber;
                    postModel.msisdn = _PhoneNumber;
                    //postModel.email = model.Email;
                    postModel.email = _Email;
                    postModel.terminal = "10";
                    postModel.signature = HelperController.GenerateSignatureKey(config.UserId + config.Password + getcurrentdata.OrderNo);
                    postModel.payment_channel = PaymentChannel;

                    response = await HelperController.FasPayApiPost<TransactionPostResponse, TransactionPostModel>(postModel, _config.GetSection("FastPay:PostTransaction").Value.ToString());
                    Console.Write(postModel);




                    // var getcurrentdata = ITransactionManager.GetDataBookByOrderId(model.OrderNo);
                    // getcurrentdata.TotalPay = model.TotalPay.Value;
                    getcurrentdata.SnapToken = response.trx_id;
                    // getcurrentdata.PriceAmount = model.PriceAmount.Value;
                    //getcurrentdata.VoucherCode = getcurrentdata.VoucherCode;
                    //getcurrentdata.Potongan = model.Potongan.Value;



                    if (model.BankSelected == "ovo")
                    {
                        getcurrentdata.PayMethod = "ovo";
                    }
                    else
                    {
                        getcurrentdata.PayMethod = "bank_transfer";
                    }


                    TalentModel Talentdata = new TalentModel();
                    Talentdata.Id = Convert.ToInt32(getcurrentdata.TalentId);
                    Talentdata = IMasterManager.GetTalent(Talentdata);

                    getcurrentdata.PaymentChannel = postModel.payment_channel;
                    getcurrentdata.ExpiredDate = DateTime.Now.AddDays(Convert.ToInt32(getPaymentDeadline.ParamValue));
                    getcurrentdata.Status = (int)BookingFlow.WaitingPayment;
                    getcurrentdata.PaymentShare = Talentdata != null ? Talentdata.PaymentShare : 0;
                    getcurrentdata.SalePrice = Talentdata != null ? Talentdata.SalePrice : 0;
                    getcurrentdata.VoucherCode = getcurrentdata.VoucherCode;
                    ITransactionManager.UpdateBookData(getcurrentdata);

                    //if (!string.IsNullOrEmpty(model.VoucherCode))
                    //{
                    //    var voucherModel = IMasterManager.GetVoucherByCode(model.VoucherCode);
                    //    voucherModel.IsClaimed = true;
                    //    voucherModel.IsUsed = true;
                    //    voucherModel.UpdatedBy = HelperController.GetCookie("UserId");
                    //    IMasterManager.UpdateVoucher(voucherModel);
                    //}

                    PaymentBookLogModel paymodel = new PaymentBookLogModel();
                    paymodel.BookId = getcurrentdata.Id;
                    paymodel.OrderId = response.bill_no;
                    paymodel.SnapToken = response.trx_id;
                    paymodel.StatusCode = response.response_code;
                    paymodel.TransactionStatus = response.response_desc;
                    paymodel.CreatedDate = DateTime.Now;
                    ITransactionManager.CreatePaymentBookLog(paymodel);

                    HelperController.NotificationEmail(getcurrentdata.Email, getcurrentdata.Status.ToString(), EmailTargetType.User, getcurrentdata.Id, getcurrentdata.OrderNo, firstname).Wait();
                    return Json(response);
                }
                else
                {
                    return Json("Error");
                }


                /// Deactivate Saldo
                /// 

                //if (Talentdata != null)
                //{
                //    if (Talentdata.IsPriority)
                //    {
                //        SaldoModel saldoData = new SaldoModel();
                //        saldoData.TalentId = Talentdata.Id;
                //        saldoData = ITransactionManager.GetSaldoByTalentId(saldoData);
                //        if (saldoData != null)
                //        {
                //            saldoData.SaldoUsedAmt += getcurrentdata.TotalPay;
                //            saldoData = ITransactionManager.UpdateSaldo(saldoData);
                //        }
                //    }
                //}

                // COMMENT By Erdendi Because Remaining Voucher has decreased when user using Promo Code
                //if(!string.IsNullOrEmpty(getcurrentdata.VoucherCode))
                //{
                //    VoucherTransactionModel VOM = new VoucherTransactionModel();
                //    VOM.BookId = paymodel.BookId.Value;
                //    VOM.TalentId = Talentdata.Id;
                //    VOM.VoucherCd = getcurrentdata.VoucherCode;
                //    var GetVoucher = IMasterManager.GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                //    VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                //    VOM.qty = 1;
                //    VOM.UserId = Convert.ToInt32(Userdata.Id);
                //    VOM.Amount = getcurrentdata.Potongan.Value;

                //    ITransactionManager.CreateVoucherTransaction(VOM);

                //    if(GetVoucher != null)
                //    {
                //        VoucherModel VM = new VoucherModel();
                //        VM = GetVoucher;
                //        VM.RemainingCount = VM.RemainingCount - 1;
                //        VM.Budget = VM.Budget - VOM.Amount;
                //        IMasterManager.UpdateVoucher(VM);
                //    }

                //}


               
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(model.UserId), "PayFasPay", ex.Message);
                return Json("Error");
            }
          
        }

        public IActionResult UpdateWaitForPaymentStatus(string json, string bookId)
        {

            try
            {
                var model = new TransactionResultModel();
                var PayLog = new PaymentBookLogModel();

                model = JsonConvert.DeserializeObject<TransactionResultModel>(json);
                PayLog.OrderId = model.order_id;
                PayLog.BookId = Convert.ToInt32(bookId);
                PayLog.SnapToken = model.transaction_id;
                PayLog.StatusCode = model.status_code;
                PayLog.TransactionStatus = model.transaction_status;

                ITransactionManager.CreatePaymentBookLog(PayLog);


                //Controller.UpdateBookingStatus(Convert.ToInt32(bookId), 2);

                BookModel modeldata = new BookModel();
                modeldata.Id = Convert.ToInt32(bookId);
                var getcurrentdata = ITransactionManager.GetBookConfirmation(modeldata);
                getcurrentdata.UpdatedBy = "GOPAY";
                //getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId");
                //getcurrentdata.TotalPay = model.gross_amount;
                getcurrentdata.SnapToken = model.transaction_id;
                //getcurrentdata.PriceAmount = model.gross_amount;
                //getcurrentdata.Potongan = model.gross_amount;
                if (model.status_code == "201")
                {
                    getcurrentdata.Status = (int)BookingFlow.WaitingPayment;
                }
                else if (model.status_code == "200")
                {
                    //getcurrentdata.Status = (int)BookingFlow.Paid;
                    getcurrentdata.Status = (int)BookingFlow.ProjectAccepted;

                    if (!string.IsNullOrEmpty(getcurrentdata.VoucherCode))
                    {
                        var voucherModel = IMasterManager.GetVoucherByCode(getcurrentdata.VoucherCode);
                        if (voucherModel != null)
                        {
                            voucherModel.IsClaimed = true;
                            voucherModel.IsUsed = true;
                            voucherModel.UpdatedBy = "GOPAY";
                            IMasterManager.UpdateVoucher(voucherModel);
                        }

                        TalentModel Talentdata = new TalentModel();
                        Talentdata.Id = Convert.ToInt32(getcurrentdata.TalentId);
                        Talentdata = IMasterManager.GetTalent(Talentdata);
                        
                    }
                }

                getcurrentdata.IsActive = (int)Status.Active;
                getcurrentdata.PayMethod = model.payment_type;

                var getCurrentData = ITransactionManager.UpdateBookData(getcurrentdata);


                //HelperController.ConvertJsonToClass<TransactionResultModel>(json);


                return Json("Ok");
            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(HelperController.GetCookie("UserId")), "Index", ex.Message);

                return Json("Error");
                throw;
            }



        }

    }
}