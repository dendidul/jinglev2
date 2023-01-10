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
   public class ClaimAgencyDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ClaimAgencyDao(IConfiguration config)
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

        public IList<ClaimAgencyModel> GetAllClaimAgency()
        {
            var data = new List<ClaimAgencyModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ClaimAgencyModel>("sp_Tbl_Trx_ClaimAgencySelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ClaimAgencyModel GetClaimAgency(int id)
        {
            var data = new ClaimAgencyModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);


                    data = conn.Query<ClaimAgencyModel>("sp_Tbl_Trx_ClaimAgencySelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ClaimAgencyModel CreateClaimAgency(ClaimAgencyModel model)
        {
            var data = new ClaimAgencyModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Period", model.Period);
                    param.Add("@ClaimDate", model.ClaimDate);
                    param.Add("@AgencyId", model.AgencyId);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Status", model.Status);
                    param.Add("@ClaimId", model.ClaimId);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@IsActive",(int)Status.Active) ;



                    data = conn.Query<ClaimAgencyModel>("sp_Tbl_Trx_ClaimAgencyInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public ClaimAgencyModel UpdateClaimAgency(ClaimAgencyModel model)
        {
            var data = new ClaimAgencyModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@Period", model.Period);
                    param.Add("@ClaimDate", model.ClaimDate);
                    param.Add("@AgencyId", model.AgencyId);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Status", model.Status);
                    param.Add("@Amount", model.Amount);
                    param.Add("@UpdatedBy", model.UpdatedBy);
                    param.Add("@IsActive", (int)Status.Active);
                    param.Add("@ClaimId", model.ClaimId);
                  


                    data = conn.Query<ClaimAgencyModel>("sp_Tbl_Trx_ClaimAgencyUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteClaimAgency(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_ClaimAgencyDelete", param,
                             commandType: CommandType.StoredProcedure);



            }


        }

        public IList<ClaimAgencyDetailsModel> GetAllClaimAgencyDetails()
        {
            var data = new List<ClaimAgencyDetailsModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ClaimAgencyDetailsModel>("sp_Tbl_Trx_ClaimAgency_DetailsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ClaimAgencyDetailsModel CreateClaimAgencyDetails(ClaimAgencyDetailsModel model)
        {
            var data = new ClaimAgencyDetailsModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@ClaimId", model.ClaimId);
                    param.Add("@BookId", model.BookId);
                 



                    data = conn.Query<ClaimAgencyDetailsModel>("sp_Tbl_Trx_ClaimAgency_DetailsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteClaimAgencyDetails(int id)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tbl_Trx_ClaimAgency_DetailsDelete", param,
                             commandType: CommandType.StoredProcedure);



            }


        }



    }
}
