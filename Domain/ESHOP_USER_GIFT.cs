using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_USER_GIFT
    {
        [Key]
        public int USER_GIFT_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int GIFT_ID { get; set; }
        public DateTime USER_GIFT_DATE { get; set; }
        public int USER_GIFT_STATUS { get; set; }
        public int USER_GIFT_QUANTITY { get; set; }
        public string USER_GIFT_DESC { get; set; }
        public string USER_GIFT_FEILD { get; set; }
        public string USER_GIFT_FIELD1 { get; set; }
        public int USER_GIFT_ACTIVE { get; set; }        
    }
}
