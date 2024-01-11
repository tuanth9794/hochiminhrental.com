using System;
using System.Collections.Generic;
using System.Text;
using CoreCnice.Connect;
using CoreCnice.Domain;
using System.Linq;

namespace CoreCnice.Servicer
{
    
    public class StoreServices
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public IList<ESHOP_STORE> StoreList(int store_active)
        {
            try
            {
                var _Storeslist = _context.ESHOP_STORE.Where(x => x.ESHOP_STORE_ACTIVE == store_active);

                return _Storeslist.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
