using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class VoucherModel
    {
        public int Id { get; set; }

        public string VoucherCd { get; set; }

        public string VoucherNm { get; set; }

        public string VoucherDesc { get; set; }

        public int? RemainingCount { get; set; }

        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }

        public DateTime? StartDate { get; set; }

        public string StartDateTemp { get; set; }

        public DateTime? EndDate { get; set; }

        public string EndDateTemp { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int IsActive { get; set; }

        public bool isVisible { get; set; }

        public bool? IsUsed { get; set; }

        public bool? IsClaimed { get; set; }

        public int? UsesPerCustomer { get; set; }

        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }

        public string SentTo { get; set; }

        public decimal Budget { get; set; }
        public decimal Discount { get; set; }

        //public IList<VoucherTalentViewModel> VoucherTalent { get; set; }

        public IList<VoucherTalentViewModel> VoucherSelectedTalent { get; set; }

        [NotMapped]
        public bool _IsActive
        {
            get
            {
                if (IsActive == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }


    public class VoucherAllTalentModel
    {
        public int Id { get; set; }

        public string VoucherCd { get; set; }

        public string VoucherNm { get; set; }

        public DateTime? EndDate { get; set; }

        public int TalentId { get; set; }

        public string TalentNm { get; set; }
    }
}
