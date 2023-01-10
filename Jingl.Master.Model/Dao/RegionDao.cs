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
    public class RegionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RegionDao(IConfiguration config)
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

        public IList<RegionModel> GetAllRegion()
        {
            var data = new List<RegionModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<RegionModel>("sp_Tbl_Mst_RegionSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }


    }
}
