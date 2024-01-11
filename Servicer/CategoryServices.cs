using System;
using System.Collections.Generic;
using System.Text;
using CoreCnice.Connect;
using CoreCnice.Domain;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreCnice.Servicer
{
    public class CategoryServices
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        //public CategoryServices(BulSoftCmsConnectContext context)
        //{
        //  _context = context;
        //}


        public IList<ESHOP_CATEGORIES> CategoryList(int Cat_type)
        {
            try
            {
                var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE == Cat_type).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,

                });

                return Categorieslist.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Hiển thị danh sách menu ngoài
        /// </summary>
        /// <param name="Cat_type"></param>
        /// <returns></returns>
        public IList<ESHOP_CATEGORIES> CategoryListOut(int Cat_type)
        {
            try
            {
                var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != Cat_type).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,

                });

                return Categorieslist.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// hiển thị danh sách menu bên tay trái của admin
        /// </summary>
        /// <returns></returns>
        public IList<ESHOP_CATEGORIES> MenuAdmin()
        {
            var MenuAdmin = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == 0 && x.CAT_TYPE == 5).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_URL = p.CAT_URL,
                CAT_CODE = p.CAT_CODE,
            });

            return MenuAdmin.ToList();
        }

        /// <summary>
        /// Kiểm tra có thư mục con
        /// </summary>
        /// <param name="Cat_id"></param>
        /// <returns></returns>
        public int CountCatChild(int Cat_id)
        {
            var MenuAdmin = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == Cat_id && x.CAT_TYPE == 5).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_URL = p.CAT_URL,
            });
            return MenuAdmin.ToList().Count;
        }

        /// <summary>
        /// hiển thị danh sách menu bên tay trái của admin
        /// </summary>
        /// <returns></returns>
        public IList<ESHOP_CATEGORIES> MenuCatChild(int id)
        {
            var MenuAdmin = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == id).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_URL = p.CAT_URL,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
            }).OrderByDescending(x => x.CAT_PERIOD_ORDER);

            return MenuAdmin.ToList();
        }

        public async Task<IList<ESHOP_CATEGORIES>> MenuHome(int Position)
        {
            var MenuAdmin = await _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && (x.CAT_POSITION == Position || x.CAT_POSITION == 4) && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_URL = p.CAT_URL,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
            }).OrderBy(x => x.CAT_PERIOD_ORDER).Take(8).ToListAsync();


            return MenuAdmin.ToList();
        }

        /// <summary>
        /// hiển thị danh sách menu con
        /// </summary>
        /// <returns></returns>
        public IList<ESHOP_CATEGORIES> MenuCatCh(int id)
        {
            var MenuAdmin = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == id).OrderBy(x => x.CAT_PERIOD_ORDER).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_URL = p.CAT_URL,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN,
                CAT_PERIOD = p.CAT_PERIOD,
                CAT_IMAGE1 = p.CAT_IMAGE1
            });

            return MenuAdmin.ToList();
        }

        public IList<ESHOP_CATEGORIES> MenuCatChAD(int id)
        {
            var MenuAdmin = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == id).OrderBy(x => x.CAT_PERIOD).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_NAME_EN = p.CAT_NAME_EN,
                CAT_URL = p.CAT_URL,
                CAT_SEO_URL = p.CAT_SEO_URL,
                CAT_PERIOD = p.CAT_PERIOD,
                CAT_IMAGE1 = p.CAT_IMAGE1
            });

            return MenuAdmin.ToList();
        }
        public async Task<string> getCountNews(int id)
        {
            var ListCount = await _context.ESHOP_NEWS_CAT.Where(x => x.CAT_ID == id).ToListAsync();

            return ListCount.ToList().Count.ToString();
        }

        public async Task<string> getIconDrop(int id)
        {
            var ListCount = await _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id).ToListAsync();

            return ListCount.ToList().Count.ToString();
        }

        public async Task<int> GetProCon(int id)
        {
            var ListCount = await _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id).ToListAsync();

            return ListCount.ToList().Count;
        }
    }
}
