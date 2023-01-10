using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.User.ViewModel;
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
    public class ParameterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ParameterDao(IConfiguration config)
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

        public ParameterModel GetParameter(int Id)
        {
            var data = new ParameterModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", Id);


                data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }


        public ParameterModel GetParameterByCode(string Code)
        {
            var data = new ParameterModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@paramCode", Code);


                data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterByCode", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public IList<ParameterModel> AdmGetAllParameter()
        {
            var data = new List<ParameterModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public ParameterModel CreateParam(ParameterModel model)
        {
            var data = new ParameterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@ParamCode", model.ParamCode);
                    param.Add("@ParamName", model.ParamName);
                    param.Add("@ParamValue", model.ParamValue);
                    param.Add("@IsActive", model.IsActive);

                    data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ParameterModel UpdateParam(ParameterModel model)
        {
            var data = new ParameterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@ParamCode", model.ParamCode);
                    param.Add("@ParamName", model.ParamName);
                    param.Add("@ParamValue", model.ParamValue);
                    param.Add("@IsActive", model.IsActive);

                    data = conn.Query<ParameterModel>("sp_Tbl_Mst_ParameterUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteParameter(int id)
        {
          
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Mst_ParameterDelete", param,
                           commandType: CommandType.StoredProcedure);



            }


        }

    }
}
