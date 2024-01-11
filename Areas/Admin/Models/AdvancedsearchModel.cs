using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Areas.Admin.Models
{
    public class AdvancedsearchModel
    {
        public string SEARCHNAME { get; set; }
        public decimal SEARCHPRICEMIN { get; set; }
        public decimal SEARCHPRICEMAX { get; set; }
        public string SEARCHBATHROOM { get; set; }
        public string SEARCHBEDROOM { get; set; }
        public string SEARCHACREAGE { get; set; }
        public int? SEARCHSTATUS { get; set; }
        public int? SEARCHCATID { get; set; }
    }
}
