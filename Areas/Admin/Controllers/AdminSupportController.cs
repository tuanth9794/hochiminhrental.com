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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminSupportController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        private readonly IHostingEnvironment he;

        public AdminSupportController(IHostingEnvironment e)
        {
            he = e;
        }

        // GET: AdminSupport
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_ONLINE.ToListAsync());
        }

        public virtual ObjectResult SupportList()
        {
            try
            {
                var SupportList = _context.ESHOP_ONLINE.Select(p => new ESHOP_ONLINE
                {
                    ONLINE_ID = p.ONLINE_ID,
                    ONLINE_DESC = p.ONLINE_DESC,
                    ONLINE_NICKNAME = p.ONLINE_NICKNAME,
                    ONLINE_TYPE = p.ONLINE_TYPE,
                    ONLINE_ORDER = p.ONLINE_ORDER,
                    ONLINE_IMAGE = p.ONLINE_IMAGE,
                    ONLINE_ACTIVE = p.ONLINE_ACTIVE
                }).OrderByDescending(p => p.ONLINE_ID);
                //  return Json(json);
                return new ObjectResult(SupportList);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        // GET: AdminSupport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ONLINE = await _context.ESHOP_ONLINE
                .SingleOrDefaultAsync(m => m.ONLINE_ID == id);
            if (eSHOP_ONLINE == null)
            {
                return NotFound();
            }

            return View(eSHOP_ONLINE);
        }

        // GET: AdminSupport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminSupport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ONLINE_ID,ONLINE_NICKNAME,ONLINE_DESC,ONLINE_ORDER,ONLINE_TYPE,CUS_ID,NEWS_ID,ONLINE_OUT_DATE,ONLINE_START_BEGIN_DATE,ONLINE_DESC_EN,ONLINE_IP,ONLINE_LOCATION,ONLINE_COMBACK,ONLINE_LINK,ONLINE_ACTIVE,ONLINE_GUID,ONLINE_IMAGE")] ESHOP_ONLINE eSHOP_ONLINE)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string path = String.Empty;
                    string pathfodel = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        eSHOP_ONLINE.ONLINE_IMAGE = fileName;
                    }
                }
                _context.Add(eSHOP_ONLINE);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string path = String.Empty;
                    string pathfodel = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Online/" + eSHOP_ONLINE.ONLINE_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Online/" + eSHOP_ONLINE.ONLINE_ID, fileName);
                        AppendToFile(pathfodel, path, file);
                    }
                }

                return RedirectToAction("Edit", "Admin/AdminSupport", new { id = eSHOP_ONLINE.ONLINE_ID });
            }
            return View(eSHOP_ONLINE);
        }

        // GET: AdminSupport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ONLINE = await _context.ESHOP_ONLINE.SingleOrDefaultAsync(m => m.ONLINE_ID == id);
            if (eSHOP_ONLINE == null)
            {
                return NotFound();
            }
            return View(eSHOP_ONLINE);
        }

        // POST: AdminSupport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ONLINE_ID,ONLINE_NICKNAME,ONLINE_DESC,ONLINE_ORDER,ONLINE_TYPE,CUS_ID,NEWS_ID,ONLINE_OUT_DATE,ONLINE_START_BEGIN_DATE,ONLINE_DESC_EN,ONLINE_IP,ONLINE_LOCATION,ONLINE_COMBACK,ONLINE_LINK,ONLINE_ACTIVE,ONLINE_GUID,ONLINE_IMAGE")] ESHOP_ONLINE eSHOP_ONLINE)
        {
            if (id != eSHOP_ONLINE.ONLINE_ID)
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
                            eSHOP_ONLINE.ONLINE_IMAGE = fileName;
                        }
                    }

                    _context.Update(eSHOP_ONLINE);
                    await _context.SaveChangesAsync();

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Online/" + eSHOP_ONLINE.ONLINE_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Online/" + eSHOP_ONLINE.ONLINE_ID, fileName);
                            AppendToFile(pathfodel, path, file);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_ONLINEExists(eSHOP_ONLINE.ONLINE_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(eSHOP_ONLINE);
        }

        // GET: AdminSupport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_ONLINE = await _context.ESHOP_ONLINE
                .SingleOrDefaultAsync(m => m.ONLINE_ID == id);
            if (eSHOP_ONLINE == null)
            {
                return NotFound();
            }

            return View(eSHOP_ONLINE);
        }

        // POST: AdminSupport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_ONLINE = await _context.ESHOP_ONLINE.SingleOrDefaultAsync(m => m.ONLINE_ID == id);
            _context.ESHOP_ONLINE.Remove(eSHOP_ONLINE);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_ONLINEExists(int id)
        {
            return _context.ESHOP_ONLINE.Any(e => e.ONLINE_ID == id);
        }

        public string DeleteSP(int id)
        {
            string result = "";
            var entity = _context.ESHOP_ONLINE.FirstOrDefault(item => item.ONLINE_ID == id);
            if (entity != null)
            {
                _context.ESHOP_ONLINE.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
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
                    if (System.IO.File.Exists(fullPath) == true)
                    {
                        System.IO.File.Delete(fullPath);
                    }
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
    }
}
