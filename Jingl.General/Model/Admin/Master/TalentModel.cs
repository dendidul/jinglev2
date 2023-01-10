using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class TalentModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Profesion { get; set; }
        public string UserName { get; set; }

        public string TalentNm { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string LinkImg { get; set; }
        public string BgrImg { get; set; }
        public string IdCardImg { get; set; }
        public string AcNumImg { get; set; }
        public string NpwpImg { get; set; }
        public string VideoIntro { get; set; }
        public string VideoIntroId { get; set; }
        public int VideoIntroTotalLikes { get; set; }
        public string Gender { get; set; }

        public string LinkScm { get; set; }

        public int? FollowersCount { get; set; }

        public int? RdyVideo { get; set; }

        public decimal Rate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CategoryName { get; set; }

        public decimal PriceAmount { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PaymentShare { get; set; }
        public string LevelName { get; set; }
        public int Level { get; set; }
        public int? Status { get; set; }
        public int FavoriteCount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsUnderAgency { get; set; }
        public int IsActive { get; set; }
        public string Note { get; set; }
        public string Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IList<TalentVideoModel> ListTalentVideo { get; set; }
        public IList<QuestionVideoModel> ListQuestionVideo { get; set; }
        public IList<ViewCommentVideoModel> ListPostCommentVideo { get; set; }
        public IList<ViewReplyCommentVideoModel> ListReplyCommentVideo { get; set; }
        public IList<ViewSubCommentVideoModel> ListSubReplyCommentVideo { get; set; }
        public IList<BookModel> ReactionVideoList { get; set; }
        public IList<BookModel> TalentBookList { get; set; }

        public IList<TalentCategoryViewModel> TalentCategory { get; set; }
        public IList<TalentCategoryViewModel> TalentSelectedCategory { get; set; }

        public int TotalBook { get; set; }
        public int CompletedBook { get; set; }
        public int OnGoingBook { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Income { get; set; }
        public decimal OrderPercentage { get; set; }

        public string Bank { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public string ReferralUserCode { get; set; }
        public string UserCode { get; set; }
        public int? IdCardFileId { get; set; }
        public int? AgencyId { get; set; }
        public int? AccountNumberFileId { get; set; }
        public int? NPWPFileId { get; set; }
        public int sequence { get; set; }
        public int BookingCount { get; set; }

        public bool IsTalentAcceptOrder { get; set; } 

        public string Instagram { get; set; }
        public string AgencyNm { get; set; }
        public string Facebook { get; set; }
        public int RoleId { get; set; }
        public int? WishlistId { get; set; }
        public UserModel Booker { get; set; }
        public IList<QuestionModel> QuestionList { get; set; }
        public int? TotalLikes { get; set; }
    }
}
