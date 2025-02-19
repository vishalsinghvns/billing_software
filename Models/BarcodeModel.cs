using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class BarcodeModel
    {
        public List<ItemHeadModel> ItemHeadList { get; set; }
        
        public int[] codeid { get; set; }
        public int[] count { get; set; }
    }
}