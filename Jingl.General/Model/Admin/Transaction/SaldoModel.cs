using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class SaldoModel
    {
        public Int64 Id { get; set; }
        public Int64? TalentId { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SaldoAmt { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SaldoUsedAmt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SisaSaldoAmt { get; set; }
        public string TalentNm { get; set; }
        public string LinkImg { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }

    }
}
