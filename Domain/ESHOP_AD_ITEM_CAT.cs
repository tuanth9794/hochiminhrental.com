using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_AD_ITEM_CAT
    {
        [Key]
        public int AD_ITEM_CAT_ID { get; set; }
        public int? AD_ITEM_ID { get; set; }
        public int? CAT_ID { get; set; }       
    }
}
