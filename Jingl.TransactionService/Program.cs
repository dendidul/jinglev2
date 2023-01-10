using System;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
//using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using SendGrid;
using SendGrid.Helpers.Mail;
using Jingl.General.Model.Admin.UserManagement;
using System.Text;
using System.Security.Cryptography;

namespace Jingl.TransactionService
{
    class Program
    {

        private const string passPhrase = "Pas5pr@se";
        private const string saltValue = "s@1tValue";
        private const string hashAlgorithm = "SHA1";
        private const int passwordIterations = 2;
        private const string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        private const int keySize = 256;


        public IDbConnection Connection
        {
            get
            {
                //return new SqlConnection("Server=tcp:sophielastic.database.windows.net,1433;Initial Catalog=JINGL_STG;Persist Security Info=False;User ID=sophielastic;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                //return new SqlConnection("Server=tcp:jinglprod.database.windows.net,1433;Initial Catalog=JINGPROD;Persist Security Info=False;User ID=jinglprod;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                //return new SqlConnection("Server=tcp:jingldbserver.database.windows.net,1433;Initial Catalog=Fameoprod;Persist Security Info=False;User ID=jingldbserversa;Password=jingldbserver1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                return new SqlConnection("Server=tcp:fameodb.database.windows.net,1433;Initial Catalog=Fameoprd;Persist Security Info=False;User ID=fameodb;Password=azurefameo1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                //return new SqlConnection("Server=tcp:fameodb.database.windows.net,1433;Initial Catalog=Fameoprd;Persist Security Info=False;User ID=fameodb;Password=azurefameo1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");


                //  return new SqlConnection("Server=localhost\\sqlexpress;Initial Catalog=FameoPrd;Persist Security Info=False;User ID=dendi;Password=osama123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=3600;");
            }
        }

        static void Main(string[] args)
        {
            ProcessTransactionJob().Wait();
        }

        [NoAutomaticTriggerAttribute]
        public static async Task ProcessTransactionJob()
        {
            while (true)
            {
                try
                {

                    Program Program = new Program();
                    Program.SendNotification();                   
                    Program.ProcessSetTalentVideo();
                    Program.RefundBooking();
                    Program.CompletedBooking();
                    Program.CalculationIncome();
                    //Program.CalculateTalentLevelAndRate();
                    Program.CancelledBooking();
                    Program.UpdatePeriod();
                    Program.DeactivateVoucher();
                    //  Program.updatePassword();



                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred in processing pending requests. Error : {0}", ex.Message);

                }
                await Task.Delay(TimeSpan.FromMinutes(3));
            }
        }

        public static string Encrypt(string plainString)
        {
            string encryptedString = string.Empty;
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainString);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            encryptedString = Convert.ToBase64String(cipherTextBytes);


