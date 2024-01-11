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
    public class ProDescIner : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? NewsId)
        {
            var listNew = (from n in _context.ESHOP_PROPERTIES
                           join c in _context.ESHOP_NEWS_PROPERTIES on n.PROP_ID equals c.PROP_ID
                           where c.NEWS_ID == NewsId
                           select n);
            ViewBag.Id = NewsId;
            var Group = listNew.GroupBy(x => x.PROP_PARENT_ID).Select(g => g.First()).OrderBy(p => p.PRO_ORDER).Where(x => x.PROP_ACTIVE == 1);

            List<ESHOP_PROPERTIES> list = new List<ESHOP_PROPERTIES>();
            if (Group.ToList().Count > 0)
            {
                foreach (var item in Group)
                {
                    var listitem = _context.ESHOP_PROPERTIES.SingleOrDefault(x => x.PROP_ID == item.PROP_PARENT_ID);
                    if(listitem != null)
                    {
                        list.Add(listitem);
                    }
                }
            }

            var listGroup = list.ToList();

            return View(listGroup);
        }
    }
}
