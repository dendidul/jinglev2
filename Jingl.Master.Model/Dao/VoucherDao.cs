using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
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
    public class VoucherDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        #region INITIAL
        public VoucherDao(IConfiguration config)
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
        #endregion

        #region METHOD
        public VoucherModel CheckVoucherCode(int BookId, string VoucherCd, decimal PriceAmount)
        {
            var voucher = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@BookId", BookId);
                    param.Add("@VoucherCd", VoucherCd);
                    param.Add("@PriceAmount", PriceAmount);

                    voucher = conn.Query<VoucherModel>("sp_GetVerifyVoucherCode", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if(voucher!=null)
                    { 
                        var dataList = new List<VoucherTalentViewModel>();
                        param = new DynamicParameters();
                        param.Add("@Voucherid", voucher.Id);

                        dataList = conn.Query<VoucherTalentViewModel>("sp_Tbl_Mst_VoucherTalentSelect", param,
                                   commandType: CommandType.StoredProcedure).ToList();

                        voucher.VoucherSelectedTalent = dataList.Where(x => x.VoucherId == voucher.Id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return voucher;
        }

        public VoucherModel GetVoucherByCode(string VoucherCd)
        {
            var voucher = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherCd", VoucherCd);


                    voucher = conn.Query<VoucherModel>("sp_GetVoucherByCode", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return voucher;
        }

        public VoucherModel CreateVoucher(VoucherModel model)
        {
            var data = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@VoucherNm", model.VoucherNm);
                    param.Add("@VoucherDesc", model.VoucherDesc);
                    param.Add("@RemainingCount", model.RemainingCount);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Percentage", model.Percentage);
                    param.Add("@StartDate", model.StartDate);
                    param.Add("@EndDate", model.EndDate);
                    param.Add("@CreatedBy", model.CreatedBy);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@MinValue", model.MinValue);
                    param.Add("@MaxValue", model.MaxValue);
                    param.Add("@SentTo", model.SentTo);
                    param.Add("@Budget", model.Budget);
                    param.Add("@UsesPerCustomer", model.UsesPerCustomer);

                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public VoucherTalentViewModel CreateVoucherTalent(VoucherTalentViewModel model)
        {
            var data = new VoucherTalentViewModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherId", model.VoucherId);
                    param.Add("@TalentId", model.TalentId);

                    data = conn.Query<VoucherTalentViewModel>("sp_Tbl_Mst_VoucherTalentInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public void DeleteVoucher(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", id);

                    conn.Execute("sp_Tbl_Mst_VoucherDelete", param,
                                commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteVoucherTalentByVoucherId(int VoucherId)
        {

            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@VoucherId", VoucherId);


                    //param.Add("@RoleId", 2);

                    conn.Execute("sp_Tbl_Mst_VoucherTalentDelete", param,
                                 commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public VoucherModel GetVoucher(VoucherModel model)
        {
            var data = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                    var dataList = new List<VoucherTalentViewModel>();
                    param = new DynamicParameters();
                    param.Add("@Voucherid", model.Id);

                    dataList = conn.Query<VoucherTalentViewModel>("sp_Tbl_Mst_VoucherTalentSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    data.VoucherSelectedTalent = dataList.Where(x => x.VoucherId == model.Id).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<VoucherTalentViewModel> GetVoucherTalentData(int VoucherId)
        {
            var data = new List<VoucherTalentViewModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<VoucherTalentViewModel>("sp_Tbl_Mst_VoucherTalentSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    data = data.Where(x => x.VoucherId == VoucherId).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<VoucherModel> GetAllVoucher()
        {
            var data = new List<VoucherModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }        

        public VoucherModel UpdateVoucher(VoucherModel model)
        {
            var data = new VoucherModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@VoucherCd", model.VoucherCd);
                    param.Add("@VoucherNm", model.VoucherNm);
                    param.Add("@VoucherDesc", model.VoucherDesc);
                    param.Add("@RemainingCount", model.RemainingCount);
                    param.Add("@Amount", model.Amount);
                    param.Add("@Percentage", model.Percentage);
                    param.Add("@StartDate", model.StartDate);
                    param.Add("@EndDate", model.EndDate);
                    param.Add("@UpdatedBy", model.UpdatedBy);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@IsUsed", model.IsUsed);
                    param.Add("@IsClaimed", model.IsClaimed);
                    param.Add("@MinValue", model.MinValue);
                    param.Add("@MaxValue", model.MaxValue);
                    param.Add("@SentTo", model.SentTo);
                    param.Add("@Budget", model.Budget);
                    param.Add("@UsesPerCustomer", model.UsesPerCustomer);

                    data = conn.Query<VoucherModel>("sp_Tbl_Mst_VoucherUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<VoucherAllTalentModel> GetAllVoucherByTalent()
        {
            var data = new List<VoucherAllTalentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    data = conn.Query<VoucherAllTalentModel>("sp_Tbl_Mst_GetAllVoucherByTalent", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        #endregion
    }
}
