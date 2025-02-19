using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class CustomerModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string mobileno { get; set; }
        public string whatsapno { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? DOB { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy }")]
        public DateTime? AnnDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string NameNo { get; set; }
    }
}