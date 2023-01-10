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

namespace Jingl.Transaction.Model.Dao
{
   public class WishlistDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public WishlistDao(IConfiguration config)
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

        public WishlistModel CreateWishlistData(WishlistModel model)
        {
            var data = new WishlistModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TalentId", model.TalentId);
                param.Add("@IsActive", 1);
             

                data = conn.Query<WishlistModel>("sp_Tbl_Trx_WishlistInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public void RemoveWishlistData(WishlistModel model)
        {

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TalentId", model.TalentId);
                                             
                conn.Execute("sp_RemoveWishlistData", param,
                             commandType: CommandType.StoredProcedure);

            }
        }

        public IList<TalentCategoryViewModel> GetWishListByUserId(int userId)
        {
            var data = new List<TalentCategoryViewModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@userId", userId);


                data = conn.Query<TalentCategoryViewModel>("Sp_GetWishlistByUserId", param,
                           commandType: CommandType.StoredProcedure).ToList();


            }

            return data;
        }

        public int GetWishlistIdByUserTalent(WishlistModel model)
        {
            int id = 0;

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TalentId", model.TalentId);


                id = conn.Query<Int32>("Sp_GetWishlistIdByUserTalent", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();


            }

            return id;
        }

        

    }
}
