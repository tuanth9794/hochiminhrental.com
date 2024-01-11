using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using CoreCnice.Models;

namespace BulSoftCMS.Components
{
    public class ProductSeller : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? idsaler)
        {

            var NewsList = _context.ESHOP_NEWS.Where(x => x.NEWS_TYPE == 1).Select(p => new ESHOP_NEWS
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_TITLE = p.NEWS_TITLE,
                NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                NEWS_ORDER = p.NEWS_ORDER,
                NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                NEWS_IMAGE1 = p.NEWS_IMAGE1,
                NEWS_CODE = p.NEWS_CODE,
                NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                NEWS_FIELD3 = p.NEWS_FIELD3,
                NEWS_FIELD4 = p.NEWS_FIELD4,
                NEWS_PRICE1 = p.NEWS_PRICE1,
                NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                NEWS_UPDATE = p.NEWS_UPDATE,
                NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                USER_ID = p.USER_ID,
                NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
            }).OrderByDescending(p => p.NEWS_PUBLISHDATE);


            var NewsListTitle = (from n in NewsList
                                 join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                 join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                 join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                 where cs.CUSTOMER_ID == idsaler
                                 select new
                                 {
                                     n.NEWS_ID,
                                     n.NEWS_CODE,
                                     n.NEWS_TITLE,
                                     n.NEWS_DESC,
                                     n.NEWS_TARGET,
                                     n.NEWS_SEO_KEYWORD,
                                     n.NEWS_SEO_DESC,
                                     n.NEWS_SEO_TITLE,
                                     n.NEWS_SEO_URL,
                                     n.NEWS_FILEHTML,
                                     n.NEWS_PUBLISHDATE,
                                     n.NEWS_UPDATE,
                                     n.NEWS_SHOWTYPE,
                                     n.NEWS_SHOWINDETAIL,
                                     n.NEWS_FEEDBACKTYPE,
                                     n.NEWS_TYPE,
                                     n.NEWS_PERIOD,
                                     n.NEWS_ORDER_PERIOD,
                                     n.NEWS_ORDER,
                                     n.NEWS_PRICE1,
                                     n.NEWS_PRICE2,
                                     n.NEWS_PRICE3,
                                     n.NEWS_IMAGE1,
                                     n.NEWS_IMAGE2,
                                     n.NEWS_TITLE_EN,
                                     n.NEWS_DESC_EN,
                                     n.NEWS_URL,
                                     c.CAT_SEO_URL,
                                     n.NEWS_FILEHTML_EN,
                                     n.NEWS_HTML_EN1,
                                     n.NEWS_HTML_EN2,
                                     n.NEWS_HTML_EN3,
                                     n.NEWS_FIELD4,
                                     n.NEWS_TITLE_JS,
                                     n.NEWS_SEO_URL_EN,
                                     c.CAT_SEO_URL_EN
                                 }).Select(p => new NewsModelCat
                                 {
                                     NEWS_ID = p.NEWS_ID,
                                     NEWS_CODE = p.NEWS_CODE,
                                     NEWS_TITLE = p.NEWS_TITLE,
                                     NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                     NEWS_DESC_EN = p.NEWS_DESC_EN,
                                     NEWS_DESC = p.NEWS_DESC,
                                     NEWS_URL = p.NEWS_URL,
                                     NEWS_TARGET = p.NEWS_TARGET,
                                     NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                     NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                     NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                     NEWS_SEO_URL = p.NEWS_SEO_URL,
                                     NEWS_FILEHTML = p.NEWS_FILEHTML,
                                     NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                     NEWS_UPDATE = p.NEWS_UPDATE,
                                     NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                     NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                     NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                     NEWS_TYPE = p.NEWS_TYPE,
                                     NEWS_PERIOD = p.NEWS_PERIOD,
                                     NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                     NEWS_ORDER = p.NEWS_ORDER,
                                     NEWS_PRICE1 = p.NEWS_PRICE1,
                                     NEWS_PRICE2 = p.NEWS_PRICE2,
                                     NEWS_PRICE3 = p.NEWS_PRICE3,
                                     NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                     NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                     CAT_SEO_URL = p.CAT_SEO_URL,
                                     NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                     NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                     NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                     NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                     NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                     NEWS_FIELD4 = p.NEWS_FIELD4,
                                     NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                     CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                 }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

            return View(NewsListTitle);          
        }
    }
}
