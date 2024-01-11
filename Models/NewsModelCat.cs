﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Models
{
    public class NewsModelCat
    {
        public int NEWS_ID { get; set; }
        public string NEWS_CODE { get; set; }
        public string NEWS_TITLE { get; set; }
        public string NEWS_DESC { get; set; }
        public string NEWS_URL { get; set; }
        public string NEWS_TARGET { get; set; }
        public string NEWS_SEO_KEYWORD { get; set; }
        public string NEWS_SEO_DESC { get; set; }
        public string NEWS_SEO_TITLE { get; set; }
        public string NEWS_SEO_URL { get; set; }
        public string NEWS_FILEHTML { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? NEWS_PUBLISHDATE { get; set; }
        public DateTime? NEWS_UPDATE { get; set; }
        public int? NEWS_SHOWTYPE { get; set; }
        public int? NEWS_SHOWINDETAIL { get; set; }
        public int? NEWS_FEEDBACKTYPE { get; set; }
        public int? NEWS_TYPE { get; set; }
        public int? NEWS_PERIOD { get; set; }
        public int? NEWS_ORDER_PERIOD { get; set; }
        public int? NEWS_ORDER { get; set; }
        public decimal? NEWS_PRICE1 { get; set; }
        public decimal? NEWS_PRICE2 { get; set; }
        public decimal? NEWS_PRICE3 { get; set; }
        public decimal? NEWS_PRICE4 { get; set; }
        public string NEWS_IMAGE1 { get; set; }
        public string NEWS_IMAGE2 { get; set; }
        public string CAT_SEO_URL { get; set; }
        public string NEWS_TITLE_EN { get; set; }
        public string NEWS_DESC_EN { get; set; }
        public string NEWS_FIELD4 { get; set; }
        public string NEWS_HTML_EN1 { get; set; }
        public string NEWS_HTML_EN2 { get; set; }
        public string NEWS_HTML_EN3 { get; set; }
        public string NEWS_FILEHTML_EN { get; set; }
        public string NEWS_TITLE_JS { get; set; }
        public string NEWS_SEO_URL_EN { get; set; }
        public string CAT_SEO_URL_EN { get; set; }
        public int? NEWS_PRINTTYPE { get; set; }

        public string NEWS_TIME_AVALBLE { get; set; }
    }
}