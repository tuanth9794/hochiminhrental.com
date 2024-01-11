using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCnice.Domain
{
   public class CustomerConfirm
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerConfirm_Id { get; set; }
        public int CUSTOMER_ID { get; set; }
        public DateTime? CustomerConfirm_Date { get; set; }
        public int? CustomerConfirm_Active { get; set; }
        public int? CustomerConfirm_Type { get; set; }
        public string CustomerConfirm_Comment { get; set; }
        public int NEWS_ID { get; set; }
        public int? Customer_request_id { get; set; }
        public DateTime? CustomerConfirm_PublishDate { get; set; }
    }
}
