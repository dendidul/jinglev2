using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Jingl.General.Model.Admin.Master
{
    public class AllTalentVideo
    {
        public IList<TalentVideoModel> ListTalentVideo { get; set; }
        public IList<BookModel> ReactionVideoList { get; set; }
        public IList<QuestionVideoModel> QuestionVideoList { get; set; }
        public IList<ViewCommentVideoModel> ListCommentVideo { get; set; }
        public IList<ViewReplyCommentVideoModel> ListReplyCommentVideo { get; set; }
        public IList<ViewSubCommentVideoModel> ListSubCommentVideo { get; set; }
    }
}
