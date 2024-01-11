using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_WEBLINKS
    {
        [Key]
        public int WEBSITE_LINKS_ID { get; set; }
        public string WEBSITE_LINKS_NAME { get; set; }
        public string WEBSITE_LINKS_URL { get; set; }
        public string WEBSITE_LINKS_TARGET { get; set; }
        public int WEBSITE_LINKS_LANGUAGE { get; set; }
        public int WEBSITE_LINKS_ORDER { get; set; }
        public int WEBSITE_LINK_PE_ID { get; set; }
        public int WEBSITE_RANK { get; set; }
    }
}
