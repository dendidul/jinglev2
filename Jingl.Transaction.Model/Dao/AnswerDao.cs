using Dapper;
using Jingl.General.Enum;
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
    public class AnswerDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public AnswerDao(IConfiguration config)
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

        public AnswerModel CreateAnswer(AnswerModel model)
        {
            var data = new AnswerModel();

            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@QuestionId", model.QuestionId);
                param.Add("@Answer", model.Answer);

                data = conn.Query<AnswerModel>("sp_Tbl_Trx_AnswerInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return data;
        }

    }
}
