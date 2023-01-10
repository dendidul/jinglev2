using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class ClaimModel
    {
        public int Id { get; set; }

        public string ClmNumber { get; set; }

        public int? UserId { get; set; }
        public string ClearingCode { get; set; }

        public string BankName { get; set; }
        public string UserCode { get; set; }

        public string AccountNumber { get; set; }

        public string UserName { get; set; }

        public string TalentNm { get; set; }
        public string BeneficiaryName { get; set; }

        public string Period { get; set; }

        public decimal? Amount { get; set; }
        public string Message { get; set; }

        public int? Status { get; set; }

        public string StatusNm { get; set; }

        public DateTime? PaidDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
