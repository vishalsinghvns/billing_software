using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class ItemGroupModel
    {
        public int id { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
        public Nullable<bool> is_deleted { get; set; }
    }
}