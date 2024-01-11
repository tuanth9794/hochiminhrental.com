using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_EMAIL
    {
        [Key]
        public int EMAIL_ID { get; set; }
        public int EMAIL_STT { get; set; }
        public string EMAIL_DESC { get; set; }
        public string EMAIL_FROM { get; set; }
        public string EMAIL_TO { get; set; }
        public string EMAIL_CC { get; set; }
        public string EMAIL_BCC { get; set; }
        public int? EMAIL_PUBLISHDATE_DAY { get; set; }
        public int? EMAIL_CUSTOMER_ALL { get; set; }
        public DateTime? EMAIL_PUBLISHDATE { get; set; }
        public DateTime? EMAIL_PUBLISHDATE_START { get; set; }
        public int? USER_ID { get; set; }        
    }
}
