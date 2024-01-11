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
    public class FilterCat : ViewComponent
    {

        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int Cat)
        {

            //var listNew = _context.ESHOP_PROPERTIES.Where(x => x.PROP_ACTIVE == Cat && x.PROP_PARENT_ID == 0);

            var listNew = (from catd in _context.ESHOP_CATEGORIES
                           join catPro in _context.ESHOP_CAT_PRO on catd.CAT_ID equals catPro.CAT_ID
                           join Pro in _context.ESHOP_PROPERTIES on catPro.PROP_ID equals Pro.PROP_ID
                           where catPro.CAT_ID == Cat
                           select Pro);

            var Group = listNew.GroupBy(x => x.PROP_ID).Select(g => g.First()).OrderBy(p => p.PRO_ORDER);

            return View(Group);
        }
    }
}
