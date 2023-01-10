using System;
using System.Collections.Generic;
using System.Text;

using Jingl.Service.Interface;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Transaction.Model.Dao;
using Jingl.General.Model.User.ViewModel;
using Jingl.Master.Model.Dao;
using Jingl.General.Model.Agency.Master;
using Jingl.General.Model.Admin.Master;

namespace Jingl.Service.Manager
{
    public class TransactionManager : ITransactionManager
    {
        private readonly PaymentBookLogDao PaymentBookLogDao;
        private readonly NotificationDao NotificationDao;
        private readonly SupportDao SupportDao;
        private readonly BookDao BookDao;
        private readonly TalentVideoDao TalentVideoDao;
        private readonly FilesDao FilesDao;
        private readonly WishlistDao WishlistDao;
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly ClaimDao ClaimDao;
        private readonly RefundDao RefundDao;
        private readonly TalentRegistrationDao TalentRegistrationDao;
        private readonly SaldoDao SaldoDao;
        private readonly TopupDao TopupDao;
        private readonly VoucherTransactionDao VoucherTransactionDao;
        private readonly ClaimAgencyDao ClaimAgencyDao;
        private readonly QuestionDao QuestionDao;
        private readonly AnswerDao AnswerDao;
        private readonly CommentDao CommentDao;
        private readonly TalentDao TalentDao;
        private readonly LiveStreamDao LiveStreamDao;
        private readonly LiveStreamStickerDao LiveStreamStickerDao;
        private readonly RequestChangeDao RequestChangeDao;


        public TransactionManager(IConfiguration _config)
        {
            this.PaymentBookLogDao = new PaymentBookLogDao(_config);
            this.NotificationDao = new NotificationDao(_config);
            this.SupportDao = new SupportDao(_config);
            this._config = _config;
            this.FilesDao = new FilesDao(_config);
            this.BookDao = new BookDao(_config);
            this.TalentVideoDao = new TalentVideoDao(_config);
            this.WishlistDao = new WishlistDao(_config);
            this._logger = new Logger(_config);
            this.RefundDao = new RefundDao(_config);
            this.ClaimDao = new ClaimDao(_config);
            this.TalentRegistrationDao = new TalentRegistrationDao(_config);
            this.SaldoDao = new SaldoDao(_config);
            this.TopupDao = new TopupDao(_config);
            this.VoucherTransactionDao = new VoucherTransactionDao(_config);
            this.ClaimAgencyDao = new ClaimAgencyDao(_config);
            this.QuestionDao = new QuestionDao(_config);
            this.AnswerDao = new AnswerDao(_config);
            this.CommentDao = new CommentDao(_config);
            this.TalentDao = new TalentDao(_config);
            this.LiveStreamDao = new LiveStreamDao(_config);
            this.LiveStreamStickerDao = new LiveStreamStickerDao(_config);
            this.RequestChangeDao = new RequestChangeDao(_config);
        }


        #region WISHLIST
        public WishlistModel CreateWishlistData(WishlistModel model)
        {
            var data = new WishlistModel();

            try
            {
                data = WishlistDao.CreateWishlistData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateWishlistData", ex.Message, "Service");

            }

            return data;
        }
        public IList<TalentCategoryViewModel> GetWishListByUserId(int UserId)
        {
            IList<TalentCategoryViewModel> data = new List<TalentCategoryViewModel>();

            try
            {
                data = WishlistDao.GetWishListByUserId(UserId);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetWishListByUserId", ex.Message, "Service");

            }

            return data;
        }
        public int GetWishlistIdByUserTalent(WishlistModel model)
        {
            int id = 0;
            try
            {
                id = WishlistDao.GetWishlistIdByUserTalent(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetWishlistIdByUserTalent", ex.Message, "Service");

            }
            return id;
        }
        public void RemoveWishlistData(WishlistModel model)
        {
            try
            {
                WishlistDao.RemoveWishlistData(model);

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "RemoveWishlistData", ex.Message, "Service");
            }
        }
        #endregion

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }

