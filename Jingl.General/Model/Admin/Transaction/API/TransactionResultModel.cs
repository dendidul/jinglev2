using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API
{
    public class TransactionResultModel
    {
        public string status_code { get; set; }
        public string status_message { get; set; }
        public string transaction_id { get; set; }
        public string order_id { get; set; }
        public decimal gross_amount { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string currency { get; set; }
      
        public string fraud_status { get; set; }
        public string pdf_url { get; set; }
        public string finish_redirect_url { get; set; }

        public string bill_key { get; set; }
        public string bill_code { get; set; }
        public string signature_key { get; set; }
        public List<va_numbers> va_numbers { get; set; }
        public List<payment_amounts> payment_amounts { get; set; }

    }

    public class va_numbers
    {
        public string bank { get; set; }
        public string va_number { get; set; }
    }
    public class payment_amounts
    {
        public decimal amount { get; set; }
        public DateTime paid_at { get; set; }
    }

}
