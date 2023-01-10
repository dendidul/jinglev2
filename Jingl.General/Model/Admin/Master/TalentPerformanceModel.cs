using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class TalentPerformanceModel
    {
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Income { get; set; }
        public decimal OrderPercentage { get; set; }

        public string TalentNm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CategoryName { get; set; }
        public string Period { get; set; }

        public int TotalBook { get; set; }
        public int CompletedBook { get; set; }
        public int OnGoingBook { get; set; }       
        public int? Status { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int RoleId { get; set; }
    }
}
