using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TopupViewModel
    {
        public SaldoModel SaldoModel { get; set; }
        public TopupModel TopupModel { get; set; }
        public List<TopupModel> ListHistoryTopUp { get; set; }
    }
}
