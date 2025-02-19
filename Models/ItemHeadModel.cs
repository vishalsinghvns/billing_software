using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class ItemHeadModel
    {
        public int id { get; set; }
        public string ItemGroup { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string ProductCode { get; set; }
        public Nullable<decimal> itemMRP { get; set; }
        public Nullable<decimal> itemPurchaseRate { get; set; }
        public Nullable<decimal> itemSaleRate { get; set; }
        public string itemSaleAs { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> itemSGST { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> itemCGST { get; set; }
        public Nullable<decimal> itemIGST { get; set; }
        public Nullable<decimal> itemSGST_disp { get; set; }
        public Nullable<decimal> itemCGST_disp { get; set; }
        public Nullable<decimal> itemIGST_disp { get; set; }
        public Nullable<bool> itemPurchaseTax { get; set; }
        public Nullable<bool> itemSaleTax { get; set; }
        public string itemMeasureUnit { get; set; }
        public Nullable<long> itemOpenQty { get; set; }
        public Nullable<long> itemAvlQty { get; set; }
        public Nullable<decimal> itemOpenValue { get; set; }
        public string itemHSN_SAC { get; set; }
        public Nullable<bool> is_deleted { get; set; }

        public byte[] itemBarcodeImage { get; set; }
        public string itemBarcode { get; set; }
        public string ImageUrl { get; set; }

        public string ItemGroupName { get; set; }
        public string itembarcodeimg { get; set; }
        public Nullable<long> counts { get; set; }
        public HttpPostedFileBase excelfile { get; set; }
    }
}