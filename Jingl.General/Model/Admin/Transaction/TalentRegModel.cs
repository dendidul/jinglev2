using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TalentRegModel
    {
        public int id { get; set; }

        public string RegNum { get; set; }

        public int? UserId { get; set; }

        public string TalentNm { get; set; }

        public string Email { get; set; }

        public string Instagram { get; set; }

        public string Facebook { get; set; }
        public string Profesion { get; set; }
        public string StatusNm { get; set; }

        public int? Rdy { get; set; }

        public int? Status { get; set; }

        public string Note { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public List<TalentCategoryViewModel> TalentCategory { get; set; }
        public int? CategoryId { get; set; }
    }
}
