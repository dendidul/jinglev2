using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class BookLogModel
    {
       
            public int Id { get; set; }

            public int? BookId { get; set; }

            public int? Status { get; set; }

            public DateTime? CreatedDate { get; set; }

            public int? CreatedBy { get; set; }

            public int? Sequence { get; set; }
       
    }
}
