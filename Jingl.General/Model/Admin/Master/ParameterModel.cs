using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Master
{
   public  class ParameterModel
    {
        public int Id { get; set; }

        public string ParamCode { get; set; }

        public string ParamName { get; set; }

        public string ParamValue { get; set; }

        public bool? IsActive { get; set; }
    }
}
