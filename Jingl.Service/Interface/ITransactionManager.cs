using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Model.Agency.Master;
using System;
using System.Collections.Generic;
using System.Text;
using Jingl.General.Model.Admin.Master;

namespace Jingl.Service.Interface
{
    public interface ITransactionManager
    {
        IList<BookModel> GetAllBook();
        BookModel GetDataBook(BookModel model);
        BookModel CreateBookData(BookModel model);
        BookModel UpdateBookData(BookModel model);
        WishlistModel CreateWishlistData(WishlistModel model);
        BookModel GetBookConfirmation(BookModel model);
        IList<TalentCategoryViewModel> GetWishListByUserId(int userId);
        IList<BookModel> GetBookingByUserId(int UserId);
        IList<SupportModel> GetAllSupport();
        SupportModel CreateSupport(SupportModel model);
        SupportModel GetSupport(SupportModel model);
        IList<NotificationModel> GetNotificationForUser(int UserId);
        IList<NotificationModel> GetNotificationForTalent(int UserId);
        NotificationModel InsertNotification(NotificationModel model);
        PaymentBookLogModel CreatePaymentBookLog(PaymentBookLogModel model);
        BookModel GetDataBookByOrderId(string orderNo);
        FilesModel CreateFiles(FilesModel model);
        IList<TalentVideoModel> GetTalentVideos(int TalentId);
        //IList<TalentVideoModel> GetTalentVideosPertanyaan(int VideoId);
        IList<TalentVideoModel> GetUserVideos(int UserId);
        RatingModel CreateRatingFiles(RatingModel model);
        IList<BookModel> GetBookingByTalentId(int UserId);
        LandingModel GetBookingByVideoId(int VideoId);
        IList<BookModel> GetBookingPaidByTalentId(int TalentId);
        void IsReadedNotification(int Id);
        IList<BookModel> AdmGetAllBook();
        IList<BookModel> AgnGetAllBook(TalentParamModel model);
        BookModel AdmGetDataBook(BookModel model);
        SupportModel UpdateSupport(SupportModel model);
        void CreateTalentVideo(TalentVideoModel model);
        void DeleteTalentVideoByTalentId(int TalentId);
        void DeleteTalentVideo(int Id);
        IList<TalentVideoModel> GetAllVideo();
        IList<TalentVideoModel> GetAllVideo2(TalentVideoModel model);
        IList<BookModel> GetAllReactionVideo2(BookModel model);
        TalentLikeVideoModel CreateTalentVideoLike(TalentLikeVideoModel model);
        TalentLikeVideoModel RemoveTalentVideoLike(TalentLikeVideoModel model);
        IList<ClaimModel> GetClaimByPeriod(string Period);
        IList<ClaimModel> GetAllClaim();
        ClaimModel GetClaim(int id);
        ClaimModel CreateClaim(ClaimModel model);
        ClaimModel UpdateClaim(ClaimModel model);
        IList<RefundModel> GetRefundByBatchNumber(string Period);
        IList<RefundModel> GetAllRefund();
        RefundModel GetRefund(int id);
        RefundModel CreateRefund(RefundModel model);
        RefundModel UpdateRefund(RefundModel model);
        IList<BookModel> GetDailyPayment(BookModel model);
        IList<TalentVideoModel> GetAllVideoByCategory(int? CategoryId);
        TalentRegModel GetTalentRegistration(int Id);
        IList<TalentRegModel> GetAllTalentRegistration();
        TalentRegModel CreateTalentRegistration(TalentRegModel model);
        TalentRegModel UpdateTalentRegistration(TalentRegModel model);
        FilesWatchModel CreateFilesWatch(FilesWatchModel model);
        void DeleteBook(int id);
        void DeleteSupport(int id);
        BookModel AdmUpdateBookData(BookModel model);
        int GetWishlistIdByUserTalent(WishlistModel model);
        void RemoveWishlistData(WishlistModel model);

        IList<VoucherTransactionModel> GetAllVoucherTransaction();
        VoucherTransactionModel GetVoucherTransaction(int id);
        VoucherTransactionModel CreateVoucherTransaction(VoucherTransactionModel model);
        void DeleteVoucherTransaction(int id);
        IList<BookModel> GetReactionByTalentId(int TalentId);
        IList<BookModel> GetAllReactionVideo(int TalentId);
        IList<BookModel> GetTransactionOrders(string Period);
        BookModel GetPotensialSalesPerPeriod(string Period);
        BookModel GetTotalSalesPerPeriod(string Period);
        #region SALDO & TOPUP

