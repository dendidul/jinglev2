using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class BookModel
    {
        public int Id { get; set; }

        public string ProjectNm { get; set; }

        public int? TalentId { get; set; }
        public int? BookCategory { get; set; }

        public string Email { get; set; }
        public string CategoryNm { get; set; }

        public string TalentNm { get; set; }
        public string TalentPhotos { get; set; }

        public string PhoneNumber { get; set; }

        public string BriefNeeds { get; set; }

        public string From { get; set; }

        public string To { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Deadline { get; set; }

        public string Notification { get; set; }

        public string VoucherCode { get; set; }
        public decimal? PriceAmount { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PaymentShare { get; set; }
        public decimal? TotalPay { get; set; }

        public decimal? Potongan { get; set; }

        public string PayMethod { get; set; }

        public int? FileId { get; set; }
        public int Status { get; set; }

        public int? BookedBy { get; set; }

        public string CreatedBy { get; set; }
        public string OrderNo { get; set; }
        public string SnapToken { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int IsPublic { get; set; }

        public int IsActive { get; set; }
        public int IsSentToTalent { get; set; }

        public string ClientKeyId { get; set; }
        public string Link { get; set; }
        public string VaNumber { get; set; }
        public string BankName { get; set; }
        public string TransactionStatus { get; set; }
        public string BankSelected { get; set; }

        public DateTime? ExpiredDate { get; set; }
        public string PaymentChannel { get; set; }
        public DateTime? sentDate { get; set; }
        public string UserName { get; set; }
        public decimal Rate { get; set; }
        public int RateVideo { get; set; }
        public int IsCalculated { get; set; }
        public string Period { get; set; }

        public int UserOfTalentId { get; set; }
        public string TalentEmail { get; set; }
      

        public string BookId { get; set; }
        public string CustomerName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string UserCode { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime BeginDate { get; set; }
        public int? IsEmail { get; set; }

        public int ViewsCount { get; set; }
        public string UserImage { get; set; }
        public string CustName { get; set; }

        public string CountdownStatus { get; set; }
        public string CountdownText { get; set; }
        public int Countdown { get; set; }

        public string FileName {get;set;}
        public string ThumbLink { get; set; }
        public string Review { get; set; }

        public bool IsAvailable { get; set; }
        public decimal? SisaSaldoAmt { get; set; }

        public decimal? TalentPrice { get; set; }

        public string Reason { get; set; }

        public decimal TalentIncome { get; set; }
        public decimal FameoIncome { get; set; }
        public int? AgencyId { get; set; }

        public int? ReactionFileId { get; set; }
        public string ReactionLink { get; set; }

        public string DateData { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalOrderDone { get; set; }
        public int TotalOrderAbandonOrder { get; set; }
        public int TotalOrderInProgress { get; set; }
        public int TotalOrderAbandonCart { get; set; }
        public DateTime? OrderCompleteDate { get; set; }
        public decimal? Amount { get; set; }
        public int UserId { get; set; }      
        public string fileName { get; set; }

        [NotMapped]
        public bool ispublic_
        {
            get
            {
                return IsPublic != 0;
            }
            set
            {
                IsPublic = value ? 1 : 0;
            }
        }

        [NotMapped]
        public int offset { get; set; }
        public int fetch { get; set; }
    }
}
