using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
   public class ESHOP_BASKET
    {
        [Key]
        public int BASKET_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int NEWS_ID { get; set; }
        public decimal BASKET_PRICE { get; set; }
        public int BASKET_QUANTITY { get; set; }
        public DateTime BASKET_PUBLISHDATE { get; set; }
        public DateTime BASKET_UPDATE { get; set; }
        public decimal BASKET_AMOUNT { get; set; }
        public Guid CUSTOMER_OID { get; set; }
    }
}
