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
    public class FilterCatHome : ViewComponent
    {

        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int Cat)
        {

            //var listNew = _context.ESHOP_PROPERTIES.Where(x => x.PROP_ACTIVE == Cat && x.PROP_PARENT_ID == 0);

            var listNew = _context.ESHOP_PROPERTIES.Where(x => x.PRO_TYPE_HOME == Cat);               

            var Group = listNew.GroupBy(x => x.PROP_ID).Select(g => g.First()).OrderBy(p => p.PRO_ORDER);

            return View(Group);
        }
    }
}
