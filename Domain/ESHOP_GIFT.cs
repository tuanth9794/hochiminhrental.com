using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_GIFT
    {
        [Key]
        public int GIFT_ID { get; set; }
        public string GIFT_NAME { get; set; }
        public string GIFT_DESC { get; set; }
        public string GIFT_IMAGE { get; set; }
        public int GIFT_POINT { get; set; }
        public int GIFT_AMOUNT { get; set; }
        public int GIFT_MAX_AMOUNT { get; set; }
        public string GIFT_CONTENT { get; set; }
        public DateTime GIFT_PULISHDATE { get; set; }
        public int GIF_ACTIVE { get; set; }
        public DateTime GIFT_USE_PUBLISHDATE { get; set; }
        
    }
}
