using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
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
    public class StickerDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public StickerDao(IConfiguration config)
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

        public IList<StickerModel> GetAllSticker()
        {
            var data = new List<StickerModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<StickerModel>("sp_Tbl_Mst_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<StickerModel> GetAllStickerByCategory(int? CategoryId)
        {
            var data = new List<StickerModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<StickerModel>("sp_Tbl_Mst_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    if(CategoryId.HasValue)
                    {
                        data = data.Where(x => x.StickerCategoryId == CategoryId).ToList();
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public StickerModel GetSticker(StickerModel model)
        {
            var data = new StickerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<StickerModel>("sp_Tbl_Mst_StickerSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public StickerModel CreateSticker(StickerModel model)
        {
            var data = new StickerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@StickerCd", model.StickerCd);
                    param.Add("@StickerNm", model.StickerNm);
                    param.Add("@StickerCategoryId", model.StickerCategoryId);
                    param.Add("@StickerDescription", model.StickerDescription);
                    param.Add("@Amount", model.Amount);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now);                 
                    param.Add("@IsActive", true);


                    data = conn.Query<StickerModel>("sp_Tbl_Mst_StickerInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public StickerModel UpdateSticker(StickerModel model)
        {
            var data = new StickerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@StickerCd", model.StickerCd);
                    param.Add("@StickerNm", model.StickerNm);
                    param.Add("@StickerCategoryId", model.StickerCategoryId);
                    param.Add("@StickerDescription", model.StickerDescription);
                    param.Add("@Amount", model.Amount);                   
                    param.Add("@UpdateBy", DateTime.Now);
                    param.Add("@UpdatedDate", model.CreatedBy);
                    param.Add("@IsActive", true);


                    data = conn.Query<StickerModel>("sp_Tbl_Mst_StickerUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteSticker(int id)
        {
            var data = new StickerModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Mst_StickerDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }
    }
}
