using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class VoucherTalentViewModel
    {
        public int VoucherId { get; set; }

        public int TalentId { get; set; }
        
        public string  TalentNm { get; set; }
        
        public string LinkImg { get; set; }

        public decimal PriceAmount { get; set; }

        public decimal SalePrice { get; set; }

        public decimal PaymentShare { get; set; }
        
        public int UserId { get; set; }

        public string Profesion { get; set; }

        public int WishlistId { get; set; }

        public string Link { get; set; }

        public int ViewsCount { get; set; }

        public int? FileId { get; set; }

        public int IsAvailable { get; set; }
    }
}
