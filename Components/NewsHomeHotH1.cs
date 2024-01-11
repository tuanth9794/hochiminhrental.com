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
    public class NewsHomeHotH1 : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {
            var model = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SHOWHOME == Position).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_NAME = p.CAT_NAME,
                CAT_TYPE = p.CAT_TYPE,
                CAT_URL = p.CAT_URL,
                CAT_DESC_EN = p.CAT_DESC_EN,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_CODE = p.CAT_CODE,
                CAT_DESC = p.CAT_DESC,
            }).OrderByDescending(p => p.CAT_ID);

            return View(model);
        }
    }
}
