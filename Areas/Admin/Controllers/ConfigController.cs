using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ConfigController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        private readonly IHostingEnvironment he;

        public ConfigController(IHostingEnvironment e)
        {
            he = e;
        }
        // GET: Config
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_CONFIG.ToListAsync());
        }

        // GET: Config/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONFIG = await _context.ESHOP_CONFIG
                .SingleOrDefaultAsync(m => m.CONFIG_ID == id);
            if (eSHOP_CONFIG == null)
            {
                return NotFound();
            }

            return View(eSHOP_CONFIG);
        }

        // GET: Config/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Config/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CONFIG_ID,CONFIG_TITLE,CONFIG_KEYWORD,CONFIG_DESCRIPTION,CONFIG_HITCOUNTER,CONFIG_FAVICON,CONFIG_ORDER,CONFIG_LANGUAGE,CONFIG_EMAIL,CONFIG_SMTP,CONFIG_PORT,CONFIG_PASSWORD,CONFIG_NAME,CONFIG_ADD,CONFIG_FACEBOOK,CONFIG_GOOGLE,AD_ITEM_LANGUAGE,AD_ITEM_FIELD1,AD_ITEM_FIELD2,CONFIG_NAME_US,CONFIG_DESCRIPTION_EN,CONFIG_FIELD1,CONFIG_FIELD3,CONFIG_FIELD2")] ESHOP_CONFIG eSHOP_CONFIG)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eSHOP_CONFIG);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Config", new { id = eSHOP_CONFIG.CONFIG_ID });
            }
            return View(eSHOP_CONFIG);
        }

        // GET: Config/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            id = 1;
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
            if (eSHOP_CONFIG == null)
            {
                return NotFound();
            }
            return View(eSHOP_CONFIG);
        }

        // POST: Config/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CONFIG_ID,CONFIG_TITLE,CONFIG_KEYWORD,CONFIG_DESCRIPTION,CONFIG_HITCOUNTER,CONFIG_FAVICON,CONFIG_ORDER,CONFIG_LANGUAGE,CONFIG_EMAIL,CONFIG_SMTP,CONFIG_PORT,CONFIG_PASSWORD,CONFIG_NAME,CONFIG_ADD,CONFIG_FACEBOOK,CONFIG_GOOGLE,AD_ITEM_LANGUAGE,AD_ITEM_FIELD1,AD_ITEM_FIELD2,CONFIG_NAME_US,CONFIG_FOOTER,CONFIG_DESCRIPTION_EN,CONFIG_FIELD1,CONFIG_FIELD3,CONFIG_FIELD2,CONFIG_FIELD4,CONFIG_FIELD5")] ESHOP_CONFIG eSHOP_CONFIG)
        {
            id = 1;
            if (id != eSHOP_CONFIG.CONFIG_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CONFIG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CONFIGExists(eSHOP_CONFIG.CONFIG_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_CONFIG);
            }
            return View(eSHOP_CONFIG);
        }

        // GET: Config/Edit/5
        public async Task<IActionResult> MetaSeo(int? id)
        {
            id = 1;
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
            if (eSHOP_CONFIG == null)
            {
                return NotFound();
            }
            return View(eSHOP_CONFIG);
        }

        // POST: Config/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MetaSeo(int id, [Bind("CONFIG_ID,CONFIG_TITLE,CONFIG_KEYWORD,CONFIG_DESCRIPTION,CONFIG_HITCOUNTER,CONFIG_FAVICON,CONFIG_ORDER,CONFIG_LANGUAGE,CONFIG_EMAIL,CONFIG_SMTP,CONFIG_PORT,CONFIG_PASSWORD,CONFIG_NAME,CONFIG_ADD,CONFIG_FACEBOOK,CONFIG_GOOGLE,AD_ITEM_LANGUAGE,AD_ITEM_FIELD1,AD_ITEM_FIELD2,CONFIG_NAME_US,CONFIG_FOOTER,CONFIG_DESCRIPTION_EN,CONFIG_FIELD1,CONFIG_FIELD3,CONFIG_FIELD2,CONFIG_FIELD4,CONFIG_FIELD5")] ESHOP_CONFIG eSHOP_CONFIG)
        {
            id = 1;
            if (id != eSHOP_CONFIG.CONFIG_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data");
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data", fileName);
                            AppendToFile(pathfodel, path, file);
                            eSHOP_CONFIG.CONFIG_FAVICON = fileName;
                        }                   
                    }
                    _context.Update(eSHOP_CONFIG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CONFIGExists(eSHOP_CONFIG.CONFIG_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_CONFIG);
            }
            return View(eSHOP_CONFIG);
        }


        // GET: Config/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CONFIG = await _context.ESHOP_CONFIG
                .SingleOrDefaultAsync(m => m.CONFIG_ID == id);
            if (eSHOP_CONFIG == null)
            {
                return NotFound();
            }

            return View(eSHOP_CONFIG);
        }

        // POST: Config/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);
            _context.ESHOP_CONFIG.Remove(eSHOP_CONFIG);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_CONFIGExists(int id)
        {
            return _context.ESHOP_CONFIG.Any(e => e.CONFIG_ID == id);
        }

        public void AppendToFile(string pathfodel, string fullPath, IFormFile content)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(pathfodel);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(pathfodel);

                }
                else
                {

                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    content.CopyTo(stream);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        // GET: Config
        public IActionResult ConfigEmail()
        {
            return View();
        }

        //GET : Categories Chức năng menu
        public virtual ObjectResult EmailConfig()
        {
            try
            {
                var EmailList = _context.ESHOP_EMAIL.Select(p => new ESHOP_EMAIL
                {
                    EMAIL_ID = p.EMAIL_ID,
                    EMAIL_DESC = p.EMAIL_DESC,
                    EMAIL_CC = p.EMAIL_CC,
                    EMAIL_FROM = p.EMAIL_FROM,
                    EMAIL_TO = p.EMAIL_TO,
                    EMAIL_PUBLISHDATE = p.EMAIL_PUBLISHDATE,
                }).OrderByDescending(p => p.EMAIL_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(EmailList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Config/Edit/5
        public async Task<IActionResult> EditEmail(int? id)
        {            
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_EMAIL = await _context.ESHOP_EMAIL.SingleOrDefaultAsync(m => m.EMAIL_ID == id);
            if (eSHOP_EMAIL == null)
            {
                return NotFound();
            }
            return View(eSHOP_EMAIL);
        }

        // POST: Config/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmail(int id, [Bind("EMAIL_ID,EMAIL_STT,EMAIL_DESC,EMAIL_FROM,EMAIL_TO,EMAIL_CC,EMAIL_BCC,EMAIL_PUBLISHDATE_DAY,EMAIL_CUSTOMER_ALL,EMAIL_PUBLISHDATE,EMAIL_PUBLISHDATE_START,USER_ID")] ESHOP_EMAIL eSHOP_EMAIL)
        {          
            if (id != eSHOP_EMAIL.EMAIL_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_EMAIL);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CONFIGExists(eSHOP_EMAIL.EMAIL_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_EMAIL);
                //return RedirectToAction(nameof(ConfigEmail));
            }
            return View(eSHOP_EMAIL);
        }
    }
}
