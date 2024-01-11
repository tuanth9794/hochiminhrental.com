using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{   
    public class ESHOP_STORE
    {
        [Key]
        public int ESHOP_STORE_ID { get; set; }
        public string ESHOP_STORE_NAME { get; set; }
        public string ESHOP_STORE_ADDRESS { get; set; }
        public string ESHOP_STORE_EMAIL { get; set; }
        public string ESHOP_STORE_PHONE { get; set; }
        public string ESHOP_STORE_IMAGE { get; set; }      
        public string ESHOP_STORE_UN { get; set; }       
        public int? ESHOP_STORE_ACTIVE { get; set; }
        public int? ESHOP_STORE_TYPE { get; set; }
    }
}
