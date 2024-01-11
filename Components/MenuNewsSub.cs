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
    public class MenuNewsSub : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {

            var modela = (from item in _context.ESHOP_NEWS
                          where item.NEWS_PERIOD == Position
                          select item
                         ).OrderByDescending(p => p.NEWS_PERIOD);

            return View(modela);
        }
    }
}
