using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_CONFIG
    {
        [Key]
        public int CONFIG_ID { get; set; }
        public string CONFIG_TITLE { get; set; }
        public string CONFIG_KEYWORD { get; set; }
        public string CONFIG_DESCRIPTION { get; set; }
        public int? CONFIG_HITCOUNTER { get; set; }
        public string CONFIG_FAVICON { get; set; }
        public int? CONFIG_ORDER { get; set; }
        public int? CONFIG_LANGUAGE { get; set; }
        public string CONFIG_EMAIL { get; set; }
        public string CONFIG_SMTP { get; set; }
        public string CONFIG_PORT { get; set; }
        public string CONFIG_PASSWORD { get; set; }
        public string CONFIG_NAME { get; set; }
        public string CONFIG_ADD { get; set; }
        public string CONFIG_FACEBOOK { get; set; }
        public string CONFIG_GOOGLE { get; set; }
        //public int AD_ITEM_LANGUAGE { get; set; }
        //public string AD_ITEM_FIELD1 { get; set; }
        //public string AD_ITEM_FIELD2 { get; set; }
        public string CONFIG_NAME_US { get; set; }
        public string CONFIG_FOOTER { get; set; }
        public string CONFIG_CONTACT { get; set; }
        public string CONFIG_DESCRIPTION_EN { get; set; }
        public string CONFIG_FIELD1 { get; set; }
        public string CONFIG_FIELD2 { get; set; }
        public string CONFIG_FIELD3 { get; set; }
        public string CONFIG_FIELD4 { get; set; }
        public string CONFIG_FIELD5 { get; set; }

    }
}
