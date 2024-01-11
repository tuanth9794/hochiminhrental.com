using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreCnice.Models;
using CoreCnice.Connect;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CoreCnice.UtilsCs;

namespace BulSoftCMS.Controllers
{
    public class HomeController : Controller
    {
        string SessionKeyName = "CustomerSesion1";
        string SessionKeyName1 = "CustomerSesion";

        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public  IActionResult Index()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult QuickViews(int id)
        {
            //return ViewComponent("NewsQuickView");
            return ViewComponent("AmbulList", new { IdNews = id });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
