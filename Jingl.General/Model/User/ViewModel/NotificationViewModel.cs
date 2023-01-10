using Jingl.General.Model.Admin.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.ViewModel
{
    public class NotificationViewModel
    {
        public IList<NotificationModel> UserNotificationList { get; set; }
        public IList<NotificationModel> TalentNotificationList { get; set; }
        public IList<BookModel> ListActiveBook { get; set; }
        public IList<BookModel> ListFinishBook { get; set; }
        public int CountUserNotification { get; set; }
        public int CountTalentNotification { get; set; }
        public int RoleId { get; set; }
        public int CountActiveBook { get; set; }
        public int CountFinishBook { get; set; }

    }
}