        public BookModel CreateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.CreateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateBookData", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetAllBook()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetAllBook();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBook", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetBookingByUserId(int UserId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingByUserId(UserId);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingByUserId", ex.Message, "Service");

            }

            return data;
        }
        public LandingModel GetBookingByVideoId(int VideoId)
        {
            var data = new LandingModel();

            try
            {
                data = BookDao.GetBookingByVideoId(VideoId);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingByVideoId", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetDataBook(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetDataBook(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataBook", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetBookConfirmation(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetBookConfirmation(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookConfirmation", ex.Message, "Service");

            }

            return data;
        }


        public BookModel UpdateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.UpdateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateBookData", ex.Message, "Service");

            }


            return data;
        }
              

        public IList<SupportModel> GetAllSupport()
        {
            IList<SupportModel> data = new List<SupportModel>();

            try
            {
                data = SupportDao.GetAllSupport();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllBook", ex.Message, "Service");

            }

            return data;
        }
        public SupportModel CreateSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.CreateSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSupport", ex.Message, "Service");

            }

            return data;
        }

        public FilesModel CreateFiles(FilesModel model)
        {
            var data = new FilesModel();

            try
            {
                data = FilesDao.CreateFiles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateFiles", ex.Message, "Service");

            }

            return data;
        }


        public SupportModel GetSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.GetSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSupport", ex.Message, "Service");

            }

            return data;

        }

        public IList<NotificationModel> GetNotificationForUser(int UserId)
        {
            IList<NotificationModel> data = new List<NotificationModel>();

            try
            {
                data = NotificationDao.GetNotificationForUser(UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetNotificationForUser", ex.Message, "Service");

            }

            return data;
        }

        public IList<NotificationModel> GetNotificationForTalent(int UserId)
        {
            IList<NotificationModel> data = new List<NotificationModel>();

            try
            {
                data = NotificationDao.GetNotificationForTalent(UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetNotificationForTalent", ex.Message, "Service");

            }

            return data;
        }

        public NotificationModel InsertNotification(NotificationModel model)
        {
            NotificationModel data = new NotificationModel();

            try
            {
                data = NotificationDao.InsertNotification(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "InsertNotification", ex.Message, "Service");

            }

            return data;
        }

        public PaymentBookLogModel CreatePaymentBookLog(PaymentBookLogModel model)
        {
            var data = new PaymentBookLogModel();

            try
            {
                data = PaymentBookLogDao.CreatePaymentBookLog(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePaymentBookLog", ex.Message, "Service");

            }

            return data;
        }

        public BookModel GetDataBookByOrderId(string orderNo)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.GetDataBookByOrderId(orderNo);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDataBookByOrderId", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetTalentVideos(int TalentId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = TalentVideoDao.GetTalentVideos(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentVideos", ex.Message, "Service");

            }

            return data;
        }

        //public IList<LandingModel> GetTalentVideosPertanyaan(int VideoId)
        //{
        //    IList<LandingModel> data = new List<LandingModel>();

        //    try
        //    {
        //        data = TalentVideoDao.GetTalentVideosPertanyaan(VideoId);


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentVideosPertanyaan", ex.Message, "Service");

        //    }

        //    return data;
        //}


        public IList<TalentVideoModel> GetUserVideos(int UserId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = BookDao.GetUserVideos(UserId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUserVideos", ex.Message, "Service");

            }

            return data;
        }

        public RatingModel CreateRatingFiles(RatingModel model)
        {
            var data = new RatingModel();

            try
            {
                data = FilesDao.CreateRatingFiles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRatingFiles", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetBookingByTalentId(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingByTalentId", ex.Message, "Service");

            }

            return data;
        }
        public IList<BookModel> GetBookingPaidByTalentId(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookingPaidByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookingPaidByTalentId", ex.Message, "Service");

            }

            return data;
        }

        public void IsReadedNotification(int Id)
        {
            try
            {
                NotificationDao.IsReadedNotification(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "IsReadedNotification", ex.Message, "Service");

            }
        }
        public IList<BookModel> AdmGetAllBook()
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.AdmGetAllBook();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetAllBook", ex.Message, "Service");

            }

            return data;
        }
        public IList<BookModel> AgnGetAllBook(TalentParamModel model)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.AgnGetAllBook(model.Id,model.UserId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AgnGetAllBook", ex.Message, "Service");

            }

            return data;
        }
        public BookModel AdmGetDataBook(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.AdmGetDataBook(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetDataBook", ex.Message, "Service");

            }

            return data;
        }

        public SupportModel UpdateSupport(SupportModel model)
        {
            var data = new SupportModel();

            try
            {
                data = SupportDao.UpdateSupport(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateSupport", ex.Message, "Service");

            }

            return data;
        }

        public void CreateTalentVideo(TalentVideoModel model)
        {
            try
            {
                TalentVideoDao.CreateTalentVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentVideo", ex.Message, "Service");

            }
        }

        public void DeleteTalentVideoByTalentId(int TalentId)
        {
            try
            {
                TalentVideoDao.DeleteTalentVideoByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTalentVideoByTalentId", ex.Message, "Service");

            }
        }

        public void DeleteTalentVideo(int Id)
        {
            try
            {
                TalentVideoDao.DeleteTalentVideo(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTalentVideo", ex.Message, "Service");

            }
        }

        public IList<TalentVideoModel> GetAllVideo()
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = TalentVideoDao.GetAllVideo();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetAllVideo2(TalentVideoModel model)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = TalentVideoDao.GetAllVideo2(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetAllReactionVideo(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetAllReactionVideo(TalentId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllReactionVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetAllReactionVideo2(BookModel model)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetAllReactionVideo2(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllReactionVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ClaimModel> GetClaimByPeriod(string Period)
        {
            IList<ClaimModel> data = new List<ClaimModel>();

            try
            {
                data = ClaimDao.GetClaimByPeriod(Period);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetClaimByPeriod", ex.Message, "Service");

            }

            return data;
        }

        public IList<ClaimModel> GetAllClaim()
        {
            IList<ClaimModel> data = new List<ClaimModel>();

            try
            {
                data = ClaimDao.GetAllClaim();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel GetClaim(int id)
        {
           ClaimModel data = new ClaimModel();

            try
            {
                data = ClaimDao.GetClaim(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel CreateClaim(ClaimModel model)
        {
            var data = new ClaimModel();

            try
            {
                data = ClaimDao.CreateClaim(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateClaim", ex.Message, "Service");

            }

            return data;
        }

        public ClaimModel UpdateClaim(ClaimModel model)
        {
            var data = new ClaimModel();

            try
            {
                data = ClaimDao.UpdateClaim(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateClaim", ex.Message, "Service");

            }

            return data;
        }

        public IList<RefundModel> GetRefundByBatchNumber(string BatchNumber)
        {
            IList<RefundModel> data = new List<RefundModel>();

            try
            {
                data = RefundDao.GetRefundByBatchNumber(BatchNumber);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRefundByBatchNumber", ex.Message, "Service");

            }

            return data;
        }

        public IList<RefundModel> GetAllRefund()
        {
            IList<RefundModel> data = new List<RefundModel>();

            try
            {
                data = RefundDao.GetAllRefund();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel GetRefund(int id)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.GetRefund(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel CreateRefund(RefundModel model)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.CreateRefund(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRefund", ex.Message, "Service");

            }

            return data;
        }

        public RefundModel UpdateRefund(RefundModel model)
        {
            var data = new RefundModel();

            try
            {
                data = RefundDao.UpdateRefund(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateRefund", ex.Message, "Service");

            }

            return data;
        }

        public IList<BookModel> GetDailyPayment(BookModel model)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetDailyPayment(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDailyPayment", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentVideoModel> GetAllVideoByCategory(int? CategoryId)
        {
            IList<TalentVideoModel> data = new List<TalentVideoModel>();

            try
            {
                data = FilesDao.GetAllVideoByCategory(CategoryId);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideoByCategory", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel GetTalentRegistration(int Id)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.GetTalentRegistration(Id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public IList<TalentRegModel> GetAllTalentRegistration()
        {
            IList<TalentRegModel> data = new List<TalentRegModel>();

            try
            {
                data = TalentRegistrationDao.GetAllTalentRegistration();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVideoByCategory", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel CreateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.CreateTalentRegistration(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public TalentRegModel UpdateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();

            try
            {
                data = TalentRegistrationDao.UpdateTalentRegistration(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTalentRegistration", ex.Message, "Service");

            }

            return data;
        }

        public FilesWatchModel CreateFilesWatch(FilesWatchModel model)
        {
            var data = new FilesWatchModel();

            try
            {
                data = FilesDao.CreateFilesWatch(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateFilesWatch", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteBook(int id)
        {
            try
            {
                BookDao.DeleteBook(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteBook", ex.Message, "Service");

            }
        }

        public void DeleteSupport(int id)
        {
            try
            {
                SupportDao.DeleteSupport(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteSupport", ex.Message, "Service");

            }
        }

        public BookModel AdmUpdateBookData(BookModel model)
        {
            var data = new BookModel();

            try
            {
                data = BookDao.AdmUpdateBookData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmUpdateBookData", ex.Message, "Service");

            }


            return data;
        }


        public void DeleteVoucherTransaction(int id)
        {
            try
            {
                VoucherTransactionDao.DeleteVoucherTransaction(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteVoucherTransaction", ex.Message, "Service");

            }
        }

        public VoucherTransactionModel CreateVoucherTransaction(VoucherTransactionModel model)
        {
            var data = new VoucherTransactionModel();

            try
            {
                data = VoucherTransactionDao.CreateVoucherTransaction(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateVoucherTransaction", ex.Message, "Service");

            }

            return data;
        }


        public VoucherTransactionModel GetVoucherTransaction(int id)
        {
            var data = new VoucherTransactionModel();

            try
            {
                data = VoucherTransactionDao.GetVoucherTransaction(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetVoucherTransaction", ex.Message, "Service");

            }

            return data;
        }


        public IList<VoucherTransactionModel> GetAllVoucherTransaction()
        {
            IList<VoucherTransactionModel> data = new List<VoucherTransactionModel>();

            try
            {
                data = VoucherTransactionDao.GetAllVoucherTransaction();
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllVoucherTransaction", ex.Message, "Service");

            }

            return data;
        }




        #region SALDO & TOPUP
        public SaldoModel CreateSaldo(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.CreateSaldo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSaldo", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel UpdateSaldo(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.UpdateSaldo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateSaldo", ex.Message, "Service");

            }


            return data;
        }
        public IList<SaldoModel> GetAllSaldo()
        {
            IList<SaldoModel> data = new List<SaldoModel>();

            try
            {
                data = SaldoDao.GetAllSaldo();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllSaldo", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel GetSaldoById(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.GetSaldoById(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSaldoById", ex.Message, "Service");

            }


            return data;
        }
        public SaldoModel GetSaldoByTalentId(SaldoModel model)
        {
            var data = new SaldoModel();

            try
            {
                data = SaldoDao.GetSaldoByTalentId(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSaldoByTalentId", ex.Message, "Service");

            }


            return data;
        }

        public TopupModel CreateTopup(TopupModel model)
        {
            var data = new TopupModel();

            try
            {
                data = TopupDao.CreateTopup(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateSaldo", ex.Message, "Service");

            }


            return data;

        }
        public IList<TopupModel> GetTopupBySaldoId(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();

            try
            {
                data = TopupDao.GetTopupBySaldoId(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllSaldo", ex.Message, "Service");

            }


            return data;
        }
        public IList<TopupModel> GetTopupByStatus(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();

            try
            {
                data = TopupDao.GetTopupByStatus(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRequestTopup", ex.Message, "Service");

            }


            return data;
        }
        public TopupModel TopupApproval(TopupModel model)
        {
            TopupModel data = new TopupModel();

            try
            {
                data = TopupDao.TopupApproval(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "TopupApproval", ex.Message, "Service");

            }


            return data;
        }
        public TopupModel GetTopupById(TopupModel model)
        {
            TopupModel data = new TopupModel();

            try
            {
                data = TopupDao.GetTopupById(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "TopupApproval", ex.Message, "Service");

            }


            return data;
        }

        public IList<ClaimAgencyModel> GetAllClaimAgency()
        {

            IList<ClaimAgencyModel> data = new List<ClaimAgencyModel>();

            try
            {
                data = ClaimAgencyDao.GetAllClaimAgency();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllClaimAgency", ex.Message, "Service");

            }


            return data;
        }

        public ClaimAgencyModel GetClaimAgency(int id)
        {
            ClaimAgencyModel data = new ClaimAgencyModel();

            try
            {
                data = ClaimAgencyDao.GetClaimAgency(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetClaimAgency", ex.Message, "Service");

            }


            return data;
        }

        public ClaimAgencyModel CreateClaimAgency(ClaimAgencyModel model)
        {
            ClaimAgencyModel data = new ClaimAgencyModel();

            try
            {
                data = ClaimAgencyDao.CreateClaimAgency(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateClaimAgency", ex.Message, "Service");

            }


            return data;
        }

        public ClaimAgencyModel UpdateClaimAgency(ClaimAgencyModel model)
        {
            ClaimAgencyModel data = new ClaimAgencyModel();

            try
            {
                data = ClaimAgencyDao.UpdateClaimAgency(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateClaimAgency", ex.Message, "Service");

            }


            return data;
        }

        public void DeleteClaimAgency(int id)
        {
            try
            {
                ClaimAgencyDao.DeleteClaimAgency(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteClaimAgency", ex.Message, "Service");

            }
        }

        public IList<ClaimAgencyDetailsModel> GetAllClaimAgencyDetails()
        {
            IList<ClaimAgencyDetailsModel> data = new List<ClaimAgencyDetailsModel>();

            try
            {
                data = ClaimAgencyDao.GetAllClaimAgencyDetails();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllClaimAgencyDetails", ex.Message, "Service");

            }


            return data;
        }

        public ClaimAgencyDetailsModel CreateClaimAgencyDetails(ClaimAgencyDetailsModel model)
        {
            ClaimAgencyDetailsModel data = new ClaimAgencyDetailsModel();

            try
            {
                data = ClaimAgencyDao.CreateClaimAgencyDetails(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateClaimAgencyDetails", ex.Message, "Service");

            }


            return data;
        }

        public void DeleteClaimAgencyDetails(int id)
        {
            try
            {
                ClaimAgencyDao.DeleteClaimAgencyDetails(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteClaimAgencyDetails", ex.Message, "Service");

            }
        }

        public IList<BookModel> GetBookByAgencyId(BookModel model)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetBookByAgencyId(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetBookByAgencyId", ex.Message, "Service");

            }


            return data;
        }

        public IList<BookModel> GetReactionByTalentId(int TalentId)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetReactionByTalentId(TalentId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetReactionByTalentId", ex.Message, "Service");

            }


            return data;
        }

        public IList<BookModel> GetTransactionOrders(string Period)
        {
            IList<BookModel> data = new List<BookModel>();

            try
            {
                data = BookDao.GetTransactionOrders(Period);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTransactionOrders", ex.Message, "Service");

            }


            return data;
        }

        public BookModel GetPotensialSalesPerPeriod(string Period)
        {
            BookModel data = new BookModel();

            try
            {
                data = BookDao.GetPotensialSalesPerPeriod(Period);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPotensialSalesPerPeriod", ex.Message, "Service");

            }


            return data;
        }

        public BookModel GetTotalSalesPerPeriod(string Period)
        {
            BookModel data = new BookModel();

            try
            {
                data = BookDao.GetTotalSalesPerPeriod(Period);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTotalSalesPerPeriod", ex.Message, "Service");

            }


            return data;
        }
        #endregion

        public QuestionModel CreateQuestion(QuestionModel model)
        {
            var data = new QuestionModel();

            try
            {
                data = QuestionDao.CreateQuestion(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateQuestion", ex.Message, "Service");

            }

            return data;
        }

        public AnswerModel CreateAnswer(AnswerModel model)
        {
            var data = new AnswerModel();

            try
            {
                data = AnswerDao.CreateAnswer(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateAnswer", ex.Message, "Service");

            }

            return data;
        }

        public CommentModel CreateComment(CommentModel model)
        {
            var data = new CommentModel();

            try
            {
                data = CommentDao.CreateComment(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateComment", ex.Message, "Service");

            }

            return data;
        }

        public PostCommentModel CreatePostComment(PostCommentModel model)
        {
            var data = new PostCommentModel();

            try
            {
                data = CommentDao.CreatePostComment(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePostComment", ex.Message, "Service");

            }

            return data;
        }

        public SubCommentModel CreatePostSubComment(SubCommentModel model)
        {
            var data = new SubCommentModel();

            try
            {
                data = CommentDao.CreatePostSubComment(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePostSubComment", ex.Message, "Service");

            }

            return data;
        }
        public IList<QuestionModel> GetAllQuestion()
        {
            IList<QuestionModel> data = new List<QuestionModel>();

            try
            {
                data = QuestionDao.GetAllQuestion();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllQuestion", ex.Message, "Service");

            }

            return data;
        }
        public IList<QuestionModel> GetDetailQuestion()
        {
            IList<QuestionModel> data = new List<QuestionModel>();

            try
            {
                data = QuestionDao.GetDetailQuestion();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetDetailQuestion", ex.Message, "Service");

            }

            return data;
        }

        public QuestionLikeModel CreateQuestionLike(QuestionLikeModel model)
        {
            var data = new QuestionLikeModel();

            try
            {
                data = QuestionDao.CreateQuestionLike(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateQuestionLike", ex.Message, "Service");

            }

            return data;
        }

        public QuestionLikeModel RemoveQuestionLike(QuestionLikeModel model)
        {
            var data = new QuestionLikeModel();
            try
            {
                QuestionDao.RemoveQuestionLike(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "RemoveQuestionLike", ex.Message, "Service");
            }
            return data;
        }

       
        public TalentLikeVideoModel CreateTalentVideoLike(TalentLikeVideoModel model)
        {
            var data = new TalentLikeVideoModel();

            try
            {
                data = TalentDao.CreateTalentVideoLike(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTalentVideoLike", ex.Message, "Service");

            }

            return data;
        }

        public TalentLikeVideoModel RemoveTalentVideoLike(TalentLikeVideoModel model)
        {
            var data = new TalentLikeVideoModel();
            try
            {
                TalentDao.RemoveTalentVideoLike(model);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "RemoveTalentVideoLike", ex.Message, "Service");
            }
            return data;
        }

        public QuestionVideoModel CreateQuestionVideo(QuestionVideoModel model)
        {
            var data = new QuestionVideoModel();

            try
            {
                data = TalentDao.CreateQuestionVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateQuestionVideo", ex.Message, "Service");

            }

            return data;
        }

        public AnswerQuestionVideoModel CreateAnswerQuestionVideo(AnswerQuestionVideoModel model)
        {
            var data = new AnswerQuestionVideoModel();

            try
            {
                data = TalentDao.CreateAnswerQuestionVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateAnswerQuestionVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<QuestionVideoModel> GetAllQuestionVideo()
        {
            IList<QuestionVideoModel> data = new List<QuestionVideoModel>();

            try
            {
                data = TalentDao.GetAllQuestionVideo();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllQuestionVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<QuestionVideoModel> GetAllQuestionVideo2(QuestionVideoModel model)
        {
            IList<QuestionVideoModel> data = new List<QuestionVideoModel>();

            try
            {
                data = TalentDao.GetAllQuestionVideo2(model);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllQuestionVideo", ex.Message, "Service");

            }

            return data;
        }

        public PostCommentVideoModel CreatePostCommentVideo(PostCommentVideoModel model)
        {
            var data = new PostCommentVideoModel();

            try
            {
                data = CommentDao.CreatePostCommentVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePostCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public CommentVideoModel CreateCommentVideo(CommentVideoModel model)
        {
            var data = new CommentVideoModel();

            try
            {
                data = CommentDao.CreateCommentVideo(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public SubCommentVideoModel CreatePostSubComment(SubCommentVideoModel model)
        {
            var data = new SubCommentVideoModel();

            try
            {
                data = CommentDao.CreatePostSubComment(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePostSubComment", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewCommentVideoModel> GetAllPostCommentVideo()
        {
            IList<ViewCommentVideoModel> data = new List<ViewCommentVideoModel>();

            try
            {
                data = TalentDao.GetAllPostCommentVideo();



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPostCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewCommentVideoModel> GetAllPostCommentVideo2(ViewCommentVideoModel model)
        {
            IList<ViewCommentVideoModel> data = new List<ViewCommentVideoModel>();

            try
            {
                data = TalentDao.GetAllPostCommentVideo2(model);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPostCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo()
        {
            IList<ViewReplyCommentVideoModel> data = new List<ViewReplyCommentVideoModel>();

            try
            {
                data = TalentDao.GetAllReplyCommentVideo();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllReplyCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo2(ViewReplyCommentVideoModel model)
        {
            IList<ViewReplyCommentVideoModel> data = new List<ViewReplyCommentVideoModel>();

            try
            {
                data = TalentDao.GetAllReplyCommentVideo2(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllReplyCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo()
        {
            IList<ViewSubCommentVideoModel> data = new List<ViewSubCommentVideoModel>();

            try
            {
                data = TalentDao.GetSubReplyCommentVideo();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSubReplyCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo2(ViewSubCommentVideoModel model)
        {
            IList<ViewSubCommentVideoModel> data = new List<ViewSubCommentVideoModel>();

            try
            {
                data = TalentDao.GetSubReplyCommentVideo2(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetSubReplyCommentVideo", ex.Message, "Service");

            }

            return data;
        }

        public QuestionComment CreateQuestionComment(QuestionComment model)
        {
            var data = new QuestionComment();

            try
            {
                data = QuestionDao.CreateQuestionComment(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateQuestionComment", ex.Message, "Service");

            }

            return data;
        }

        public IList<GetQuestionComment> GetQuestionComment()
        {
            IList<GetQuestionComment> data = new List<GetQuestionComment>();

            try
            {
                data = QuestionDao.GetQuestionComment();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetQuestionComment", ex.Message, "Service");

            }

            return data;
        }

        #region LiveStream

        public IList<LiveStreamModel> GetAllLiveStream()
        {
            IList<LiveStreamModel> data = new List<LiveStreamModel>();

            try
            {
                data = LiveStreamDao.GetAllLiveStream();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllLiveStream", ex.Message, "Service");

            }

            return data;
        }

        public IList<LiveStreamModel> GetAllLiveStreamByUserId(int? UserId)
        {
            IList<LiveStreamModel> data = new List<LiveStreamModel>();

            try
            {
                data = LiveStreamDao.GetAllLiveStreamByUserId(UserId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllLiveStreamByUserId", ex.Message, "Service");

            }

            return data;
        }

        public LiveStreamModel GetLiveStream(LiveStreamModel model)
        {
            var data = new LiveStreamModel();

            try
            {
                data = LiveStreamDao.GetLiveStream(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetLiveStream", ex.Message, "Service");

            }

            return data;
        }

        public LiveStreamModel CreateLiveStream(LiveStreamModel model)
        {

            var data = new LiveStreamModel();

            try
            {
                data = LiveStreamDao.CreateLiveStream(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateLiveStream", ex.Message, "Service");

            }

            return data;
        }

        public LiveStreamModel UpdateLiveStream(LiveStreamModel model)
        {
            var data = new LiveStreamModel();

            try
            {
                data = LiveStreamDao.UpdateLiveStream(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateLiveStream", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteLiveStream(int id)
        {
            try
            {
                LiveStreamDao.DeleteLiveStream(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteLiveStreamSticker", ex.Message, "Service");

            }
        }

        public IList<LiveStreamStickerModel> GetAllLiveStreamSticker()
        {
            IList<LiveStreamStickerModel> data = new List<LiveStreamStickerModel>();

            try
            {
                data = LiveStreamStickerDao.GetAllLiveStreamSticker();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllLiveStreamSticker", ex.Message, "Service");

            }

            return data;
        }

        public IList<LiveStreamStickerModel> GetAllLiveStreamStickerByLiveStreamId(int? LiveStreamId)
        {
            IList<LiveStreamStickerModel> data = new List<LiveStreamStickerModel>();

            try
            {
                data = LiveStreamStickerDao.GetAllLiveStreamStickerByLiveStreamId(LiveStreamId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllLiveStreamStickerByLiveStreamId", ex.Message, "Service");

            }

            return data;
        }

        public LiveStreamStickerModel GetLiveStreamSticker(LiveStreamStickerModel model)
        {
            var data = new LiveStreamStickerModel();

            try
            {
                data = LiveStreamStickerDao.GetLiveStreamSticker(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetLiveStreamSticker", ex.Message, "Service");

            }

            return data;
        }

        public LiveStreamStickerModel CreateLiveStreamSticker(LiveStreamStickerModel model)
        {
            var data = new LiveStreamStickerModel();

            try
            {
                data = LiveStreamStickerDao.CreateLiveStreamSticker(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateLiveStreamSticker", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteLiveStreamSticker(int id)
        {
            try
            {
               LiveStreamStickerDao.DeleteLiveStreamSticker(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteLiveStreamSticker", ex.Message, "Service");

            }
        }

        public FilesLikeModel CreateFilesLikes(FilesLikeModel model)
        {
            var data = new FilesLikeModel();

            try
            {
                data = FilesDao.CreateFilesLikes(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateFilesLikes", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteFileLikes(FilesLikeModel model)
        {
            try
            {
                FilesDao.DeleteFileLikes(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteFileLikes", ex.Message, "Service");

            }
        }

        public int GetCountFilesLikes(FilesLikeModel model)
        {
            var data = 0;

            try
            {
                data = FilesDao.GetCountFilesLikes(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetCountFilesLikes", ex.Message, "Service");

            }

            return data;
        }

        public IList<FilesModel> GetFiles(int id)
        {
            IList<FilesModel> data = new List<FilesModel>();

            try
            {
                data = FilesDao.GetFiles(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetFiles", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteVoucherTransactionByBookId(int BookId)
        {
            try
            {
                VoucherTransactionDao.DeleteVoucherTransactionByBookId(BookId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteVoucherTransactionByBookId", ex.Message, "Service");

            }
        }



        #endregion


        public IList<RequestChangeModel> GetAllRequestChange()
        {
            IList<RequestChangeModel> data = new List<RequestChangeModel>();

            try
            {
                data = RequestChangeDao.GetAllRequestChange();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRequestChange", ex.Message, "Service");

            }

            return data;
        }

        public RequestChangeModel GetRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();

            try
            {
                data = RequestChangeDao.GetRequestChange(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRequestChange", ex.Message, "Service");

            }

            return data;
        }

        public RequestChangeModel CreateRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();

            try
            {
                data = RequestChangeDao.CreateRequestChange(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRequestChange", ex.Message, "Service");

            }

            return data;
        }

        public RequestChangeModel UpdateRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();

            try
            {
                data = RequestChangeDao.UpdateRequestChange(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "RequestChangeModel", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteRequestChange(string RequestCode)
        {
            try
            {
                RequestChangeDao.DeleteRequestChange(RequestCode);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteRequestChange", ex.Message, "Service");

            }
        }

        public IList<QuestionModel> GetAllQuestionByTalentId(int TalentId)
        {
            IList<QuestionModel> data = new List<QuestionModel>();

            try
            {
                data = QuestionDao.GetAllQuestionByTalentId(TalentId);



            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllQuestionByTalentId", ex.Message, "Service");

            }

            return data;
        }
    }
}
