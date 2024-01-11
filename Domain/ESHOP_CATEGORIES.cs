using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_CATEGORIES
    {
        //public ESHOP_CATEGORIES()
        //{
        //    this.Cat1 = new HashSet<ESHOP_CATEGORIES>();          
        //}

        [Key]
        public int CAT_ID { get; set; }
        public string CAT_NAME { get; set; }
        public string CAT_CODE { get; set; }
        public string CAT_DESC { get; set; }
        public string CAT_URL { get; set; }
        public string CAT_TARGET { get; set; }
        public int? CAT_STATUS { get; set; }
        public int? CAT_PARENT_ID { get; set; }
        public int? CAT_PAGEITEM { get; set; }
        public int? CAT_PERIOD { get; set; }
        public int? CAT_PERIOD_ORDER { get; set; }
        public string CAT_SEO_TITLE_EN { get; set; }
        public string CAT_SEO_TITLE { get; set; }
        public string CAT_SEO_DESC { get; set; }
        public string CAT_SEO_KEYWORD { get; set; }
        public string CAT_SEO_URL { get; set; }
        public int? CAT_SHOWHOME { get; set; }
        public int? CAT_TYPE { get; set; }
        public int? CAT_SHOWFOOTER { get; set; }
        public int? CAT_POSITION { get; set; }
        public int? CAT_ROWITEM { get; set; }
        public string CAT_IMAGE1 { get; set; }
        public string CAT_IMAGE2 { get; set; }
        public string CAT_IMAGE3 { get; set; }
        public string CAT_DESC_EN { get; set; }
        public string CAT_FIELD1 { get; set; }
        public string CAT_FIELD2 { get; set; }    
        public string CAT_NAME_EN { get; set; }
        public string CAT_DESC_JS { get; set; }
        public string CAT_NAME_JS { get; set; }
        public string CAT_SEO_URL_EN { get; set; }

        public string CAT_SEO_META_DESC_EN { get; set; }

        public string CAT_SEO_META_CANONICAL { get; set; }

        public string CAT_LIENKET_EN { get; set; }
        //public virtual ICollection<ESHOP_CATEGORIES> Cat1 { get; set; }

        //public virtual ESHOP_CATEGORIES escat_pr { get; set; }
        //[ForeignKey("CategoryId")]
        //public virtual List<ESHOP_CATEGORIES> escat_pr { get; set; }
    }
}
