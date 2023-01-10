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
    public class RequestChangeDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RequestChangeDao(IConfiguration config)
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

        public IList<RequestChangeModel> GetAllRequestChange()
        {
            var data = new List<RequestChangeModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RequestCode", null);


                    data = conn.Query<RequestChangeModel>("sp_Tbl_Trx_Request_ChangeSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RequestChangeModel GetRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RequestCode", model.RequestCode);


                    data = conn.Query<RequestChangeModel>("sp_Tbl_Trx_Request_ChangeSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RequestChangeModel CreateRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@RequestCode", model.RequestCode);
                    param.Add("@RequestType", model.RequestType);
                    param.Add("@UserId", model.UserId);
                    param.Add("@IsActive", (int)Status.Active);
                  

                    data = conn.Query<RequestChangeModel>("sp_Tbl_Trx_Request_ChangeInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public RequestChangeModel UpdateRequestChange(RequestChangeModel model)
        {
            var data = new RequestChangeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RequestCode", model.RequestCode);
                    param.Add("@RequestType", model.RequestType);
                    param.Add("@UserId", model.UserId);
                    param.Add("@IsActive", model.IsActive);


                    data = conn.Query<RequestChangeModel>("sp_Tbl_Trx_Request_ChangeUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteRequestChange(string RequestCode)
        {
            var data = new RequestChangeModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@RequestCode", RequestCode);



                conn.Execute("sp_Tbl_Trx_Request_ChangeDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
