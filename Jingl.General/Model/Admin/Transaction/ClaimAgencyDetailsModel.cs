using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class ClaimAgencyDetailsModel
    {
        public int Id { get; set; }

        public int? ClaimId { get; set; }

        public int? BookId { get; set; }
    }
}
