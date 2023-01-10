using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jingl.Transaction.Model.Dao
{
    public class BookDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public BookDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }


        public IList<BookModel> GetAllBook()
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<BookModel>("sp_Tbl_Trx_BookSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public IList<BookModel> GetTransactionOrders(string Period)
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Period", Period);


                data = conn.Query<BookModel>("sp_GetTransactionOrder", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public BookModel GetPotensialSalesPerPeriod(string Period)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Period", Period);


                data = conn.Query<BookModel>("sp_getPotensialSalesPerPeriod", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public BookModel GetTotalSalesPerPeriod(string Period)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Period", Period);


                data = conn.Query<BookModel>("sp_getValueSalesPerPeriod", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
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

        public BookModel GetDataBookByOrderId(string orderNo)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@orderId", orderNo);


                data = conn.Query<BookModel>("GetBookingByOrderId", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public BookModel GetBookConfirmation(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@BookId", model.Id);


                data = conn.Query<BookModel>("SP_GetBookConfirmation", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();


            }

            return data;
        }

        public BookModel CreateBookData(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@BookCategory", model.BookCategory);
                param.Add("@ProjectNm", model.ProjectNm);
                param.Add("@TalentId", model.TalentId);
                param.Add("@BriefNeeds", model.BriefNeeds);
                param.Add("@From", model.From);
                param.Add("@To", model.To);
                param.Add("@Notification", model.Notification);
                param.Add("@VoucherCode", model.VoucherCode);
                param.Add("@TotalPay", model.TotalPay);
                param.Add("@PriceAmount", model.PriceAmount);
                param.Add("@SalePrice", model.SalePrice);
                param.Add("@PaymentShare", model.PaymentShare);
                param.Add("@Potongan", model.Potongan);
                param.Add("@PayMethod", model.PayMethod);
                param.Add("@Email", model.Email);
                param.Add("@PhoneNumber", model.PhoneNumber);
                //param.Add("@PriceAmount", model.PriceAmount);
                param.Add("@Status", BookingFlow.Submit);
                param.Add("@BookedBy", model.BookedBy);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreatedDate",DateTime.Now);
                param.Add("@IsPublic", model.IsPublic);
                param.Add("@IsActive", Status.Active);
                param.Add("@Period", model.Period);
                param.Add("@ReactionFileId", model.ReactionFileId);

                data = conn.Query<BookModel>("sp_Tbl_Trx_BookInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

                var param2 = new DynamicParameters();
                param2.Add("@BookingId", data.Id);

                conn.Execute("Sp_SetOrderNoToBooking", param2, commandType: CommandType.StoredProcedure);

            }

            return data;
        }

        public IList<BookModel> GetBookingByUserId(int userId)
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@userId", userId);


                data = conn.Query<BookModel>("Sp_GetBookingByUserID", param,
                           commandType: CommandType.StoredProcedure).ToList();


            }

            return data;
        }

        public IList<BookModel> GetBookingByTalentId(int UserId)
        {
            var data = new List<BookModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", UserId);

                
                    data = conn.Query<BookModel>("Sp_GetBookingByTalentId", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LandingModel GetBookingByVideoId(int VideoId)
        {
            var data = new LandingModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@videoId", VideoId);


                    data = conn.Query<LandingModel>("Sp_GetBookingByVideoId", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<BookModel> GetReactionByTalentId(int TalentId)
        {
            var data = new List<BookModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);


                    data = conn.Query<BookModel>("Sp_GetReactionVideoByTalentId", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<BookModel> GetAllReactionVideo(int TalentId)
        {
            var data = new List<BookModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);


                    data = conn.Query<BookModel>("Sp_GetAllReactionVideo", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<BookModel> GetAllReactionVideo2(BookModel model)
        {
            var data = new List<BookModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@talentname ", model.TalentNm);
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@Offset", model.offset);
                    param.Add("@fetch", model.fetch);

                    data = conn.Query<BookModel>("Sp_GetAllReactionVideo2", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<BookModel> GetBookingPaidByTalentId(int UserId)
        {
            var data = new List<BookModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", UserId);
                    param.Add("@DateNow", DateTime.Now);


                    data = conn.Query<BookModel>("Sp_GetPaidBookingByTalentId", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public BookModel UpdateBookData(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@BookCategory", model.BookCategory);
                param.Add("@ProjectNm", model.ProjectNm);
                param.Add("@TalentId", model.TalentId);
                param.Add("@BriefNeeds", model.BriefNeeds);
                param.Add("@From", model.From);
                param.Add("@To", model.To);
                param.Add("@Notification", model.Notification);
                param.Add("@VoucherCode", model.VoucherCode);
                param.Add("@TotalPay", model.TotalPay);
                param.Add("@PayMethod", model.PayMethod);
                param.Add("@Email", model.Email);
                param.Add("@PhoneNumber", model.PhoneNumber);
                param.Add("@Status", model.Status);
                param.Add("@PriceAmount", model.PriceAmount);
                param.Add("@SalePrice", model.SalePrice);
                param.Add("@PaymentShare", model.PaymentShare);
                param.Add("@Potongan", model.Potongan);
                param.Add("@OrderNo", model.OrderNo);
                param.Add("@SnapToken", model.SnapToken);
                param.Add("@BookedBy", model.BookedBy);
                param.Add("@FileId", model.FileId);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsPublic", model.IsPublic);
                param.Add("@IsActive", model.IsActive);
                param.Add("@PaymentChannel", model.PaymentChannel);
                param.Add("@ExpiredDate", model.ExpiredDate);
                param.Add("@Period", model.Period);
                param.Add("@Review", model.Review);
                param.Add("@Reason", model.Reason);
                param.Add("@ReactionFileId", model.ReactionFileId);


                data = conn.Query<BookModel>("sp_Tbl_Trx_BookUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }


        public BookModel AdmUpdateBookData(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@BookCategory", model.BookCategory);
                param.Add("@ProjectNm", model.ProjectNm);
                param.Add("@TalentId", model.TalentId);
                param.Add("@BriefNeeds", model.BriefNeeds);
                param.Add("@From", model.From);
                param.Add("@To", model.To);
                param.Add("@Deadline", model.Deadline);
                param.Add("@Notification", model.Notification);
                param.Add("@VoucherCode", model.VoucherCode);
                param.Add("@TotalPay", model.TotalPay);
                param.Add("@PayMethod", model.PayMethod);
                param.Add("@Email", model.Email);
                param.Add("@PhoneNumber", model.PhoneNumber);
                param.Add("@Status", model.Status);
                param.Add("@PriceAmount", model.PriceAmount);
                param.Add("@SalePrice", model.SalePrice);
                param.Add("@PaymentShare", model.PaymentShare);
                param.Add("@Potongan", model.Potongan);
                param.Add("@OrderNo", model.OrderNo);
                param.Add("@SnapToken", model.SnapToken);
                param.Add("@BookedBy", model.BookedBy);
                param.Add("@FileId", model.FileId);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsPublic", model.IsPublic);
                param.Add("@IsActive", model.IsActive);
                param.Add("@PaymentChannel", model.PaymentChannel);
                param.Add("@ExpiredDate", model.ExpiredDate);
                param.Add("@Period", model.Period);
                //param.Add("@Deadline", model.Deadline);
                param.Add("@Rate", model.Rate);
                param.Add("@ReactionFileId", model.ReactionFileId);
                param.Add("@Reason", model.Reason);
                param.Add("@Review", model.Review);


                data = conn.Query<BookModel>("sp_Tbl_Trx_AdmBookUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }



        public IList<TalentVideoModel> GetUserVideos(int UserId)
        {
            var data = new List<TalentVideoModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", UserId);


                    data = conn.Query<TalentVideoModel>("[Sp_GetUserVideos]", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        


       

        public IList<BookModel> AdmGetAllBook()
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<BookModel>("Sp_GetAllBookingData", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }
        public IList<BookModel> AgnGetAllBook(int id, int UserId)
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);
                param.Add("@AgencyId", UserId);

                data = conn.Query<BookModel>("Sp_GetAllBookingDataAgency", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }
        public BookModel AdmGetDataBook(BookModel model)
        {
            var data = new BookModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);


                data = conn.Query<BookModel>("Sp_GetAllBookingData", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }


        public IList<BookModel> GetDailyPayment(BookModel model)
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@beginDate", model.BeginDate);
                param.Add("@endDate", model.EndDate);


                data = conn.Query<BookModel>("Sp_DailyPaymentReport", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public void DeleteBook(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_BookDelete", param,
                             commandType: CommandType.StoredProcedure);



            }


        }

        public IList<BookModel> GetBookByAgencyId(BookModel model)
        {
            var data = new List<BookModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Period", model.Period);
                param.Add("@AgencyId", model.AgencyId);


                data = conn.Query<BookModel>("Sp_GetBookingByAgent", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }







    }
}
