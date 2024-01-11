using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_ORDER_ITEM
    {
        [Key]
        public int ITEM_ID { get; set; }
        public int ORDER_ID { get; set; }
        public int NEWS_ID { get; set; }
        public int UNIT_ID { get; set; }
        public float ITEM_QUANTITY { get; set; }
        public decimal ITEM_SUBTOTAL { get; set; }
        public DateTime ITEM_PUBLISDATE { get; set; }
        public DateTime ITEM_UPDATE { get; set; }
        public decimal ITEM_PRICE { get; set; }      
    }
}
