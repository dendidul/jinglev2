using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API.FasPay
{
    public class PaymentNotificationResponse
    {
        public string response { get; set; }
        public string trx_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant { get; set; }
        public string bill_no { get; set; }
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public DateTime response_date { get; set; }
    }
}
