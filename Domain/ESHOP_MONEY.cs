using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_MONEY
    {
        [Key]
        public int NEWS_MONEY_ID { get; set; }
        public string NEWS_MONEY_TITLE { get; set; }
        public string NEWS_MONEY_BEGIN { get; set; }
        public string NEWS_MONEY_END { get; set; }
        public string NEWS_MONEY_ACTIVE { get; set; }        
    }
}
