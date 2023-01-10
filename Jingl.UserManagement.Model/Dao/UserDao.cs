using Dapper;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Jingl.General.Enum;

namespace Jingl.UserManagement.Model.Dao
{
    public class UserDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public UserDao(IConfiguration config)
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


        public UserModel CheckValidUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserName", model.UserName);
                    param.Add("@Password", model.Password);

                    data = conn.Query<UserModel>("SP_CheckValidUser", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return data;
        }

        public UserModel CheckValidUserExternal(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Email", model.Email);
                    param.Add("@SignUpType", model.SignUpType);

                    data = conn.Query<UserModel>("SP_CheckValidUserExternal", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return data;
        }

        public bool VerifyUserCode(UserModel model)
        {
            var data = new bool();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.Id);
                    param.Add("@Code", model.VerificationCode);

                    data = conn.Query<UserModel>("SP_VerifyCodeUser", param,
                               commandType: CommandType.StoredProcedure).Any();


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return data;
        }

        public void SetVerificationCode(UserModel model)
        {

            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", model.Id);


                    //data = conn.Query<UserModel>("SP_SetVerificationCodeUser", param,
                    //           commandType: CommandType.StoredProcedure).Any();
                    conn.Execute("SP_SetVerificationCodeUser", param,
                             commandType: CommandType.StoredProcedure);


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }


        }


        public UserModel UpdateUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@FirstName", model.FirstName);
                    param.Add("@LastName", model.LastName);
                    param.Add("@PhoneNumber", model.PhoneNumber);
                    param.Add("@UserName", model.UserName);
                    param.Add("@Bio", model.Bio);
                    param.Add("@Email", model.Email);
                    param.Add("@Password", model.Password);
                    param.Add("@Country", model.Country);
                    param.Add("@Region", model.Region);
                    param.Add("@BirthDate", model.BirthDate);
                    param.Add("@Gender", model.Gender);
                    param.Add("@UpdatedBy", model.Id);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@Status", model.Status);
                    param.Add("@RoleId", model.RoleId);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@ImgProfFileId", model.ImgProfFileId);
                    param.Add("@BgrProfFileId", model.BgrFileId);
                    param.Add("@IsActiveCode", model.IsActiveCode);
                    param.Add("@IsReceiveLetter", model.IsReceiveLetter);
                    param.Add("@Bank", model.Bank);
                    param.Add("@AccountNumber", model.AccountNumber);
                    param.Add("@BeneficiaryName", model.BeneficiaryName);
                    param.Add("@IsVerified", model.IsVerified);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }




        public UserModel SignUpTypeUpdate(UserModel model)
        {
            var data = new UserModel();

            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);
                    param.Add("@SignUpType", model.SignUpType);
                    data = conn.Query<UserModel>("sp_Tbl_Usm_SignUpTypeUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<UserModel> GetAllUser()
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<UserModel> GetAllUserForDropdown()
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UserModel>("sp_ddl_UserSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public IList<UserModel> GetUserByEmail(UserModel model)
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Email", model.Email);


                    data = conn.Query<UserModel>("sp_GetUserByEmail", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UserModel GetUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.Id);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public UserModel GetUserProfiles(int UserId)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", UserId);


                    data = conn.Query<UserModel>("Sp_GetUserProfiles", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



        public UserModel CreateUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@SignUpType", model.SignUpType);
                    param.Add("@FirstName", model.FirstName);
                    param.Add("@LastName", model.LastName);
                    param.Add("@PhoneNumber", model.PhoneNumber);
                    param.Add("@Bio", model.Bio);
                    param.Add("@UserName", model.UserName);
                    param.Add("@Email", model.Email);
                    param.Add("@Password", model.Password);
                    param.Add("@Country", model.Country);
                    param.Add("@Region", model.Region);
                    param.Add("@BirthDate", model.BirthDate);
                    param.Add("@Gender", model.Gender);
                    param.Add("@CreatedBy", model.Email);
                    param.Add("@CreatedDate", DateTime.Now);                  
                    param.Add("@Status",model.Status);
                    param.Add("@RoleId", model.RoleId);
                    param.Add("@IsActive", model.IsActive);
                    param.Add("@DefaultUsername", model.DefaultUsername);
                    param.Add("@DefaultPassword", model.DefaultPassword);
                    param.Add("@IsVerified", model.IsVerified);


                    data = conn.Query<UserModel>("sp_Tbl_Usm_UserInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public IList<UserModel> AdmGetAllUser()
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UserModel>("SP_GetAllUserData", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteUser(int id)
        {
          
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



              conn.Execute("sp_Tbl_Usm_UserDelete", param,
                           commandType: CommandType.StoredProcedure);



            }


        }




    }
}
