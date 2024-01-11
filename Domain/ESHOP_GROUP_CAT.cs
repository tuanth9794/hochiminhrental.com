using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_GROUP_CAT
    {
        [Key]
        public int GROUP_CAT_ID { get; set; }        
        public int GROUP_ID { get; set; }
        public int CAT_ID { get; set; }        
    }
}
