using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class PurchaseReturnModel
    {
        public int id { get; set; }
        public string PR_code { get; set; }
        public Nullable<int> PR_suppid { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? PR_billdate { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? PR_duedate { get; set; }
        public string PR_invoiceno { get; set; }
        public Nullable<decimal> PR_GrossAmt { get; set; }
        public Nullable<decimal> PR_Discount { get; set; }
        public Nullable<decimal> PR_AdditionalDisc { get; set; }
        public Nullable<decimal> PR_S_GST { get; set; }
        public Nullable<decimal> PR_C_GST { get; set; }
        public Nullable<decimal> PR_I_GST { get; set; }
        public Nullable<decimal> PR_LoadingCharge { get; set; }
        public Nullable<decimal> PR_FinalAmt { get; set; }
        public Nullable<decimal> PR_PaidAmt { get; set; }
        public Nullable<decimal> PR_BalanceAmt { get; set; }
        public string PR_pay_mode { get; set; }
        public string PR_suppName { get; set; }
        public string PR_prod_name { get; set; }
        public string PR_measureunit { get; set; }

        public Nullable<int> PR_purchasebill_id { get; set; }
        public Nullable<int> PR_itemhead_id { get; set; }
        public Nullable<decimal> PR_item_rate { get; set; }
        public Nullable<long> PR_item_qty { get; set; }
        public Nullable<decimal> PR_gross_amt { get; set; }
        public Nullable<decimal> PR_disc_amt { get; set; }
        public Nullable<decimal> PR_sgst_amt { get; set; }
        public Nullable<decimal> PR_cgst_amt { get; set; }
        public Nullable<decimal> PR_igst_amt { get; set; }
        public string PR_HSN_SAC { get; set; }
        public Nullable<decimal> PR_finalprod_amt { get; set; }

        public int[] PR_itemhead_id_arr { get; set; }
        public long[] PR_item_qty_arr { get; set; }
        public decimal[] PR_item_rate_arr { get; set; }
        public decimal[] PR_gross_amt_arr { get; set; }
        public decimal[] PR_disc_amt_arr { get; set; }
        public decimal[] PR_sgst_amt_arr { get; set; }
        public decimal[] PR_cgst_amt_arr { get; set; }
        public decimal[] PR_igst_amt_arr { get; set; }
        public decimal[] PR_finalprod_amt_arr { get; set; }
        public string[] PR_HSN_SAC_arr { get; set; }
    }
}