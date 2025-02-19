using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class CompanyModel
    {
        public int id { get; set; }
        public string CompName { get; set; }
        public string NameOnInvoice { get; set; }
        public string CompAddress { get; set; }
        public string CompCity { get; set; }
        public string CompPin { get; set; }
        public string CompState { get; set; }
        public string CompPhone1 { get; set; }
        public string CompPhone2 { get; set; }
        public string CompPhone3 { get; set; }
        public string CompPhone4 { get; set; }
        public string CompEmail { get; set; }
        public string CompWebsite { get; set; }
        public string CompPAN { get; set; }
        public string CompGST { get; set; }
        public string CompLogo { get; set; }
        public string CompBankName { get; set; }
        public string CompIFSC { get; set; }
        public string CompAccNo { get; set; }
        public string CompAccName { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}