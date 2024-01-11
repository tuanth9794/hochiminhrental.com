using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_UNITS
    {
        [Key]
        public int UNIT_ID { get; set; }
        public string UNIT_NAME { get; set; }
        public string UNIT_DESC { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public int UNIT_ACTIVE { get; set; }
    }
}
