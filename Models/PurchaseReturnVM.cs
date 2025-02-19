using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class PurchaseReturnVM
    {
        public List<PurchaseReturnModel> BillVM { get; set; }
        public List<PurchaseReturnModel> productVM { get; set; }
        public string CompName { get; set; }
        public string NameOnInvoice { get; set; }
        public string CompPhone1 { get; set; }
        public string CompEmail { get; set; }
        public string CompLogo { get; set; }
    }
}