using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
    public class ESHOP_NEWS_IMAGE
    {
        [Key]
        public int NEWS_IMG_ID { get; set; }
        public string NEWS_IMG_IMAGE1 { get; set; }
        public string NEWS_IMG_DESC { get; set; }
        public int NEWS_IMG_ORDER { get; set; }
        public int? NEWS_IMG_SHOWTYPE { get; set; }
        public int NEWS_ID { get; set; }

        //public virtual ESHOP_NEWS ESHOP_NEWS { get; set; }

        //public ESHOP_NEWS_IMAGE()
        //{
        //this.NEWS_IMG_IMAGE1 = "";
        //this.NEWS_IMG_DESC = "";
        //this.NEWS_IMG_ORDER = 0;
        //this.NEWS_IMG_SHOWTYPE = 0;             
        //}
    }
}
