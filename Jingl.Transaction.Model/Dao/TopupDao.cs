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
    public class TopupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TopupDao(IConfiguration config)
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

        public TopupModel CreateTopup(TopupModel model)
        {
            var data = new TopupModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@SaldoId", model.SaldoId);
                param.Add("@SeqNo", model.SeqNo);
                param.Add("@TopUpAmt", model.TopUpAmt);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@LastSaldoAmt", model.LastSaldoAmt);
                param.Add("@TopupStatus", model.TopupStatus);
                param.Add("@TopupSource", model.TopupSource);
                param.Add("@OrderNo", model.OrderNo);
                param.Add("@Notes", model.Notes);
                param.Add("@LastUsedSaldoAmt", model.LastUsedSaldoAmt);


                data = conn.Query<TopupModel>("sp_Tbl_Trx_TopupInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }

        public IList<TopupModel> GetTopupBySaldoId(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@SaldoId", model.SaldoId);

                data = conn.Query<TopupModel>("sp_GetTopupBySaldoId", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }

        public IList<TopupModel> GetTopupByStatus(TopupModel model)
        {
            IList<TopupModel> data = new List<TopupModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Status", model.TopupStatus);

                data = conn.Query<TopupModel>("sp_GetTopupByStatus", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }


        public TopupModel TopupApproval(TopupModel model)
        {
            var data = new TopupModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
               
                param.Add("@Id", model.Id);
                param.Add("@SaldoId", model.SaldoId);
                param.Add("@SeqNo", model.SeqNo);
                param.Add("@TopUpAmt", model.TopUpAmt);
                param.Add("@TopupStatus", model.TopupStatus);
                param.Add("@LastSaldoAmt", model.LastSaldoAmt);
                param.Add("@Notes", model.Notes);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@LastUsedSaldoAmt", model.LastUsedSaldoAmt);




                data = conn.Query<TopupModel>("sp_Tbl_Trx_TopupApprove", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }

        public TopupModel GetTopupById(TopupModel model)
        {
            var data = new TopupModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                param.Add("@Id", model.Id);

                data = conn.Query<TopupModel>("sp_GetTopupById", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }
    }
}
