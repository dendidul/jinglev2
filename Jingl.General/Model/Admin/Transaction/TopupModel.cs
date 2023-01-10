using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TopupModel
    {
        public Int64 Id { get; set; }
        public Int64? SaldoId { get; set; }
        public int? SeqNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TopUpAmt { get; set; }
        public DateTime? TopUpDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? LastSaldoAmt { get; set; }
        public int TopupStatus { get; set; }
        public string TopupSource { get; set; }
        public string Notes { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string OrderNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? LastUsedSaldoAmt { get; set; }

        public string TopupStatusText { get; set; }
        public string TopupAmtText { get; set; }
        public string EmailRequest { get; set; }
        public string TalentName { get; set; }
        public string LinkImg { get; set; }

    }
}
