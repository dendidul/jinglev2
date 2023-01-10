using Dapper;
using Jingl.General.Enum;
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
    public class TalentRegistrationDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TalentRegistrationDao(IConfiguration config)
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

        public TalentRegModel GetTalentRegistration(int Id)
        {
            var data = new TalentRegModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", Id);


                data = conn.Query<TalentRegModel>("sp_Tbl_Trx_TalentRegSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public IList<TalentRegModel> GetAllTalentRegistration()
        {
            var data = new List<TalentRegModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", null);


                data = conn.Query<TalentRegModel>("sp_Tbl_Trx_TalentRegSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public TalentRegModel CreateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TalentNm", model.TalentNm);
                param.Add("@Email", model.Email);
                param.Add("@Instagram", model.Instagram);
                param.Add("@Facebook", model.Facebook);
                param.Add("@Profession", model.Profesion);
                param.Add("@Rdy", model.Rdy);
                param.Add("@Status", (int)Registration.Submit);
                param.Add("@Note", model.Note);              
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CategoryId", model.CategoryId);

                data = conn.Query<TalentRegModel>("sp_Tbl_Trx_TalentRegInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public TalentRegModel UpdateTalentRegistration(TalentRegModel model)
        {
            var data = new TalentRegModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.id);
                param.Add("@RegNum", model.RegNum);
                param.Add("@UserId", model.UserId);
                param.Add("@TalentNm", model.TalentNm);
                param.Add("@Email", model.Email);
                param.Add("@Instagram", model.Instagram);
                param.Add("@Facebook", model.Facebook);
                param.Add("@Profession", model.Profesion);
                param.Add("@Rdy", model.Rdy);
                param.Add("@Status", model.Status);
                param.Add("@Note", model.Note);
                param.Add("@UpdatedBy", model.UpdatedBy);


                data = conn.Query<TalentRegModel>("sp_Tbl_Trx_TalentRegUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }
    }
}
