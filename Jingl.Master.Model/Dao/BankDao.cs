using Dapper;
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
    public class BankDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public BankDao(IConfiguration config)
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

        public IList<BankModel> GetAllBank()
        {
            var data = new List<BankModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<BankModel>("sp_Tbl_Mst_BankSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }
    }
}
