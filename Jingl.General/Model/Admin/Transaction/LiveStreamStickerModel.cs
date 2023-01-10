using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class LiveStreamStickerModel
    {
        public int Id { get; set; }

        public int? LiveStreamId { get; set; }

        public int? UserWatchId { get; set; }

        public int? StickerId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
