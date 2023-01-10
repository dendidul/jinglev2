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
     public  class PayMethodDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public PayMethodDao(IConfiguration config)
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

        public IList<PayMethodModel> GetAllPayMethod()
        {
            var data = new List<PayMethodModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<PayMethodModel>("sp_Tbl_Mst_PayMethodSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public PayMethodModel GetDataPayMethod(PayMethodModel model)
        {
            var data = new PayMethodModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);


                data = conn.Query<PayMethodModel>("sp_Tbl_Mst_PayMethodSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public PayMethodModel CreatePayMethodData(PayMethodModel model)
        {
            var data = new PayMethodModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@PayMethodNm", model.PayMethodNm);
                param.Add("@PayMethodDesc", model.PayMethodDesc);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreatedDate", DateTime.Now);
                param.Add("@IsActive", model.IsActive);

                data = conn.Query<PayMethodModel>("sp_Tbl_Mst_PayMethodInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public PayMethodModel UpdatePayMethodData(PayMethodModel model)
        {
            var data = new PayMethodModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@PayMethodNm", model.PayMethodNm);
                param.Add("@PayMethodDesc", model.PayMethodDesc);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsActive", model.IsActive);


                data = conn.Query<PayMethodModel>("sp_Tbl_Mst_CategoryUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }



    }
}
