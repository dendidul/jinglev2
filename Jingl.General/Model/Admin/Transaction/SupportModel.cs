using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class SupportModel
    {
        public int Id { get; set; }

        public string Details { get; set; }
        public string Subject { get; set; }

        public string EmailAddress { get; set; }

        public int? Status { get; set; }
        public string StatusName { get; set; }
        public string UserName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? IsActive { get; set; }
    }
}
