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
    public class FilterItem : ViewComponent
    {
        string SessionKeyName = "CustomerSesion3";
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IViewComponentResult Invoke(int? PropId)
        {

            int TypePrice = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                TypePrice = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }          

            var listNew = (from n in _context.ESHOP_PROPERTIES                          
                           where n.PROP_PARENT_ID == PropId && (n.PRO_TYPE == 0 || n.PRO_TYPE == TypePrice)
                           select n);
         
            var Group = listNew.GroupBy(x => x.PROP_ID).Select(g => g.First()).OrderBy(p => p.PRO_ORDER);

            return View(Group);
        }
    }
}
