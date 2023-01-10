using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class PlatformModel
    {
        public int? id { get; set; }

        public string PlatformName { get; set; }

        public string PlatformDesc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
