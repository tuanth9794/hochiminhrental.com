using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreCnice.Connect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IActionResult Index()
        {
            try
            {
                var ListCust = _context.ESHOP_CUSTOMER.Where(x => x.CUSTOMER_SHOWTYPE != 0);

                ViewBag.Customer = ListCust.ToList().Count.ToString();

                var ListOrder = _context.ESHOP_ORDER.Where(x => x.ORDER_STATUS != 5);

                ViewBag.Order = ListOrder.ToList().Count.ToString();

                var ListContact = _context.ESHOP_CONTACT.Where(x => x.CONTACT_SHOWTYPE != 5);

                ViewBag.Contact = ListContact.ToList().Count.ToString();

                var ListHitcounter = _context.ESHOP_CONFIG;
                if (ListHitcounter.ToList()[0].CONFIG_HITCOUNTER == null)
                {
                    ViewBag.Counter = 0;
                }
                else
                {
                    ViewBag.Counter = ListHitcounter.ToList()[0].CONFIG_HITCOUNTER;
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
