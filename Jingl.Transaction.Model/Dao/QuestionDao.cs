using Dapper;
using Jingl.General.Enum;
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
    public class QuestionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public QuestionDao(IConfiguration config)
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

        public QuestionModel CreateQuestion(QuestionModel model)
        {
            var data = new QuestionModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TalentId", model.TalentId);
                param.Add("@Question", model.Question);
                param.Add("@IsActive", model.IsActive);
                param.Add("@IsAnswered", model.IsAnswered);
                data = conn.Query<QuestionModel>("sp_Tbl_Trx_QuestionInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }


        public QuestionComment CreateQuestionComment(QuestionComment model)
        {
            var data = new QuestionComment();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CommentMsg", model.CommentMsg);
                param.Add("@QuestionId", model.QuestionId);
                param.Add("@UserId", model.UserId);
                data = conn.Query<QuestionComment>("sp_Tbl_Trx_QuestionCommentInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public IList<QuestionModel> GetAllQuestion()
        {
            var data = new List<QuestionModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionId", null);


                data = conn.Query<QuestionModel>("sp_Tbl_Trx_GetAllQuestion", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<QuestionModel> GetAllQuestionByTalentId(int TalentId)
        {
            var data = new List<QuestionModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@TalentId", TalentId);


                data = conn.Query<QuestionModel>("sp_Tbl_Trx_GetAllQuestionByTalentId", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }



        

        public IList<QuestionModel> GetDetailQuestion()
        {
            var data = new List<QuestionModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionId", null);


                data = conn.Query<QuestionModel>("sp_Tbl_Trx_QuestionSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public IList<GetQuestionComment> GetQuestionComment()
        {
            var data = new List<GetQuestionComment>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ComID", null);


                data = conn.Query<GetQuestionComment>("sp_Tbl_Trx_QuestionCommentSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();

            }

            return data;
        }

        public QuestionLikeModel CreateQuestionLike(QuestionLikeModel model)
        {
            var data = new QuestionLikeModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionId", model.QuestionId);
                param.Add("@UserId", model.UserId);
                param.Add("@IsActive", 1);


                data = conn.Query<QuestionLikeModel>("sp_Tbl_Trx_Question_LikeInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public void RemoveQuestionLike(QuestionLikeModel model)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionId", model.QuestionId);
                param.Add("@UserId", model.UserId);

                conn.Execute("sp_Tbl_Trx_Question_LikeDelete", param,
                             commandType: CommandType.StoredProcedure);

            }
        }
    }
}
