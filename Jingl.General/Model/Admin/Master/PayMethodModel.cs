using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class PayMethodModel
    {
        public int Id { get; set; }

        public string PayMethodNm { get; set; }

        public string PayMethodDesc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int IsActive { get; set; }
    }
}
