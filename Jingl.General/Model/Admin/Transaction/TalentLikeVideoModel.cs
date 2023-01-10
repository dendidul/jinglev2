using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TalentLikeVideoModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}
