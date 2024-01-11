using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Areas.Admin.Models
{
    public class CategoriesModel
    {
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
        public string CAT_DESC_EN { get; set; }
    }
}
