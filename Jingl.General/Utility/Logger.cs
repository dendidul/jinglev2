using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Jingl.General.Model.Admin.Transaction;
using Dapper;
using System.Linq;

namespace Jingl.General.Utility
{
    public class Logger
    {
        private readonly IConfiguration _config;


        public Logger(IConfiguration config)
        {
          
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public void WriteFunctionLog(string destinationFolder, string user, string functionName, string message,string ErrorSource)
        {
            if (Directory.Exists(destinationFolder))
            {
                string pathfile = destinationFolder + "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                //if (File.Exists(pathfile))
                //{

                //    StreamWriter wr = File.AppendText(pathfile);
                //    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy hh:mm") + " - " + user + " == " + functionName + " == " + message);
                //    wr.Close();
                //}
                //else
                //{
                //    File.Create(pathfile).Dispose();
                //    StreamWriter wr = File.AppendText(pathfile);
                //    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy hh:mm") + " - " + user + " == " + functionName + " == " + message);
                //    wr.Close();
                //}


            }

            else
            {
                DirectoryInfo di = Directory.CreateDirectory(destinationFolder);
                //string pathfile = destinationFolder + "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                //if (File.Exists(pathfile))
                //{
                //    StreamWriter wr = File.AppendText(pathfile);
                //    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy hh:mm") + " - " + user + " == " + functionName + " == " + message);
                //    wr.Close();
                //}
                //else
                //{
                //    File.Create(pathfile).Dispose();
                //    StreamWriter wr = File.AppendText(pathfile);
                //    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy hh:mm") + " - " + user + " == " + functionName + " == " + message);
                //    wr.Close();
                //}
            }


            var data = new ErrorLogModel ();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ErrorSource", ErrorSource);
                param.Add("@ErrorMessage", message);
                param.Add("@FunctionName", functionName);


                data = conn.Query<ErrorLogModel>("sp_Tbl_Error_LogInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
        }
    }
}
