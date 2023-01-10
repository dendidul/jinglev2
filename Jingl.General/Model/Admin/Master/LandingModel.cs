using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class LandingModel
    {
        public int VideoId { get; set; }
        public string TalentNm { get; set; }
     
        public string BookedBy { get; set; }

        public int IsPublic { get; set; }

        public string Link { get; set; }
        public string CategoryNm { get; set; }

    }
}
