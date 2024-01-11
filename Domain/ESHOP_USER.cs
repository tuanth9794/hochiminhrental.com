using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_USER
    {
        [Key]
        public int USER_ID { get; set; }
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email Address")]
        public string USER_NAME { get; set; }
        public string USER_UN { get; set; }
        public string USER_PW { get; set; }
        public int? GROUP_ID { get; set; }
        public int? USER_TYPE { get; set; }
        public int? USER_ACTIVE { get; set; }
        public string SALT { get; set; }
        public DateTime? USER_PUBLISHDATE { get; set; }
        public DateTime? USER_START_WORK { get; set; }
        public DateTime? USER_END_WORK { get; set; }
        public IEnumerable<ESHOP_GROUP> GroupList { get; set; }
    }
}
