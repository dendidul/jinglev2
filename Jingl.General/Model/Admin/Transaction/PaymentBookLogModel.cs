using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class PaymentBookLogModel
    {
        public int Id { get; set; }

        public int? BookId { get; set; }

        public string OrderId { get; set; }

        public string SnapToken { get; set; }

        public string StatusCode { get; set; }

        public string TransactionStatus { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
