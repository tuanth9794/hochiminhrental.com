using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;

namespace BulSoftCMS.Components
{
    public class NewsImages : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? NewsID)
        {
            var model = _context.ESHOP_NEWS_IMAGE.Where(x => x.NEWS_ID == NewsID).Select(p => new ESHOP_NEWS_IMAGE
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_IMG_DESC = p.NEWS_IMG_DESC,
                NEWS_IMG_ID = p.NEWS_IMG_ID,
                NEWS_IMG_IMAGE1 = p.NEWS_IMG_IMAGE1,
                NEWS_IMG_ORDER = p.NEWS_IMG_ORDER,             
            }).OrderByDescending(p => p.NEWS_IMG_ID);

            return View(model);
        }
    }
}
