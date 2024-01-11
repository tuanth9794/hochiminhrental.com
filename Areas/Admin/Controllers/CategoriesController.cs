using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using CoreCnice.Servicer;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using CoreCnice.UtilsCs;

namespace CoreCnice.Areas.Admin.Controllers
{

    //[Authorize("MyCookieAuthenticationScheme")]
    [Authorize]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        //private readonly BulSoftCmsConnectContext _context;

        //public CategoriesController(BulSoftCmsConnectContext context)
        //{
        //    _context = context;
        //}
        private readonly IHostingEnvironment he;

        public CategoriesController(IHostingEnvironment e)
        {
            he = e;
        }
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        private readonly CategoryServices _categoriesSevercie = new CategoryServices();
        //List Này
        public virtual IActionResult List()
        {
            return View();
        }

        //GET : Categories Chức năng menu
        public virtual ObjectResult CategoriesList()
        {
            try
            {
                var countries = _categoriesSevercie.CategoryList(5);
                //  return Json(json);
                return new ObjectResult(countries);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //GET : Categories Menu ngoài
        public virtual ObjectResult CategoriesListOut()
        {
            try
            {
                var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,
                    CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                });

                List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

                foreach (var item in Categorieslist.ToList())
                {
                    catList.Add(item);
                    var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID,
                        CAT_PARENT_ID = p.CAT_PARENT_ID,
                        CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                    });

                    foreach (var itemLoad in listChild.ToList())
                    {
                        catList.Add(itemLoad);
                        var listChild_Load = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == itemLoad.CAT_ID).Select(p => new ESHOP_CATEGORIES
                        {
                            CAT_NAME = "-----------------------" + p.CAT_NAME,
                            CAT_ID = p.CAT_ID,
                            CAT_PARENT_ID = p.CAT_PARENT_ID,
                            CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                        });
                        catList.AddRange(listChild_Load);
                    }
                }
                //  return Json(json);
                return new ObjectResult(catList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View();
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var catPr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != id).OrderByDescending(p => p.CAT_ID);

            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES
                .SingleOrDefaultAsync(m => m.CAT_ID == id);

            if (eSHOP_CATEGORIES == null)
            {
                return NotFound();
            }

            //eSHOP_CATEGORIES.escat_pr =  catPr.ToList();

            return View(eSHOP_CATEGORIES);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();

            cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_NAME = p.CAT_NAME,

            }).OrderByDescending(p => p.CAT_ID).ToList();
            cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });
            ViewBag.ListCatPr = cat_pr;
            ESHOP_CATEGORIES es_cs = new ESHOP_CATEGORIES();
            es_cs.CAT_TYPE = 1;
            es_cs.CAT_SHOWHOME = 0;
            es_cs.CAT_POSITION = 3;
            es_cs.CAT_SHOWFOOTER = 0;
            es_cs.CAT_STATUS = 1;
            es_cs.CAT_PERIOD = 1;
            es_cs.CAT_PERIOD_ORDER = 1;
            return View(es_cs);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CAT_ID,CAT_NAME,CAT_CODE,CAT_DESC,CAT_URL,CAT_TARGET,CAT_STATUS,CAT_PARENT_ID,CAT_PAGEITEM,CAT_PERIOD,CAT_SEO_TITLE,CAT_SEO_DESC,CAT_SEO_KEYWORD,CAT_SEO_URL,CAT_SHOWHOME,CAT_TYPE,CAT_SHOWFOOTER,CAT_POSITION,CAT_IMAGE1,CAT_IMAGE2,CAT_PERIOD_ORDER,CAT_DESC_EN,CAT_SEO_TITLE_EN,CAT_NAME_EN,CAT_SEO_URL_EN")] ESHOP_CATEGORIES eSHOP_CATEGORIES)
        {
            if (ModelState.IsValid)
            {
                var listnews = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == eSHOP_CATEGORIES.CAT_SEO_URL);
                if (listnews.ToList().Count > 0)
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages");
                            path = Path.Combine(he.WebRootPath, "UploadImages", fileName);
                            AppendToFile(pathfodel, path, file);
                            eSHOP_CATEGORIES.CAT_IMAGE1 = fileName;
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages");
                            path = Path.Combine(he.WebRootPath, "UploadImages", fileName1);
                            AppendToFile(pathfodel, path, file2);
                            eSHOP_CATEGORIES.CAT_IMAGE2 = fileName1;
                        }
                    }
                    _context.Add(eSHOP_CATEGORIES);
                    await _context.SaveChangesAsync();

                    List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();

                    cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE == 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_ID = p.CAT_ID,
                        CAT_NAME = p.CAT_NAME,

                    }).OrderByDescending(p => p.CAT_ID).ToList();

                    cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });

                    ViewBag.ListCatPr = cat_pr;

                    //return RedirectToAction(nameof(Index));
                    ViewBag.Error = "Cập nhật thành công";
                }
                else
                {
                    ViewBag.Error = "Chuyên mục trùng tên";
                }
                //return View(eSHOP_CATEGORIES);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Categories", new { id = eSHOP_CATEGORIES.CAT_ID });
            }
            return View(eSHOP_CATEGORIES);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();

            cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != id && x.CAT_TYPE != 5).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_NAME = p.CAT_NAME,

            }).OrderByDescending(p => p.CAT_ID).ToList();

            cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });

            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);

            if (eSHOP_CATEGORIES == null)
            {
                return NotFound();
            }

            ViewBag.ListCatPr = cat_pr;
            return View(eSHOP_CATEGORIES);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CAT_ID,CAT_NAME,CAT_CODE,CAT_DESC,CAT_URL,CAT_TARGET,CAT_STATUS,CAT_PARENT_ID,CAT_PAGEITEM,CAT_PERIOD,CAT_SEO_TITLE,CAT_SEO_DESC,CAT_SEO_KEYWORD,CAT_SEO_URL,CAT_SHOWHOME,CAT_TYPE,CAT_SHOWFOOTER,CAT_POSITION,CAT_IMAGE1,CAT_IMAGE2,CAT_DESC_EN,CAT_PERIOD_ORDER,CAT_SEO_TITLE_EN,CAT_NAME_EN,CAT_SEO_URL_EN")] ESHOP_CATEGORIES eSHOP_CATEGORIES)
        {
            if (id != eSHOP_CATEGORIES.CAT_ID)
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
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/");
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/", fileName);
                            AppendToFile(pathfodel, path, file);
                            eSHOP_CATEGORIES.CAT_IMAGE1 = fileName;
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/");
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/", fileName1);
                            AppendToFile(pathfodel, path, file2);
                            eSHOP_CATEGORIES.CAT_IMAGE2 = fileName1;
                        }
                    }
                    _context.Update(eSHOP_CATEGORIES);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CATEGORIESExists(eSHOP_CATEGORIES.CAT_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();

                cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != id && x.CAT_TYPE != 5).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_ID = p.CAT_ID,
                    CAT_NAME = p.CAT_NAME,

                }).OrderByDescending(p => p.CAT_ID).ToList();

                cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });

                ViewBag.ListCatPr = cat_pr;
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_CATEGORIES);
            }
            return View(eSHOP_CATEGORIES);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES
                .SingleOrDefaultAsync(m => m.CAT_ID == id);
            if (eSHOP_CATEGORIES == null)
            {
                return NotFound();
            }

            return View(eSHOP_CATEGORIES);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);
            _context.ESHOP_CATEGORIES.Remove(eSHOP_CATEGORIES);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Admin/Categories");
        }



        private bool ESHOP_CATEGORIESExists(int id)
        {
            return _context.ESHOP_CATEGORIES.Any(e => e.CAT_ID == id);
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

        #region cat

        // GET: Categories/Create
        public IActionResult CreateCat()
        {
            var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID
            });

            List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

            foreach (var item in Categorieslist.ToList())
            {
                catList.Add(item);
                var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = "------------" + p.CAT_NAME,
                    CAT_ID = p.CAT_ID
                });
                catList.AddRange(listChild);
            }

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });
            ViewBag.ListCatPr = catList;

            ESHOP_CATEGORIES es_cs = new ESHOP_CATEGORIES();
            es_cs.CAT_TYPE = 1;
            es_cs.CAT_SHOWHOME = 0;
            es_cs.CAT_POSITION = 3;
            es_cs.CAT_SHOWFOOTER = 0;
            es_cs.CAT_STATUS = 1;
            es_cs.CAT_PERIOD = 1;
            es_cs.CAT_PERIOD_ORDER = 1;
            es_cs.CAT_FIELD2 = "<meta name='robots' content='noindex'>";
            return View(es_cs);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCat([Bind("CAT_ID,CAT_NAME,CAT_CODE,CAT_DESC,CAT_URL,CAT_TARGET,CAT_STATUS,CAT_PARENT_ID,CAT_PAGEITEM,CAT_PERIOD,CAT_SEO_TITLE,CAT_SEO_DESC,CAT_SEO_KEYWORD,CAT_SEO_URL,CAT_SHOWHOME,CAT_TYPE,CAT_SHOWFOOTER,CAT_POSITION,CAT_IMAGE1,CAT_IMAGE2,CAT_PERIOD_ORDER,CAT_DESC_EN,CAT_FIELD1,CAT_SEO_TITLE_EN,CAT_FIELD2,CAT_NAME_JS,CAT_NAME_EN,CAT_DESC_JS,CAT_SEO_URL_EN,CAT_SEO_META_DESC_EN,CAT_SEO_META_CANONICAL,CAT_LIENKET_EN")] ESHOP_CATEGORIES eSHOP_CATEGORIES)
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
                        eSHOP_CATEGORIES.CAT_IMAGE1 = fileName;
                    }

                    var file2 = Request.Form.Files[1];

                    if (file2 != null && file2.FileName.Length > 0)
                    {
                        var fileName1 = Path.GetFileName(file2.FileName);
                        eSHOP_CATEGORIES.CAT_IMAGE2 = fileName1;
                    }

                    var file3 = Request.Form.Files[2];

                    if (file3 != null && file3.FileName.Length > 0)
                    {
                        var fileName2 = Path.GetFileName(file3.FileName);
                        eSHOP_CATEGORIES.CAT_IMAGE3 = fileName2;
                    }
                }

                _context.Add(eSHOP_CATEGORIES);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string path = String.Empty;
                    string pathfodel = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName);
                        AppendToFile(pathfodel, path, file);

                    }

                    var file2 = Request.Form.Files[1];

                    if (file2 != null && file2.FileName.Length > 0)
                    {
                        var fileName1 = Path.GetFileName(file2.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName1);
                        AppendToFile(pathfodel, path, file2);

                    }

                    var file3 = Request.Form.Files[2];

                    if (file3 != null && file3.FileName.Length > 0)
                    {
                        var fileName2 = Path.GetFileName(file3.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName2);
                        AppendToFile(pathfodel, path, file3);

                    }
                }
                ViewBag.Error = "Cập nhật thành công";
                //return View(eSHOP_CATEGORIES);
                //return RedirectToAction("EditCat", new { id = eSHOP_CATEGORIES.CAT_ID, Area = "Admin" });
                return RedirectToAction("EditCat", "Admin/Categories", new { id = eSHOP_CATEGORIES.CAT_ID });
                //return RedirectToAction("/Admin/Categories/EditCat/" + eSHOP_CATEGORIES.CAT_ID);
            }
            return View(eSHOP_CATEGORIES);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> EditCat(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();


            cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_ID != 65).Select(p => new ESHOP_CATEGORIES
            {
                CAT_ID = p.CAT_ID,
                CAT_NAME = p.CAT_NAME,

            }).OrderByDescending(p => p.CAT_ID).ToList();

            cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });

            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);

            if (eSHOP_CATEGORIES == null)
            {
                return NotFound();
            }

            ViewBag.ListCatPr = cat_pr;
            return View(eSHOP_CATEGORIES);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCat(int id, [Bind("CAT_ID,CAT_NAME,CAT_CODE,CAT_DESC,CAT_URL,CAT_TARGET,CAT_STATUS,CAT_PARENT_ID,CAT_PAGEITEM,CAT_PERIOD,CAT_SEO_TITLE,CAT_SEO_DESC,CAT_SEO_KEYWORD,CAT_SEO_URL,CAT_SHOWHOME,CAT_TYPE,CAT_SHOWFOOTER,CAT_POSITION,CAT_IMAGE1,CAT_IMAGE2,CAT_DESC_EN,CAT_PERIOD_ORDER,CAT_FIELD1,CAT_SEO_TITLE_EN,CAT_FIELD2,CAT_NAME_JS,CAT_NAME_EN,CAT_DESC_JS,CAT_SEO_URL_EN,CAT_SEO_META_DESC_EN,CAT_SEO_META_CANONICAL,CAT_LIENKET_EN")] ESHOP_CATEGORIES eSHOP_CATEGORIES)
        {
            if (id != eSHOP_CATEGORIES.CAT_ID)
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
                            eSHOP_CATEGORIES.CAT_IMAGE1 = fileName;
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            eSHOP_CATEGORIES.CAT_IMAGE2 = fileName1;
                        }

                        var file3 = Request.Form.Files[2];

                        if (file3 != null && file3.FileName.Length > 0)
                        {
                            var fileName2 = Path.GetFileName(file3.FileName);
                            eSHOP_CATEGORIES.CAT_IMAGE3 = fileName2;
                        }
                    }

                    _context.Update(eSHOP_CATEGORIES);
                    await _context.SaveChangesAsync();

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName);
                            AppendToFile(pathfodel, path, file);
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName1);
                            AppendToFile(pathfodel, path, file2);
                        }

                        var file3 = Request.Form.Files[2];

                        if (file3 != null && file3.FileName.Length > 0)
                        {
                            var fileName2 = Path.GetFileName(file3.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Cats/" + eSHOP_CATEGORIES.CAT_ID, fileName2);
                            AppendToFile(pathfodel, path, file3);

                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CATEGORIESExists(eSHOP_CATEGORIES.CAT_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();


                cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_ID != 65).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_ID = p.CAT_ID,
                    CAT_NAME = p.CAT_NAME,

                }).OrderByDescending(p => p.CAT_ID).ToList();

                cat_pr.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });

                ViewBag.ListCatPr = cat_pr;
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_CATEGORIES);
            }
            return View(eSHOP_CATEGORIES);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCatConfirmed(int id)
        {
            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);
            _context.ESHOP_CATEGORIES.Remove(eSHOP_CATEGORIES);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("List", "Admin/Categories");
        }

        public string DeleteCatID(int id)
        {
            string result = "";
            var entity = _context.ESHOP_CATEGORIES.FirstOrDefault(item => item.CAT_ID == id);
            if (entity != null)
            {
                _context.ESHOP_CATEGORIES.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
        }


        [HttpGet]
        public async Task<JsonResult> GetCatListOut(string txtSearch, int? page)
        {
            int pageSize = 15;

            var Categorieslist = await _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
            }).ToListAsync();

            List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

            foreach (var item in Categorieslist.ToList())
            {
                catList.Add(item);
                var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = " ------------ " + p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,
                    CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                });

                foreach (var itemLoad in listChild.ToList())
                {
                    catList.Add(itemLoad);
                    var listChild_Load = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == itemLoad.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = " ----------------------- " + p.CAT_NAME,
                        CAT_ID = p.CAT_ID,
                        CAT_PARENT_ID = p.CAT_PARENT_ID,
                        CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                    });
                    catList.AddRange(listChild_Load);
                }
            }

            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                catList = catList.Where(s => s.CAT_CODE.Contains(txtSearch) || s.CAT_NAME.Contains(txtSearch)).ToList();
            }

            if (page > 0)
            {

            }
            else
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;

            int totalPage = catList.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = catList.Skip(start).Take(pageSize);

            // return Json(listPost);
            //return Json(new { data = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateId(int id, int sttAd)
        {
            var cat = await _context.ESHOP_CATEGORIES.FindAsync(id);
            if (cat != null)
            {
                cat.CAT_PERIOD_ORDER = sttAd;
                _context.Update(cat);
                await _context.SaveChangesAsync();
                return Ok(sttAd);
            }
            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> DeleteCat(int? id)
        {
            var cat = await _context.ESHOP_CATEGORIES.FindAsync(id);
            if (cat != null)
            {
                var NewsCat = await (from _cat in _context.ESHOP_CATEGORIES
                                     join newscat in _context.ESHOP_NEWS_CAT on _cat.CAT_ID equals newscat.CAT_ID
                                     where newscat.CAT_ID == id
                                     select newscat).ToListAsync();
                if (NewsCat.ToList().Count > 0)
                {
                    return Ok("False");
                }
                else
                {
                    _context.ESHOP_CATEGORIES.Remove(cat);
                    await _context.SaveChangesAsync();
                    return Ok("OK");
                }
            }
            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeoUrlEn()
        {
            var cat = await _context.ESHOP_CATEGORIES.ToListAsync();
            if (cat != null)
            {
                foreach (var item in cat.ToList())
                {
                    string catEn = item.CAT_NAME_EN;
                    if (!String.IsNullOrEmpty(catEn))
                    {
                        catEn = catEn.Replace("/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g", "a");
                        catEn = catEn.Replace("/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g", "e");
                        catEn = catEn.Replace("/ì|í|ị|ỉ|ĩ/g", "i");
                        catEn = catEn.Replace("/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g", "o");
                        catEn = catEn.Replace("/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g", "u");
                        catEn = catEn.Replace("/ỳ|ý|ỵ|ỷ|ỹ/g", "y");
                        catEn = catEn.Replace("/đ/g", "d");
                        catEn = catEn.Replace("/ỳ|ý|ỵ|ỷ|ỹ/g", "y");
                        catEn = catEn.Replace(@"/!|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\|\&|\#|\[|\]|~|$|_/g", "-");
                        catEn = catEn.Replace("/-+-/g", "-");
                        catEn = catEn.Replace(@"/^\-+|\-+$/g", "-");
                        catEn = catEn.Replace(" ", "-");
                        catEn = catEn.ToString().ToLower();
                        item.CAT_SEO_URL_EN = catEn;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }
        #endregion

        #region FilterCat
        public async Task<IActionResult> CatFilterList(int id)
        {
            var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);
            if (eSHOP_CATEGORIES == null)
            {
                return NotFound();
            }
            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == 0 && x.PROP_ID != 1))
            {
                Cats.Add(new SelectListItem
                {
                    Text = item.PROP_NAME,
                    Value = item.PROP_ID.ToString()
                });
            }

            ViewBag.Cats = Cats;

            //List<ESHOP_PROPERTIES> cat_pr = new List<ESHOP_PROPERTIES>();

            //cat_pr = _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == 0 && x.PROP_ID != 1).Select(p => new ESHOP_PROPERTIES
            //{
            //    PROP_ID = p.PROP_ID,
            //    PROP_NAME = p.PROP_NAME,

            //}).OrderByDescending(p => p.PROP_ID).ToList();
            //cat_pr.Insert(0, new ESHOP_PROPERTIES { PROP_ID = 0, PROP_NAME = "Vui lòng lựa chọn bộ lọc" });
            //ViewBag.ListCatPr = cat_pr;
            ViewBag.Title = eSHOP_CATEGORIES.CAT_NAME;
            ViewBag.Code = eSHOP_CATEGORIES.CAT_CODE;
            ViewBag.catId = id;
            return View();
        }

        public virtual ObjectResult CatListFilter(int id)
        {
            try
            {
                var CatFilterList = (from cat in _context.ESHOP_CATEGORIES
                                     join catPro in _context.ESHOP_CAT_PRO on cat.CAT_ID equals catPro.CAT_ID
                                     join Pro in _context.ESHOP_PROPERTIES on catPro.PROP_ID equals Pro.PROP_ID
                                     where catPro.CAT_ID == id
                                     select Pro);
                //  return Json(json);
                return new ObjectResult(CatFilterList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CatFilterList(int id, [Bind("ESHOP_CAT_PRO_ID,CAT_ID,PROP_ID,CAT_PRO_FIELD1,CAT_PRO_FIELD2")] ESHOP_CAT_PRO eSHOP_CAT_PRO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] ListCat = Request.Form["Employee[]"];
                    if (Utils.CIntDef(ListCat.ToList().Count) == 0)
                    {
                        ViewBag.Error = "Vui lòng chọn thuộc tính";
                    }
                    else
                    {
                        if (ListCat.ToList().Count() > 0)
                        {                                                  

                            foreach (var itemcat in ListCat.ToList())
                            {
                                int idcheck = int.Parse(itemcat.ToString());
                                var entity = _context.ESHOP_CAT_PRO.FirstOrDefault(item => item.PROP_ID == idcheck && item.CAT_ID == id);
                                if (entity == null)
                                {
                                    ESHOP_CAT_PRO nc = new ESHOP_CAT_PRO();
                                    nc.CAT_ID = id;
                                    nc.PROP_ID = int.Parse(itemcat.ToString());
                                    _context.Add(nc);
                                    await _context.SaveChangesAsync();
                                }

                            }
                        }
                        //var entity = _context.ESHOP_CAT_PRO.FirstOrDefault(item => item.PROP_ID == eSHOP_CAT_PRO.PROP_ID && item.CAT_ID == id);
                        //if (entity == null)
                        //{
                        //    eSHOP_CAT_PRO.CAT_ID = id;
                        //    _context.Add(eSHOP_CAT_PRO);
                        //    await _context.SaveChangesAsync();
                        //}
                        ViewBag.Error = "Cập nhật thành công";
                    }


                    var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);
                    if (eSHOP_CATEGORIES == null)
                    {
                        return NotFound();
                    }
                    var Cats = new List<SelectListItem>();
                    //Çalışanlarımızı listemize aktarıyorum    
                    foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == 0 && x.PROP_ID != 1))
                    {
                        Cats.Add(new SelectListItem
                        {
                            Text = item.PROP_NAME,
                            Value = item.PROP_ID.ToString()
                        });
                    }

                    ViewBag.Cats = Cats;
                    ViewBag.Title = eSHOP_CATEGORIES.CAT_NAME;
                    ViewBag.Code = eSHOP_CATEGORIES.CAT_CODE;
                    ViewBag.catId = id;   
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                var eSHOP_CATEGORIES = await _context.ESHOP_CATEGORIES.SingleOrDefaultAsync(m => m.CAT_ID == id);
                if (eSHOP_CATEGORIES == null)
                {
                    return NotFound();
                }
                var Cats = new List<SelectListItem>();
                //Çalışanlarımızı listemize aktarıyorum    
                foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_PARENT_ID == 0 && x.PROP_ID != 1))
                {
                    Cats.Add(new SelectListItem
                    {
                        Text = item.PROP_NAME,
                        Value = item.PROP_ID.ToString()
                    });
                }

                ViewBag.Cats = Cats;
                ViewBag.Title = eSHOP_CATEGORIES.CAT_NAME;
                ViewBag.Code = eSHOP_CATEGORIES.CAT_CODE;
                ViewBag.catId = id;                
                return View();
            }
        }

        public string DeleteProCat(int id,int cat_id)
        {
            string result = "";
            var entity = _context.ESHOP_CAT_PRO.FirstOrDefault(item => item.CAT_ID == cat_id&& item.PROP_ID == id);
            if (entity != null)
            {
                _context.ESHOP_CAT_PRO.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
        }
        #endregion
    }
}
