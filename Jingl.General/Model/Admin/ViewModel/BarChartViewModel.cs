using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Model.Admin.ViewModel
{
    public class BarChartModel
    {
        public Xaxis x_axis { get; set; }
        //public Yaxis y_axis { get; set; }
        public List<TransactionData> series { get; set; }
    }

    public class Xaxis
    {
        public List<string> labels { get; set; }
    }

    public class Yaxis
    {
        public string format { get; set; }
        public string unit { get; set; }
    }

    public class TransactionData
    {
        public List<int> data { get; set; }
        public string name { get; set; }
       

    }

   
   

}
