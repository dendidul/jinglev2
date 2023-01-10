using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class QuestionLikeModel
    {
        public int id { get; set; }

        public int? QuestionId { get; set; }

        public int? UserId { get; set; }

        public bool? IsActive { get; set; }
    }
}
