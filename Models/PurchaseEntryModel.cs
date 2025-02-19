using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class PurchaseEntryModel
    {
        public int id { get; set; }
        public string p_entrycode { get; set; }
        public Nullable<int> supllierid { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? billdate { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? duedate { get; set; }
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
        public string invoice_no { get; set; }
        public string pay_mode { get; set; }
        public string supp_name { get; set; }
        public string prod_name { get; set; }
        public string measureunit { get; set; }

        public Nullable<int> purchasebill_id { get; set; }
        public Nullable<int> itemhead_id { get; set; }
        public Nullable<decimal> item_rate { get; set; }
        public Nullable<long> item_qty { get; set; }
        public Nullable<decimal> gross_amt { get; set; }
        public Nullable<decimal> disc_amt { get; set; }
        public Nullable<decimal> sgst_amt { get; set; }
        public Nullable<decimal> cgst_amt { get; set; }
        public Nullable<decimal> igst_amt { get; set; }
        public string HSN_SAC { get; set; }
        public Nullable<decimal> finalprod_amt { get; set; }

        public int[] itemhead_id_arr { get; set; }
        public long[] item_qty_arr { get; set; }
        public decimal[] item_rate_arr { get; set; }
        public decimal[] gross_amt_arr { get; set; }
        public decimal[] disc_amt_arr { get; set; }
        public decimal[] sgst_amt_arr { get; set; }
        public decimal[] cgst_amt_arr { get; set; }
        public decimal[] igst_amt_arr { get; set; }
        public decimal[] finalprod_amt_arr { get; set; }
        public string[] HSN_SAC_arr { get; set; }
    }
}