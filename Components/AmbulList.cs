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
    public class AmbulList : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? IdNews)
        {
            var ListNewsCat = (from c in _context.ESHOP_NEWS
                               join nc in _context.ESHOP_NEWS_IMAGE on c.NEWS_ID equals nc.NEWS_ID
                               where nc.NEWS_ID == IdNews
                               select nc);
            return View(ListNewsCat);
        }
    }
}
