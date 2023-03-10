using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API.FasPay
{
    public class InquiryPaymentResponse
    {
        public string response { get; set; }
        public string trx_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant { get; set; }
        public string bill_no { get; set; }
        public string payment_reff { get; set; }
        public DateTime payment_date { get; set; }
        public string payment_status_code { get; set; }
        public string payment_status_desc { get; set; }
        public string response_code { get; set; }
        public string response_desc { get; set; }

    }
}
