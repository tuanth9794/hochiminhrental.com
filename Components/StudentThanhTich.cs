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
    public class StudentThanhTich : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? Position)
        {
            var model = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE == Position && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_NAME = p.CAT_NAME,
                CAT_TYPE = p.CAT_TYPE,
                CAT_URL = p.CAT_URL,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_DESC_EN = p.CAT_DESC_EN,
            }).OrderByDescending(p => p.CAT_ID);

            return View(model);
        }
    }
}
