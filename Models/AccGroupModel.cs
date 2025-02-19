using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class AccGroupModel
    {
        public int id { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public Nullable<bool> is_deleted { get; set; }
    }
}