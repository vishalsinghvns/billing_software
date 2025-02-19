using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class RepairModel
    {
        public int id { get; set; }
        public string billno { get; set; }
        public string repairno { get; set; }
        public string Custmr { get; set; }
        public string phoneno { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> usrid { get; set; }
        public string Usrname { get; set; }
        public string status { get; set; }
        public string prod_name { get; set; }
        public Nullable<decimal> Amount { get; set; }

        public Nullable<int> repbillid { get; set; }
        public Nullable<int> productid { get; set; }
        public Nullable<long> quanti { get; set; }

        public int[] S_itemhead_id_arr { get; set; }
        public long[] S_item_qty_arr { get; set; }

        public decimal[] S_finalprod_amt_arr { get; set; }
    }
}