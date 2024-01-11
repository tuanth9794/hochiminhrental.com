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
    public class NewsHomeSlider : ViewComponent
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
                CAT_IMAGE1 = p.CAT_IMAGE1,
                CAT_IMAGE2 = p.CAT_IMAGE2,
                CAT_URL = p.CAT_URL, 
            }).OrderByDescending(p => p.CAT_ID);

            return View(model);
        }
    }
}
