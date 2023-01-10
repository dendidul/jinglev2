using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class RefundModel
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }

        public string RefundNumber { get; set; }

        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string ClearingCode { get; set; }
        public string CustomerName { get; set; }
        public string BeneficiaryName { get; set; }

        public string BankName { get; set; }
        public string BankCode { get; set; }

        public string AccountNumber { get; set; }

        public string BatchNumber { get; set; }
        public string StatusNm { get; set; }
        public decimal Amount { get; set; }
        

        public DateTime? RequestDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int? BookId { get; set; }
    }
}
