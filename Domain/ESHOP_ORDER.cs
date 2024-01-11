using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_ORDER
    {
        [Key]
        public int ORDER_ID { get; set; }
        public int CUSTOMER_ID { get; set; }        
        public string ORDER_CODE { get; set; }
        public decimal? ORDER_TOTAL_ALL { get; set; }
        public DateTime? ORDER_PUBLISHDATE { get; set; }
        public DateTime? ORDER_UPDATE { get; set; }
        public string RECEIVER_FULLNAME { get; set; }
        public string RECEIVER_ADDRESS { get; set; }
        public string RECEIVER_PHONE1 { get; set; }
        public string RECEIVER_EMAIL { get; set; }
        public string ORDER_DESC { get; set; }
        public int? ORDER_STATUS { get; set; }
        public int? ORDER_DAY { get; set; }
        public int? ORDER_MONTH { get; set; }
        public int? ORDER_YEAR { get; set; }      
        public string ORDER_IP { get; set; }     
       
    }
}
