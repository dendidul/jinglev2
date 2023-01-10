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
    public class CommentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public CommentDao(IConfiguration config)
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

        public CommentModel CreateComment(CommentModel model)
        {
            var data = new CommentModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Message", model.Message);
                param.Add("@TalentId", model.TalentId);
                param.Add("@UserId", model.UserId);
                param.Add("@ObjectId", model.ObjectId);
                data = conn.Query<CommentModel>("sp_Tbl_Trx_PostCommentInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public PostCommentModel CreatePostComment(PostCommentModel model)
        {
            var data = new PostCommentModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CommentMsg", model.CommentMsg);
                param.Add("@CommentedDate", model.CommentedDate);
                param.Add("@PostId", model.PostId);
                param.Add("@UserId", model.UserId);
                data = conn.Query<PostCommentModel>("sp_Tbl_Trx_CommentInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public SubCommentModel CreatePostSubComment(SubCommentModel model)
        {
            var data = new SubCommentModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CommentMsg", model.CommentMsg);
                param.Add("@CommentedDate", model.CommentedDate);
                param.Add("@ComId", model.ComId);
                param.Add("@UserId", model.UserId);
                data = conn.Query<SubCommentModel>("sp_Tbl_Trx_SubCommentsInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public PostCommentVideoModel CreatePostCommentVideo(PostCommentVideoModel model)
        {
            var data = new PostCommentVideoModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Message", model.Message);
                param.Add("@FileId", model.FileId);
                param.Add("@UserId", model.UserId);
                param.Add("@IsActive", model.IsActive);
                data = conn.Query<PostCommentVideoModel>("sp_Tbl_Trx_PostCommentVideoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

        public CommentVideoModel CreateCommentVideo(CommentVideoModel model)
        {
            var data = new CommentVideoModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CommentMsg", model.CommentMsg);
                param.Add("@CommentedDate", model.CommentedDate);
                param.Add("@PostId", model.PostId);
                param.Add("@UserId", model.UserId);
                param.Add("@IsActive", model.IsActive);
                data = conn.Query<CommentVideoModel>("sp_Tbl_Trx_CommentVideoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }
        public SubCommentVideoModel CreatePostSubComment(SubCommentVideoModel model)
        {
            var data = new SubCommentVideoModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CommentMsg", model.CommentMsg);
                param.Add("@CommentedDate", model.CommentedDate);
                param.Add("@ComId", model.ComId);
                param.Add("@UserId", model.UserId);
                param.Add("@IsActive", model.IsActive);
                data = conn.Query<SubCommentVideoModel>("sp_Tbl_Trx_SubCommentVideoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }
    }
}
