using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_THEME
    {
        [Key]
        public int ESHOP_THEME_ID { get; set; }
        public string ESHOP_THEME_NAME { get; set; }
        public string ESHOP_THEME_LINK_FILE { get; set; }      
        public int CAT_ID { get; set; }
        public int NEWS_ID { get; set; }
        public int ESHOP_THEME_TYPE { get; set; }        
    }
}
