using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction.API
{

    public class TransactionOrder
    {
        public  transaction_details transaction_details {get;set;}
    }

    public class transaction_details
    {
       
        public string order_id { get; set; }
        public decimal gross_amount { get; set; }
    }
}
