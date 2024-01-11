using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_NEWS_ATT
    {
        [Key]
        public int NEWS_ATT_ID { get; set; }
        public string NEWS_ATT_NAME { get; set; }
        public string NEWS_ATT_FILE { get; set; }
        public string NEWS_ATT_URL { get; set; }
        public int NEWS_ATT_ORDER { get; set; }
        public int EXT_ID { get; set; }
        public int NEWS_ID { get; set; }
        public string NEWS_ATT_FIELD1 { get; set; }
        public string NEWS_ATT_FIELD2 { get; set; }
        public string NEWS_ATT_FIELD3 { get; set; }
    }
}
