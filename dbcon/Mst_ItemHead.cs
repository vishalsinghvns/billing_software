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
    
    public partial class Mst_ItemHead
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
        public Nullable<decimal> itemSGST { get; set; }
        public Nullable<decimal> itemCGST { get; set; }
        public Nullable<decimal> itemIGST { get; set; }
        public Nullable<bool> itemPurchaseTax { get; set; }
        public Nullable<bool> itemSaleTax { get; set; }
        public string itemMeasureUnit { get; set; }
        public Nullable<long> itemOpenQty { get; set; }
        public Nullable<long> itemAvlQty { get; set; }
        public Nullable<decimal> itemOpenValue { get; set; }
        public string itemHSN_SAC { get; set; }
        public string itembarcodeimg { get; set; }
        public string itemBarcode { get; set; }
        public Nullable<bool> is_deleted { get; set; }
    }
}
