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
    public class AdminContactController : Controller
    {
        //private readonly BulSoftCmsConnectContext _context;

        //public AdminContactController(BulSoftCmsConnectContext context)
        //{
        //    _context = context;
        //}

        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public virtual ObjectResult ContactList()
        {
            try
            {
                var ContactList = _context.ESHOP_CONTACT.Select(p => new ESHOP_CONTACT
                {
                    CONTACT_ID = p.CONTACT_ID,
                    CONTACT_NAME = p.CONTACT_NAME,
                    CONTACT_CONTENT = p.CONTACT_CONTENT,
                    CONTACT_EMAIL = p.CONTACT_EMAIL,
                    CONTACT_PHONE = p.CONTACT_PHONE,
                    CONTACT_PUBLISHDATE = p.CONTACT_PUBLISHDATE,
                    CONTACT_TITLE = p.CONTACT_TITLE,

                }).OrderByDescending(p => p.CONTACT_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(ContactList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: AdminContact
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: AdminContact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONTACT = await _context.ESHOP_CONTACT
                .SingleOrDefaultAsync(m => m.CONTACT_ID == id);
            if (eSHOP_CONTACT == null)
            {
                return NotFound();
            }

            return View(eSHOP_CONTACT);
        }

        // GET: AdminContact/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminContact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CONTACT_ID,CONTACT_NAME,CONTACT_EMAIL,CONTACT_PHONE,CONTACT_ADDRESS,CONTACT_TITLE,CONTACT_CONTENT,CONTACT_PUBLISHDATE,CONTACT_ANSWER,CONTACT_SHOWTYPE,CONTACT_TYPE")] ESHOP_CONTACT eSHOP_CONTACT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eSHOP_CONTACT);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Contact", new { id = eSHOP_CONTACT.CONTACT_ID });
            }
            return View(eSHOP_CONTACT);
        }

        // GET: AdminContact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONTACT = await _context.ESHOP_CONTACT.SingleOrDefaultAsync(m => m.CONTACT_ID == id);
            if (eSHOP_CONTACT == null)
            {
                return NotFound();
            }
            return View(eSHOP_CONTACT);
        }

        // POST: AdminContact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CONTACT_ID,CONTACT_NAME,CONTACT_EMAIL,CONTACT_PHONE,CONTACT_ADDRESS,CONTACT_TITLE,CONTACT_CONTENT,CONTACT_PUBLISHDATE,CONTACT_ANSWER,CONTACT_SHOWTYPE,CONTACT_TYPE")] ESHOP_CONTACT eSHOP_CONTACT)
        {
            if (id != eSHOP_CONTACT.CONTACT_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CONTACT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CONTACTExists(eSHOP_CONTACT.CONTACT_ID))
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
            return View(eSHOP_CONTACT);
        }

        // GET: AdminContact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONTACT = await _context.ESHOP_CONTACT
                .SingleOrDefaultAsync(m => m.CONTACT_ID == id);
            if (eSHOP_CONTACT == null)
            {
                return NotFound();
            }

            return View(eSHOP_CONTACT);
        }

        // POST: AdminContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_CONTACT = await _context.ESHOP_CONTACT.SingleOrDefaultAsync(m => m.CONTACT_ID == id);
            _context.ESHOP_CONTACT.Remove(eSHOP_CONTACT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_CONTACTExists(int id)
        {
            return _context.ESHOP_CONTACT.Any(e => e.CONTACT_ID == id);
        }

        public string DeleteContact(int id)
        {
            string result = "";
            var entity = _context.ESHOP_CONTACT.FirstOrDefault(item => item.CONTACT_ID == id);
            if (entity != null)
            {
                _context.ESHOP_CONTACT.Remove(entity);
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
