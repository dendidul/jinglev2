using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class CommentModel
    {
        public int PostId { get; set; }
        public string Message { get; set; }
        public int TalentId { get; set; }
        public int UserId { get; set; }
        public int ObjectId { get; set; }
        public DateTime? PostedDate { get; set; }
    }
}
