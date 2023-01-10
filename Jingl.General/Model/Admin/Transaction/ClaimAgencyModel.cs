using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class ClaimAgencyModel
    {
        public int Id { get; set; }

        public string Period { get; set; }

        public DateTime? ClaimDate { get; set; }

        public int? AgencyId { get; set; }

        public string AgencyNm { get; set; }

        public decimal? Amount { get; set; }

        public int? Status { get; set; }

        public string StatusNm { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? IsActive { get; set; }

        public string ClmNumber { get; set; }

        public int? ClaimId { get; set; }

        public List<ClaimAgencyDetailsModel> ListDetails { get; set; }

        public List<BookModel> BookingList { get; set; }

    }
}
