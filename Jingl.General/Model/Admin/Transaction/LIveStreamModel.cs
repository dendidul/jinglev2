using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class LiveStreamModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? CategoryId { get; set; }

        public string LiveStreamNm { get; set; }

        public string SecretKey { get; set; }

        public int? ViewerCount { get; set; }

        public DateTime? LiveSchedule { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool? IsLive { get; set; }

        public bool? IsActive { get; set; }

    }
}
