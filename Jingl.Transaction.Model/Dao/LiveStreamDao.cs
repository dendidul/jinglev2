using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Transaction;
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
    public class LiveStreamDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public LiveStreamDao(IConfiguration config)
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

        public IList<LiveStreamModel> GetAllLiveStream()
        {
            var data = new List<LiveStreamModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<LiveStreamModel> GetAllLiveStreamByUserId(int? UserId)
        {
            var data = new List<LiveStreamModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    if (UserId.HasValue)
                    {
                        data = data.Where(x => x.UserId == UserId).ToList();
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public LiveStreamModel GetLiveStream(LiveStreamModel model)
        {
            var data = new LiveStreamModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LiveStreamModel CreateLiveStream(LiveStreamModel model)
        {
            var data = new LiveStreamModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@UserId", model.UserId);
                    param.Add("@CategoryId", model.CategoryId);
                    param.Add("@LiveStreamNm", model.LiveStreamNm);
                    param.Add("@SecretKey", model.SecretKey);
                    param.Add("@ViewerCount", model.ViewerCount);
                    param.Add("@LiveSchedule", model.LiveSchedule);
                    param.Add("@IsLive", model.IsLive);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IsActive", true);


                    data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LiveStreamModel UpdateLiveStream(LiveStreamModel model)
        {
            var data = new LiveStreamModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@UserId", model.UserId);
                    param.Add("@CategoryId", model.CategoryId);
                    param.Add("@LiveStreamNm", model.LiveStreamNm);
                    param.Add("@SecretKey", model.SecretKey);
                    param.Add("@ViewerCount", model.ViewerCount);
                    param.Add("@LiveSchedule", model.LiveSchedule);
                    param.Add("@IsLive", model.IsLive);
                    param.Add("@UpdateBy", DateTime.Now);
                    param.Add("@UpdatedDate", model.CreatedBy);
                    param.Add("@IsActive", true);


                    data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteLiveStream(int id)
        {
            var data = new LiveStreamModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_LiveStreamDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }
    }
}
