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
    public class MenuMobile : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {
            var model = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && (x.CAT_POSITION == Position || x.CAT_POSITION == 4) && x.CAT_PARENT_ID == 0).OrderByDescending(x => x.CAT_PERIOD_ORDER).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_NAME = p.CAT_NAME,
                CAT_TYPE = p.CAT_TYPE,
                CAT_IMAGE1 = p.CAT_IMAGE1,
                CAT_URL = p.CAT_URL,
                CAT_DESC = p.CAT_DESC,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                CAT_NAME_EN = p.CAT_NAME_EN,
            }).OrderBy(x => x.CAT_PERIOD_ORDER);

            var Group = model.GroupBy(x => x.CAT_ID).Select(g => g.First()).OrderBy(x => x.CAT_PERIOD_ORDER);

            return View(Group);
        }
    }
}
