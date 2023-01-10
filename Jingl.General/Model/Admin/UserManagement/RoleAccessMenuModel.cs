using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.UserManagement
{
    public class RoleAccessMenuModel
    {
        public int Id { get; set; }

        public int? RoleId { get; set; }

        public int? MenuId { get; set; }

        public bool? IsActive { get; set; }
    }
}
