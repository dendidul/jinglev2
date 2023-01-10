using System;
using System.Collections.Generic;
using System.Text;


namespace Jingl.General.Model.Admin.Transaction
{
    public class PostCommentModel
    {
        public int ComId { get; set; }
        public string CommentMsg { get; set; }
        public DateTime? CommentedDate { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
