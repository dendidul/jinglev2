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

namespace Jingl.Master.Model.Dao
{
    public class TalentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TalentDao(IConfiguration config)
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

        public TalentModel CreateTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId",model.UserId);                                 
                    param.Add("@FollowersCount", model.FollowersCount);
                    param.Add("@RdyVideo", model.RdyVideo);
                    param.Add("@Rate", model.Rate);
                    param.Add("@PriceAmount", model.PriceAmount);
                    param.Add("@SalePrice", model.SalePrice);
                    param.Add("@PaymentShare", model.PaymentShare);
                    param.Add("@Status", model.Status);
                    param.Add("@IsActive",true);
                    param.Add("@CreatedBy", model.UserId);
                    param.Add("@Profession", model.Profesion);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IdCardFileId", model.IdCardFileId);
                    param.Add("@AccountNumberFileId", model.AccountNumberFileId);
                    param.Add("@NPWPFileId", model.NPWPFileId);
                    param.Add("@Instagram", model.Instagram);
                    param.Add("@Facebook", model.Facebook);
                    param.Add("@IsAvailable", model.IsAvailable);
                    param.Add("@IsUnderAgency", model.IsUnderAgency);
                    param.Add("@AgencyId", model.AgencyId);

                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public TalentCategoryViewModel CreateTalentCategory(TalentCategoryViewModel model)
        {
            var data = new TalentCategoryViewModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@CategoryId", model.CategoryId);
                   
                    //param.Add("@RoleId", 2);

                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategoryInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public void DeleteTalentCategoryByTalentId(int TalentId)
        {
           
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@TalentId", TalentId);
                 

                    //param.Add("@RoleId", 2);

                  conn.Execute("sp_Tbl_Mst_TalentCategoryDelete", param,
                               commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }





        public TalentModel GetTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


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


        public IList<TalentCategoryViewModel> GetTalentCategoryData(int TalentId)
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategorySelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    data = data.Where(x => x.TalentId == TalentId).ToList(); 


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



        public TalentModel GetTalentProfiles(int UserId)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", UserId);


                    data = conn.Query<TalentModel>("Sp_GetTalentProfiles", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentModel> GetAllTalent()
        {
            var data = new List<TalentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TalentModel GetTalentByUserId(int UserId)
        {
            var data = new TalentModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);


                data = conn.Query<TalentModel>("sp_getTalentByUserId", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }


        public IList<TalentModel> AdmGetAllTalent()
        {
            var data = new List<TalentModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<TalentModel>("Sp_GetAllTalentData", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }

        public IList<TalentModel> GetAllTalentForDropdown()
        {
            var data = new List<TalentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentModel>("sp_ddl_TalentSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentModel> GetAllTalentAgency(int UserId)
        {
            var data = new List<TalentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);
                    param.Add("@UserId", UserId);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentAgencySelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentCategoryViewModel> GetAllTalentByCategory(int CategoryId)
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@CategoryId", CategoryId);


                    data = conn.Query<TalentCategoryViewModel>("Sp_GetTalentByCategory", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<TalentCategoryViewModel> GetDistinctAllTalentByCategory()
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                   // param.Add("@CategoryId", CategoryId);


                    data = conn.Query<TalentCategoryViewModel>("sp_GetDistinctAllTalentCategory", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }




        public TalentModel UpdateTalent(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);                 
                    param.Add("@UserId", model.UserId);               
                  
                    param.Add("@FollowersCount", model.FollowersCount);
                    param.Add("@Profession", model.Profesion);
                    param.Add("@RdyVideo", model.RdyVideo);
                    param.Add("@Level", model.Level);
                    param.Add("@Email", model.Email);
                    param.Add("@Rate", model.Rate);
                    param.Add("@PriceAmount", model.PriceAmount);
                    param.Add("@SalePrice", model.SalePrice);
                    param.Add("@PaymentShare", model.PaymentShare);
                    param.Add("@Status", model.Status);
                    param.Add("@UpdatedBy", model.UserId);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@IdCardFileId", model.IdCardFileId);
                    param.Add("@AccountNumberFileId", model.AccountNumberFileId);
                    param.Add("@NPWPFileId", model.NPWPFileId);                   
                    param.Add("@Instagram", model.Instagram);
                    param.Add("@Facebook", model.Facebook);
                    param.Add("@Note", model.Note);
                    param.Add("@IsAvailable", model.IsAvailable);
                    param.Add("@IsUnderAgency", model.IsUnderAgency);
                    param.Add("@AgencyId", model.AgencyId);


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TalentModel UpdateBilling(TalentModel model)
        {
            var data = new TalentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@Bank", model.Bank);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@BeneficiaryName", model.BeneficiaryName);
             


                    data = conn.Query<TalentModel>("sp_Tbl_Mst_TalentUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentCategoryViewModel> GetTalentCategoryAllData()
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategoryAll", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<TalentCategoryViewModel> GetTalentCategoryAllDataNew()
        {
            var data = new List<TalentCategoryViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentCategoryViewModel>("sp_Tbl_Mst_TalentCategoryAll_New", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        #region TalentPerformance
        public IList<TalentPerformanceModel> GetTalentPerformance()
        {
            var data = new List<TalentPerformanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TalentPerformanceModel>("sp_Tbl_Mst_TalentPerformance", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }
        
        public IList<TalentPerformanceModel> GetTalentPerformanceAgency(int AgencyId)
        {
            var data = new List<TalentPerformanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);
                    param.Add("@AgencyId", AgencyId);

                    data = conn.Query<TalentPerformanceModel>("sp_Tbl_Mst_TalentPerformanceAgency", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<TalentPerformanceModel> GetTalentPerformanceByPeriod(string Period)
        {
            var data = new List<TalentPerformanceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Period", Period);


                    data = conn.Query<TalentPerformanceModel>("sp_Tbl_Mst_TalentPerformanceByPeriod", param,
                               commandType: CommandType.StoredProcedure).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }
        #endregion

        public TalentLikeVideoModel CreateTalentVideoLike(TalentLikeVideoModel model)
        {
            var data = new TalentLikeVideoModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@FileId", model.FileId);
                param.Add("@UserId", model.UserId);
                param.Add("@IsActive", 1);


                data = conn.Query<TalentLikeVideoModel>("sp_Tbl_Trx_Talent_VideoLikeInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public void RemoveTalentVideoLike(TalentLikeVideoModel model)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@FileId", model.FileId);
                param.Add("@UserId", model.UserId);

                conn.Execute("sp_Tbl_Trx_Talent_VideoLikeDelete", param,
                             commandType: CommandType.StoredProcedure);

            }
        }

        public QuestionVideoModel CreateQuestionVideo(QuestionVideoModel model)
        {
            var data = new QuestionVideoModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@FileId", model.FileId);
                param.Add("@TalentId", model.TalentId);
                param.Add("@Question", model.Question);
                param.Add("@IsActive", model.IsActive);
                param.Add("@IsAnswered", model.IsAnswered);
                data = conn.Query<QuestionVideoModel>("sp_Tbl_Trx_QuestionVideoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public AnswerQuestionVideoModel CreateAnswerQuestionVideo(AnswerQuestionVideoModel model)
        {
            var data = new AnswerQuestionVideoModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionVideoId", model.QuestionVideoId);
                param.Add("@Answer", model.Answer);

                data = conn.Query<AnswerQuestionVideoModel>("sp_Tbl_Trx_AnswerVideoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public IList<QuestionVideoModel> GetAllQuestionVideo()
        {
            var data = new List<QuestionVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@TalentId", null);


                data = conn.Query<QuestionVideoModel>("sp_Tbl_Trx_QuestionVideoSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<QuestionVideoModel> GetAllQuestionVideo2(QuestionVideoModel model)
        {
            var data = new List<QuestionVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@TalentId", null);
                param.Add("@talentname", model.TalentName);
                param.Add("@Offset", model.offset);
                param.Add("@fetch", model.fetch);

                data = conn.Query<QuestionVideoModel>("sp_Tbl_Trx_QuestionVideoSelect2", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }


        public IList<ViewCommentVideoModel> GetAllPostCommentVideo()
        {
            var data = new List<ViewCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@PostId", null);


                data = conn.Query<ViewCommentVideoModel>("sp_Tbl_Trx_Post_Comment_VideoSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<ViewCommentVideoModel> GetAllPostCommentVideo2(ViewCommentVideoModel model)
        {
            var data = new List<ViewCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@PostId", null);
                param.Add("@Offset", model.offset);
                param.Add("@fetch", model.fetch);

                data = conn.Query<ViewCommentVideoModel>("sp_Tbl_Trx_Post_Comment_VideoSelect2", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo()
        {
            var data = new List<ViewReplyCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ComID", null);


                data = conn.Query<ViewReplyCommentVideoModel>("sp_Tbl_Trx_Comment_VideoSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<ViewReplyCommentVideoModel> GetAllReplyCommentVideo2(ViewReplyCommentVideoModel model)
        {
            var data = new List<ViewReplyCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ComID", null);
                param.Add("@Offset", model.offset);
                param.Add("@fetch", model.fetch);


                data = conn.Query<ViewReplyCommentVideoModel>("sp_Tbl_Trx_Comment_VideoSelect2", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo()
        {
            var data = new List<ViewSubCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@SubComID", null);


                data = conn.Query<ViewSubCommentVideoModel>("sp_Tbl_Trx_SubComment_VideoSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }
        public IList<ViewSubCommentVideoModel> GetSubReplyCommentVideo2(ViewSubCommentVideoModel model)
        {
            var data = new List<ViewSubCommentVideoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@SubComID", null);
                param.Add("@Offset", model.offset);
                param.Add("@fetch", model.fetch);


                data = conn.Query<ViewSubCommentVideoModel>("sp_Tbl_Trx_SubComment_VideoSelect2", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }
    }
}
