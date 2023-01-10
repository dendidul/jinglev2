using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jingl.General.Model.Admin.UserManagement
{
    public class UserModel
    {
        public int Id { get; set; }
        public int TalentId { get; set; }

        public string UserCode { get; set; }
        

        public string FirstName { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string PhoneNumberArea { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string ProfPicLink { get; set; }
        public string BgrLink{ get; set; }


        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public string Location { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Status { get; set; }
        public int? RoleId { get; set; }

        public int IsActive { get; set; }

        public string VerificationCode { get; set;  }
        public string RoleNm { get; set; }

        public string Profesi { get; set; }

        public IList<TalentCategoryViewModel> TalentCategory { get; set; }

        public IList<TalentCategoryViewModel> TalentSelectedCategory { get; set; }

        public int IsReceiveLetter { get; set; }
        public bool IsActiveCode { get; set; }

        public int? ImgProfFileId { get; set; }

        public int? BgrFileId { get; set; }

        public string SignUpType { get; set; }
        public string AcNumImg { get; set; }
        public string NpwpImg { get; set; }
        public string IdCardImg { get; set; }
        public string Bank { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BeneficiaryName { get; set; }

        public string DefaultUsername { get; set; }
        public string DefaultPassword { get; set; }
        public string IsVerified { get; set; }

        public TalentVideoModel VideoProfile { get; set; }

    }
}
