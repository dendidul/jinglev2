using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class AnswerQuestionVideoModel
    {
        public int AnswerVideoId { get; set; }
        public int QuestionVideoId { get; set; }
        public string Answer { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
