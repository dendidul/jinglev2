using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
    public class UserProfileViewModel
    {
        public UserModel UserModel { get; set; }
        public TalentModel TalentModel { get; set; }
        public List<BookModel> TalentBookList { get; set; }
        public List<BookModel> TalentReactionVideoList { get; set; }
        public List<TalentCategoryViewModel> ListTalentWishlist { get; set; }
        public List<BookModel> ListBooking { get; set; }
        public List<QuestionModel> QuestionList { get; set; }
    }
}
