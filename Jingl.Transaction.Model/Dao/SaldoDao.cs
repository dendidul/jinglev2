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
    public class SaldoDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public SaldoDao(IConfiguration config)
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

        public SaldoModel CreateSaldo (SaldoModel model)
        {
            var data = new SaldoModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@TalentId", model.TalentId);
                param.Add("@CreatedBy", model.CreatedBy);

                data = conn.Query<SaldoModel>("sp_Tbl_Trx_SaldoInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }

        public SaldoModel UpdateSaldo (SaldoModel model)
        {
            var data = new SaldoModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
               
                param.Add("@Id", model.Id);
                param.Add("@TalentId", model.TalentId);
                param.Add("@SaldoAmt", model.SaldoAmt);
                param.Add("@SaldoUsedAmt", model.SaldoUsedAmt);
                param.Add("@IsActive", model.IsActive);


                data = conn.Query<SaldoModel>("sp_Tbl_Trx_SaldoUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public IList<SaldoModel> GetAllSaldo()
        {
            var data = new List<SaldoModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                data = conn.Query<SaldoModel>("sp_GetAllSaldo", param,
                           commandType: CommandType.StoredProcedure).ToList();
            }

            return data;
        }

        public SaldoModel GetSaldoById(SaldoModel model)
        {
            var data = new SaldoModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);

                data = conn.Query<SaldoModel>("sp_GetSaldoById", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }

        public SaldoModel GetSaldoByTalentId(SaldoModel model)
        {
            var data = new SaldoModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@TalentId", model.TalentId);

                data = conn.Query<SaldoModel>("sp_GetSaldoByTalentId", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return data;
        }

    }
}
