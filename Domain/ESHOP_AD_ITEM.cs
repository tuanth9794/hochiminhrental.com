using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_AD_ITEM
    {
        [Key]
        public int AD_ITEM_ID { get; set; }
        public string AD_ITEM_CODE { get; set; }
        public string AD_ITEM_FILENAME { get; set; }
        public string AD_ITEM_DESC { get; set; }
        public int? AD_ITEM_TYPE { get; set; }
        public int? AD_ITEM_HEIGHT { get; set; }
        public int? AD_ITEM_WIDTH { get; set; }
        public int? AD_ITEM_CLICKCOUNT { get; set; }
        public string AD_ITEM_URL { get; set; }
        public DateTime? AD_ITEM_PUBLISHDATE { get; set; }
        public DateTime? AD_ITEM_DATEFROM { get; set; }
        public DateTime? AD_ITEM_DATETO { get; set; }
        public int? AD_ITEM_ORDER { get; set; }
        public int? AD_ITEM_POSITION { get; set; }
        public string AD_ITEM_TARGET { get; set; }
        public int? AD_ITEM_COUNT { get; set; }
        public int? AD_ITEM_LANGUAGE { get; set; }
        public string AD_ITEM_FIELD1 { get; set; }
        public string AD_ITEM_FIELD2 { get; set; }
    }
}
