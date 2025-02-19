using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class SaleVM
    {
        public List<SaleEntryModel> BillVM { get; set; }
        public List<SaleEntryModel> productVM { get; set; }
        public string CompName { get; set; }
        public string NameOnInvoice { get; set; }
        public string CompPhone1 { get; set; }
        public string CompEmail { get; set; }
        public string CompLogo { get; set; }
        public string CompAdd { get; set; }
        public string CompCity { get; set; }
        public string CompState { get; set; }
        public string CompPIN { get; set; }
        public string CompGST { get; set; }
        public string CompBank { get; set; }
        public string CompIFSC { get; set; }
        public string CompAccNo { get; set; }
        public string CompAccName { get; set; }
    }
}