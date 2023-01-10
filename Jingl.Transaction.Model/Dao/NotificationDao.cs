using Dapper;
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
    public class NotificationDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public NotificationDao(IConfiguration config)
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

        public IList<NotificationModel> GetNotificationForUser(int UserId)
        {
            var data = new List<NotificationModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@To", UserId);


                data = conn.Query<NotificationModel>("SP_GetNotificationUser", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public IList<NotificationModel> GetNotificationForTalent(int UserId)
        {
            var data = new List<NotificationModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@To", UserId);


                data = conn.Query<NotificationModel>("SP_GetNotificationTalent", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public void IsReadedNotification(int Id)
        {
          
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);


                conn.Execute("IsReadedNotification", param,
                            commandType: CommandType.StoredProcedure);



            }
        }

        public NotificationModel InsertNotification(NotificationModel model)
        {
            var data = new NotificationModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@To", model.To);
                    param.Add("@BookId", model.BookId);
                    param.Add("@Message", model.Message);
                    param.Add("@NotifType", model.NotifType);
                    param.Add("@NotifCategory", model.NotifCategory);
                    param.Add("@CreatedBy", "System");
                    param.Add("@IsReaded", 0);
                    param.Add("@IsActive", 1);
                    param.Add("@Link", model.Link);
                    param.Add("@IsSent", 0);



                    data = conn.Query<NotificationModel>("sp_Tbl_Trx_NotificationInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }



    }
}
