using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreCnice.Domain
{
   public class Customer_Request
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Customer_request_id { get; set; }
        public int CUSTOMER_ID { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_Phone { get; set; }
        public string Customer_Quoctich { get; set; }
        public string Customer_DuAn { get; set; }
        public int? Customer_PN { get; set; }
        public int? Customer_PB { get; set; }
        public string Customer_Dt { get; set; }
        public string Customer_Ns { get; set; }
        public int? Customer_TimeThue { get; set; }
        public string Customer_Desc { get; set; }
        public int? Customer_Check { get; set; }
        public string Customer_Image { get; set; }
        public int? Customer_Active { get; set; }
        public int? Customer_Request_Order { get; set; }
        public string Customer_Field1 { get; set; }
        public string Customer_Field2 { get; set; }

        public DateTime? Customer_TimeAv { get; set; }

        public DateTime? Customer_PublishDate { get; set; }
    }
}
