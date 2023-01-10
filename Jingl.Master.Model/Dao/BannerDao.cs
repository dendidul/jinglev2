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
    public class BannerDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public BannerDao(IConfiguration config)
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

        public IList<BannerModel> GetAllBanner()
        {
            var data = new List<BannerModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<BannerModel>("sp_Tbl_Mst_BannerSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public BannerModel GetBanner(BannerModel model)
        {
            var data = new BannerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<BannerModel>("sp_Tbl_Mst_BannerSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public BannerModel CreateBanner(BannerModel model)
        {
            var data = new BannerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                  
                    param.Add("@BannerCategory", model.BannerCategory);
                    param.Add("@FileId", model.FileId);
                    param.Add("@BannerNm", model.BannerNm);
                    param.Add("@BannerDesc", model.BannerDesc);
                    param.Add("@Link", model.Link);
                    param.Add("@Sequence", model.Sequence);                 
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@IsActive", true);
                    param.Add("@IsVisible", model.IsVisible);

                    data = conn.Query<BannerModel>("sp_Tbl_Mst_BannerInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public BannerModel UpdateBanner(BannerModel model)
        {
            var data = new BannerModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@BannerCategory", model.BannerCategory);
                    param.Add("@FileId", model.FileId);
                    param.Add("@BannerNm", model.BannerNm);
                    param.Add("@BannerDesc", model.BannerDesc);
                    param.Add("@Link", model.Link);
                    param.Add("@Sequence", model.Sequence);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@IsActive", true);
                    param.Add("@IsVisible", model.IsVisible);


                    data = conn.Query<BannerModel>("sp_Tbl_Mst_BannerUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteBanner(int id)
        {
            var data = new BannerModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



               conn.Execute("sp_Tbl_Mst_BannerDelete", param,
                           commandType: CommandType.StoredProcedure);



            }


        }

    }
}
