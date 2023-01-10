using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.ViewModel
{
    public class RefundFormModel
    {
        public string BatchNumber { get; set; }
        public string Status { get; set; }
        public IList<RefundModel> ListRefundModel { get; set; }

        public string FilterStatus { get; set; }       
    }
}
