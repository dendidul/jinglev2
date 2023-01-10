using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.User.Notification;
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
    public class DeviceDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public DeviceDao(IConfiguration config)
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


        public DeviceModel CreateDevice(DeviceModel model)
        {
            var data = new DeviceModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@Name", model.Name);
                param.Add("@PushEndpoint", model.PushEndpoint);
                param.Add("@PushP256DH", model.PushP256DH);
                param.Add("@PushAuth", model.PushAuth);

                data = conn.Query<DeviceModel>("sp_DevicesInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public IList<DeviceModel> GetAllDevice()
        {
            var data = new List<DeviceModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<DeviceModel>("sp_DevicesSelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public IList<DeviceModel> GetDeviceByUserId(int UserId)
        {
            var data = new List<DeviceModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);


                data = conn.Query<DeviceModel>("sp_DevicesByUserId", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public void DeleteDevice(int Id)
        {
            var data = new List<DeviceModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);


                conn.Execute("sp_DevicesDelete", param,
                            commandType: CommandType.StoredProcedure);



            }
        }

    }
}
