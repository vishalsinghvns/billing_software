using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class AccHeadModel
    {
        public int id { get; set; }
        public Nullable<int> AccGroupid { get; set; }
        public string AccName { get; set; }
        public string AccCode { get; set; }
        public Nullable<decimal> OpeningAmount { get; set; }
        public string RegType { get; set; }
        public string AccGST { get; set; }
        public string AccPAN { get; set; }
        public string AccContactPerson { get; set; }
        public string AccArea { get; set; }
        public string AccAddress { get; set; }
        public string AccCity { get; set; }
        public string AccPincode { get; set; }
        public string AccPhone { get; set; }
        public string AccEmail { get; set; }
        public Nullable<bool> is_deleted { get; set; }

        public string AccGroupName { get; set; }
    }
}