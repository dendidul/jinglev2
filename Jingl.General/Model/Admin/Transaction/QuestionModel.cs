using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public int? UserId { get; set; }
        public int? TalentId { get; set; }
        public string Question { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAnswered { get; set; }
        public string UserProfPicLink { get; set; }
        public string TalentProfPicLink { get; set; }
        public string UserName { get; set; }
        public string TalentName { get; set; }
        public string Answer { get; set; }
        public DateTime? QuestionDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? TotalLike { get; set; }
    }
}
