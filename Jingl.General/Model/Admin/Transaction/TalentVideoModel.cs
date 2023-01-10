using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class TalentVideoModel
    {
        public int Id { get; set; }
        public int? TalentId { get; set; }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        public int? FileId { get; set; }
        public string TalentNm { get; set; }
        public string Link { get; set; }
        public string ProfImg { get; set; }
        public string Thumbnails { get; set; }
        public string ThumbLink { get; set; }
        public string ProjectNm { get; set; }
        public int FileCategory { get; set; }
        public int Rate { get; set; }
        public int ViewsCount { get; set; }
        public bool? IsActive { get; set; }
        public string VideoNm { get; set; }
        public int Sequence { get; set; }
        public int? BookCategory { get; set; }
        public string CategoryName { get; set; }
        public int TotalLikes { get; set; }
        public string To { get; set; }
        public bool IsPublic { get; set; }
        public IList<TalentVideoModel> ListTalentVideo { get; set; }
        public IList<BookModel> ReactionVideoList { get; set; }
        public IList<QuestionVideoModel> QuestionVideoList { get; set; }
        public IList<CommentVideoModel> CommentVideoList { get; set; }

        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
        //public IList<QuestionVideoModel> QuestionVideoList
        //{
        //    get
        //    {
        //        if (_QuestionVideoList == null)
        //        {
        //            _QuestionVideoList = new List<QuestionVideoModel>
        //            {
        //                new QuestionVideoModel
        //                {
        //                    QuestionVideoId = 1
        //                },
        //                new QuestionVideoModel
        //                {
        //                    QuestionVideoId = 2
        //                }
        //            };
        //        }

        //        return _QuestionVideoList;
        //    }

        //}

        //public IList<CommentVideoModel> CommentVideoList
        //{
        //    get
        //    {
        //        if (_CommentVideoList == null)
        //        {
        //            _CommentVideoList = new List<CommentVideoModel>
        //            {
        //                new CommentVideoModel
        //                {
        //                    ComId = 1
        //                },
        //                new CommentVideoModel
        //                {
        //                    ComId = 2
        //                }
        //            };
        //        }

        //        return _CommentVideoList;
        //    }

        //}



    }
}
