using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class SaleEntryModel
    {
        public virtual CustomerModel Cust { get; set; }

        public int id { get; set; }
        public Nullable<int> userid { get; set; }
        public string s_entrycode { get; set; }
        public string custname { get; set; }
        public string custphone { get; set; }
        public Nullable<System.DateTime> billdate_s { get; set; }
        public string invoiceno { get; set; }
        public Nullable<decimal> GrossAmt_s { get; set; }
        public Nullable<decimal> Discount_s { get; set; }
        public Nullable<decimal> AdditionalDisc_s { get; set; }
        public Nullable<decimal> SGST_s { get; set; }
        public Nullable<decimal> CGST_s { get; set; }
        public Nullable<decimal> IGST_s { get; set; }
        public Nullable<decimal> FinalAmt_s { get; set; }
        public Nullable<decimal> PaidAmt_s { get; set; }
        public Nullable<decimal> BalanceAmt_s { get; set; }
        public string paymode { get; set; }
        public string cust_name { get; set; }
        public string prod_name { get; set; }
        public string measurunit { get; set; }
        public string Usrname { get; set; }
        public Nullable<decimal> mrpprice { get; set; }
        public Nullable<int> salebillid { get; set; }
        public Nullable<int> itemheadid_s { get; set; }
        public Nullable<decimal> itemrate_s { get; set; }
        public Nullable<long> itemqty_s { get; set; }
        public Nullable<decimal> gross_amt_s { get; set; }
        public Nullable<decimal> discamount_s { get; set; }
        public Nullable<decimal> sgstamt_s { get; set; }
        public Nullable<decimal> cgstamt_s { get; set; }
        public Nullable<decimal> igstamt_s { get; set; }
        public string HSNSAC_s { get; set; }
        public Nullable<decimal> finalprice_s { get; set; }

        public int[] S_itemhead_id_arr { get; set; }
        public long[] S_item_qty_arr { get; set; }
        public decimal[] S_item_rate_arr { get; set; }
        public decimal[] S_item_mrp_arr { get; set; }
        public decimal[] S_gross_amt_arr { get; set; }
        public decimal[] S_disc_amt_arr { get; set; }
        public decimal[] S_sgst_amt_arr { get; set; }
        public decimal[] S_cgst_amt_arr { get; set; }
        public decimal[] S_igst_amt_arr { get; set; }
        public decimal[] S_finalprod_amt_arr { get; set; }
        public string[] S_HSN_SAC_arr { get; set; }
    }
}