using Dapper;
using Jingl.General.Enum;
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
    public class RoleDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RoleDao(IConfiguration config)
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

        public IList<RoleModel> GetAllRole()
        {
            var data = new List<RoleModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<RoleModel>("sp_Tbl_Usm_RoleSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public RoleModel GetRole(RoleModel model)
        {
            var data = new RoleModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<RoleModel>("sp_Tbl_Usm_RoleSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RoleModel CreateRoleData(RoleModel model)
        {
            var data = new RoleModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@RoleNm", model.RoleNm);
                param.Add("@RoleDesc", model.RoleDesc);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreatedDate", DateTime.Now);
                param.Add("@IsActive", true);

                data = conn.Query<RoleModel>("sp_Tbl_Usm_RoleInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public RoleModel UpdateRoleData(RoleModel model)
        {
            var data = new RoleModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@RoleNm", model.RoleNm);
                param.Add("@RoleDesc", model.RoleDesc);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsActive", true);


                data = conn.Query<RoleModel>("sp_Tbl_Usm_RoleUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public void DeleteRole(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Usm_RoleDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }
    }
}
