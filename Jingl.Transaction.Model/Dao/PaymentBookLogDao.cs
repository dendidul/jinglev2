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
    public class PaymentBookLogDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public PaymentBookLogDao(IConfiguration config)
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

        public PaymentBookLogModel CreatePaymentBookLog(PaymentBookLogModel model)
        {
            var data = new PaymentBookLogModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@BookId", model.BookId);
                param.Add("@OrderId", model.OrderId);
                param.Add("@SnapToken", model.SnapToken);
                param.Add("@StatusCode", model.StatusCode);
                param.Add("@TransactionStatus", model.TransactionStatus);
                param.Add("@CreatedDate", DateTime.Now);


                data = conn.Query<PaymentBookLogModel>("sp_Tbl_Trx_Book_Payment_LogInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

    


    }
}
