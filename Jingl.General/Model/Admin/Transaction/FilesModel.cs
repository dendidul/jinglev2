using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class FilesModel
    {
        public int Id { get; set; }

        public string Link { get; set; }

        public string FileName { get; set; }

        public string FileDesc { get; set; }

        public string FileType { get; set; }

        public int? FileCategory { get; set; }

        public int? ViewCount { get; set; }

        public TimeSpan? FileDuration { get; set; }

        public int? OwnerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
