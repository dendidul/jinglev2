using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class IncomeModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string SourceIncome { get; set; }

        public string Period { get; set; }

        public decimal? Income { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
