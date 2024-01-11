using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
  public  class NEWS_CUSTOMER_POST
    {
        [Key]
        public int NEWS_CUS_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int NEWS_ID { get; set; }
    }
}
