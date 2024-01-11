using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_COMPA
    {
        [Key]
        public int NEWS_COMPA_ID { get; set; }
        public int NEWS_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int STATUS { get; set; }        
    }
}
