using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.Transaction
{
   public class WishlistModel
    {
        public int id { get; set; }

        public int? UserId { get; set; }

        public int? TalentId { get; set; }

        public int FavouriteCount { get; set; }

        public bool? IsActive { get; set; }

    }
}
