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
    public class RefundDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RefundDao(IConfiguration config)
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


        public IList<RefundModel> GetRefundByBatchNumber(string Period)
        {
            var data = new List<RefundModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@BatchNumber", Period);


                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundByBatchNumber", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<RefundModel> GetAllRefund()
        {
            var data = new List<RefundModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RefundModel GetRefund(int id)
        {
            var data = new RefundModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);


                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RefundModel CreateRefund(RefundModel model)
        {
            var data = new RefundModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.UserId);
                    param.Add("@CustomerName", model.CustomerName);
                    param.Add("@BankName", model.BankName);
                    param.Add("@BatchNumber", model.BatchNumber);                    
                    param.Add("@AccountNumber", model.AccountNumber);                 
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@Status", model.Status);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@OrderNo", model.OrderNo);
                    param.Add("@Amount", model.Amount);



                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public RefundModel UpdateRefund(RefundModel model)
        {
            var data = new RefundModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@RefundNumber", model.RefundNumber);
                    param.Add("@CustomerName", model.CustomerName);
                    param.Add("@UserId", model.UserId);
                    param.Add("@BankName", model.BankName);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@BatchNumber", model.BatchNumber);
                    param.Add("@OrderNo", model.OrderNo);
                    param.Add("@RequestDate", model.RequestDate);
                    param.Add("@PaidDate", model.PaidDate);
                    param.Add("@Status", model.Status);
                    param.Add("@Amount", model.Amount);
                    param.Add("@UpdateBy", model.UpdatedBy);

                    data = conn.Query<RefundModel>("sp_Tbl_Trx_RefundUpdate", param,
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
