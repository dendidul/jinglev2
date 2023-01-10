using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
    public class RequestChangeModel
    {

        public int Id { get; set; }

        public string RequestCode { get; set; }

        public string RequestType { get; set; }
        public string Password { get; set; }

        public int? UserId { get; set; }

        public int? IsActive { get; set; }

    }
}
