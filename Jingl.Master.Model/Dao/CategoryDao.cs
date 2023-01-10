using Dapper;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
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
    public class CategoryDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public CategoryDao(IConfiguration config)
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

        public IList<CategoryModel> GetAllCategory()
        {
            var data = new List<CategoryModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", null);


                data = conn.Query<CategoryModel>("sp_Tbl_Mst_CategorySelect", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public IList<CategoryModel> GetCategoryByType(string CategoryType)
        {
            var data = new List<CategoryModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CategoryType", CategoryType);


                data = conn.Query<CategoryModel>("sp_GetCategoryByType", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return data;
        }

        public CategoryModel GetDataCategory(CategoryModel model)
        {
            var data = new CategoryModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);


                data = conn.Query<CategoryModel>("sp_Tbl_Mst_CategorySelect", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }

        public CategoryModel CreateCategoryData(CategoryModel model)
        {
            var data = new CategoryModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@CategoryType", model.CategoryType);
                param.Add("@CategoryNm", model.CategoryNm);
                param.Add("@CategoryDesc", model.CategoryDesc);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreatedDate",DateTime.Now);
                param.Add("@IsActive", model.IsActive);
                param.Add("@CategoryCode", model.CategoryCode);


                data = conn.Query<CategoryModel>("sp_Tbl_Mst_CategoryInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return data;
        }

        public CategoryModel UpdateCategoryData(CategoryModel model)
        {
            var data = new CategoryModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", model.Id);
                param.Add("@CategoryType", model.CategoryType);
                param.Add("@CategoryNm", model.CategoryNm);
                param.Add("@CategoryDesc", model.CategoryDesc);
                param.Add("@UpdatedBy", model.UpdatedBy);
                param.Add("@UpdatedDate", DateTime.Now);
                param.Add("@IsActive", model.IsActive);
                param.Add("@CategoryCode", model.CategoryCode);
                

                data = conn.Query<CategoryModel>("sp_Tbl_Mst_CategoryUpdate", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();



            }

            return data;
        }


        public void DeleteCategoryData(int id)
        {
          
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);
              


               conn.Execute("sp_Tbl_Mst_CategoryDelete", param,
                           commandType: CommandType.StoredProcedure);



            }

          
        }


    }
}
