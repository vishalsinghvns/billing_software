//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Preora.dbcon
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_PurchaseBill
    {
        public int id { get; set; }
        public string p_entrycode { get; set; }
        public Nullable<int> supllierid { get; set; }
        public Nullable<System.DateTime> billdate { get; set; }
        public Nullable<System.DateTime> duedate { get; set; }
        public string invoice_no { get; set; }
        public Nullable<decimal> GrossAmt { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> AdditionalDisc { get; set; }
        public Nullable<decimal> S_GST { get; set; }
        public Nullable<decimal> C_GST { get; set; }
        public Nullable<decimal> I_GST { get; set; }
        public Nullable<decimal> LoadingCharge { get; set; }
        public Nullable<decimal> FinalAmt { get; set; }
        public Nullable<decimal> PaidAmt { get; set; }
        public Nullable<decimal> BalanceAmt { get; set; }
        public string pay_mode { get; set; }
    }
}
