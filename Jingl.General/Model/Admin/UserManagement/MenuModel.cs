using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.UserManagement
{
    public class MenuModel
    {
        public int? Id { get; set; }

        public string MenuNm { get; set; }

        public string MenuDesc { get; set; }

        public string Link { get; set; }

        public int? ParentMenuId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
