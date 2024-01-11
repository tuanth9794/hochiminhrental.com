using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using Microsoft.AspNetCore.Http;
using CoreCnice.UtilsCs;

namespace BulSoftCMS.Components
{
    public class RangePrice : ViewComponent
    {
        string SessionKeyName = "CustomerSesion3";
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(decimal? minPrice, decimal? maxPrice)
        {
            int TypePrice = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                TypePrice = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.ChonNgay = TypePrice;


            var ListOder = _context.ESHOP_NEWS.Where(x => x.NEWS_PRICE1 != 0 && x.NEWS_TYPE != 0).OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1);
            if (ListOder.ToList().Count > 0)
            {
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = Utils.CIntDef(ListOder.ToList()[0].NEWS_PRICE1);
            }
            else
            {
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = Utils.CIntDef(0);
            }

            //}
            return View();
        }
    }
}
