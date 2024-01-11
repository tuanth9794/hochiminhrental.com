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
using CoreCnice.UtilsCs;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PictureController : Controller
    {
        //private readonly BulSoftCmsConnectContext _context;

        //public PictureController(BulSoftCmsConnectContext context)
        //{
        //    _context = context;
        //}
        private readonly IHostingEnvironment he;

        public PictureController(IHostingEnvironment e)
        {
            he = e;
        }
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        public virtual ObjectResult PictureList()
        {
            try
            {
                var PictureList = _context.ESHOP_AD_ITEM.Select(p => new ESHOP_AD_ITEM
                {
                    AD_ITEM_ID = p.AD_ITEM_ID,
                    AD_ITEM_FILENAME = p.AD_ITEM_FILENAME,
                    AD_ITEM_PUBLISHDATE = p.AD_ITEM_PUBLISHDATE,
                    AD_ITEM_COUNT = p.AD_ITEM_COUNT,
                    AD_ITEM_TYPE = p.AD_ITEM_TYPE,
                    AD_ITEM_CODE = p.AD_ITEM_CODE

                }).OrderByDescending(p => p.AD_ITEM_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(PictureList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: Picture
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_AD_ITEM.ToListAsync());
        }

        // GET: Picture/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_AD_ITEM = await _context.ESHOP_AD_ITEM
                .SingleOrDefaultAsync(m => m.AD_ITEM_ID == id);
            if (eSHOP_AD_ITEM == null)
            {
                return NotFound();
            }

            return View(eSHOP_AD_ITEM);
        }

        // GET: Picture/Create
        public IActionResult Create()
        {
            ESHOP_AD_ITEM es_cs = new ESHOP_AD_ITEM();
            es_cs.AD_ITEM_TYPE = 0;
            es_cs.AD_ITEM_LANGUAGE = 0;
            es_cs.AD_ITEM_POSITION = 3;

            var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                CAT_TYPE = p.CAT_TYPE,
            });

            List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

            foreach (var item in Categorieslist.ToList())
            {
                catList.Add(item);
                var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = "------------" + p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,
                    CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                    CAT_TYPE = p.CAT_TYPE,
                });
                foreach (var itemList in listChild.ToList())
                {
                    catList.Add(itemList);
                    var listChildAd = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemList.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID,
                        CAT_PARENT_ID = p.CAT_PARENT_ID,
                        CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                        CAT_TYPE = p.CAT_TYPE,
                    });

                    foreach (var itemListCon in listChildAd.ToList())
                    {
                        catList.Add(itemListCon);
                        var listChildAdCon = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemListCon.CAT_ID).Select(p => new ESHOP_CATEGORIES
                        {
                            CAT_NAME = "--------------------------" + p.CAT_NAME,
                            CAT_ID = p.CAT_ID,
                            CAT_PARENT_ID = p.CAT_PARENT_ID,
                            CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                            CAT_TYPE = p.CAT_TYPE,
                        });
                        catList.AddRange(listChildAdCon);
                    }
                }
            }

            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in catList.Where(x => x.CAT_TYPE == 1 || x.CAT_TYPE == 6))
            {
                Cats.Add(new SelectListItem
                {
                    Text = ReturnName(int.Parse(item.CAT_PARENT_ID.ToString()), item.CAT_NAME),
                    Value = item.CAT_ID.ToString()
                });
            }

            ViewBag.ListCatPr = Cats;

            return View(es_cs);
        }

        // POST: Picture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AD_ITEM_ID,AD_ITEM_CODE,AD_ITEM_FILENAME,AD_ITEM_DESC,AD_ITEM_TYPE,AD_ITEM_HEIGHT,AD_ITEM_WIDTH,AD_ITEM_CLICKCOUNT,AD_ITEM_URL,AD_ITEM_PUBLISHDATE,AD_ITEM_DATEFROM,AD_ITEM_DATETO,AD_ITEM_ORDER,AD_ITEM_POSITION,AD_ITEM_TARGET,AD_ITEM_COUNT,AD_ITEM_LANGUAGE,AD_ITEM_FIELD1,AD_ITEM_FIELD2")] ESHOP_AD_ITEM eSHOP_AD_ITEM)
        {
            if (ModelState.IsValid)
            {
                string path = String.Empty;
                string pathfodel = String.Empty;

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];                  

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                       
                        eSHOP_AD_ITEM.AD_ITEM_FILENAME = fileName;
                    }
                }

                string[] ListCat = Request.Form["Employee[]"];

                if (ListCat.ToList().Count() > 0)
                {
                    foreach (var itemcat in ListCat.ToList())
                    {
                        int idcat = Utils.CIntDef(itemcat.ToString());
                        var listCatAd = _context.ESHOP_AD_ITEM_CAT.SingleOrDefault(x => x.CAT_ID == idcat && x.AD_ITEM_ID == eSHOP_AD_ITEM.AD_ITEM_ID);
                        if (listCatAd != null)
                        {

                        }
                        else
                        {
                            ESHOP_AD_ITEM_CAT nc = new ESHOP_AD_ITEM_CAT();
                            nc.CAT_ID = int.Parse(itemcat.ToString());
                            nc.AD_ITEM_ID = eSHOP_AD_ITEM.AD_ITEM_ID;
                            _context.Add(nc);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                eSHOP_AD_ITEM.AD_ITEM_PUBLISHDATE = DateTime.Now;
                _context.Add(eSHOP_AD_ITEM);
                await _context.SaveChangesAsync();


                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];                   

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/" + eSHOP_AD_ITEM.AD_ITEM_ID);
                        path = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/" + eSHOP_AD_ITEM.AD_ITEM_ID, fileName);
                        AppendToFile(pathfodel, path, file);
                    }      
                }           

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Picture", new { id = eSHOP_AD_ITEM.AD_ITEM_ID });
            }
            return View();
        }

        // GET: Picture/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_AD_ITEM = await _context.ESHOP_AD_ITEM.SingleOrDefaultAsync(m => m.AD_ITEM_ID == id);
            if (eSHOP_AD_ITEM == null)
            {
                return NotFound();
            }

            var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID,
                CAT_PARENT_ID = p.CAT_PARENT_ID,
                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                CAT_TYPE = p.CAT_TYPE,
            });

            List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

            foreach (var item in Categorieslist.ToList())
            {
                catList.Add(item);
                var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = "------------" + p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,
                    CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                    CAT_TYPE = p.CAT_TYPE,
                });
                foreach (var itemList in listChild.ToList())
                {
                    catList.Add(itemList);
                    var listChildAd = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemList.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID,
                        CAT_PARENT_ID = p.CAT_PARENT_ID,
                        CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                        CAT_TYPE = p.CAT_TYPE,
                    });

                    foreach (var itemListCon in listChildAd.ToList())
                    {
                        catList.Add(itemListCon);
                        var listChildAdCon = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemListCon.CAT_ID).Select(p => new ESHOP_CATEGORIES
                        {
                            CAT_NAME = "--------------------------" + p.CAT_NAME,
                            CAT_ID = p.CAT_ID,
                            CAT_PARENT_ID = p.CAT_PARENT_ID,
                            CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                            CAT_TYPE = p.CAT_TYPE,
                        });
                        catList.AddRange(listChildAdCon);
                    }

                }
                //catList.AddRange(listChild);
            }


            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in catList.Where(x => x.CAT_TYPE == 1 || x.CAT_TYPE == 6))
            {
                Cats.Add(new SelectListItem
                {
                    Text = ReturnName(int.Parse(item.CAT_PARENT_ID.ToString()), item.CAT_NAME),
                    Value = item.CAT_ID.ToString()
                });
            }

            ViewBag.ListCatPr = Cats;

            return View(eSHOP_AD_ITEM);
        }

        // POST: Picture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AD_ITEM_ID,AD_ITEM_CODE,AD_ITEM_FILENAME,AD_ITEM_DESC,AD_ITEM_TYPE,AD_ITEM_HEIGHT,AD_ITEM_WIDTH,AD_ITEM_CLICKCOUNT,AD_ITEM_URL,AD_ITEM_PUBLISHDATE,AD_ITEM_DATEFROM,AD_ITEM_DATETO,AD_ITEM_ORDER,AD_ITEM_POSITION,AD_ITEM_TARGET,AD_ITEM_COUNT,AD_ITEM_LANGUAGE,AD_ITEM_FIELD1,AD_ITEM_FIELD2")] ESHOP_AD_ITEM eSHOP_AD_ITEM)
        {
            if (id != eSHOP_AD_ITEM.AD_ITEM_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string path = String.Empty;
                string pathfodel = String.Empty;

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];               

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        eSHOP_AD_ITEM.AD_ITEM_FILENAME = fileName;
                    }
                }

                try
                {

                    _context.Update(eSHOP_AD_ITEM);
                    await _context.SaveChangesAsync();

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/" + eSHOP_AD_ITEM.AD_ITEM_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/" + eSHOP_AD_ITEM.AD_ITEM_ID, fileName);
                            AppendToFile(pathfodel, path, file);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_AD_ITEMExists(eSHOP_AD_ITEM.AD_ITEM_ID))
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
                var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = p.CAT_NAME,
                    CAT_ID = p.CAT_ID,
                    CAT_PARENT_ID = p.CAT_PARENT_ID,
                    CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                    CAT_TYPE = p.CAT_TYPE,
                });

                List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

                foreach (var item in Categorieslist.ToList())
                {
                    catList.Add(item);
                    var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID,
                        CAT_PARENT_ID = p.CAT_PARENT_ID,
                        CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                        CAT_TYPE = p.CAT_TYPE,
                    });
                    foreach (var itemList in listChild.ToList())
                    {
                        catList.Add(itemList);
                        var listChildAd = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemList.CAT_ID).Select(p => new ESHOP_CATEGORIES
                        {
                            CAT_NAME = "------------------" + p.CAT_NAME,
                            CAT_ID = p.CAT_ID,
                            CAT_PARENT_ID = p.CAT_PARENT_ID,
                            CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                            CAT_TYPE = p.CAT_TYPE,
                        });

                        foreach (var itemListCon in listChildAd.ToList())
                        {
                            catList.Add(itemListCon);
                            var listChildAdCon = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_PARENT_ID == itemListCon.CAT_ID).Select(p => new ESHOP_CATEGORIES
                            {
                                CAT_NAME = "--------------------------" + p.CAT_NAME,
                                CAT_ID = p.CAT_ID,
                                CAT_PARENT_ID = p.CAT_PARENT_ID,
                                CAT_PERIOD_ORDER = p.CAT_PERIOD_ORDER,
                                CAT_TYPE = p.CAT_TYPE,
                            });
                            catList.AddRange(listChildAdCon);
                        }

                    }
                    //catList.AddRange(listChild);
                }


                var Cats = new List<SelectListItem>();
                //Çalışanlarımızı listemize aktarıyorum    
                foreach (var item in catList.Where(x => x.CAT_TYPE == 1 || x.CAT_TYPE == 6))
                {
                    Cats.Add(new SelectListItem
                    {
                        Text = ReturnName(int.Parse(item.CAT_PARENT_ID.ToString()), item.CAT_NAME),
                        Value = item.CAT_ID.ToString()
                    });
                }

                ViewBag.ListCatPr = Cats;
                return View(eSHOP_AD_ITEM);
            }


            return View(eSHOP_AD_ITEM);
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
        // GET: Picture/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_AD_ITEM = await _context.ESHOP_AD_ITEM
                .SingleOrDefaultAsync(m => m.AD_ITEM_ID == id);
            if (eSHOP_AD_ITEM == null)
            {
                return NotFound();
            }

            return View(eSHOP_AD_ITEM);
        }

        // POST: Picture/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_AD_ITEM = await _context.ESHOP_AD_ITEM.SingleOrDefaultAsync(m => m.AD_ITEM_ID == id);
            _context.ESHOP_AD_ITEM.Remove(eSHOP_AD_ITEM);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin/Picture");
        }

        private bool ESHOP_AD_ITEMExists(int id)
        {
            return _context.ESHOP_AD_ITEM.Any(e => e.AD_ITEM_ID == id);
        }

        public string DeletePicID(int id)
        {
            string result = "";
            var entity = _context.ESHOP_AD_ITEM.FirstOrDefault(item => item.AD_ITEM_ID == id);
            if (entity != null)
            {
                _context.ESHOP_AD_ITEM.Remove(entity);
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
        public async Task<JsonResult> GetPicJson(string txtSearch, int? page)
        {
            int pageSize = 30;

            var PictureList = await (_context.ESHOP_AD_ITEM.Select(p => new ESHOP_AD_ITEM
            {
                AD_ITEM_ID = p.AD_ITEM_ID,
                AD_ITEM_FILENAME = p.AD_ITEM_FILENAME,
                AD_ITEM_PUBLISHDATE = p.AD_ITEM_PUBLISHDATE,
                AD_ITEM_COUNT = p.AD_ITEM_COUNT,
                AD_ITEM_TYPE = p.AD_ITEM_TYPE,
                AD_ITEM_CODE = p.AD_ITEM_CODE,
                AD_ITEM_POSITION = p.AD_ITEM_POSITION,
            }).OrderByDescending(p => p.AD_ITEM_PUBLISHDATE)).ToListAsync();

            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                PictureList = PictureList.Where(s => s.AD_ITEM_CODE.Contains(txtSearch)).ToList();
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

            int totalPage = PictureList.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = PictureList.OrderByDescending(x => x.AD_ITEM_PUBLISHDATE).Skip(start).Take(pageSize);

            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePicture(int? id)
        {
            string pathfodel = "";
            string path = "";
            var entity = await _context.ESHOP_AD_ITEM.FindAsync(id);
            if (entity != null)
            {
                var fileName = Path.GetFileName(entity.AD_ITEM_FILENAME);

                pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/");

                path = Path.Combine(he.WebRootPath, "UploadImages/Data/Banner/", fileName);

                bool exists = System.IO.File.Exists(path);
                if (!exists)
                {

                }
                else
                {
                    System.IO.File.Delete(path);
                }

                _context.ESHOP_AD_ITEM.RemoveRange(entity);
                await _context.SaveChangesAsync();
                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }

        public string DeletePicFolder(int id)
        {
            string result = "";
            var entity = _context.ESHOP_AD_ITEM.FirstOrDefault(item => item.AD_ITEM_ID == id);
            if (entity != null)
            {
                string path = String.Empty;
                string pathfodel = String.Empty;


                pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/Aditems/" + entity.AD_ITEM_ID);

                path = Path.Combine(he.WebRootPath, "UploadImages/Data/Aditems/" + entity.AD_ITEM_ID, entity.AD_ITEM_FILENAME);

                AppendToFileDeleta(pathfodel, path);

                entity.AD_ITEM_FILENAME = null;
                _context.Update(entity);
                _context.SaveChangesAsync();
                result = "Xóa hình thành công";
            }
            else
            {
                result = "Không tìm thấy đường dẫn hình";
            }
            return result;
        }

        public void AppendToFileDeleta(string pathfodel, string fullPath)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(pathfodel);
                if (!exists)
                {

                }
                else
                {
                    if (System.IO.File.Exists(fullPath) == true)
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public string ReturnName(int catpr, string name)
        {
            string src = "";

            if (catpr != 0)
            {
                src = "-------" + name;
            }
            else
            {
                src = name;
            }

            return src;
        }
    }
}
