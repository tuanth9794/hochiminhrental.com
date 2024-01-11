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
    public class CatHomeCatOne : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {
            var model = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SHOWHOME == Position && x.CAT_TYPE == 1).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_NAME = p.CAT_NAME,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_TYPE = p.CAT_TYPE,
                CAT_IMAGE1 = p.CAT_IMAGE1,
                CAT_IMAGE3 = p.CAT_IMAGE3,            
                CAT_URL = p.CAT_URL,
                CAT_DESC = p.CAT_DESC,
                CAT_DESC_EN = p.CAT_DESC_EN,
                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
            }).OrderByDescending(p => p.CAT_ID);

            return View(model);
        }
    }
}
