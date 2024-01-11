using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_PROPERTIES
    {
        [Key]
        public int PROP_ID { get; set; }
        public string PROP_NAME { get; set; }
        public string PROP_DESC { get; set; }
        public int PROP_PARENT_ID { get; set; }
        public int? PROP_ACTIVE { get; set; }
        public int? PROP_RANK { get; set; }
        public string PRO_IMAGES { get; set; }
        public int? PRO_ORDER { get; set; }
        public string PRO_COLOR { get; set; }
        public string PRO_NAME_EN { get; set; }
        public int? PRO_TYPE { get; set; }

        public int? PRO_TYPE_HOME { get; set; }
    }
}
