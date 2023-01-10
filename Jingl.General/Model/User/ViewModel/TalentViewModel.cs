using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
   public class TalentViewModel
    {

        public IList<CategoryModel> ListCategoryModel { get; set; }
        public IList<TalentModel> ListTalentModel { get; set; }
        public IList<TalentCategoryViewModel> ListTalentViewModel { get; set; }
        public IList<TalentVideoModel> ListVideo { get; set; }

    }
}
