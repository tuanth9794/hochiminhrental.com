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
    public class ProDescInerItem : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? NewsId,int? pro_id)
        {
            var listNew = (from n in _context.ESHOP_PROPERTIES
                           join c in _context.ESHOP_NEWS_PROPERTIES on n.PROP_ID equals c.PROP_ID
                           where c.NEWS_ID == NewsId && n.PROP_PARENT_ID == pro_id
                           select n);
            ViewBag.Id = NewsId;
            var Group = listNew.GroupBy(x => x.PROP_ID).Select(g => g.First()).OrderBy(p => p.PRO_ORDER);         

            return View(Group);
        }
    }
}
