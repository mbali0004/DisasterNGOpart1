using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisasterNGOpart1.Controllers
{
    public class InfoModel
    {
        public double TotalMonetaryDonations { get; set; }
        public int TotalGoodsReceived { get; set; }
        public List<ActiveDisasterViewModel> ActiveDisasters { get; set; }

        public class ActiveDisasterViewModel
        {
            public string Name { get; set; }
            public double MoneyAllocated { get; set; }
            public int GoodsAllocated { get; set; }
        }
    }
}