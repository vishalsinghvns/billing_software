using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class PurchaseVM
    {
        public List<PurchaseEntryModel> BillVM { get; set; }
        public List<PurchaseEntryModel> productVM { get; set; }
        public string CompName { get; set; }
        public string NameOnInvoice { get; set; }
        public string CompPhone1 { get; set; }
        public string CompEmail { get; set; }
        public string CompLogo { get; set; }
    }
}