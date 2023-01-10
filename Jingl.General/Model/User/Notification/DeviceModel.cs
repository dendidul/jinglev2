using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.User.Notification
{
    public class DeviceModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }

        public string PushEndpoint { get; set; }

        public string PushP256DH { get; set; }

        public string PushAuth { get; set; }
    }
}
