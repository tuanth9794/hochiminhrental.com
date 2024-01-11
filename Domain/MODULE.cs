using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class MODULE
    {
        [Key]
        public int NEWS_MODULE_ID { get; set; }
        public string NEWS_MODULE_NAME { get; set; }
        public int NEWS_MODULE_LANG { get; set; }
        public int NEWS_MODULE_STATUS { get; set; }
        public int NEWS_MODULE_ORDER { get; set; }
    }
}
