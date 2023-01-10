using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class VoucherTransactionModel
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public string VoucherCd { get; set; }
        public int BookId { get; set; }
        public int TalentId { get; set; }
        public int UserId { get; set; }
        public int qty { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
