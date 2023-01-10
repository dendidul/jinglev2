using Dapper;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jingl.Admin.UserManagement.Model.Dao
{
   public  class RolesAccMenuDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RolesAccMenuDao(IConfiguration config)
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

        public IList<RoleMenuViewModel> BuildRoleMenu(int roleid)
        {

            try
            {

                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RoleId", roleid);

                    var data = conn.Query<RoleMenuViewModel>("SP_BuildRoleMenu", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public bool CheckMenuForFoles(int roleid,string ControllerName)
        {

            try
            {

                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RoleId", roleid);
                    param.Add("@ControllerName", ControllerName);

                    var data = conn.Query<RoleMenuViewModel>("SP_CheckMenuForFoles", param,
                               commandType: CommandType.StoredProcedure).Any();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public IList<RoleAccessMenuModel> GetAllRoleAccess()
        {
            var data = new List<RoleAccessMenuModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<RoleAccessMenuModel>("sp_Tbl_Usm_RoleAccMenuSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RoleAccessMenuModel CreateRoleAccessMenu(RoleAccessMenuModel model)
        {
            var data = new RoleAccessMenuModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@RoleId", model.RoleId);
                    param.Add("@MenuId", model.MenuId);
                    param.Add("@IsActive", true);                  


                    data = conn.Query<RoleAccessMenuModel>("sp_Tbl_Usm_RoleAccMenuInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteAccessMenu(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@id", id);
                 


                   conn.Query("sp_Tbl_Usm_RoleAccMenuDelete", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

    }
}
