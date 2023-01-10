using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
   public  class StickerModel
    {
        public int Id { get; set; }

        public string StickerCd { get; set; }

        public string StickerNm { get; set; }

        public int StickerCategoryId { get; set; }

        public string StickerDescription { get; set; }

        public decimal? Amount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
