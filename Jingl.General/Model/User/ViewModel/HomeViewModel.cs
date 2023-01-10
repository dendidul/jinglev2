using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
    public class HomeViewModel
    {
        public IList<CategoryModel> ListCategoryModel { get; set; }
        public IList<TalentModel> ListTalentModel { get; set; }
        public IList<TalentCategoryViewModel> ListTalentCategoryModel { get; set; }
        public IList<BannerModel> ListBanner { get; set; }

        public IList<TalentVideoModel> ListBestVideo { get; set; }
        public IList<TalentVideoModel> ListMostWatchVideo { get; set; }
        public IList<TalentVideoModel> ListAllVideo { get; set; }



    }
}
