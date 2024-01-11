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
    public class BannerLeft : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {
            var model = _context.ESHOP_AD_ITEM.Where(x => x.AD_ITEM_POSITION == Position).Select(p => new ESHOP_AD_ITEM
            {
                AD_ITEM_ID = p.AD_ITEM_ID,
                AD_ITEM_FILENAME = p.AD_ITEM_FILENAME,
                AD_ITEM_PUBLISHDATE = p.AD_ITEM_PUBLISHDATE,
                AD_ITEM_URL = p.AD_ITEM_URL,
                AD_ITEM_ORDER = p.AD_ITEM_ORDER,
                AD_ITEM_CODE = p.AD_ITEM_CODE,
                AD_ITEM_DESC = p.AD_ITEM_DESC
            }).OrderByDescending(p => p.AD_ITEM_ORDER);

            return View(model);
        }
    }
}
