using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using CoreCnice.UtilsCs;

namespace BulSoftCMS.Components
{
    public class PaginationCat : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position, string URL)
        {
            string page = HttpContext.Request.Query["page"];

            var model = (from c in _context.ESHOP_CATEGORIES
                         join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                         join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                         where nc.CAT_ID == Position && (n.NEWS_PRINTTYPE == null || n.NEWS_PRINTTYPE == 1)
                         select n
                         ).OrderByDescending(x => x.NEWS_ID);
            var Group = model.GroupBy(x => x.NEWS_ID).Select(g => g.First()).OrderBy(p => p.NEWS_ID);
            ViewBag.Url = URL;
            ViewBag.page = page;
            ViewBag.pageDangchon = Utils.CIntDef(page);
            return View(Group);
        }
    }
}
