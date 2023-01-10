using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class QuestionVideoModel
    {
        public int QuestionVideoId { get; set; }
        public int? UserId { get; set; }
        public int? FileId { get; set; }
        public int TalentId { get; set; }
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
        public string LinkVideo { get; set; }
        public string Thumbnails { get; set; }

        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
    }
}
