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
    public class LiveStreamStickerDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public LiveStreamStickerDao(IConfiguration config)
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

        public IList<LiveStreamStickerModel> GetAllLiveStreamSticker()
        {
            var data = new List<LiveStreamStickerModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<LiveStreamStickerModel>("sp_Tbl_Trx_LiveStream_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<LiveStreamStickerModel> GetAllLiveStreamStickerByLiveStreamId(int? LiveStreamId)
        {
            var data = new List<LiveStreamStickerModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<LiveStreamStickerModel>("sp_Tbl_Trx_LiveStream_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    if (LiveStreamId.HasValue)
                    {
                        data = data.Where(x => x.LiveStreamId == LiveStreamId).ToList();
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public LiveStreamStickerModel GetLiveStreamSticker(LiveStreamStickerModel model)
        {
            var data = new LiveStreamStickerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<LiveStreamStickerModel>("sp_Tbl_Trx_LiveStream_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LiveStreamStickerModel CreateLiveStreamSticker(LiveStreamStickerModel model)
        {
            var data = new LiveStreamStickerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@LiveStreamId", model.LiveStreamId);
                    param.Add("@UserWatchId", model.UserWatchId);
                    param.Add("@StickerId", model.StickerId);
                    param.Add("@Amount", model.Amount);               
                   
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IsActive", true);


                    data = conn.Query<LiveStreamStickerModel>("sp_Tbl_Trx_LiveStream_StickerInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        //public LiveStreamModel UpdateLiveStream(LiveStreamModel model)
        //{
        //    var data = new LiveStreamModel();
        //    try
        //    {
        //        using (IDbConnection conn = Connection)
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Id", model.Id);
        //            param.Add("@UserId", model.UserId);
        //            param.Add("@CategoryId", model.CategoryId);
        //            param.Add("@LiveStreamNm", model.LiveStreamNm);
        //            param.Add("@SecretKey", model.SecretKey);
        //            param.Add("@ViewerCount", model.ViewerCount);
        //            param.Add("@LiveSchedule", model.LiveSchedule);
        //            param.Add("@IsLive", model.IsLive);
        //            param.Add("@UpdateBy", DateTime.Now);
        //            param.Add("@UpdatedDate", model.CreatedBy);
        //            param.Add("@IsActive", true);


        //            data = conn.Query<LiveStreamModel>("sp_Tbl_Trx_LiveStreamUpdate", param,
        //                       commandType: CommandType.StoredProcedure).FirstOrDefault();

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;

        //    }
        //    return data;
        //}

        public void DeleteLiveStreamSticker(int id)
        {
            var data = new LiveStreamModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_LiveStream_StickerDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }
    }
}
