using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class PostCommentVideoModel
    {
        public int PostId { get; set; }
        public string Message { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public DateTime? PostedDate { get; set; }
        public bool? IsActive { get; set; }
    }


    public class ViewCommentVideoModel
    {
        public int PostId { get; set; }
        public string Message { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public DateTime? PostedDate { get; set; }
        public bool? IsActive { get; set; }
        public int TalentId { get; set; }
        public string PostBy { get; set; }
        public string UserProfPicLink { get; set; }
        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
    }


    public class ViewReplyCommentVideoModel
    {
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime? CommentedDate { get; set; }
        public bool? IsActive { get; set; }
        public string Commentby { get; set; }
        public int TalentId { get; set; }
        public string UserProfPicLink { get; set; }
        public int? FileId { get; set; }
        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
    }

    public class ViewSubCommentVideoModel
    {
        public int SubComID { get; set; }
        public string CommentMsg { get; set; }
        public int ComID { get; set; }
        public int UserID { get; set; }
        public DateTime? CommentedDate { get; set; }
        public bool? IsActive { get; set; }
        public string SubCommentBy { get; set; }
        public int TalentId { get; set; }
        public string UserProfPicLink { get; set; }
        public int? FileId { get; set; }
        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
    }
}
