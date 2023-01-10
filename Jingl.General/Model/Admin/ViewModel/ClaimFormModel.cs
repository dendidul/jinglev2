using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.ViewModel
{
    public class ClaimFormModel
    {
        public string Period { get; set; }
        public string Status { get; set; }
        public string FilterStatus { get; set; }
        public IList<ClaimModel> ListClaimModel { get; set; }
    }
}
