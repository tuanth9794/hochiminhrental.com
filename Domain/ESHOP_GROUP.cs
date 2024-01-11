using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_GROUP
    {
        [Key]
        public int GROUP_ID { get; set; }
        public string GROUP_CODE { get; set; }
        public string GROUP_NAME { get; set; }
        public int? GROUP_TYPE { get; set; }
        public DateTime? GROUP_PUBLISHDATE { get; set; }     
    }
}
