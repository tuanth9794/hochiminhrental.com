﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;

namespace BulSoftCMS.Components
{
    public class MenuLeft : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {

            var modela = (from item in _context.ESHOP_CATEGORIES
                          where (item.CAT_POSITION == Position || item.CAT_POSITION == 4)
                          select item
                         ).OrderByDescending(p => p.CAT_PERIOD_ORDER);

            return View(modela);
        }
    }
}
