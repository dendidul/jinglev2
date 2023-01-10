using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string CategoryType { get; set; }

        public string CategoryNm { get; set; }

        public string CategoryDesc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public string CategoryCode { get; set; }
    }
}