        SaldoModel CreateSaldo(SaldoModel model);
        SaldoModel UpdateSaldo(SaldoModel model);
        IList<SaldoModel> GetAllSaldo();
        SaldoModel GetSaldoById(SaldoModel model);
        SaldoModel GetSaldoByTalentId(SaldoModel model);

        TopupModel CreateTopup(TopupModel model);
        IList<TopupModel> GetTopupBySaldoId(TopupModel model);
        IList<TopupModel> GetTopupByStatus(TopupModel model);
        TopupModel TopupApproval(TopupModel model);
        TopupModel GetTopupById(TopupModel model);

        #endregion

        #region Claim Agency


        IList<ClaimAgencyModel> GetAllClaimAgency();
        ClaimAgencyModel GetClaimAgency(int id);
        ClaimAgencyModel CreateClaimAgency(ClaimAgencyModel model);
        ClaimAgencyModel UpdateClaimAgency(ClaimAgencyModel model);
        void DeleteClaimAgency(int id);
        IList<ClaimAgencyDetailsModel> GetAllClaimAgencyDetails();
        ClaimAgencyDetailsModel CreateClaimAgencyDetails(ClaimAgencyDetailsModel model);
        void DeleteClaimAgencyDetails(int id);
        IList<BookModel> GetBookByAgencyId(BookModel model);

        #endregion

        #region QA
        QuestionModel CreateQuestion(QuestionModel model);
        AnswerModel CreateAnswer(AnswerModel model);
        IList<QuestionModel> GetAllQuestion();
        IList<QuestionModel> GetDetailQuestion();
        QuestionLikeModel CreateQuestionLike(QuestionLikeModel model);
        QuestionLikeModel RemoveQuestionLike(QuestionLikeModel model);
        QuestionComment CreateQuestionComment(QuestionComment model);
        IList<GetQuestionComment> GetQuestionComment();
        #endregion

        #region Comment
        CommentModel CreateComment(CommentModel model);
        PostCommentModel CreatePostComment(PostCommentModel model);
        SubCommentModel CreatePostSubComment(SubCommentModel model);
        #endregion

        #region VideoQuestion
        QuestionVideoModel CreateQuestionVideo(QuestionVideoModel model);
        AnswerQuestionVideoModel CreateAnswerQuestionVideo(AnswerQuestionVideoModel model);
        IList<QuestionVideoModel> GetAllQuestionVideo();
        IList<QuestionVideoModel> GetAllQuestionVideo2(QuestionVideoModel model);
        PostCommentVideoModel CreatePostCommentVideo(PostCommentVideoModel model);
        CommentVideoModel CreateCommentVideo(CommentVideoModel model);
        SubCommentVideoModel CreatePostSubComment(SubCommentVideoModel model);
        IList<ViewCommentVideoModel> GetAllPostCommentVideo();
        IList<ViewCommentVideoModel> GetAllPostCommentVideo2(ViewCommentVideoModel model);
        IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo();
        IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo2(ViewReplyCommentVideoModel model);
        IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo();
        IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo2(ViewSubCommentVideoModel model);
        #endregion


        #region LiveStream

        IList<LiveStreamModel> GetAllLiveStream();
        IList<LiveStreamModel> GetAllLiveStreamByUserId(int? UserId);
        LiveStreamModel GetLiveStream(LiveStreamModel model);
        LiveStreamModel CreateLiveStream(LiveStreamModel model);
        LiveStreamModel UpdateLiveStream(LiveStreamModel model);
        void DeleteLiveStream(int id);

        #endregion

        #region LiveStreamSticker

        IList<LiveStreamStickerModel> GetAllLiveStreamSticker();
        IList<LiveStreamStickerModel> GetAllLiveStreamStickerByLiveStreamId(int? LiveStreamId);
        LiveStreamStickerModel GetLiveStreamSticker(LiveStreamStickerModel model);
        LiveStreamStickerModel CreateLiveStreamSticker(LiveStreamStickerModel model);
        void DeleteLiveStreamSticker(int id);

        #endregion

        FilesLikeModel CreateFilesLikes(FilesLikeModel model);
        void DeleteFileLikes(FilesLikeModel model);
        int GetCountFilesLikes(FilesLikeModel model);
        IList<FilesModel> GetFiles(int id);
        void DeleteVoucherTransactionByBookId(int BookId);

        IList<RequestChangeModel> GetAllRequestChange();
        RequestChangeModel GetRequestChange(RequestChangeModel model);
        RequestChangeModel CreateRequestChange(RequestChangeModel model);
        RequestChangeModel UpdateRequestChange(RequestChangeModel model);
        void DeleteRequestChange(string RequestCode);

        IList<QuestionModel> GetAllQuestionByTalentId(int TalentId);


    }
}
