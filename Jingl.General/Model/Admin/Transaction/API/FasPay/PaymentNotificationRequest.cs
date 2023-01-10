using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API.FasPay
{
    public class PaymentNotificationRequest
    {
        public string request { get; set; }
        public string trx_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant { get; set; }
        public string bill_no { get; set; }
        public string payment_reff { get; set; }
        public string payment_date { get; set; }
        public string payment_status_code { get; set; }
        public string payment_status_desc { get; set; }
        public decimal bill_total { get; set; }
        public decimal payment_total { get; set; }
        public string payment_channel_uid { get; set; }
        public string payment_channel { get; set; }
        public string signature { get; set; }
        public string response_date { get; set; }
    }
}
