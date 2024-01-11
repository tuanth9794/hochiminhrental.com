using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
  public  class ESHOP_NEWS_CAT
    {
        [Key]
        public int NEWS_CAT_ID { get; set; }
        public int NEWS_ID { get; set; }
        public int CAT_ID { get; set; }
    }
}
