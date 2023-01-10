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
    public class VoucherTransactionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public VoucherTransactionDao(IConfiguration config)
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


        public IList<VoucherTransactionModel> GetAllVoucherTransaction()
        {
            var data = new List<VoucherTransactionModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<VoucherTransactionModel>("sp_Tbl_Trx_VoucherSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public VoucherTransactionModel GetVoucherTransaction(int id)
        {
            var data = new VoucherTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);


                    data = conn.Query<VoucherTransactionModel>("sp_Tbl_Trx_VoucherSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public VoucherTransactionModel CreateVoucherTransaction(VoucherTransactionModel model)
        {
            var data = new VoucherTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherId", model.VoucherId);
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@BookId", model.BookId);
                    param.Add("@TalentId", model.TalentId);
                    param.Add("@UserId", model.UserId);
                    param.Add("@qty", model.qty);
                    param.Add("@Amount", model.Amount);



                    data = conn.Query<VoucherTransactionModel>("sp_Tbl_Trx_VoucherInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteVoucherTransaction(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);
             

                conn.Execute("sp_Tbl_Trx_VoucherDelete", param,
                             commandType: CommandType.StoredProcedure);

            }
        }

        public void DeleteVoucherTransactionByBookId(int BookId)
        {
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@BookId", BookId);


                conn.Execute("sp_DeleteVoucherTransactionByBookId", param,
                             commandType: CommandType.StoredProcedure);

            }
        }



    }


    


}
