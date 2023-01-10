using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class QuestionComment
    {
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
    }

    public class GetQuestionComment
    {
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Commentby { get; set; }
        public string UserProfPicLink { get; set; }
    }
}
