using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.ViewModel
{
    public class DailyPaymentModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<BookModel> ListPaymentData { get; set; }

    }
}
