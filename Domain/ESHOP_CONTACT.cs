using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_CONTACT
    {
        [Key]
        public int CONTACT_ID { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_EMAIL { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string CONTACT_ADDRESS { get; set; }
        public string CONTACT_TITLE { get; set; }
        public string CONTACT_CONTENT { get; set; }
        public string CONTACT_ATT1 { get; set; }
        public DateTime? CONTACT_PUBLISHDATE { get; set; }
        public string CONTACT_ANSWER { get; set; }
        public int? CONTACT_SHOWTYPE { get; set; }
        public int? CONTACT_TYPE { get; set; }
        

    }
}
