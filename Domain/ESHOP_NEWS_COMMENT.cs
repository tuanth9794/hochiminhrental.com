using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_NEWS_COMMENT
    {
        [Key]
        public int COMMENT_ID { get; set; }
        public string COMMENT_NAME { get; set; }
        public string COMMENT_EMAIL { get; set; }
        public string COMMENT_CONTENT { get; set; }
        public int COMMENT_STATUS { get; set; }
        public int CUS_ID { get; set; }
        public int NEWS_ID { get; set; }
        public DateTime COMMENT_PUBLISHDATE { get; set; }
        public string COMMENT_FIELD1 { get; set; }
        public string COMMENT_FIELD2 { get; set; }
    }
}