            return encryptedString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedString)
        {
            string decryptedString = string.Empty;
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedString);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            decryptedString = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.  
            return decryptedString;
        }


        public void DeactivateVoucher()
        {
            try
            {

                Console.WriteLine("...................");
                Console.WriteLine("Start Deactivate Expired Voucher");

                var GetVoucher = GetAllVoucher().Where(x => x.EndDate.Value < DateTime.Now && x.IsActive == 1).ToList();

                if(GetVoucher.Count() > 0)
                {
                    foreach (var i in GetVoucher)
                    {

                        VoucherModel Vm = new VoucherModel();
                        Vm = i;
                        Vm.IsActive = 0;
                        UpdateVoucher(Vm);

                        Console.WriteLine("Done Deactivate Voucher : " + i.VoucherCd);

                    }

                    Console.WriteLine("Done Deactivate Voucher");
                    Console.WriteLine("...................");

                }
                else
                {
                    Console.WriteLine("No Voucher has deactivate");
                    Console.WriteLine("...................");
                }



            }
            catch (Exception ex)
            {

                throw;
            }

        }
            

        public void CancelledBooking()
        {
            try
            {

                var getlist = GetListBook((int)CheckingBook.CheckingExpiredPaymentTime);
                var getSubmitlist = GetListBook((int)CheckingBook.CheckingExpiredForSubmitStatus);
                if (getlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Cancel for Expired Book from Waiting for Payment");

                    foreach (var i in getlist)
                    {
                        BookLogModel model = new BookLogModel();
                        model.BookId = i.Id;
                        model.Status = (int)BookingFlow.Expired;
                        Console.WriteLine("Start Cancel for Order No : " + i.OrderNo);
                        //SetCancelledBookById(i.Id);
                        UpdateStatusBooking(i.Id, (int)BookingFlow.Expired);

                        if(!string.IsNullOrEmpty(i.VoucherCode))
                        {
                            VoucherTransactionModel VOM = new VoucherTransactionModel();
                            VOM.BookId = i.Id;
                            VOM.TalentId = i.TalentId.Value;
                            VOM.VoucherCd = i.VoucherCode;
                            var GetVoucher = GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                            VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                            VOM.qty = -1;
                            VOM.Amount = i.Potongan.Value;
                            VOM.UserId = i.BookedBy.Value;

                           CreateVoucherTransaction(VOM);

                            if (GetVoucher != null)
                            {
                                VoucherModel VM = new VoucherModel();
                                VM = GetVoucher;
                                VM.RemainingCount = VM.RemainingCount + 1;
                                VM.Budget = VM.Budget + i.Potongan.Value;
                                UpdateVoucher(VM);

                            }

                        }

                        InsertBookLog(model);

                        //NotificationModel notifmodel = new NotificationModel();
                        //notifmodel.BookId = i.Id;
                        //notifmodel.NotifType = "U";
                        //notifmodel.To = i.BookedBy;
                        //notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "UNotifBook" && x.ParamCode == i.Status.ToString()).FirstOrDefault().ParamValue;
                        //InputNotification(notifmodel);
                        NotificationEmail(i.Email, model.Status.ToString(), EmailTargetType.User, i).Wait();


                        Console.WriteLine("Done Cancel for Order No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Cancel for Expired Book");
                    Console.WriteLine("...................");
                }

                if(getSubmitlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Cancel for Expired Book From Submit Status");

                    foreach (var i in getSubmitlist)
                    {
                        BookLogModel model = new BookLogModel();
                        model.BookId = i.Id;
                        model.Status = (int)BookingFlow.Expired;
                        Console.WriteLine("Start Cancel for Order No : " + i.OrderNo);
                        //SetCancelledBookById(i.Id);
                        UpdateStatusBooking(i.Id, (int)BookingFlow.Expired);


                        if (!string.IsNullOrEmpty(i.VoucherCode))
                        {
                            VoucherTransactionModel VOM = new VoucherTransactionModel();
                            VOM.BookId = i.Id;
                            VOM.TalentId = i.TalentId.Value;
                            VOM.VoucherCd = i.VoucherCode;
                            var GetVoucher = GetAllVoucher().Where(x => x.VoucherCd.ToLower() == VOM.VoucherCd.ToLower()).FirstOrDefault();
                            VOM.VoucherId = GetVoucher != null ? GetVoucher.Id : 0;
                            VOM.qty = -1;
                            VOM.UserId = i.BookedBy.Value;

                            CreateVoucherTransaction(VOM);

                            if (GetVoucher != null)
                            {
                                VoucherModel VM = new VoucherModel();
                                VM = GetVoucher;
                                VM.RemainingCount = VM.RemainingCount + 1;
                                UpdateVoucher(VM);

                            }
                        }


                        InsertBookLog(model);

                        //NotificationModel notifmodel = new NotificationModel();
                        //notifmodel.BookId = i.Id;
                        //notifmodel.NotifType = "U";
                        //notifmodel.To = i.BookedBy;
                        //notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "UNotifBook" && x.ParamCode == i.Status.ToString()).FirstOrDefault().ParamValue;
                        //InputNotification(notifmodel);
                        NotificationEmail(i.Email, model.Status.ToString(), EmailTargetType.User, i).Wait();


                        Console.WriteLine("Done Cancel for Order No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Cancel for Expired Book");
                    Console.WriteLine("...................");
                }

                Console.WriteLine(".");





            }
            catch (Exception ex)
            {

            }
        }

        public void SendNotification() /// send notif untuk booking yang sebentar lagi akan payment expired dan order yang sebentar lagi akan melampaui batas deadline pengerjaan
        {
            var CheckingForNotifUserWillExpirePayment = GetListBook((int)CheckingBook.CheckingForNotifUserWillExpirePayment);
            var CheckingForNotifTalentBookingNearOfDeadline = GetListBook((int)CheckingBook.CheckingForNotifTalentBookingNearOfDeadline);
            var CheckLimitSaldo = GetAllLimitSaldo();

            if (CheckingForNotifUserWillExpirePayment.Count() > 0)
            {
                foreach(var i in CheckingForNotifUserWillExpirePayment)
                {
                    NotificationModel notifmodel = new NotificationModel();
                    notifmodel.BookId = i.Id;
                    notifmodel.NotifType = "U";
                    notifmodel.To = i.BookedBy;
                    notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "UNotifBook" && x.ParamCode == i.Status.ToString()).FirstOrDefault().ParamValue;
                    InputNotification(notifmodel);
                    //NotificationEmail(i.Email, i.Status.ToString(), EmailTargetType.User, i).Wait();
                }
            }

            if (CheckingForNotifTalentBookingNearOfDeadline.Count() > 0)
            {
                foreach (var i in CheckingForNotifTalentBookingNearOfDeadline)
                {
                    NotificationModel notifmodel = new NotificationModel();
                    notifmodel.BookId = i.Id;
                    notifmodel.NotifType = "U";
                    notifmodel.To = i.BookedBy;
                    notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "TNotifBook" && x.ParamCode == "NearOfDeadline").FirstOrDefault().ParamValue;
                    InputNotification(notifmodel);
                    //EmailNotificationForNearlyDeadlineOrder(i.Email, "NearOfDeadline", EmailTargetType.Talent, i).Wait();
                }
            }

            //if(CheckLimitSaldo.Count() > 0)
            //{
            //    foreach (var i in CheckLimitSaldo)
            //    {
            //        NotificationModel notifmodel = new NotificationModel();
            //        notifmodel.NotifType = "T";
            //        notifmodel.To = i.UserId;
            //        notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "TopupNotif" && x.ParamCode == "Limit").FirstOrDefault().ParamValue;
            //        InputNotification(notifmodel);
            //        EmailLimitSaldo(i.UserEmail, i.TalentNm).Wait();
            //    }
            //}

            //NotificationEmail(sendto, StatusCode, EmailType, model).Wait();
        }


        public void CalculateTalentLevelAndRate()
        {
            try
            {

                var CheckDate = DateTime.Now;

                var scheduleDate = AdmGetAllParameter().Where(x => x.ParamCode == "ClcDateLevel" && x.ParamName == "ClcDateLevel").FirstOrDefault().ParamValue;

                if (CheckDate.Day == Convert.ToInt32(scheduleDate)
                    && CheckDate.Hour == 0
                    )
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Process for Calculation Level and Talent Price");

                    CalculationTalentLevelAndPrice();

                    Console.WriteLine("Done Process for Calculation Level and Talent Price");
                    Console.WriteLine("...................");
                }
                else
                {
                    Console.WriteLine(".");
                }



            }
            catch (Exception ex)
            {

            }
        }


        public void CalculationIncome()
        {
            try
            {

               var getlist = GetListBook((int)CheckingBook.CheckingCompletedBooking);
                getlist = getlist.Where(x => x.IsCalculated == 0).ToList();
                if (getlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Process for Calculation Book");

                    foreach (var i in getlist)
                    {
                        //BookLogModel model = new BookLogModel();
                        //model.BookId = i.Id;
                        //model.Status = (int)BookingFlow.RateTalent;
                        Console.WriteLine("Start Calculation for Order No : " + i.OrderNo);
                        IncomeModel incomeModel = new IncomeModel();
                        incomeModel.UserId = GetTalent(i.TalentId.Value).UserId;
                        incomeModel.SourceIncome = i.OrderNo;
                        incomeModel.Period = i.Period;
                        incomeModel.Income = i.TalentPrice * (i.PaymentShare/100);
                        CreateIncome(incomeModel);



                        UpdateIsCalculated(i.Id);

                        //UpdateStatusBooking(i.Id, (int)BookingFlow.RateTalent);
                        //InsertBookLog(model);

                        //RatingModel RatingModel = new RatingModel();
                        //RatingModel.FileId = i.FileId;
                        //RatingModel.IsActive = true;
                        //RatingModel.UserId = null;
                        //RatingModel.Rate = 5;
                        //RatingModel.CreatedBy = "System";
                        //SetRatingFiles(RatingModel);

                        Console.WriteLine("Done Calculation for Order No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Process for Calculation Book");
                    Console.WriteLine("...................");
                }

                Console.WriteLine(".");





            }
            catch (Exception ex)
            {

            }
        }




        public void CompletedBooking()
        {
            try
            {

                var getlist = GetListBook((int)CheckingBook.CheckingUnCompleted);

                var GetReview = AdmGetAllParameter().Where(x => x.ParamName == "Reason" && x.ParamCode == "8").FirstOrDefault();
                var Review = GetReview != null ? GetReview.ParamValue : "";

                if (getlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Process for Completed Book");

                    foreach (var i in getlist)
                    {
                        BookLogModel model = new BookLogModel();
                        model.BookId = i.Id;
                        model.Status = (int)BookingFlow.RateTalent;
                        Console.WriteLine("Start Completed for Order No : " + i.OrderNo);
                        //SetCancelledBookById(i.Id);
                        UpdateStatusBooking(i.Id, (int)BookingFlow.RateTalent);
                        InsertBookLog(model);

                        RatingModel RatingModel = new RatingModel();
                        RatingModel.FileId = i.FileId;
                        RatingModel.IsActive = true;
                        RatingModel.UserId = null;
                        RatingModel.Rate = 5;
                        RatingModel.CreatedBy = "System";
                        SetRatingFiles(RatingModel);

                        BookLogModel Rmodel = new BookLogModel();
                        Rmodel.BookId = i.Id;
                        Rmodel.Status = (int)BookingFlow.ReactionVideo;
                       // Console.WriteLine("Start Set Review for Order No : " + i.OrderNo);
                        //SetCancelledBookById(i.Id);
                        UpdateStatusBookingForReview(i.Id, (int)BookingFlow.RateTalent, Review);
                        InsertBookLog(Rmodel);



                        Console.WriteLine("Done Completed for Order No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Process for Completed Book");
                    Console.WriteLine("...................");
                }

                Console.WriteLine(".");





            }
            catch (Exception ex)
            {

            }
        }



        public void RefundBooking()
        {
            try
            {

                var getlist = GetListBook((int)CheckingBook.CheckingOutOfDeadlineInProgress);
                var GetRefundMessage = AdmGetAllParameter().Where(x => x.ParamName == "Reason" && x.ParamCode == "-2").FirstOrDefault();
                var RefundMessage = GetRefundMessage != null ? GetRefundMessage.ParamValue : "";
                if (getlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Process for Refund Book");

                    foreach (var i in getlist)
                    {
                        BookLogModel model = new BookLogModel();
                        model.BookId = i.Id;
                        model.Status = (int)BookingFlow.Refund;
                        Console.WriteLine("Start Process for Refund No : " + i.OrderNo);
                        UpdateStatusBookingForRefund(i.Id, (int)BookingFlow.Refund,RefundMessage);
                        InsertBookLog(model);

                        NotificationModel notifmodel = new NotificationModel();
                        notifmodel.BookId = i.Id;
                        notifmodel.NotifType = "U";
                        notifmodel.To = i.BookedBy;
                        notifmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "UNotifBook" && x.ParamCode == model.Status.ToString()).FirstOrDefault().ParamValue;
                        InputNotification(notifmodel);
                        NotificationEmail(i.Email, model.Status.ToString(), EmailTargetType.User, i).Wait();


                        NotificationModel notifTalentmodel = new NotificationModel();
                        notifTalentmodel.BookId = i.Id;
                        notifTalentmodel.NotifType = "T";
                        notifTalentmodel.To = i.UserOfTalentId;
                        notifTalentmodel.Message = AdmGetAllParameter().Where(x => x.ParamName == "TNotifBook" && x.ParamCode == model.Status.ToString()).FirstOrDefault().ParamValue;
                        InputNotification(notifTalentmodel);
                        NotificationEmail(i.TalentEmail, model.Status.ToString(), EmailTargetType.Talent, i).Wait();

                        //var getUserData = GetUser(i.BookedBy.Value);
                        //RefundModel RefundModel = new RefundModel();
                        //RefundModel.RequestDate = DateTime.Now.AddHours(7);
                        //RefundModel.CreatedBy = "System";
                        //RefundModel.CustomerName = 
                        

                        Console.WriteLine("Done Process for Refund No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Process for Refund Book");
                    Console.WriteLine("...................");
                }

                Console.WriteLine(".");





            }
            catch (Exception ex)
            {

            }
        }

        public void UpdatePeriod()
        {
            var DayPeriodChange = 1;
            var DayClaimPeriodChange = 1;

            if(DateTime.Now.Day == DayPeriodChange)
            {
                ParameterModel model = new ParameterModel();
                model.ParamName = "Period";
                model.ParamCode = "Period";
                model.ParamValue = "M" +DateTime.Now.ToString("yyyy-MM");
                UpdateParameter(model);
            }

            if (DateTime.Now.Day == DayClaimPeriodChange)
            {
                ParameterModel model = new ParameterModel();
                model.ParamName = "ClmPeriod";
                model.ParamCode = "ClmPeriod";
                model.ParamValue = "M" + DateTime.Now.ToString("yyyy-MM");
                UpdateParameter(model);
            }

        }

        public void ProcessSetTalentVideo()
        {
            try
            {
                var getlist = GetListBook(5).Where(x => x.IsSentToTalent == 0).ToList(); // 5 adalah status setelah materi video dikirimkan oleh talent
                if (getlist.Count() > 0)
                {
                    Console.WriteLine("...................");
                    Console.WriteLine("Start Process Set Talent Video");

                    foreach (var i in getlist)
                    {
                        BookLogModel model = new BookLogModel();
                        model.BookId = i.Id;
                        Console.WriteLine("Start Send Video for Order No : " + i.OrderNo);
                        TalentVideoModel TalentVideoModel = new TalentVideoModel();
                        TalentVideoModel.TalentId = i.TalentId;
                        TalentVideoModel.VideoNm = i.ProjectNm;
                        TalentVideoModel.FileId = i.FileId;
                        TalentVideoModel.BookCategory = i.BookCategory;
                        SetVideoAsTalentVideo(TalentVideoModel, i.Id);

                        Console.WriteLine("Done Send Video for Order No : " + i.OrderNo);
                    }
                    Console.WriteLine("Done Process Set Talent Video");
                    Console.WriteLine("...................");
                }

                Console.WriteLine(".");





            }
            catch (Exception ex)
            {

            }
        }


        public void InsertBookLog(BookLogModel model)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@BookId", model.BookId);
                param.Add("@Status", model.Status);
                param.Add("@CreatedBy", 0);

                conn.Execute("sp_Tbl_Trx_Book_logInsert", param, commandType: CommandType.StoredProcedure);

                //conn.Execute("update Tbl_Trx_Book set status = -1, updateddate = @updateddate, UpdatedBy = @UpdatedBy where id = @Id",
                //    new
                //    {
                //        @Id = id,
                //        @updateddate = DateTime.Now.AddHours(7),
                //        @UpdatedBy = "System"
                //    }
                //    );



            }
        }

        public void CalculationTalentLevelAndPrice()
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();


                conn.Execute("Sp_CalcTalentLevelAndPrice", param, commandType: CommandType.StoredProcedure);



            }
        }


        public void UpdateStatusBooking(int id, int status)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Book set status = @status, updateddate = @updateddate, UpdatedBy = @UpdatedBy where id = @Id",
                    new
                    {
                        @Id = id,
                        @updateddate = DateTime.Now.AddHours(7),
                        @UpdatedBy = "System",
                        @status = status
                    }
                    );



            }
        }


        public void UpdateStatusBookingForReview(int id, int status,string Review)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Book set status = @status, updateddate = @updateddate, UpdatedBy = @UpdatedBy, Review = @Review where id = @Id",
                    new
                    {
                        @Id = id,
                        @updateddate = DateTime.Now.AddHours(7),
                        @UpdatedBy = "System",
                        @status = status,
                        @Review = Review
                    }
                    );



            }
        }

        public void UpdateStatusBookingForRefund(int id, int status,string Reason)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Book set status = @status, updateddate = @updateddate, UpdatedBy = @UpdatedBy, Reason = @Reason  where id = @Id",
                    new
                    {
                        @Id = id,
                        @updateddate = DateTime.Now.AddHours(7),
                        @UpdatedBy = "System",
                        @status = status,
                        @Reason = Reason
                    }
                    );



            }
        }


        public void UpdateIsCalculated(int id)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Book set IsCalculated = 1, updateddate = @updateddate, UpdatedBy = @UpdatedBy where id = @Id",
                    new
                    {
                        @Id = id,
                        @updateddate = DateTime.Now.AddHours(7),
                        @UpdatedBy = "System"

                    }
                    );



            }
        }


        public BookModel GetDataBook(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);


                data = conn.Query<BookModel>("sp_Tbl_Trx_BookSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public IList<BookModel> GetListBookForCancelBook()
        {
            var ListBook = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();



                ListBook = conn.Query<BookModel>("Sp_getBookingWaitingForPayment", param,
                commandType: CommandType.StoredProcedure).ToList();

                ListBook = ListBook.Where(x => x.ExpiredDate > DateTime.Now).ToList();



            }

            return ListBook;
        }

        public IList<BookModel> GetListBook(int status)
        {
            var ListBook = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var Period = AdmGetAllParameter().Where(x => x.ParamCode == "Period" && x.ParamName == "Period").FirstOrDefault().ParamValue;
                var param = new DynamicParameters();
                param.Add("@code", status);
                param.Add("@Period", Period);


                ListBook = conn.Query<BookModel>("Sp_GetBookingByCheckingCode", param,
                commandType: CommandType.StoredProcedure).ToList();

                if (status == 0)
                {
                    ListBook = ListBook.Where(x => x.ExpiredDate > DateTime.Now.AddHours(7)).ToList();
                }

            }

            return ListBook;
        }




        public void SetVideoAsTalentVideo(TalentVideoModel model, int BookId)
        {
            var param = new DynamicParameters();
            param.Add("@TalentId", model.TalentId);
            param.Add("@FileId", model.FileId);
            param.Add("@VideoNm", model.VideoNm);
            param.Add("@IsActive", 1);
            param.Add("@BookCategory", model.BookCategory);

            var param2 = new DynamicParameters();
            param2.Add("@BookId", BookId);


            using (IDbConnection conn = Connection)
            {
                conn.Query<TalentVideoModel>("sp_Tbl_Trx_TalentVideoInsert", param,
                commandType: CommandType.StoredProcedure).ToList();

                conn.Query<TalentVideoModel>("Sp_SetSentToTalentFromBook", param2,
                commandType: CommandType.StoredProcedure).ToList();

            }

        }


        public IList<BookModel> GetOutOfDeadlineBooking()
        {
            var ListBook = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();



                ListBook = conn.Query<BookModel>("Sp_GetInProgressBooking", param,
                commandType: CommandType.StoredProcedure).ToList();

                ListBook = ListBook.Where(x => x.Deadline > DateTime.Now.AddHours(7)).ToList();

            }

            return ListBook;
        }


        public IList<BookModel> GetAllWaitingForPaymentBook()
        {
            var ListBook = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();



                ListBook = conn.Query<BookModel>("Sp_getBookingWaitingForPayment", param,
                commandType: CommandType.StoredProcedure).ToList();





            }

            return ListBook;
        }

        public IList<SaldoModel> GetAllLimitSaldo()
        {
            var listSaldo = new List<SaldoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                listSaldo = conn.Query<SaldoModel>("[sp_GetSaldoLimit]", param,
                commandType: CommandType.StoredProcedure).ToList();

            }

            return listSaldo;
        }



        public void CreateIncome(IncomeModel model)
        {

            var data = new IncomeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@SourceIncome", model.SourceIncome);
                    param.Add("@Period", model.Period);
                    param.Add("@Income", model.Income);

                    conn.Query<IncomeModel>("sp_Tbl_Trx_IncomeInsert", param,
                                    commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }


        }

        public TalentModel GetTalent(int Id)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", Id);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<ParameterModel> AdmGetAllParameter()
        {
            var data = new List<ParameterModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<BookModel> GetFinishBookingList()
        {
            var ListBook = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();


                ListBook = conn.Query<BookModel>("Sp_GetFinishBookingList", param,
                commandType: CommandType.StoredProcedure).ToList();

            }

            return ListBook;
        }

        public RatingModel SetRatingFiles(RatingModel model)
        {
            var data = new RatingModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@FileId", model.FileId);
                    param.Add("@Rate", model.Rate);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", model.CreatedDate);



                    data = conn.Query<RatingModel>("sp_Tbl_Trx_Files_RatingInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public NotificationModel InputNotification(NotificationModel model)
        {
            var data = new NotificationModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@To", model.To);
                    param.Add("@BookId", model.BookId);
                    param.Add("@Message", model.Message);
                    param.Add("@NotifType", model.NotifType);
                    param.Add("@CreatedBy", "System");
                    param.Add("@IsReaded", 0);
                    param.Add("@IsActive", 1);
                    param.Add("@IsSent", 0);



                    data = conn.Query<NotificationModel>("sp_Tbl_Trx_NotificationInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



        public async Task<bool> EmailLimitSaldo(string sendto, string Name)
        {
            try
            {
                var SendGridApiKey = AdmGetAllParameter().Where(x => x.ParamCode == "ApiKey" && x.ParamName == "SendGrid").FirstOrDefault().ParamValue;
                var Template = GetEmailNotification(2);
                var EmailTemplate = Template.TemplateValue;
                EmailTemplate = EmailTemplate.Replace("@status", "Saldo Limit");
                EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + " saldo kamu sudah mau habis nih yuk ditopup");           


                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Saldo Sudah Limit", EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }

        public async Task<bool> EmailNotificationForNearlyDeadlineOrder(string sendto, string StatusCode, EmailTargetType EmailType, BookModel model)
        {
            try
            {
                var Template = GetAllEmailNotification().Where(x => x.TemplateNm == "T-Ord4DaysNotRecord").FirstOrDefault();
                var EmailTemplate = Template.TemplateValue;
                var Company = AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName =AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();

                var CheckSubjdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "UOrdSubjEmail").FirstOrDefault();
                var Subj = CheckSubjdata != null ? CheckSubjdata.ParamValue : "";

                EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);

                var SendGridApiKey = AdmGetAllParameter().Where(x => x.ParamCode == "ApiKey" && x.ParamName == "SendGrid").FirstOrDefault().ParamValue;

                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, model.OrderNo + " - " + Subj, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
               // InsertLog(0, "EmailNewUserSignUp", ex.Message);
                return false;

            }

        }



        public async Task<bool> NotificationEmail(string sendto, string StatusCode, EmailTargetType EmailType, BookModel model)
        {
            try
            {
                var Template = GetEmailNotification(1);
                var EmailTemplate = Template.TemplateValue;

                var Company = AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName = AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();

                //var Statusmessage = IMasterManager.GetParameterByCode(StatusCode).ParamValue;
                var CheckSubjdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "UOrdSubjEmail").FirstOrDefault();
                var Subj = CheckSubjdata != null ? CheckSubjdata.ParamValue : "";

                var CheckMsgdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "UOrdMsgEmail").FirstOrDefault();
                var Msg = CheckMsgdata != null ? CheckMsgdata.ParamValue : "";
                var firstName = "";

                if (EmailType == EmailTargetType.Talent)
                {

                    //Checkdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TNotifBook").FirstOrDefault();
                    //Statusmessage = Checkdata != null ? Checkdata.ParamValue : "";

                    CheckSubjdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TOrdSubjEmail").FirstOrDefault();
                    Subj = CheckSubjdata != null ? CheckSubjdata.ParamValue : "";

                    CheckMsgdata = AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TOrdMsgEmail").FirstOrDefault();
                    Msg = CheckMsgdata != null ? CheckMsgdata.ParamValue : "";


                }
               

                var JinglUrl = AdmGetAllParameter().Where(x => x.ParamCode == "Url" && x.ParamName == "Jingl").FirstOrDefault().ParamValue;
                var SendGridApiKey = AdmGetAllParameter().Where(x => x.ParamCode == "ApiKey" && x.ParamName == "SendGrid").FirstOrDefault().ParamValue;
                EmailTemplate = EmailTemplate.Replace("@status", Subj);
                EmailTemplate = EmailTemplate.Replace("@Message", Msg);

                if (EmailType == EmailTargetType.Talent)
                {
                    var talentMdl = new TalentModel();
                    firstName = "Talent";

                    talentMdl = GetTalent(Convert.ToInt32(model.TalentId));
                    if(talentMdl != null)
                    {
                        var userMdl = new UserModel();
                        userMdl = GetUser(Convert.ToInt32(talentMdl.UserId));
                        firstName = userMdl.FirstName;
                    }

                    EmailTemplate = EmailTemplate.Replace("@FirstName", firstName);
                    EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl + "\\Account\\BookingDetail?bookId=" + model.Id.ToString() + "&IsEmail=1");

                }
                else
                {
                    var userMdl = new UserModel();
                    firstName = "User";

                    userMdl = GetUser(Convert.ToInt32(model.BookId));
                    if (userMdl != null)
                    {
                        firstName = userMdl.FirstName;
                    }

                    EmailTemplate = EmailTemplate.Replace("@FirstName", firstName);

                    if (StatusCode == Convert.ToInt32(BookingFlow.Refund).ToString())
                    {
                        EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl + "\\Account\\RequestRefund?OrderNo=" + model.OrderNo.ToString());
                    }
                    else
                    {
                        EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl + "\\Booking\\CheckBookingData?bookId=" + model.Id.ToString() + "&IsEmail=1");
                    }

                }


                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, model.OrderNo + " - " + Subj, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                //InsertLog(0, "NotificationEmail", ex.Message);
                return false;

            }

        }


        public EmailNotificationModel GetEmailNotification(int Id)
        {
            var data = new EmailNotificationModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", Id);


                data = conn.Query<EmailNotificationModel>("sp_Tbl_Mst_EmailTemplateSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public IList<EmailNotificationModel> GetAllEmailNotification()
        {
            var data = new List<EmailNotificationModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", null);


                data = conn.Query<EmailNotificationModel>("sp_Tbl_Mst_EmailTemplateSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }


        public RefundModel CreateRefund(RefundModel model)
        {
            var data = new RefundModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@CustomerName", model.CustomerName);
                    param.Add("@BankName", model.BankName);
                    param.Add("@BatchNumber", model.BatchNumber);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@RequestDate", model.RequestDate);
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@Status", model.Status);
                    param.Add("@CreatedBy", model.CreatedBy);



                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public RefundModel UpdateRefund(RefundModel model)
        {
            var data = new RefundModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.UserId);
                    param.Add("@RefundNumber", model.RefundNumber);
                    param.Add("@UserId", model.UserId);
                    param.Add("@BankName", model.BankName);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@BatchNumber", model.BatchNumber);
                    param.Add("@RequestDate", model.RequestDate);
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@Status", model.Status);

                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public void UpdateParameter(ParameterModel model)
        {
         
           
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    //conn.Execute("update Tbl_Usm_User set Password = '" + model.Password + "' where id = " + model.Id);
                    conn.Query("update Tbl_Mst_Parameter set ParamValue = @ParamValue where ParamCode = @ParamCode and ParamName = @ParamName ",new
                            {
                                 @ParamValue = model.ParamValue,
                                 @ParamCode = model.ParamCode,
                                 @ParamName = model.ParamName
                            });



                }

            
        }

        public void SetEncryptPassword(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    conn.Execute("update Tbl_Usm_User set Password = '"+model.Password+"' where id = "+model.Id);



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public void updatePassword()
        {
            var getdata = GetAllUser();
            foreach(var a in getdata)
            {
                UserModel model = new UserModel();
                model.Password = Program.Encrypt(a.Password);
                model.Id = a.Id;
                SetEncryptPassword(model);


            }
        }

        public VoucherTransactionModel CreateVoucherTransaction(VoucherTransactionModel model)
        {
            var data = new VoucherTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherId", model.VoucherId);
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@BookId", model.BookId);
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@UserId", model.UserId);
                    param.Add("@qty", model.qty);
                    param.Add("@Amount", model.Amount);


                    data = conn.Query<VoucherTransactionModel>("sp_Tbl_Trx_VoucherInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



        public List<UserModel> GetAllUser()
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }





        public UserModel GetUser(int id)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<VoucherModel> GetAllVoucher()
        {
            var data = new List<VoucherModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public VoucherModel UpdateVoucher(VoucherModel model)
        {
            var data = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@VoucherNm", model.VoucherNm);
                    param.Add("@VoucherDesc", model.VoucherDesc);
                    param.Add("@RemainingCount", model.RemainingCount);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Percentage", model.Percentage);
                    param.Add("@StartDate", model.StartDate);
                    param.Add("@EndDate", model.EndDate);
                    param.Add("@UpdatedBy", model.UpdatedBy);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@IsUsed", model.IsUsed);
                    param.Add("@IsClaimed", model.IsClaimed);
                    param.Add("@MinValue", model.MinValue);
                    param.Add("@MaxValue", model.MaxValue);
                    param.Add("@SentTo", model.SentTo);
                    param.Add("@Budget", model.Budget);
                    param.Add("@UsesPerCustomer", model.UsesPerCustomer);

                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }





    }


}
