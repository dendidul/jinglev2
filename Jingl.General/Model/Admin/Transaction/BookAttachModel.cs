using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class BookAttachModel
    {
        public int Id { get; set; }

        public int? BookId { get; set; }

        public int? FileId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
