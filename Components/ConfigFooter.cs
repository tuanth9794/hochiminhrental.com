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
    public class ConfigFooter : ViewComponent
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke()
        {
            var model = _context.ESHOP_CONFIG.Select(p => new ESHOP_CONFIG
            {
                CONFIG_ID = p.CONFIG_ID,
                CONFIG_FOOTER = p.CONFIG_FOOTER,
                CONFIG_FACEBOOK = p.CONFIG_FACEBOOK,
                CONFIG_GOOGLE = p.CONFIG_GOOGLE,
                CONFIG_ADD = p.CONFIG_ADD, 
            }).Take(1);

            return View(model);
        }
    }
}
