using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jingl.UserManagement.Model.Dao
{
    public class MenuDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public MenuDao(IConfiguration config)
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


        public IList<MenuModel> GetAllMenu()
        {
            var data = new List<MenuModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<MenuModel>("sp_Tbl_Usm_MenuSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



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
