using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.UserManagement
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string RoleNm { get; set; }

        public string RoleDesc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }

       
    }
}
