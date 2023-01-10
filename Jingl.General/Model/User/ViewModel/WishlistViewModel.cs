using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
    public class WishlistViewModel
    {
        public List<TalentCategoryViewModel> ListTalent { get; set; }
        public List<TalentVideoModel> ListVideo { get; set; }
    }
}
