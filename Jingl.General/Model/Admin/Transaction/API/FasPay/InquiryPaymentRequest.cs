using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API.FasPay
{
    public class InquiryPaymentRequest
    {
        public string request { get; set; }
        public string trx_id { get; set; }
        public string merchant_id { get; set; }
        public string bill_no { get; set; }
        public string signature { get; set; }
    }
}
