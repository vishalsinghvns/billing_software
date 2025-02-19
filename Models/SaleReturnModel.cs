using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class SaleReturnModel
    {
        public int id { get; set; }
        public int shopid { get; set; }
        public string SRshopname { get; set; }
        public string SRUsrname { get; set; }
        public string SR_entrycode { get; set; }
        public string SR_custname { get; set; }
        //public Nullable<int> SR_supplierid { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? SR_billdate { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? SR_duedate { get; set; }
        public string SR_invoicetype { get; set; }
        public string SR_invoiceno { get; set; }
        public Nullable<decimal> SR_GrossAmt { get; set; }
        public Nullable<decimal> SR_Discount { get; set; }
        public Nullable<decimal> SR_AdditionalDisc { get; set; }
        public Nullable<decimal> SR_SGST { get; set; }
        public Nullable<decimal> SR_CGST { get; set; }
        public Nullable<decimal> SR_IGST { get; set; }
        public Nullable<decimal> SR_othercharge { get; set; }
        public Nullable<decimal> SR_FinalAmt { get; set; }
        public Nullable<decimal> SR_PaidAmt { get; set; }
        public Nullable<decimal> SR_BalanceAmt { get; set; }
        public string SR_paymode { get; set; }
        public string SR_cust_name { get; set; }
        public string SR_prod_name { get; set; }
        public string SR_measurunit { get; set; }
        public Nullable<decimal> SR_mrp { get; set; }

        public Nullable<int> SR_salebillid { get; set; }
        public Nullable<int> SR_itemheadid { get; set; }
        public Nullable<decimal> SR_itemrate { get; set; }
        public Nullable<long> SR_itemqty { get; set; }
        public Nullable<decimal> SR_gross_amt { get; set; }
        public Nullable<decimal> SR_discamount { get; set; }
        public Nullable<decimal> SR_sgstamt { get; set; }
        public Nullable<decimal> SR_cgstamt { get; set; }
        public Nullable<decimal> SR_igstamt { get; set; }
        public string SR_HSNSAC { get; set; }
        public Nullable<decimal> SR_finalprice { get; set; }

        public int[] SR_itemhead_id_arr { get; set; }
        public long[] SR_item_qty_arr { get; set; }
        public decimal[] SR_item_rate_arr { get; set; }
        public decimal[] SR_item_MRP_arr { get; set; }
        public decimal[] SR_gross_amt_arr { get; set; }
        public decimal[] SR_disc_amt_arr { get; set; }
        public decimal[] SR_sgst_amt_arr { get; set; }
        public decimal[] SR_cgst_amt_arr { get; set; }
        public decimal[] SR_igst_amt_arr { get; set; }
        public decimal[] SR_finalprod_amt_arr { get; set; }
        public string[] SR_HSN_SAC_arr { get; set; }
    }
}