using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class NotificationModel
    {
        public int Id { get; set; }


        public int? BookId { get; set; }
        public int? To { get; set; }
        public string NotifType { get; set; }
        public string Message { get; set; }
        public string NotifCategory { get; set; }
        public string CreatedBy { get; set; }
        public int IsReaded { get; set; }
        public string ImgProfPic { get; set; }
        public string Link { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }

        public int ClaimId { get; set; }
    }
}
