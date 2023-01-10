using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class AgencyModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string AgencyNm { get; set; }

        public string Description { get; set; }

        public string PICNm { get; set; }

        public string Telp { get; set; }

        public string Email { get; set; }

        public string BankNm { get; set; }

        public string BeneficiaryNm { get; set; }

        public string AccountNo { get; set; }

        public string NPWPNm { get; set; }

        public string NPWPNo { get; set; }

        public string NPWPAddress { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
