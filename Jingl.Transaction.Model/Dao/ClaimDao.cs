using Dapper;
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
    public class ClaimDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ClaimDao(IConfiguration config)
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

        public IList<ClaimModel> GetClaimByPeriod(string Period)
        {
            var data = new List<ClaimModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Period", Period);


                    data = conn.Query<ClaimModel>("Sp_GetClaimByPeriod", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<ClaimModel> GetAllClaim()
        {
            var data = new List<ClaimModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ClaimModel>("sp_Tbl_Trx_ClaimSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ClaimModel GetClaim(int id)
        {
            var data = new ClaimModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);


                    data = conn.Query<ClaimModel>("sp_Tbl_Trx_ClaimSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ClaimModel CreateClaim(ClaimModel model)
        {
            var data = new ClaimModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();                 
                    param.Add("@UserId", model.UserId);
                    param.Add("@BankName", model.BankName);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@Period", model.Period);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Status", model.Status);
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@Message", model.Message);
                
                  

                    data = conn.Query<ClaimModel>("sp_Tbl_Trx_ClaimInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public ClaimModel UpdateClaim(ClaimModel model)
        {
            var data = new ClaimModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@ClmNumber", model.ClmNumber);
                    param.Add("@UserId", model.UserId);
                    param.Add("@BankName", model.BankName);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@Period", model.Period);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Status", model.Status);
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@UpdateBy", model.UpdatedBy);
                    param.Add("@Message", model.Message);


                    data = conn.Query<ClaimModel>("sp_Tbl_Trx_ClaimUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



    }
}
