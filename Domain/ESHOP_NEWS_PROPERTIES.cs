using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCnice.Domain
{
    public class ESHOP_NEWS_PROPERTIES
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal NEWS_PROP_ID { get; set; }
        public decimal NEWS_ID { get; set; }
        public decimal PROP_ID { get; set; }      
    }
}
