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
    public class HistoriesContent : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? catid)
        {
            var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                               join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                               join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                               where nc.CAT_ID == catid
                               select new
                               {
                                   n.NEWS_TITLE,
                                   n.NEWS_PUBLISHDATE,
                                   n.NEWS_SEO_URL,
                                   n.NEWS_URL,
                                   n.NEWS_PRICE1,
                                   n.NEWS_PRICE2,
                                   n.NEWS_CODE,
                                   n.NEWS_IMAGE1,
                                   n.NEWS_DESC,
                                   n.NEWS_IMAGE2,
                                   c.CAT_SEO_URL,
                                   n.NEWS_ID
                               }).Select(n => new NewsModelCat
                               {
                                   NEWS_TITLE = n.NEWS_TITLE,
                                   NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                   NEWS_SEO_URL = n.NEWS_SEO_URL,
                                   NEWS_URL = n.NEWS_URL,
                                   NEWS_PRICE1 = n.NEWS_PRICE1,
                                   NEWS_PRICE2 = n.NEWS_PRICE2,
                                   NEWS_CODE = n.NEWS_CODE,
                                   NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                   NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                   CAT_SEO_URL = n.CAT_SEO_URL,
                                   NEWS_DESC = n.NEWS_DESC,
                                   NEWS_ID = n.NEWS_ID,
                               }).OrderByDescending(x => x.NEWS_PUBLISHDATE).Take(8);

            return View(ListNewsCat);
        }
    }
}
