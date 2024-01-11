using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_NEWS_NEWS
    {
        [Key]
        public int NEWS_NEWS_ID { get; set; }
        public int NEWS_ID1 { get; set; }
        public int NEWS_ID2 { get; set; }       
    }
}
