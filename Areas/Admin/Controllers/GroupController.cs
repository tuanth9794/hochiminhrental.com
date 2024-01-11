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
    public class GroupController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        // GET: Group
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Group/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_GROUP = await _context.ESHOP_GROUP
                .SingleOrDefaultAsync(m => m.GROUP_ID == id);
            if (eSHOP_GROUP == null)
            {
                return NotFound();
            }

            return View(eSHOP_GROUP);
        }
        public virtual ObjectResult GroupList()
        {
            try
            {
                var GroupList = _context.ESHOP_GROUP.Select(p => new ESHOP_GROUP
                {
                    GROUP_ID = p.GROUP_ID,
                    GROUP_NAME = p.GROUP_NAME,
                    GROUP_PUBLISHDATE = p.GROUP_PUBLISHDATE,
                    GROUP_CODE = p.GROUP_CODE,
                    GROUP_TYPE = p.GROUP_TYPE

                }).OrderByDescending(p => p.GROUP_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(GroupList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: Group/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GROUP_ID,GROUP_CODE,GROUP_NAME,GROUP_TYPE,GROUP_PUBLISHDATE")] ESHOP_GROUP eSHOP_GROUP)
        {
            if (ModelState.IsValid)
            {
                eSHOP_GROUP.GROUP_PUBLISHDATE = DateTime.Now;
                _context.Add(eSHOP_GROUP);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Admin/Group", new { id = eSHOP_GROUP.GROUP_ID });
                //return RedirectToAction(nameof(Index));
            }
            return View(eSHOP_GROUP);
        }

        // GET: Group/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_GROUP = await _context.ESHOP_GROUP.SingleOrDefaultAsync(m => m.GROUP_ID == id);
            if (eSHOP_GROUP == null)
            {
                return NotFound();
            }
            return View(eSHOP_GROUP);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GROUP_ID,GROUP_CODE,GROUP_NAME,GROUP_TYPE,GROUP_PUBLISHDATE")] ESHOP_GROUP eSHOP_GROUP)
        {
            if (id != eSHOP_GROUP.GROUP_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_GROUP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_GROUPExists(eSHOP_GROUP.GROUP_ID))
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
            return View(eSHOP_GROUP);
        }

        // GET: Group/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_GROUP = await _context.ESHOP_GROUP
                .SingleOrDefaultAsync(m => m.GROUP_ID == id);
            if (eSHOP_GROUP == null)
            {
                return NotFound();
            }

            return View(eSHOP_GROUP);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_GROUP = await _context.ESHOP_GROUP.SingleOrDefaultAsync(m => m.GROUP_ID == id);
            _context.ESHOP_GROUP.Remove(eSHOP_GROUP);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_GROUPExists(int id)
        {
            return _context.ESHOP_GROUP.Any(e => e.GROUP_ID == id);
        }

        public string DeleteGR(int id)
        {
            string result = "";
            var entity = _context.ESHOP_GROUP.FirstOrDefault(item => item.GROUP_ID == id);
            if (entity != null)
            {
                _context.ESHOP_GROUP.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
        }
    }
}
