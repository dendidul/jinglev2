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
    public class SupportDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public SupportDao(IConfiguration config)
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

        public IList<SupportModel> GetAllSupport()
        {
            var data = new List<SupportModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<SupportModel>("sp_Tbl_Trx_SupportSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }
        public SupportModel GetSupport(SupportModel model)
        {
            var data = new SupportModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<SupportModel>("sp_Tbl_Trx_SupportSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public SupportModel CreateSupport(SupportModel model)
        {
            var data = new SupportModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Details", model.Details);
                    param.Add("@Subject", model.Subject);
                    param.Add("@EmailAddress", model.EmailAddress);
                    param.Add("@Status", Registration.Submit);
                    
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IsActive", (int)Status.Active);

                    data = conn.Query<SupportModel>("sp_Tbl_Trx_SupportInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public SupportModel UpdateSupport(SupportModel model)
        {
            var data = new SupportModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@Details", model.Details);
                    param.Add("@Subject", model.Subject);
                    param.Add("@EmailAddress", model.EmailAddress);
                    param.Add("@Status", model.Status);
                    param.Add("@UpdatedBy", model.CreatedBy);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@IsActive", (int)Status.Active);


                    data = conn.Query<SupportModel>("sp_Tbl_Trx_SupportUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteSupport(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_SupportDelete", param,
                             commandType: CommandType.StoredProcedure);



            }


        }


    }
}
