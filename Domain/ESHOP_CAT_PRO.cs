using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Domain
{
    public class ESHOP_CAT_PRO
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal ESHOP_CAT_PRO_ID { get; set; }
        public string CAT_PRO_FIELD2 { get; set; }
        public string CAT_PRO_FIELD1 { get; set; }
        public int CAT_ID { get; set; }
        public decimal PROP_ID { get; set; }    
    }
}
