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
    public class EmailNotificationDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public EmailNotificationDao(IConfiguration config)
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

        public EmailNotificationModel GetEmailNotification(int Id)
        {
            var data = new EmailNotificationModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", Id);


                data = conn.Query<EmailNotificationModel>("sp_Tbl_Mst_EmailTemplateSelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public IList<EmailNotificationModel> GetAllEmailNotification()
        {
            var data = new List<EmailNotificationModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@id", null);


                data = conn.Query<EmailNotificationModel>("sp_Tbl_Mst_EmailTemplateSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

    }
}
