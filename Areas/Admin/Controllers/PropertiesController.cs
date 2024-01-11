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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PropertiesController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        private readonly IHostingEnvironment he;

        public PropertiesController(IHostingEnvironment e)
        {
            he = e;
        }

        public virtual ObjectResult PropertiesList(int id)
        {
            try
            {
                if (id == 0)
                {
                    var PropertiesList = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id && x.PROP_ID != 1 && x.PROP_ACTIVE == 1).Select(p => new ESHOP_PROPERTIES
                    {
                        PROP_ID = p.PROP_ID,
                        PROP_DESC = p.PROP_DESC,
                        PROP_NAME = p.PROP_NAME,
                        PROP_ACTIVE = p.PROP_ACTIVE,
                    }).OrderBy(p => p.PROP_ID);
                    return new ObjectResult(PropertiesList);
                }
                else
                {
                    var PropertiesList = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
                    {
                        PROP_ID = p.PROP_ID,
                        PROP_DESC = p.PROP_DESC,
                        PROP_NAME = p.PROP_NAME,
                        PROP_ACTIVE = p.PROP_ACTIVE,
                    }).OrderBy(p => p.PROP_ID);
                    return new ObjectResult(PropertiesList);
                }
                //  return Json(json);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual ObjectResult PropertiesListFill(int id)
        {
            try
            {
                if (id == 0)
                {
                    var PropertiesList = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id && x.PROP_ID != 1 && x.PROP_ACTIVE == 2).Select(p => new ESHOP_PROPERTIES
                    {
                        PROP_ID = p.PROP_ID,
                        PROP_DESC = p.PROP_DESC,
                        PROP_NAME = p.PROP_NAME,
                        PROP_ACTIVE = p.PROP_ACTIVE,
                    }).OrderBy(p => p.PROP_ID);
                    return new ObjectResult(PropertiesList);
                }
                else
                {
                    var PropertiesList = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == id && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
                    {
                        PROP_ID = p.PROP_ID,
                        PROP_DESC = p.PROP_DESC,
                        PROP_NAME = p.PROP_NAME,
                        PROP_ACTIVE = p.PROP_ACTIVE,
                    }).OrderBy(p => p.PROP_ID);
                    return new ObjectResult(PropertiesList);
                }
                //  return Json(json);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Properties
        public IActionResult Index()
        {
            return View();
        }

        // GET: Properties
        public IActionResult IndexFilter()
        {
            return View();
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_PROPERTIES = await _context.ESHOP_PROPERTIES
                .SingleOrDefaultAsync(m => m.PROP_ID == id);
            if (eSHOP_PROPERTIES == null)
            {
                return NotFound();
            }

            return View(eSHOP_PROPERTIES);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            List<ESHOP_PROPERTIES> cat_pr = new List<ESHOP_PROPERTIES>();

            cat_pr = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == 0 && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
            {
                PROP_ID = p.PROP_ID,
                PROP_NAME = p.PROP_NAME,

            }).OrderByDescending(p => p.PROP_ID).ToList();

            cat_pr.Insert(0, new ESHOP_PROPERTIES { PROP_ID = 0, PROP_NAME = "Lựa chọn thuộc tính cha" });
            ViewBag.ListCatPr = cat_pr;
            ESHOP_PROPERTIES pr = new ESHOP_PROPERTIES();
            pr.PRO_ORDER = 1;
            pr.PROP_ACTIVE = 0;
            pr.PRO_TYPE = 0;
            return View(pr);
        }

        // POST: Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PROP_ID,PROP_NAME,PROP_DESC,PROP_PARENT_ID,PROP_ACTIVE,PROP_RANK,PRO_IMAGES,PRO_NAME_EN,PRO_TYPE,PRO_ORDER,PRO_TYPE_HOME")] ESHOP_PROPERTIES eSHOP_PROPERTIES)
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
                        eSHOP_PROPERTIES.PRO_IMAGES = fileName;
                    }
                }

                _context.Add(eSHOP_PROPERTIES);
                await _context.SaveChangesAsync();
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string path = String.Empty;
                    string pathfodel = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Properties/" + eSHOP_PROPERTIES.PROP_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Properties/" + eSHOP_PROPERTIES.PROP_ID, fileName);
                        AppendToFile(pathfodel, path, file);
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Properties", new { id = eSHOP_PROPERTIES.PROP_ID });
            }
            //return RedirectToAction("Edit", "Admin/Properties", new { id = eSHOP_PROPERTIES.PROP_ID });
            return View(eSHOP_PROPERTIES);
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
        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<ESHOP_PROPERTIES> cat_pr = new List<ESHOP_PROPERTIES>();

            cat_pr = _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != id && x.PROP_PARENT_ID == 0 && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
            {
                PROP_ID = p.PROP_ID,
                PROP_NAME = p.PROP_NAME,

            }).OrderByDescending(p => p.PROP_ID).ToList();

            cat_pr.Insert(0, new ESHOP_PROPERTIES { PROP_ID = 0, PROP_NAME = "Lựa chọn thuộc tính cha" });
            var eSHOP_PROPERTIES = await _context.ESHOP_PROPERTIES.SingleOrDefaultAsync(m => m.PROP_ID == id);
            if (eSHOP_PROPERTIES == null)
            {
                return NotFound();
            }
            ViewBag.ListCatPr = cat_pr;
            return View(eSHOP_PROPERTIES);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PROP_ID,PROP_NAME,PROP_DESC,PROP_PARENT_ID,PROP_ACTIVE,PROP_RANK,PRO_IMAGES,PRO_NAME_EN,PRO_TYPE,PRO_ORDER,PRO_TYPE_HOME")] ESHOP_PROPERTIES eSHOP_PROPERTIES)
        {
            if (id != eSHOP_PROPERTIES.PROP_ID)
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
                            eSHOP_PROPERTIES.PRO_IMAGES = fileName;
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Properties/" + eSHOP_PROPERTIES.PROP_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Properties/" + eSHOP_PROPERTIES.PROP_ID, fileName);
                            AppendToFile(pathfodel, path, file);
                        }
                    }

                    _context.Update(eSHOP_PROPERTIES);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_PROPERTIESExists(eSHOP_PROPERTIES.PROP_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            List<ESHOP_PROPERTIES> cat_pr = new List<ESHOP_PROPERTIES>();

            cat_pr = _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != id && x.PROP_PARENT_ID == 0 && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
            {
                PROP_ID = p.PROP_ID,
                PROP_NAME = p.PROP_NAME,

            }).OrderByDescending(p => p.PROP_ID).ToList();

            cat_pr.Insert(0, new ESHOP_PROPERTIES { PROP_ID = 0, PROP_NAME = "Lựa chọn thuộc tính cha" });
            ViewBag.ListCatPr = cat_pr;
            return View(eSHOP_PROPERTIES);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_PROPERTIES = await _context.ESHOP_PROPERTIES
                .SingleOrDefaultAsync(m => m.PROP_ID == id);
            if (eSHOP_PROPERTIES == null)
            {
                return NotFound();
            }

            return View(eSHOP_PROPERTIES);
        }

        // POST: Properties/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idPro)
        {
            var eshop_List = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == idPro);
            if (eshop_List.ToList().Count > 0)
            {
                ViewBag.Error = "Không thể xóa vì có chưa chuyên mục con";
                //return RedirectToAction("Edit", "Admin/Properties", new { id = idPro });
                return Redirect("/Admin/Properties/Edit/" + idPro);

            }
            else
            {
                var eSHOP_PROPERTIES = await _context.ESHOP_PROPERTIES.SingleOrDefaultAsync(m => m.PROP_ID == idPro);
                _context.ESHOP_PROPERTIES.Remove(eSHOP_PROPERTIES);
                await _context.SaveChangesAsync();
                return Redirect("/Admin/Properties");
            }
        }

        private bool ESHOP_PROPERTIESExists(decimal id)
        {
            return _context.ESHOP_PROPERTIES.Any(e => e.PROP_ID == id);
        }

        public string DeletePro(int id)
        {
            string result = "";
            var entity = _context.ESHOP_PROPERTIES.FirstOrDefault(item => item.PROP_ID == id);
            if (entity != null)
            {
                _context.ESHOP_PROPERTIES.Remove(entity);
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
