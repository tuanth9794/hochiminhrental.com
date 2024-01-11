using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminOrderController : Controller
    {
        //private readonly BulSoftCmsConnectContext _context;

        //public AdminOrderController(BulSoftCmsConnectContext context)
        //{
        //    _context = context;
        //}
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        /// <summary>
        /// Danh sachs đơn hàng
        /// </summary>
        /// <returns></returns>
        public virtual ObjectResult OrderList()
        {
            try
            {
                var OrderList = _context.ESHOP_ORDER.Select(p => new ESHOP_ORDER
                {
                    ORDER_ID = p.ORDER_ID,
                    ORDER_CODE = p.ORDER_CODE,
                    ORDER_PUBLISHDATE = p.ORDER_PUBLISHDATE,
                    ORDER_STATUS = p.ORDER_STATUS,
                    ORDER_TOTAL_ALL = p.ORDER_TOTAL_ALL,
                    RECEIVER_FULLNAME = p.RECEIVER_FULLNAME,
                    RECEIVER_ADDRESS = p.RECEIVER_EMAIL,
                    RECEIVER_PHONE1 = p.RECEIVER_PHONE1,
                    RECEIVER_EMAIL = p.RECEIVER_EMAIL,
                    ORDER_UPDATE = p.ORDER_UPDATE,
                }).OrderByDescending(p => p.ORDER_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(OrderList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: AdminOrder
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_ORDER.ToListAsync());
        }

        // GET: AdminOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ORDER = await _context.ESHOP_ORDER
                .SingleOrDefaultAsync(m => m.ORDER_ID == id);
            if (eSHOP_ORDER == null)
            {
                return NotFound();
            }

            return View(eSHOP_ORDER);
        }

        // GET: AdminOrder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ORDER_ID,CUSTOMER_ID,ORDER_CODE,ORDER_TOTAL_ALL,ORDER_PUBLISHDATE,ORDER_UPDATE,RECEIVER_FULLNAME,RECEIVER_ADDRESS,RECEIVER_PHONE1,RECEIVER_EMAIL,ORDER_DESC,ORDER_STATUS,ORDER_DAY,ORDER_MONTH,ORDER_YEAR,ORDER_IP")] ESHOP_ORDER eSHOP_ORDER)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eSHOP_ORDER);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Order", new { id = eSHOP_ORDER.CUSTOMER_ID });
            }
            return View(eSHOP_ORDER);
        }

        // GET: AdminOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ORDER = await _context.ESHOP_ORDER.SingleOrDefaultAsync(m => m.ORDER_ID == id);
            if (eSHOP_ORDER == null)
            {
                return NotFound();
            }
            return View(eSHOP_ORDER);
        }

        // POST: AdminOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ORDER_ID,CUSTOMER_ID,ORDER_CODE,ORDER_TOTAL_ALL,ORDER_PUBLISHDATE,ORDER_UPDATE,RECEIVER_FULLNAME,RECEIVER_ADDRESS,RECEIVER_PHONE1,RECEIVER_EMAIL,ORDER_DESC,ORDER_STATUS,ORDER_DAY,ORDER_MONTH,ORDER_YEAR,ORDER_IP")] ESHOP_ORDER eSHOP_ORDER)
        {
            if (id != eSHOP_ORDER.ORDER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_ORDER);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_ORDERExists(eSHOP_ORDER.ORDER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eSHOP_ORDER);
        }

        // GET: AdminOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ORDER = await _context.ESHOP_ORDER
                .SingleOrDefaultAsync(m => m.ORDER_ID == id);
            if (eSHOP_ORDER == null)
            {
                return NotFound();
            }

            return View(eSHOP_ORDER);
        }

        // POST: AdminOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_ORDER = await _context.ESHOP_ORDER.SingleOrDefaultAsync(m => m.ORDER_ID == id);
            _context.ESHOP_ORDER.Remove(eSHOP_ORDER);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_ORDERExists(int id)
        {
            return _context.ESHOP_ORDER.Any(e => e.ORDER_ID == id);
        }

        /// <summary>
        /// Danh sachs đơn hàng
        /// </summary>
        /// <returns></returns>
        public virtual ObjectResult OrderItemList(int id)
        {
            try
            {
                //var OrderItemList = _context.ESHOP_ORDER_ITEM.Where(x => x.ORDER_ID == id).Select(p => new ESHOP_ORDER_ITEM
                //{
                //    ORDER_ID = p.ORDER_ID,
                //    ITEM_ID = p.ITEM_ID,
                //    ITEM_PUBLISDATE = DateTime.Parse(p.ITEM_PUBLISDATE.ToString()),
                //    ITEM_QUANTITY = float.Parse(p.ITEM_QUANTITY.ToString()),
                //    ITEM_PRICE = decimal.Parse(p.ITEM_PRICE.ToString()),
                //    ITEM_SUBTOTAL = (decimal.Parse(p.ITEM_QUANTITY.ToString()) * decimal.Parse(p.ITEM_PRICE.ToString())),
                //    NEWS_ID = p.NEWS_ID,
                //}).OrderByDescending(p => p.ITEM_PUBLISDATE);



                var OrderItemListJoin = (from ord in _context.ESHOP_ORDER
                                         join orditem in _context.ESHOP_ORDER_ITEM
                                         on ord.ORDER_ID equals orditem.ORDER_ID
                                         join news in _context.ESHOP_NEWS
                                         on orditem.NEWS_ID equals news.NEWS_ID
                                         where ord.ORDER_ID == id
                                         select new
                                         {
                                             ord.ORDER_ID,
                                             orditem.ITEM_ID,
                                             orditem.ITEM_PUBLISDATE,
                                             ITEM_QUANTITY = float.Parse(orditem.ITEM_QUANTITY.ToString()),
                                             ITEM_PRICE = decimal.Parse(orditem.ITEM_PRICE.ToString()),
                                             ITEM_SUBTOTAL = (decimal.Parse(orditem.ITEM_QUANTITY.ToString()) * decimal.Parse(orditem.ITEM_PRICE.ToString())),
                                             news.NEWS_TITLE,
                                         });
                //  return Json(json);
                return new ObjectResult(OrderItemListJoin);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
