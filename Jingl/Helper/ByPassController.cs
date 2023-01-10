using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jingl.Web.Helper
{
    public class ByPassController
    {

        public ByPassController()
        {

        }

        public List<string> ControllerList
        {
            get
            {
                List<string> listItems = new List<string>();
                listItems.Add("AdmMenu");
                //listItems.Add("Home");
                //listItems.Add("Profile");
                //listItems.Add("Global");

                return listItems;
            }
        }

        public List<string> ControllerLists()
        {
            return ControllerList;
        }
    }
}
