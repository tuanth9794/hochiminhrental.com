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
    public class CatBanner : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {

            var modela = (from item in _context.ESHOP_AD_ITEM
                          join itemcat in _context.ESHOP_AD_ITEM_CAT on item.AD_ITEM_ID equals itemcat.AD_ITEM_ID
                          where itemcat.CAT_ID == Position
                          select new
                          {
                              item.AD_ITEM_ID,
                              item.AD_ITEM_FILENAME,
                              item.AD_ITEM_PUBLISHDATE,
                              item.AD_ITEM_URL,
                              item.AD_ITEM_ORDER,
                              item.AD_ITEM_CODE,
                              item.AD_ITEM_DESC
                          }
                         ).OrderByDescending(p => p.AD_ITEM_ORDER);

            return View(modela);
        }
    }
}
