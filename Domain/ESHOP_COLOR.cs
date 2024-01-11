using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_COLOR
    {
        [Key]
        public int COLOR_ID { get; set; }
        public string COLOR_NAME { get; set; }
        public string COLOR_DESC { get; set; }
        public int COLOR_PRIORITY { get; set; }
        public int COLOR_ACTIVE { get; set; }
        public string COLOR_FIELD1 { get; set; }
    }
}
