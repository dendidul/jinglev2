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
    public class AgencyDao
    {

        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public AgencyDao(IConfiguration config)
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

        public IList<AgencyModel> GetAllAgency()
        {
            var data = new List<AgencyModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<AgencyModel>("sp_Tbl_Mst_AgencySelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<AgencyModel> GetSingleAgency()
        {
            var data = new List<AgencyModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<AgencyModel>("[sp_Tbl_Mst_SingleAgency]", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public AgencyModel GetAgency(AgencyModel model)
        {
            var data = new AgencyModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<AgencyModel>("sp_Tbl_Mst_AgencySelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public AgencyModel CreateAgency(AgencyModel model)
        {
            var data = new AgencyModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);              
                param.Add("@AgencyNm", model.AgencyNm);
                param.Add("@Description", model.Description);
                param.Add("@PICNm", model.PICNm);
                param.Add("@Telp", model.Telp);
                param.Add("@Email", model.Email);
                param.Add("@BankNm", model.BankNm);
                param.Add("@BeneficiaryNm", model.BeneficiaryNm);
                param.Add("@AccountNo", model.AccountNo);
                param.Add("@NPWPNm", model.NPWPNm);
                param.Add("@NPWPNo", model.NPWPNo);
                param.Add("@NPWPAddress", model.NPWPAddress);              
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreatedDate", DateTime.Now);
                param.Add("@IsActive", true);

                data = conn.Query<AgencyModel>("sp_Tbl_Mst_AgencyInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public AgencyModel UpdateAgency(AgencyModel model)
        {
            var data = new AgencyModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@UserId", model.UserId);
                param.Add("@AgencyNm", model.AgencyNm);
                param.Add("@Description", model.Description);
                param.Add("@PICNm", model.PICNm);
                param.Add("@Telp", model.Telp);
                param.Add("@Email", model.Email);
                param.Add("@BankNm", model.BankNm);
                param.Add("@BeneficiaryNm", model.BeneficiaryNm);
                param.Add("@AccountNo", model.AccountNo);
                param.Add("@NPWPNm", model.NPWPNm);
                param.Add("@NPWPNo", model.NPWPNo);
                param.Add("@NPWPAddress", model.NPWPAddress);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsActive", true);


                data = conn.Query<AgencyModel>("sp_Tbl_Mst_AgencyUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public void DeleteAgency(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Mst_AgencyDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
