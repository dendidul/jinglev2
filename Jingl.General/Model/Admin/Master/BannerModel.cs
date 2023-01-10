using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class BannerModel
    {
        public int Id { get; set; }

        public string BannerCategory { get; set; }

        public int? FileId { get; set; }

        public string BannerNm { get; set; }

        public string BannerDesc { get; set; }

        public string Link { get; set; }

        public int? Sequence { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public bool? IsActive { get; set; }

        public int IsVisible { get; set; }

        public string Img { get; set; }

    }
}
