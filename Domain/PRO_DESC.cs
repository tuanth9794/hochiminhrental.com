using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class PRO_DESC
    {
        [Key]
        public int PRO_ID { get; set; }
        public string PRO_NAME { get; set; }
        public string PRO_FILE { get; set; }
        public int? NEWS_ID { get; set; }
        public int? PRO_ORDER { get; set; }
        public int? PRO_ACTIVE { get; set; }
        public int? PRO_TYPE { get; set; }
        public string PRO_IMAGES { get; set; }
        public string PRO_NAME_EN { get; set; }
        public string PRO_FILE_EN { get; set; }
    }
}
