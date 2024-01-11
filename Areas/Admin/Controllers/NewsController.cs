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
using CoreCnice.Models;
using CoreCnice.Areas.Admin.Models;
using Newtonsoft.Json;
using System.Drawing;
using LazZiya.ImageResize;
using System.Security.Claims;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        private readonly IHostingEnvironment he;
        clsFormat cl = new clsFormat();
        int IdUser = 0;
        int groupid = 0;
        int groupTyoe = 0;
        private readonly IHttpContextAccessor httpContextAccessor;
        public NewsController(IHostingEnvironment e, IHttpContextAccessor httpContextAccessor)
        {
            he = e;
            this.httpContextAccessor = httpContextAccessor;
            if (Utils.CIntDef(httpContextAccessor.HttpContext.User.Identity.Name) == 0)
            {

            }
            else
            {
                groupid = Utils.CIntDef(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value);
                groupTyoe = Utils.CIntDef(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GroupSid).Value);
                IdUser = Utils.CIntDef(httpContextAccessor.HttpContext.User.Identity.Name);
            }
        }
        // GET: News
        public async Task<IActionResult> Index()
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
                foreach (var itemn in listChild.ToList())
                {
                    catList.Add(itemn);
                    var listChildN = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == itemn.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "-------------------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID
                    });
                    catList.AddRange(listChildN);
                }

            }

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục tìm kiếm" });
            ViewBag.ListCatPr = catList;
            return View(await _context.ESHOP_NEWS.ToListAsync());
        }



        public async Task<IActionResult> IndexAll()
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

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục tìm kiếm" });
            ViewBag.ListCatPr = catList;
            return View(await _context.ESHOP_NEWS.ToListAsync());
        }

        public async Task<IActionResult> ListNews()
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

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục tìm kiếm" });
            ViewBag.ListCatPr = catList;
            return View(await _context.ESHOP_NEWS.ToListAsync());
        }

        public async Task<IActionResult> ListNewsSheet()
        {
            var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0 && x.CAT_TYPE != 0 && x.CAT_TYPE != 6 && x.CAT_TYPE != 7).Select(p => new ESHOP_CATEGORIES
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
                foreach (var itemOne in listChild.ToList())
                {
                    catList.Add(itemOne);
                    var listChildOne = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == itemOne.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------------------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID
                    });
                    catList.AddRange(listChildOne);
                }
            }

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục tìm kiếm" });
            ViewBag.ListCatPr = catList;
            return View(await _context.ESHOP_NEWS.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_NEWS = await _context.ESHOP_NEWS
                .SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            return View(eSHOP_NEWS);
        }

        public virtual ObjectResult NewsList()
        {
            try
            {
                var NewsList = _context.ESHOP_NEWS.Select(p => new ESHOP_NEWS
                {
                    NEWS_ID = p.NEWS_ID,
                    NEWS_TITLE = p.NEWS_TITLE,
                    NEWS_ORDER = p.NEWS_ORDER,
                    NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                    NEWS_IMAGE1 = p.NEWS_IMAGE1,

                }).OrderByDescending(p => p.NEWS_ID);
                //  return Json(json);
                return new ObjectResult(NewsList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: News/Create
        public IActionResult Create()
        {
            //List<ESHOP_CATEGORIES> cat_pr = new List<ESHOP_CATEGORIES>();

            //cat_pr = _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE == 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            //{
            //    CAT_ID = p.CAT_ID,
            //    CAT_NAME = p.CAT_NAME,

            //}).OrderByDescending(p => p.CAT_ID).ToList();
            //ViewBag.ListCatPr = cat_pr;

            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65))
            {
                Cats.Add(new SelectListItem
                {
                    Text = item.CAT_NAME,
                    Value = item.CAT_ID.ToString()
                });
            }

            ViewBag.Cats = Cats;

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            OrderStatus.Insert(5, new TypeRoom { id = "Penthouse", value = "Penthouse" });
            OrderStatus.Insert(6, new TypeRoom { id = "Officetel", value = "Văn phòng" });
            ViewBag.OrderStatusList = OrderStatus;

            ESHOP_NEWS es_cs = new ESHOP_NEWS();
            es_cs.NEWS_TYPE = 1;
            es_cs.NEWS_ORDER = 1;
            es_cs.NEWS_ORDER_PERIOD = 1;
            es_cs.NEWS_SHOWINDETAIL = 1;
            es_cs.NEWS_SHOWTYPE = 1;
            es_cs.NEWS_PERIOD = 1;
            es_cs.NEWS_SHOWINDETAIL = 1;
            es_cs.NEWS_FIELD5 = "<meta name='robots' content='noindex'>";
            return View(es_cs);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NEWS_ID,NEWS_CODE,NEWS_TITLE,NEWS_DESC,NEWS_URL,NEWS_TARGET,NEWS_SEO_KEYWORD,NEWS_SEO_DESC,NEWS_SEO_TITLE,NEWS_SEO_URL,NEWS_FILEHTML,NEWS_PUBLISHDATE,NEWS_UPDATE,NEWS_SHOWTYPE,NEWS_SHOWINDETAIL,NEWS_FEEDBACKTYPE,NEWS_TYPE,NEWS_PERIOD,NEWS_ORDER_PERIOD,NEWS_ORDER,NEWS_PRINTTYPE,NEWS_COUNT,NEWS_SENDEMAIL,NEWS_SENDDATE,NEWS_PRICE1,NEWS_PRICE2,NEWS_PRICE3,NEWS_IMAGE1,NEWS_IMAGE2,NEWS_IMAGE3,NEWS_IMAGE4,NEWS_FIELD3,NEWS_FIELD4,NEWS_FIELD1,NEWS_FIELD5,NEWS_FIELD2,NEWS_FILEHTML_EN,NEWS_TITLE_EN,NEWS_DESC_EN,NEWS_HTML_EN1,NEWS_HTML_EN2,NEWS_HTML_EN3,NEWS_TITLE_JS,NEWS_SEO_DESC_JS,NEWS_SEO_URL_JS,NEWS_SEO_URL_EN,NEWS_SEO_META_CANONICAL,NEWS_SEO_META_DESC_EN,NEWS_LIENKET_EN,NEWS_TIME_AVALBLE")] ESHOP_NEWS eSHOP_NEWS)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string noidungad = eSHOP_NEWS.NEWS_FILEHTML;
                    string noidungadta = eSHOP_NEWS.NEWS_FILEHTML_EN;
                    string seo_url = eSHOP_NEWS.NEWS_SEO_URL;
                    var listSeo = _context.ESHOP_NEWS.Where(x => x.NEWS_SEO_URL == seo_url);
                    if (listSeo.ToList().Count == 0)
                    {
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            string path = String.Empty;
                            string pathfodel = String.Empty;

                            if (file != null && file.FileName.Length > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                eSHOP_NEWS.NEWS_IMAGE1 = fileName;
                            }

                            var file2 = Request.Form.Files[1];

                            if (file2 != null && file2.FileName.Length > 0)
                            {
                                var fileName1 = Path.GetFileName(file2.FileName);
                                eSHOP_NEWS.NEWS_IMAGE2 = fileName1;
                            }
                        }

                        eSHOP_NEWS.NEWS_PUBLISHDATE = DateTime.Now;
                        eSHOP_NEWS.NEWS_FILEHTML = "";
                        eSHOP_NEWS.NEWS_FILEHTML_EN = "";
                        _context.Add(eSHOP_NEWS);
                        await _context.SaveChangesAsync();

                        eSHOP_NEWS.NEWS_FILEHTML = eSHOP_NEWS.NEWS_ID + "-vi.html";
                        eSHOP_NEWS.NEWS_FILEHTML_EN = eSHOP_NEWS.NEWS_ID + "-en.html";

                        _context.Update(eSHOP_NEWS);

                        await _context.SaveChangesAsync();

                        string pathfodel11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);

                        string path11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, eSHOP_NEWS.NEWS_ID.ToString() + "-vi.htm");

                        CheckToExitFileCr(pathfodel11, path11, noidungad);

                        string pathfodel111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);

                        string path111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, eSHOP_NEWS.NEWS_ID.ToString() + "-en.htm");

                        CheckToExitFileCr(pathfodel111, path111, noidungadta);

                        string[] ListCat = Request.Form["Employee[]"];

                        if (ListCat.ToList().Count() > 0)
                        {
                            var CatListNew = _context.ESHOP_NEWS_CAT.Where(m => m.NEWS_ID == eSHOP_NEWS.NEWS_ID);
                            foreach (var itemcatlist in CatListNew.ToList())
                            {
                                _context.ESHOP_NEWS_CAT.Remove(itemcatlist);
                                await _context.SaveChangesAsync();
                            }

                            foreach (var itemcat in ListCat.ToList())
                            {
                                ESHOP_NEWS_CAT nc = new ESHOP_NEWS_CAT();
                                nc.CAT_ID = int.Parse(itemcat.ToString());
                                nc.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                                _context.Add(nc);
                                await _context.SaveChangesAsync();
                            }
                        }

                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            string path = String.Empty;
                            string pathfodel = String.Empty;

                            if (file != null && file.FileName.Length > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);
                                path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, fileName);
                                AppendToFile(pathfodel, path, file);
                            }

                            var file2 = Request.Form.Files[1];

                            if (file2 != null && file2.FileName.Length > 0)
                            {
                                var fileName1 = Path.GetFileName(file2.FileName);
                                pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);
                                path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, fileName1);
                                AppendToFile(pathfodel, path, file2);
                            }
                        }

                        //return RedirectToAction(nameof(Index));
                        return RedirectToAction("Edit", "Admin/News", new { id = eSHOP_NEWS.NEWS_ID });
                        //return RedirectToAction("Edit", new { id = eSHOP_NEWS.NEWS_ID });
                    }
                    else
                    {
                        ViewBag.Error = "Bài viết trùng tên";
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            ViewBag.OrderStatusList = OrderStatus;
            return View(eSHOP_NEWS);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_ID != 65))
            {
                Cats.Add(new SelectListItem
                {
                    Text = item.CAT_NAME,
                    Value = item.CAT_ID.ToString()
                });
            }

            ViewBag.Cats = Cats;

            if (String.IsNullOrEmpty(eSHOP_NEWS.NEWS_FILEHTML))
            {

            }
            else
            {
                string path11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, id.ToString() + "-vi.htm");

                eSHOP_NEWS.NEWS_FILEHTML = CheckToExitFileRead(path11);
            }

            if (String.IsNullOrEmpty(eSHOP_NEWS.NEWS_FILEHTML_EN))
            {

            }
            else
            {
                string path11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, id.ToString() + "-en.htm");

                eSHOP_NEWS.NEWS_FILEHTML_EN = CheckToExitFileRead(path11);
            }

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            OrderStatus.Insert(5, new TypeRoom { id = "Penthouse", value = "Penthouse" });
            OrderStatus.Insert(6, new TypeRoom { id = "Officetel", value = "Văn phòng" });
            ViewBag.OrderStatusList = OrderStatus;

            return View(eSHOP_NEWS);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NEWS_ID,NEWS_CODE,NEWS_TITLE,NEWS_DESC,NEWS_URL,NEWS_TARGET,NEWS_SEO_KEYWORD,NEWS_SEO_DESC,NEWS_SEO_TITLE,NEWS_SEO_URL,NEWS_FILEHTML,NEWS_PUBLISHDATE,NEWS_UPDATE,NEWS_SHOWTYPE,NEWS_SHOWINDETAIL,NEWS_FEEDBACKTYPE,NEWS_TYPE,NEWS_PERIOD,NEWS_ORDER_PERIOD,NEWS_ORDER,NEWS_PRINTTYPE,NEWS_COUNT,NEWS_SENDEMAIL,NEWS_SENDDATE,NEWS_PRICE1,NEWS_PRICE2,NEWS_PRICE3,NEWS_IMAGE1,NEWS_IMAGE2,NEWS_IMAGE3,NEWS_IMAGE4,NEWS_FIELD3,NEWS_FIELD4,NEWS_FIELD1,NEWS_FIELD5,NEWS_FIELD2,NEWS_FILEHTML_EN,NEWS_TITLE_EN,NEWS_DESC_EN,NEWS_HTML_EN1,NEWS_HTML_EN2,NEWS_HTML_EN3,NEWS_TITLE_JS,NEWS_SEO_DESC_JS,NEWS_SEO_URL_JS,NEWS_SEO_URL_EN,NEWS_SEO_META_CANONICAL,NEWS_SEO_META_DESC_EN,NEWS_LIENKET_EN,NEWS_TIME_AVALBLE")] ESHOP_NEWS eSHOP_NEWS)
        {
            if (id != eSHOP_NEWS.NEWS_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string noidungad = eSHOP_NEWS.NEWS_FILEHTML;
                    string noidungadta = eSHOP_NEWS.NEWS_FILEHTML_EN;

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            eSHOP_NEWS.NEWS_IMAGE1 = fileName;
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            eSHOP_NEWS.NEWS_IMAGE2 = fileName1;
                        }
                    }
                    eSHOP_NEWS.NEWS_FILEHTML = id + "-vi.htm";
                    eSHOP_NEWS.NEWS_FILEHTML_EN = id + "-en.htm";
                    eSHOP_NEWS.NEWS_UPDATE = DateTime.Now;
                    _context.Update(eSHOP_NEWS);
                    await _context.SaveChangesAsync();

                    string pathfodel11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id);

                    string path11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, id.ToString() + "-vi.htm");

                    CheckToExitFile(pathfodel11, path11, noidungad);

                    string pathfodel111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);

                    string path111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, eSHOP_NEWS.NEWS_ID.ToString() + "-en.htm");

                    CheckToExitFile(pathfodel111, path111, noidungadta);

                    string[] ListCat = Request.Form["Employee[]"];

                    if (ListCat.ToList().Count() > 0)
                    {
                        //var CatListNew = _context.ESHOP_NEWS_CAT.Where(m => m.NEWS_ID == eSHOP_NEWS.NEWS_ID);
                        //foreach (var itemcatlist in CatListNew.ToList())
                        //{
                        //    _context.ESHOP_NEWS_CAT.Remove(itemcatlist);
                        //    await _context.SaveChangesAsync();
                        //}

                        foreach (var itemcat in ListCat.ToList())
                        {
                            int idcat = Utils.CIntDef(itemcat.ToString());
                            var listCatAd = _context.ESHOP_NEWS_CAT.SingleOrDefault(x => x.CAT_ID == idcat && x.NEWS_ID == id);
                            if (listCatAd != null)
                            {

                            }
                            else
                            {
                                ESHOP_NEWS_CAT nc = new ESHOP_NEWS_CAT();
                                nc.CAT_ID = int.Parse(itemcat.ToString());
                                nc.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                                _context.Add(nc);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        string path = String.Empty;
                        string pathfodel = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, fileName);
                            AppendToFile(pathfodel, path, file);
                        }

                        var file2 = Request.Form.Files[1];

                        if (file2 != null && file2.FileName.Length > 0)
                        {
                            var fileName1 = Path.GetFileName(file2.FileName);
                            pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);
                            path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, fileName1);
                            AppendToFile(pathfodel, path, file2);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_NEWSExists(eSHOP_NEWS.NEWS_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                var Cats = new List<SelectListItem>();
                //Çalışanlarımızı listemize aktarıyorum    
                foreach (var item in _context.ESHOP_CATEGORIES.Where(x => x.CAT_TYPE != 5 && x.CAT_ID != 65))
                {
                    Cats.Add(new SelectListItem
                    {
                        Text = item.CAT_NAME,
                        Value = item.CAT_ID.ToString()
                    });
                }

                ViewBag.Cats = Cats;
                List<TypeRoom> OrderStatus = new List<TypeRoom>();
                OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
                OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
                OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
                OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
                OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
                OrderStatus.Insert(5, new TypeRoom { id = "Penthouse", value = "Penthouse" });
                OrderStatus.Insert(6, new TypeRoom { id = "Officetel", value = "Văn phòng" });
                ViewBag.OrderStatusList = OrderStatus;
                ViewBag.Error = "Cập nhật thành công";
                return View(eSHOP_NEWS);
            }
            return View(eSHOP_NEWS);
        }

        public string CheckToExitFileRead(string fulpath)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(fulpath);
                if (!exists)
                {
                    return cl.ReadFile(fulpath);
                }
                else
                {
                    return "";

                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public void CheckToExitFileCr(string pathfodel, string fulpath, string noidung)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(pathfodel);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(pathfodel);
                    using (StreamWriter writer = new StreamWriter(fulpath))
                    {
                        writer.WriteLine(noidung);
                    }
                }
                else
                {
                    System.IO.File.Delete(fulpath);

                    using (StreamWriter writer = new StreamWriter(fulpath))
                    {
                        writer.WriteLine(noidung);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public void CheckToExitFile(string pathfodel, string fulpath, string noidung)
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
                    System.IO.File.Delete(fulpath);

                    using (StreamWriter writer = new StreamWriter(fulpath))
                    {
                        writer.WriteLine(noidung);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_NEWS = await _context.ESHOP_NEWS
                .SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            return View(eSHOP_NEWS);
        }

        // POST: News/Delete/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCatConfirmed(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            _context.ESHOP_NEWS.Remove(eSHOP_NEWS);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin/News");
        }


        // POST: News/Delete/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateTime(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            //_context.ESHOP_NEWS.Remove(eSHOP_NEWS);
            eSHOP_NEWS.NEWS_PUBLISHDATE = DateTime.Now;
            _context.Update(eSHOP_NEWS);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin/News/Edit/" + id);
        }

        private bool ESHOP_NEWSExists(int id)
        {
            return _context.ESHOP_NEWS.Any(e => e.NEWS_ID == id);
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

        #region listNewsImage
        public async Task<IActionResult> NewsImagesList(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            ESHOP_NEWS_IMAGE em = new ESHOP_NEWS_IMAGE();
            em.NEWS_IMG_DESC = eSHOP_NEWS.NEWS_TITLE;
            em.NEWS_IMG_ORDER = 1;
            em.NEWS_IMG_SHOWTYPE = 1;
            return View(em);
        }

        public virtual ObjectResult ListImagesNews(int id)
        {
            try
            {
                var NewsList = _context.ESHOP_NEWS_IMAGE.Where(x => x.NEWS_ID == id).Select(p => new ESHOP_NEWS_IMAGE
                {
                    NEWS_ID = p.NEWS_ID,
                    NEWS_IMG_DESC = p.NEWS_IMG_DESC,
                    NEWS_IMG_IMAGE1 = p.NEWS_IMG_IMAGE1,
                    //NEWS_IMG_SHOWTYPE = p.NEWS_IMG_SHOWTYPE,
                    NEWS_IMG_ORDER = p.NEWS_IMG_ORDER,
                    NEWS_IMG_ID = p.NEWS_IMG_ID

                }).OrderByDescending(p => p.NEWS_IMG_ID);
                //  return Json(json);
                return new ObjectResult(NewsList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewsImagesList(List<IFormFile> files, int id, [Bind("NEWS_IMG_ID,NEWS_IMG_IMAGE1,NEWS_IMG_DESC,NEWS_IMG_ORDER,NEWS_IMG_SHOWTYPE,NEWS_ID")] ESHOP_NEWS_IMAGE eSHOP_NEWS_IMAGE, string submit)
        {
            if (ModelState.IsValid)
            {
                if (files.ToList().Count > 0)
                {
                    var news = await _context.ESHOP_NEWS.FindAsync(id);



                    switch (submit)
                    {
                        case "Lưu hình ảnh":
                            var filesPath = $"{this.he.WebRootPath}/UploadImages/Data/News/" + id;
                            foreach (var file in files)
                            {
                                var eshopnewsimages = await _context.ESHOP_NEWS_IMAGE.Where(x => x.NEWS_ID == id).ToListAsync();
                                string ImageName = Path.GetFileName(file.FileName);//get filename
                                var fullFilePath = Path.Combine(filesPath, ImageName);

                                //var img = Image.FromFile(ImageName);

                                string imgnew = "wwwroot\\UploadImages\\Data\\News\\" + id + "\\" + news.NEWS_SEO_URL + "-" + (eshopnewsimages.Count() + 1) + ".jpg";

                                //using (var stream = new FileStream(fullFilePath, FileMode.Create))
                                //{
                                //    await file.CopyToAsync(stream);
                                //}

                                //var scaleImage = ImageResize.ScaleByWidth(img, 800);
                                //scaleImage.SaveAs(imgnew);

                                using (var stream = file.OpenReadStream())
                                {
                                    var uploadedImage = System.Drawing.Image.FromStream(stream);

                                    var img = ImageResize.ScaleByWidth(uploadedImage, 800); // returns System.Drawing.Image file
                                    img.SaveAs(imgnew);
                                }


                                ESHOP_NEWS_IMAGE nm = new ESHOP_NEWS_IMAGE();
                                nm.NEWS_ID = id;
                                nm.NEWS_IMG_SHOWTYPE = eSHOP_NEWS_IMAGE.NEWS_IMG_SHOWTYPE;
                                nm.NEWS_IMG_IMAGE1 = news.NEWS_SEO_URL + "-" + (eshopnewsimages.Count() + 1) + ".jpg";
                                nm.NEWS_IMG_DESC = eSHOP_NEWS_IMAGE.NEWS_IMG_DESC;
                                nm.NEWS_IMG_ORDER = eSHOP_NEWS_IMAGE.NEWS_IMG_ORDER;
                                _context.Add(nm);
                                await _context.SaveChangesAsync();
                            }
                            break;
                    }
                }
            }

            ViewBag.Error = "Upload hình ảnh thành công";
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }
            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;
            return View();
        }

        public IActionResult AddNewsImages()
        {
            return View();
        }

        //public string DeleteListImages(int id)
        //{
        //    string result = "";
        //    var entity = _context.ESHOP_NEWS_IMAGE.FirstOrDefault(item => item.NEWS_IMG_ID == id);
        //    if (entity != null)
        //    {
        //        _context.ESHOP_NEWS_IMAGE.Remove(entity);
        //        _context.SaveChangesAsync();
        //        result = "XÓA SẢN PHẨM THÀNH CÔNG";
        //    }
        //    else
        //    {
        //        result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
        //    }
        //    return result;
        //}


        [HttpGet]
        public async Task<JsonResult> GetNewsList(string txtSearch, int? page, int trang, int catId)
        {
            int pageSize = trang;

            List<ESHOP_NEWS> oditemAddList = new List<ESHOP_NEWS>();

            var NewsList = await (_context.ESHOP_NEWS.Where(x => x.NEWS_TYPE != 0).Select(p => new ESHOP_NEWS
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_TITLE = p.NEWS_TITLE,
                NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                NEWS_ORDER = p.NEWS_ORDER,
                NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                NEWS_IMAGE1 = p.NEWS_IMAGE1,
                NEWS_CODE = p.NEWS_CODE,
                NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                NEWS_FIELD3 = p.NEWS_FIELD3,
                NEWS_FIELD4 = p.NEWS_FIELD4,
                NEWS_PRICE1 = p.NEWS_PRICE1,
                NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                NEWS_UPDATE = p.NEWS_UPDATE,
                NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                USER_ID = p.USER_ID,
                NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
            }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();

            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                if (Utils.CIntDef(catId) != 0)
                {
                    var NewsListTitle = (from n in NewsList
                                         join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                         where nc.CAT_ID == catId && (n.NEWS_TITLE.ToLower().Contains(txtSearch.ToLower()))
                                         select new
                                         {
                                             n.NEWS_TITLE,
                                             n.NEWS_PUBLISHDATE,
                                             n.NEWS_SEO_URL,
                                             n.NEWS_URL,
                                             n.NEWS_PRICE1,
                                             n.NEWS_PRICE2,
                                             n.NEWS_CODE,
                                             n.NEWS_IMAGE1,
                                             n.NEWS_IMAGE2,
                                             n.NEWS_ID,
                                             n.NEWS_FILEHTML,
                                             n.NEWS_FIELD4,
                                             n.NEWS_HTML_EN1,
                                             n.NEWS_HTML_EN2,
                                             n.NEWS_HTML_EN3,
                                             n.NEWS_TYPE,
                                             n.NEWS_TITLE_EN,
                                             n.NEWS_TITLE_JS,
                                             n.NEWS_SHOWTYPE,
                                             n.NEWS_FIELD3,
                                             n.NEWS_SEO_URL_JS,
                                             n.NEWS_UPDATE,
                                             n.USER_ID,
                                             n.NEWS_TIME_AVALBLE
                                         }).Select(n => new ESHOP_NEWS
                                         {
                                             NEWS_TITLE = n.NEWS_TITLE,
                                             NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                             NEWS_SEO_URL = n.NEWS_SEO_URL,
                                             NEWS_URL = n.NEWS_URL,
                                             NEWS_PRICE1 = n.NEWS_PRICE1,
                                             NEWS_PRICE2 = n.NEWS_PRICE2,
                                             NEWS_CODE = n.NEWS_CODE,
                                             NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                             NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                             NEWS_ID = n.NEWS_ID,
                                             NEWS_FILEHTML = n.NEWS_FILEHTML,
                                             NEWS_FIELD4 = n.NEWS_FIELD4,
                                             NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                             NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                             NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                             NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                             NEWS_TYPE = n.NEWS_TYPE,
                                             NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                             NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                             NEWS_FIELD3 = n.NEWS_FIELD3,
                                             NEWS_UPDATE = n.NEWS_UPDATE,
                                             NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                             USER_ID = n.USER_ID,
                                             NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                         }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsListTitle != null)
                    {
                        oditemAddList.AddRange(NewsListTitle);
                    }

                    var NewsListCode = (from n in NewsList
                                        join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                        where nc.CAT_ID == catId && (n.NEWS_CODE.ToLower().Contains(txtSearch.ToLower()))
                                        select new
                                        {
                                            n.NEWS_TITLE,
                                            n.NEWS_PUBLISHDATE,
                                            n.NEWS_SEO_URL,
                                            n.NEWS_URL,
                                            n.NEWS_PRICE1,
                                            n.NEWS_PRICE2,
                                            n.NEWS_CODE,
                                            n.NEWS_IMAGE1,
                                            n.NEWS_IMAGE2,
                                            n.NEWS_ID,
                                            n.NEWS_FILEHTML,
                                            n.NEWS_FIELD4,
                                            n.NEWS_HTML_EN1,
                                            n.NEWS_HTML_EN2,
                                            n.NEWS_HTML_EN3,
                                            n.NEWS_TYPE,
                                            n.NEWS_TITLE_EN,
                                            n.NEWS_TITLE_JS,
                                            n.NEWS_SHOWTYPE,
                                            n.NEWS_FIELD3,
                                            n.NEWS_SEO_URL_JS,
                                            n.NEWS_UPDATE,
                                            n.USER_ID,
                                            n.NEWS_TIME_AVALBLE
                                        }).Select(n => new ESHOP_NEWS
                                        {
                                            NEWS_TITLE = n.NEWS_TITLE,
                                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                                            NEWS_URL = n.NEWS_URL,
                                            NEWS_PRICE1 = n.NEWS_PRICE1,
                                            NEWS_PRICE2 = n.NEWS_PRICE2,
                                            NEWS_CODE = n.NEWS_CODE,
                                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                            NEWS_ID = n.NEWS_ID,
                                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                                            NEWS_FIELD4 = n.NEWS_FIELD4,
                                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                            NEWS_TYPE = n.NEWS_TYPE,
                                            NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                            NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                            NEWS_FIELD3 = n.NEWS_FIELD3,
                                            NEWS_UPDATE = n.NEWS_UPDATE,
                                            NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                            USER_ID = n.USER_ID,
                                            NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                        }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsListCode != null)
                    {
                        oditemAddList.AddRange(NewsListCode);
                    }
                }
                else
                {
                    var NewsListtitle = (NewsList.Where(n => (n.NEWS_TITLE.ToLower().Contains(txtSearch.ToLower()))).Select(p => new ESHOP_NEWS
                    {
                        NEWS_ID = p.NEWS_ID,
                        NEWS_TITLE = p.NEWS_TITLE,
                        NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                        NEWS_ORDER = p.NEWS_ORDER,
                        NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                        NEWS_IMAGE1 = p.NEWS_IMAGE1,
                        NEWS_CODE = p.NEWS_CODE,
                        NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                        NEWS_FIELD3 = p.NEWS_FIELD3,
                        NEWS_FIELD4 = p.NEWS_FIELD4,
                        NEWS_PRICE1 = p.NEWS_PRICE1,
                        NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                        NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                        NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                        NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                        NEWS_UPDATE = p.NEWS_UPDATE,
                        USER_ID = p.USER_ID,
                        NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
                    }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToList();
                    if (NewsListtitle != null)
                    {
                        oditemAddList.AddRange(NewsListtitle);
                    }

                    var NewsListcode = (NewsList.Where(x => x.NEWS_CODE != null).ToList().Where(n => (n.NEWS_CODE.ToLower().Contains(txtSearch.ToLower()))).Select(p => new ESHOP_NEWS
                    {
                        NEWS_ID = p.NEWS_ID,
                        NEWS_TITLE = p.NEWS_TITLE,
                        NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                        NEWS_ORDER = p.NEWS_ORDER,
                        NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                        NEWS_IMAGE1 = p.NEWS_IMAGE1,
                        NEWS_CODE = p.NEWS_CODE,
                        NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                        NEWS_FIELD3 = p.NEWS_FIELD3,
                        NEWS_FIELD4 = p.NEWS_FIELD4,
                        NEWS_PRICE1 = p.NEWS_PRICE1,
                        NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                        NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                        NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                        NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                        NEWS_UPDATE = p.NEWS_UPDATE,
                        USER_ID = p.USER_ID,
                        NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
                    }).AsEnumerable().OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToList();
                    if (NewsListcode != null)
                    {
                        oditemAddList.AddRange(NewsListcode);
                    }
                }
            }
            else
            {
                if (Utils.CIntDef(catId) != 0)
                {
                    NewsList = (from n in NewsList
                                join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                where nc.CAT_ID == catId
                                select new
                                {
                                    n.NEWS_TITLE,
                                    n.NEWS_PUBLISHDATE,
                                    n.NEWS_SEO_URL,
                                    n.NEWS_URL,
                                    n.NEWS_PRICE1,
                                    n.NEWS_PRICE2,
                                    n.NEWS_CODE,
                                    n.NEWS_IMAGE1,
                                    n.NEWS_IMAGE2,
                                    n.NEWS_ID,
                                    n.NEWS_FILEHTML,
                                    n.NEWS_FIELD4,
                                    n.NEWS_HTML_EN1,
                                    n.NEWS_HTML_EN2,
                                    n.NEWS_HTML_EN3,
                                    n.NEWS_TYPE,
                                    n.NEWS_TITLE_EN,
                                    n.NEWS_TITLE_JS,
                                    n.NEWS_SHOWTYPE,
                                    n.NEWS_FIELD3,
                                    n.NEWS_SEO_URL_JS,
                                    n.NEWS_UPDATE,
                                    n.USER_ID,
                                    n.NEWS_TIME_AVALBLE
                                }).Select(n => new ESHOP_NEWS
                                {
                                    NEWS_TITLE = n.NEWS_TITLE,
                                    NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                    NEWS_SEO_URL = n.NEWS_SEO_URL,
                                    NEWS_URL = n.NEWS_URL,
                                    NEWS_PRICE1 = n.NEWS_PRICE1,
                                    NEWS_PRICE2 = n.NEWS_PRICE2,
                                    NEWS_CODE = n.NEWS_CODE,
                                    NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                    NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                    NEWS_ID = n.NEWS_ID,
                                    NEWS_FILEHTML = n.NEWS_FILEHTML,
                                    NEWS_FIELD4 = n.NEWS_FIELD4,
                                    NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                    NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                    NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                    NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                    NEWS_TYPE = n.NEWS_TYPE,
                                    NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                    NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                    NEWS_FIELD3 = n.NEWS_FIELD3,
                                    NEWS_UPDATE = n.NEWS_UPDATE,
                                    NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                    USER_ID = n.USER_ID,
                                    NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsList != null)
                    {
                        oditemAddList.AddRange(NewsList);
                    }
                }
                else
                {
                    oditemAddList.AddRange(NewsList);
                }
            }
            var result = oditemAddList.GroupBy(x => x.NEWS_ID).Select(y => y.First());
            if (groupTyoe == 0)
            {

            }
            else
            {
                result = result.Where(x => x.USER_ID == IdUser);
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

            int totalPage = result.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = result.OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(start).Take(pageSize);

            // return Json(listPost);
            //return Json(new { data = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }

        [HttpGet]
        public async Task<JsonResult> GetNewsListNews(string txtSearch, int? page, int trang)
        {
            int pageSize = trang;

            var NewsList = await (_context.ESHOP_NEWS.Where(x => x.NEWS_TYPE == 0).Select(p => new ESHOP_NEWS
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_TITLE = p.NEWS_TITLE,
                NEWS_ORDER = p.NEWS_ORDER,
                NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                NEWS_IMAGE1 = p.NEWS_IMAGE1,
                NEWS_CODE = p.NEWS_CODE,
            }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();

            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                NewsList = (NewsList.Where(s => s.NEWS_TITLE.Contains(txtSearch)).Select(p => new ESHOP_NEWS
                {
                    NEWS_ID = p.NEWS_ID,
                    NEWS_TITLE = p.NEWS_TITLE,
                    NEWS_ORDER = p.NEWS_ORDER,
                    NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                    NEWS_IMAGE1 = p.NEWS_IMAGE1,
                    NEWS_CODE = p.NEWS_CODE,
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToList();
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

            int totalPage = NewsList.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = NewsList.OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(start).Take(pageSize);

            // return Json(listPost);
            //return Json(new { data = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteNewsId(int? id)
        {
            var entity = await _context.ESHOP_NEWS.FindAsync(id);
            if (entity != null)
            {
                var News_Images = await _context.ESHOP_NEWS_IMAGE.Where(x => x.NEWS_ID == id).ToListAsync();
                if (News_Images.ToList().Count > 0)
                {
                    _context.ESHOP_NEWS_IMAGE.RemoveRange(News_Images);
                    await _context.SaveChangesAsync();
                }

                var ListComent = await _context.ESHOP_NEWS_COMMENT.Where(x => x.NEWS_ID == id).ToListAsync();
                if (ListComent.ToList().Count > 0)
                {
                    _context.ESHOP_NEWS_COMMENT.RemoveRange(ListComent);
                    await _context.SaveChangesAsync();
                }

                var ListNewsCat = await _context.ESHOP_NEWS_CAT.Where(x => x.NEWS_ID == id).ToListAsync();
                if (ListNewsCat.ToList().Count > 0)
                {
                    _context.ESHOP_NEWS_CAT.RemoveRange(ListNewsCat);
                    await _context.SaveChangesAsync();
                }

                _context.ESHOP_NEWS.RemoveRange(entity);
                await _context.SaveChangesAsync();

                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CapNhatLinkURL()
        {
            var entity = await _context.ESHOP_NEWS.Where(x => x.NEWS_TYPE == 1).ToListAsync();
            if (entity.ToList().Count > 0)
            {
                foreach (var item in entity)
                {
                    if (String.IsNullOrEmpty(item.NEWS_CODE) == true)
                    {

                    }
                    else
                    {
                        string rep = "-" + item.NEWS_CODE;
                        item.NEWS_SEO_URL = item.NEWS_SEO_URL.Replace(rep, "") + "-" + item.NEWS_CODE.Replace("/", "");
                        _context.ESHOP_NEWS.Update(item);
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


        [HttpPost]
        public async Task<IActionResult> CapNhatSeoUr()
        {
            var entity = await _context.ESHOP_NEWS.OrderByDescending(x => x.NEWS_PUBLISHDATE).ToListAsync();
            if (entity.ToList().Count > 0)
            {
                foreach (var item in entity)
                {
                    if (String.IsNullOrEmpty(item.NEWS_CODE) == true)
                    {

                    }
                    else
                    {
                        if (String.IsNullOrEmpty(item.NEWS_SEO_URL_EN) == true)
                        {
                            string rep = "-" + item.NEWS_CODE;
                            string catEn = item.NEWS_TITLE_EN;
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
                                item.NEWS_SEO_URL_EN = catEn + "-" + item.NEWS_CODE.Replace("/", "");
                                _context.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else if (item.NEWS_SEO_URL_EN.Contains("---"))
                        {
                            string catEn = item.NEWS_SEO_URL_EN.Replace("---", "-");
                            item.NEWS_SEO_URL_EN = catEn;
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        else if (item.NEWS_SEO_URL_EN.Contains("/"))
                        {
                            string catEn = item.NEWS_SEO_URL_EN.Replace("/", "");
                            item.NEWS_SEO_URL_EN = catEn;
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateNews(int? id)
        {
            var entity = await _context.ESHOP_NEWS.FindAsync(id);
            if (entity != null)
            {
                entity.NEWS_PUBLISHDATE = DateTime.Now;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNewsTinhTrang(int? id)
        {
            var entity = await _context.ESHOP_NEWS.FindAsync(id);
            if (entity != null)
            {
                if (entity.NEWS_SHOWTYPE == 1)
                {
                    entity.NEWS_SHOWTYPE = 0;
                }
                else
                {
                    entity.NEWS_SHOWTYPE = 1;
                }

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return Ok("OK");
            }
            else
            {
                return Ok("False");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CopyNews(int? id)
        {
            try
            {
                var entity = await _context.ESHOP_NEWS.FindAsync(id);
                string noidung = "";
                string noidungEn = "";
                if (entity != null)
                {
                    ESHOP_NEWS eSHOP_NEWS = new ESHOP_NEWS();
                    eSHOP_NEWS.NEWS_TITLE = entity.NEWS_TITLE + DateTime.Now.ToShortDateString();
                    eSHOP_NEWS.NEWS_DESC = entity.NEWS_DESC;
                    eSHOP_NEWS.NEWS_SEO_TITLE = entity.NEWS_SEO_TITLE;
                    eSHOP_NEWS.NEWS_SEO_DESC = entity.NEWS_SEO_DESC;
                    eSHOP_NEWS.NEWS_SEO_KEYWORD = entity.NEWS_SEO_KEYWORD;
                    eSHOP_NEWS.NEWS_SHOWINDETAIL = entity.NEWS_SHOWINDETAIL;
                    eSHOP_NEWS.NEWS_SHOWTYPE = entity.NEWS_SHOWTYPE;
                    eSHOP_NEWS.NEWS_SEO_URL = entity.NEWS_SEO_URL + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
                    eSHOP_NEWS.NEWS_TYPE = entity.NEWS_TYPE;
                    eSHOP_NEWS.NEWS_TITLE_EN = entity.NEWS_TITLE_EN;
                    eSHOP_NEWS.NEWS_PRICE1 = entity.NEWS_PRICE1;
                    eSHOP_NEWS.NEWS_PRICE2 = entity.NEWS_PRICE2;
                    eSHOP_NEWS.NEWS_PRICE3 = entity.NEWS_PRICE3;
                    eSHOP_NEWS.NEWS_FIELD1 = entity.NEWS_FIELD1;
                    eSHOP_NEWS.NEWS_FIELD2 = entity.NEWS_FIELD2;
                    eSHOP_NEWS.NEWS_FIELD3 = entity.NEWS_FIELD3;
                    eSHOP_NEWS.NEWS_FIELD4 = entity.NEWS_FIELD4;
                    eSHOP_NEWS.NEWS_FIELD5 = entity.NEWS_FIELD5;

                    eSHOP_NEWS.NEWS_FEEDBACKTYPE = entity.NEWS_FEEDBACKTYPE;
                    eSHOP_NEWS.NEWS_DESC_EN = entity.NEWS_DESC_EN;
                    eSHOP_NEWS.NEWS_CODE = entity.NEWS_CODE;

                    eSHOP_NEWS.NEWS_URL = entity.NEWS_URL;

                    eSHOP_NEWS.NEWS_IMAGE4 = entity.NEWS_IMAGE4;

                    eSHOP_NEWS.NEWS_COUNT = entity.NEWS_COUNT;

                    eSHOP_NEWS.NEWS_HTML_EN1 = entity.NEWS_HTML_EN1;
                    eSHOP_NEWS.NEWS_HTML_EN2 = entity.NEWS_HTML_EN2;
                    eSHOP_NEWS.NEWS_HTML_EN3 = entity.NEWS_HTML_EN3;

                    eSHOP_NEWS.NEWS_PERIOD = entity.NEWS_PERIOD;
                    eSHOP_NEWS.NEWS_ORDER_PERIOD = entity.NEWS_ORDER_PERIOD;
                    eSHOP_NEWS.NEWS_ORDER = entity.NEWS_ORDER;
                    eSHOP_NEWS.NEWS_FIELD5 = entity.NEWS_FIELD5;
                    eSHOP_NEWS.NEWS_IMAGE1 = entity.NEWS_IMAGE1;
                    eSHOP_NEWS.NEWS_IMAGE2 = entity.NEWS_IMAGE2;
                    eSHOP_NEWS.NEWS_IMAGE3 = entity.NEWS_IMAGE3;
                    eSHOP_NEWS.NEWS_IMAGE4 = entity.NEWS_IMAGE4;
                    eSHOP_NEWS.NEWS_PUBLISHDATE = DateTime.Now;

                    _context.Add(eSHOP_NEWS);
                    await _context.SaveChangesAsync();

                    eSHOP_NEWS.NEWS_FILEHTML = eSHOP_NEWS.NEWS_ID + "-vi.html";
                    eSHOP_NEWS.NEWS_FILEHTML_EN = eSHOP_NEWS.NEWS_ID + "-en.html";

                    _context.Update(eSHOP_NEWS);
                    await _context.SaveChangesAsync();

                    if (String.IsNullOrEmpty(entity.NEWS_FILEHTML))
                    {

                    }
                    else
                    {
                        string path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, id.ToString() + "-vi.htm");
                        noidung = CheckToExitFileRead(path);
                    }

                    if (String.IsNullOrEmpty(entity.NEWS_FILEHTML_EN))
                    {

                    }
                    else
                    {
                        string path1 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, id.ToString() + "-en.htm");
                        noidungEn = CheckToExitFileRead(path1);
                    }

                    string pathfodel11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);

                    string path11 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, eSHOP_NEWS.NEWS_ID.ToString() + "-vi.htm");

                    CheckToExitFileCr(pathfodel11, path11, noidung);

                    string pathfodel111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID);

                    string path111 = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID, eSHOP_NEWS.NEWS_ID.ToString() + "-en.htm");

                    CheckToExitFileCr(pathfodel111, path111, noidungEn);

                    var ListNCat = _context.ESHOP_NEWS_CAT.Where(x => x.NEWS_ID == id);

                    if (ListNCat.ToList().Count > 0)
                    {
                        var CatListNew = _context.ESHOP_NEWS_CAT.Where(m => m.NEWS_ID == eSHOP_NEWS.NEWS_ID);
                        foreach (var itemcatlist in CatListNew.ToList())
                        {
                            _context.ESHOP_NEWS_CAT.Remove(itemcatlist);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var itemcat in ListNCat.ToList())
                        {
                            ESHOP_NEWS_CAT nc = new ESHOP_NEWS_CAT();
                            nc.CAT_ID = int.Parse(itemcat.CAT_ID.ToString());
                            nc.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                            _context.Add(nc);
                            await _context.SaveChangesAsync();
                        }
                    }
                    decimal idDc = decimal.Parse(id.ToString());
                    decimal idAdd = Utils.CDecDef(eSHOP_NEWS.NEWS_ID);
                    var ListPROCat = _context.ESHOP_NEWS_PROPERTIES.Where(x => x.NEWS_ID == idDc);
                    if (ListPROCat.ToList().Count() > 0)
                    {
                        foreach (var itemcat in ListPROCat.ToList())
                        {
                            int idPro = Utils.CIntDef(itemcat.PROP_ID);
                            var CatListNew = _context.ESHOP_NEWS_PROPERTIES.Where(m => m.NEWS_ID == idAdd && m.PROP_ID == idPro);
                            if (CatListNew.ToList().Count > 0)
                            {

                            }
                            else
                            {
                                ESHOP_NEWS_PROPERTIES nc = new ESHOP_NEWS_PROPERTIES();
                                nc.PROP_ID = idPro;
                                nc.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                                _context.Add(nc);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {

                    }


                    var ListPRODesc = _context.PRO_DESC.Where(x => x.NEWS_ID == id);
                    if (ListPRODesc.ToList().Count > 0)
                    {
                        foreach (var itemcat in ListPRODesc.ToList())
                        {

                            PRO_DESC nc = new PRO_DESC();
                            nc.PRO_ACTIVE = itemcat.PRO_ACTIVE;
                            nc.PRO_FILE = itemcat.PRO_FILE;
                            nc.PRO_NAME = itemcat.PRO_NAME;
                            nc.PRO_NAME_EN = itemcat.PRO_NAME_EN;
                            nc.PRO_ORDER = itemcat.PRO_ORDER;
                            nc.PRO_IMAGES = itemcat.PRO_IMAGES;
                            nc.PRO_ACTIVE = itemcat.PRO_ACTIVE;
                            nc.PRO_FILE_EN = itemcat.PRO_FILE_EN;
                            nc.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                            _context.Add(nc);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {

                    }

                    return Ok("OK");
                }
                else
                {
                    return Ok("False");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        public async Task<IActionResult> ProductDescList(int id)
        {
            string srcMau = "";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>1</span> <span class='counter__text'>Trung tâm Giải trí Sài Gòn Superbowl</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>2</span> <span class='counter__text'>Chùa Phổ Quang</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>3</span> <span class='counter__text'>Trung tâm Thương mại Parkson</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>4</span> <span class='counter__text'>Công viên Hoàng Văn Thụ</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>5</span> <span class='counter__text'>Công viên Gia Định</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>6</span> <span class='counter__text'>Trung tâm Triển lãm & Hội chợ Tân Bình</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>7</span> <span class='counter__text'>Trung tâm Thương mại Pico Plaza</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>8</span> <span class='counter__text'>Trung tâm Thương mại Pico Plaza</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>9</span> <span class='counter__text'>Trung tâm Thương mại Pico Plaza</span></div></div>";
            //srcMau += "<div class='col-sm-6 col-xs-12'><div class='counter d-flex'><span class='counter__number'>10</span> <span class='counter__text'>Trung tâm Thương mại Pico Plaza</span></div></div>";
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }
            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;
            PRO_DESC pr = new PRO_DESC();
            pr.PRO_FILE = srcMau;
            pr.PRO_FILE_EN = srcMau;
            return View(pr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDescList(int id, [Bind("PRO_ID,PRO_NAME,PRO_FILE,NEWS_ID,PRO_ACTIVE,PRO_TYPE,PRO_IMAGES,PRO_ORDER,PRO_FILE_EN,PRO_NAME_EN")] PRO_DESC eSHOP_NEWS_IMAGE)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string pathIcon = String.Empty;
                    string pathfodelIcon = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodelIcon = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id);
                        pathIcon = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, fileName);
                        AppendToFile(pathfodelIcon, pathIcon, file);
                        eSHOP_NEWS_IMAGE.PRO_IMAGES = fileName;
                    }
                }

                eSHOP_NEWS_IMAGE.NEWS_ID = id;
                eSHOP_NEWS_IMAGE.PRO_TYPE = 1;
                _context.Add(eSHOP_NEWS_IMAGE);
                await _context.SaveChangesAsync();

                //eSHOP_NEWS_IMAGE.PRO_FILE = (eSHOP_NEWS_IMAGE.PRO_ID.ToString() + "-tienich-vi.html");
                //_context.Update(eSHOP_NEWS_IMAGE);
                //await _context.SaveChangesAsync();
                //string pathfodel = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id);

                //string path = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, eSHOP_NEWS_IMAGE.PRO_ID.ToString() + "-tienich-vi.html");
                //CheckToExitFileCr(pathfodel, path, noidungad);


                //return RedirectToAction(nameof(Index));
                ViewBag.Error = "Cập nhật thành công";
                var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
                if (eSHOP_NEWS == null)
                {
                    return NotFound();
                }
                ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
                ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
                ViewBag.NewsId = id;
                return View();
            }
            return View();
        }

        public virtual ObjectResult ProductDescListOb(int id)
        {
            try
            {
                var NewsList = _context.PRO_DESC.Where(x => x.NEWS_ID == id).Select(p => new PRO_DESC
                {
                    NEWS_ID = p.NEWS_ID,
                    PRO_NAME = p.PRO_NAME,
                    PRO_ACTIVE = p.PRO_ACTIVE,
                    //NEWS_IMG_SHOWTYPE = p.NEWS_IMG_SHOWTYPE,
                    PRO_ORDER = p.PRO_ORDER,
                    PRO_ID = p.PRO_ID

                }).OrderByDescending(p => p.PRO_ID);
                //  return Json(json);
                return new ObjectResult(NewsList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string DeleteListDesc(int id)
        {
            string result = "";
            var entity = _context.PRO_DESC.FirstOrDefault(item => item.PRO_ID == id);
            if (entity != null)
            {
                _context.PRO_DESC.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
        }


        public async Task<IActionResult> ProductDescListEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_NEWS = await _context.PRO_DESC.SingleOrDefaultAsync(m => m.PRO_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            //string fileName = "wwwroot/UploadImages/Data/News/" + eSHOP_NEWS.NEWS_ID + "/" + eSHOP_NEWS.PRO_FILE;

            //eSHOP_NEWS.PRO_FILE = cl.ReadFile(fileName.Replace("<p>", "").Replace("</p>", ""));

            var eSHOP_NEWS_TT = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == eSHOP_NEWS.NEWS_ID);

            ViewBag.Title = eSHOP_NEWS_TT.NEWS_TITLE;

            ViewBag.Code = eSHOP_NEWS_TT.NEWS_CODE;

            return View(eSHOP_NEWS);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDescListEdit(int id, [Bind("PRO_ID,PRO_NAME,PRO_FILE,NEWS_ID,PRO_ACTIVE,PRO_TYPE,PRO_IMAGES,PRO_ORDER,PRO_FILE_EN,PRO_NAME_EN")] PRO_DESC eSHOP_NEWS_IMAGE)
        {
            if (id != eSHOP_NEWS_IMAGE.PRO_ID)
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
                        string pathIcon = String.Empty;
                        string pathfodelIcon = String.Empty;

                        if (file != null && file.FileName.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            pathfodelIcon = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id);
                            pathIcon = Path.Combine(he.WebRootPath, "UploadImages/Data/News/" + id, fileName);
                            AppendToFile(pathfodelIcon, pathIcon, file);
                            eSHOP_NEWS_IMAGE.PRO_IMAGES = fileName;
                        }
                    }

                    _context.Update(eSHOP_NEWS_IMAGE);
                    await _context.SaveChangesAsync();

                    //return RedirectToAction(nameof(Index));
                    ViewBag.Error = "Cập nhật thành công";


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PRO_DESCExists(eSHOP_NEWS_IMAGE.NEWS_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(eSHOP_NEWS_IMAGE);
        }

        private bool PRO_DESCExists(int? id)
        {
            return _context.PRO_DESC.Any(e => e.PRO_ID == id);
        }


        public async Task<IActionResult> NewsPro(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }
            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            var Cats = new List<SelectListItem>();

            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1))
            {
                Cats.Add(new SelectListItem
                {
                    Text = ReturnName(item.PROP_PARENT_ID, item.PROP_NAME),
                    Value = item.PROP_ID.ToString()
                });
            }
            ViewBag.ListCatPr = Cats;

            //IQueryable<ESHOP_PROPERTIES> cat_pr = null;

            var cat_pr = (from npr in _context.ESHOP_NEWS_PROPERTIES
                          join pr in _context.ESHOP_PROPERTIES on npr.PROP_ID equals pr.PROP_ID
                          where npr.NEWS_ID == id
                          select new
                          {
                              pr.PROP_ID,
                              npr.NEWS_PROP_ID,
                              pr.PROP_NAME
                          });

            var PictureList = cat_pr.Select(p => new ESHOP_PRO_MODEL
            {
                NEWS_PROP_ID = p.NEWS_PROP_ID,
                PROP_ID = p.PROP_ID,
                PROP_NAME = p.PROP_NAME,

            }).OrderByDescending(p => p.NEWS_PROP_ID);

            ViewBag.ListAdd = PictureList;
            return View();
        }

        public async Task<IActionResult> NewsProCheck(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = true } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewsProCheck(string selectedItems, int id, [Bind("NEWS_PROP_ID,NEWS_ID,PROP_ID")] ESHOP_NEWS_PROPERTIES eSHOP_NEWS_PROPERTIES)
        {
            //List<TreeViewNode> items = (new JavaScriptSerializer()).Deserialize<List<TreeViewNode>>(selectedItems);
            List<TreeViewNode> result = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

            var listCon = _context.ESHOP_NEWS_PROPERTIES.Where(x => x.NEWS_ID == id);
            if (listCon.ToList().Count > 0)
            {
                _context.ESHOP_NEWS_PROPERTIES.RemoveRange(listCon);
                await _context.SaveChangesAsync();
            }

            if (result.ToList().Count() > 0)
            {
                foreach (var itemcat in result.ToList())
                {
                    int idPro = Utils.CIntDef(itemcat.id);
                    var CatListNew = _context.ESHOP_NEWS_PROPERTIES.Where(m => m.NEWS_ID == id && m.PROP_ID == idPro);
                    if (CatListNew.ToList().Count > 0)
                    {
                    }
                    else
                    {
                        ESHOP_NEWS_PROPERTIES nc = new ESHOP_NEWS_PROPERTIES();
                        nc.PROP_ID = idPro;
                        nc.NEWS_ID = id;
                        _context.Add(nc);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            await _context.SaveChangesAsync();

            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }
            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = true } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            //return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewsPro(int id, [Bind("NEWS_PROP_ID,NEWS_ID,PROP_ID")] ESHOP_NEWS_PROPERTIES eSHOP_NEWS_PROPERTIES)
        {
            eSHOP_NEWS_PROPERTIES.NEWS_ID = id;

            var listCon = _context.ESHOP_NEWS_PROPERTIES.Where(x => x.NEWS_ID == id);
            if (listCon.ToList().Count > 0)
            {
                _context.ESHOP_NEWS_PROPERTIES.RemoveRange(listCon);
                await _context.SaveChangesAsync();
            }

            string[] ListCat = Request.Form["Employee[]"];

            if (ListCat.ToList().Count() > 0)
            {
                foreach (var itemcat in ListCat.ToList())
                {
                    int idPro = Utils.CIntDef(itemcat);

                    var CatListNew = _context.ESHOP_NEWS_PROPERTIES.Where(m => m.NEWS_ID == id && m.PROP_ID == idPro);
                    if (CatListNew.ToList().Count > 0)
                    {

                    }
                    else
                    {
                        ESHOP_NEWS_PROPERTIES nc = new ESHOP_NEWS_PROPERTIES();
                        nc.PROP_ID = idPro;
                        nc.NEWS_ID = id;
                        _context.Add(nc);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            await _context.SaveChangesAsync();

            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }
            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1))
            {
                Cats.Add(new SelectListItem
                {
                    Text = ReturnName(item.PROP_PARENT_ID, item.PROP_NAME),
                    Value = item.PROP_ID.ToString()
                });
            }
            ViewBag.ListCatPr = Cats;

            var cat_pr = (from npr in _context.ESHOP_NEWS_PROPERTIES
                          join pr in _context.ESHOP_PROPERTIES on npr.PROP_ID equals pr.PROP_ID
                          where npr.NEWS_ID == id
                          select new
                          {
                              pr.PROP_ID,
                              npr.NEWS_PROP_ID,
                              pr.PROP_NAME
                          });

            var PictureList = cat_pr.Select(p => new ESHOP_PRO_MODEL
            {
                NEWS_PROP_ID = p.NEWS_PROP_ID,
                PROP_ID = p.PROP_ID,
                PROP_NAME = p.PROP_NAME,

            }).OrderByDescending(p => p.NEWS_PROP_ID);

            ViewBag.ListAdd = PictureList;

            return View();
        }

        public string ReturnName(decimal catpr, string name)
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

        public string DeleteNewPro(int id)
        {
            string result = "";
            var entity = _context.ESHOP_NEWS_PROPERTIES.FirstOrDefault(item => item.NEWS_PROP_ID == id);
            if (entity != null)
            {
                _context.ESHOP_NEWS_PROPERTIES.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG THÀNH CÔNG";
            }
            return result;
        }

        public string DeleteCatMap(int idnew, int idcat)
        {
            string result = "";
            var entity = _context.ESHOP_NEWS_CAT.FirstOrDefault(item => item.NEWS_ID == idnew && item.CAT_ID == idcat);
            if (entity != null)
            {
                _context.ESHOP_NEWS_CAT.Remove(entity);
                _context.SaveChangesAsync();
                result = "XÓA SẢN PHẨM THÀNH CÔNG";
            }
            else
            {
                result = "KHÔNG CẬP NHẬT THÀNH CÔNG";
            }
            return result;
        }

        public bool checkExitNewsPro(int id, int idnews)
        {
            bool fl = false;

            var Procheck = _context.ESHOP_NEWS_PROPERTIES.SingleOrDefault(x => x.NEWS_ID == idnews && x.PROP_ID == id);
            if (Procheck != null)
            {
                fl = true;
            }

            return fl;
        }

        [HttpGet]
        public async Task<JsonResult> GetListImages(string id, int? page, int trang)
        {
            int idLoad = Utils.CIntDef(id);
            int pageSize = trang;

            var NewsList = await (_context.ESHOP_NEWS_IMAGE.Where(x => x.NEWS_ID == idLoad).Select(p => new ESHOP_NEWS_IMAGE
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_IMG_DESC = p.NEWS_IMG_DESC,
                NEWS_IMG_IMAGE1 = p.NEWS_IMG_IMAGE1,
                NEWS_IMG_SHOWTYPE = p.NEWS_IMG_SHOWTYPE,
                NEWS_IMG_ORDER = p.NEWS_IMG_ORDER,
                NEWS_IMG_ID = p.NEWS_IMG_ID

            }).OrderByDescending(p => p.NEWS_IMG_ID)).ToListAsync();

            if (page > 0)
            {

            }
            else
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;

            int totalPage = NewsList.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = NewsList.OrderByDescending(x => x.NEWS_IMG_ID).Skip(start).Take(pageSize);

            // return Json(listPost);
            //return Json(new { data = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }

        [HttpPost]
        public ActionResult DeleteListImages(int id)
        {
            try
            {
                string result = "";
                var entity = _context.ESHOP_NEWS_IMAGE.FirstOrDefault(item => item.NEWS_IMG_ID == id);
                if (entity != null)
                {
                    _context.ESHOP_NEWS_IMAGE.Remove(entity);
                    _context.SaveChangesAsync();
                    result = "OK";
                }
                else
                {
                    result = "False";
                }
                return Ok("OK");
            }
            catch
            {//TODO: Log error				
                return Ok("False");
            }
        }

        #endregion


        #region timkiemnangcao
        public async Task<IActionResult> Advancedsearch()
        {

            //ViewBag.Cats = Cats;
            var Categorieslist = await _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0).Select(p => new ESHOP_CATEGORIES
            {
                CAT_NAME = p.CAT_NAME,
                CAT_ID = p.CAT_ID
            }).ToListAsync();

            List<ESHOP_CATEGORIES> catList = new List<ESHOP_CATEGORIES>();

            foreach (var item in Categorieslist.ToList())
            {
                catList.Add(item);
                var listChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == item.CAT_ID).Select(p => new ESHOP_CATEGORIES
                {
                    CAT_NAME = "------------" + p.CAT_NAME,
                    CAT_ID = p.CAT_ID
                });

                foreach (var itemChild in listChild.ToList())
                {
                    catList.Add(itemChild);
                    var listChildCon = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == itemChild.CAT_ID).Select(p => new ESHOP_CATEGORIES
                    {
                        CAT_NAME = "------------" + p.CAT_NAME,
                        CAT_ID = p.CAT_ID
                    });
                    catList.AddRange(listChildCon);
                }
            }

            catList.Insert(0, new ESHOP_CATEGORIES { CAT_ID = 0, CAT_NAME = "Lựa chọn chuyên mục cấp cha" });
            ViewBag.ListCatPr = catList;

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0 && x.PROP_ACTIVE == 2))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = true } });
            }

            foreach (var itemOne in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0 && x.PROP_ACTIVE == 2))
            {
                foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == itemOne.PROP_ID))
                {
                    nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false } });
                }
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            AdvancedsearchModel avan = new AdvancedsearchModel();
            avan.SEARCHPRICEMAX = 0;
            avan.SEARCHPRICEMIN = 0;
            avan.SEARCHSTATUS = 4;
            avan.SEARCHBATHROOM = 0.ToString();
            avan.SEARCHBEDROOM = 0.ToString();
            avan.SEARCHACREAGE = 0.ToString();
            return View(avan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Advancedsearch([Bind("SEARCHNAME,SEARCHPRICEMIN,SEARCHPRICEMAX,SEARCHBATHROOM,SEARCHBEDROOM,SEARCHACREAGE,SEARCHSTATUS,SEARCHCATID")] AdvancedsearchModel advancedSearch, string CLICK, string selectedItems)
        {
            try
            {

                if (ModelState.IsValid && !String.IsNullOrWhiteSpace(CLICK))
                {
                    string name = Utils.CStrDef(advancedSearch.SEARCHNAME);
                    var NewsList = await (_context.ESHOP_NEWS.Where(x => x.NEWS_TYPE != 0 && x.NEWS_TYPE != 4).Select(n => new ESHOP_NEWS
                    {
                        NEWS_TITLE = n.NEWS_TITLE,
                        NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                        NEWS_SEO_URL = n.NEWS_SEO_URL,
                        NEWS_URL = n.NEWS_URL,
                        NEWS_PRICE1 = n.NEWS_PRICE1,
                        NEWS_PRICE2 = n.NEWS_PRICE2,
                        NEWS_CODE = n.NEWS_CODE,
                        NEWS_IMAGE1 = n.NEWS_IMAGE1,
                        NEWS_IMAGE2 = n.NEWS_IMAGE2,
                        NEWS_ID = n.NEWS_ID,
                        NEWS_FILEHTML = n.NEWS_FILEHTML,
                        NEWS_FIELD4 = n.NEWS_FIELD4,
                        NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                        NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                        NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                        NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                        NEWS_TYPE = n.NEWS_TYPE,
                        //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                        //NEWS_GROUP = n.NEWS_GROUP
                    }).OrderByDescending(p => p.NEWS_ID)).ToListAsync();

                    if (Utils.CIntDef(advancedSearch.SEARCHCATID) != 0)
                    {
                        NewsList = (from n in NewsList
                                    join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                    where nc.CAT_ID == advancedSearch.SEARCHCATID && (n.NEWS_TITLE.ToLower().Contains(name.ToLower()) || n.NEWS_CODE.ToLower().Contains(name.ToLower()) || n.NEWS_FIELD4.ToLower().Contains(name.ToLower()))
                                    select new
                                    {
                                        n.NEWS_TITLE,
                                        n.NEWS_PUBLISHDATE,
                                        n.NEWS_SEO_URL,
                                        n.NEWS_URL,
                                        n.NEWS_PRICE1,
                                        n.NEWS_PRICE2,
                                        n.NEWS_CODE,
                                        n.NEWS_IMAGE1,
                                        n.NEWS_IMAGE2,
                                        n.NEWS_ID,
                                        n.NEWS_FILEHTML,
                                        n.NEWS_FIELD4,
                                        n.NEWS_HTML_EN1,
                                        n.NEWS_HTML_EN2,
                                        n.NEWS_HTML_EN3,
                                        n.NEWS_TYPE,
                                        n.NEWS_TITLE_EN,
                                        //n.NEWS_TITLE_JS,
                                        //n.NEWS_GROUP,
                                    }).Select(n => new ESHOP_NEWS
                                    {
                                        NEWS_TITLE = n.NEWS_TITLE,
                                        NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                        NEWS_SEO_URL = n.NEWS_SEO_URL,
                                        NEWS_URL = n.NEWS_URL,
                                        NEWS_PRICE1 = n.NEWS_PRICE1,
                                        NEWS_PRICE2 = n.NEWS_PRICE2,
                                        NEWS_CODE = n.NEWS_CODE,
                                        NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                        NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                        NEWS_ID = n.NEWS_ID,
                                        NEWS_FILEHTML = n.NEWS_FILEHTML,
                                        NEWS_FIELD4 = n.NEWS_FIELD4,
                                        NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                        NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                        NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                        NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                        NEWS_TYPE = n.NEWS_TYPE,
                                        //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                        //NEWS_GROUP = n.NEWS_GROUP
                                    }).Distinct().OrderByDescending(x => x.NEWS_ID).ToList();
                    }
                    else
                    {
                        NewsList = (from n in NewsList
                                    where (n.NEWS_TITLE.ToLower().Contains(name.ToLower()) || n.NEWS_CODE.ToLower().Contains(name.ToLower()) || n.NEWS_FIELD4.ToLower().Contains(name.ToLower()))
                                    select new
                                    {
                                        n.NEWS_TITLE,
                                        n.NEWS_PUBLISHDATE,
                                        n.NEWS_SEO_URL,
                                        n.NEWS_URL,
                                        n.NEWS_PRICE1,
                                        n.NEWS_PRICE2,
                                        n.NEWS_CODE,
                                        n.NEWS_IMAGE1,
                                        n.NEWS_IMAGE2,
                                        n.NEWS_ID,
                                        n.NEWS_FILEHTML,
                                        n.NEWS_FIELD4,
                                        n.NEWS_HTML_EN1,
                                        n.NEWS_HTML_EN2,
                                        n.NEWS_HTML_EN3,
                                        n.NEWS_TYPE,
                                        n.NEWS_TITLE_EN,
                                        //n.NEWS_TITLE_JS,
                                        //n.NEWS_GROUP,
                                    }).Select(n => new ESHOP_NEWS
                                    {
                                        NEWS_TITLE = n.NEWS_TITLE,
                                        NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                        NEWS_SEO_URL = n.NEWS_SEO_URL,
                                        NEWS_URL = n.NEWS_URL,
                                        NEWS_PRICE1 = n.NEWS_PRICE1,
                                        NEWS_PRICE2 = n.NEWS_PRICE2,
                                        NEWS_CODE = n.NEWS_CODE,
                                        NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                        NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                        NEWS_ID = n.NEWS_ID,
                                        NEWS_FILEHTML = n.NEWS_FILEHTML,
                                        NEWS_FIELD4 = n.NEWS_FIELD4,
                                        NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                        NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                        NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                        NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                        NEWS_TYPE = n.NEWS_TYPE,
                                        //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                        //NEWS_GROUP = n.NEWS_GROUP
                                    }).Distinct().OrderByDescending(x => x.NEWS_ID).ToList();
                    }

                    if (Utils.CIntDef(advancedSearch.SEARCHSTATUS) != 4)
                    {
                        NewsList = (NewsList.Where(x => x.NEWS_TYPE == advancedSearch.SEARCHSTATUS).Select(n => new ESHOP_NEWS
                        {
                            NEWS_TITLE = n.NEWS_TITLE,
                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                            NEWS_URL = n.NEWS_URL,
                            NEWS_PRICE1 = n.NEWS_PRICE1,
                            NEWS_PRICE2 = n.NEWS_PRICE2,
                            NEWS_CODE = n.NEWS_CODE,
                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                            NEWS_ID = n.NEWS_ID,
                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                            NEWS_FIELD4 = n.NEWS_FIELD4,
                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                            NEWS_TYPE = n.NEWS_TYPE,
                            //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                            //NEWS_GROUP = n.NEWS_GROUP
                        }).OrderByDescending(p => p.NEWS_ID)).ToList();
                    }

                    if (Utils.CIntDef(advancedSearch.SEARCHBATHROOM) != 0)
                    {
                        NewsList = (NewsList.Where(x => x.NEWS_HTML_EN1 == advancedSearch.SEARCHBATHROOM).Select(n => new ESHOP_NEWS
                        {
                            NEWS_TITLE = n.NEWS_TITLE,
                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                            NEWS_URL = n.NEWS_URL,
                            NEWS_PRICE1 = n.NEWS_PRICE1,
                            NEWS_PRICE2 = n.NEWS_PRICE2,
                            NEWS_CODE = n.NEWS_CODE,
                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                            NEWS_ID = n.NEWS_ID,
                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                            NEWS_FIELD4 = n.NEWS_FIELD4,
                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                            NEWS_TYPE = n.NEWS_TYPE,
                            //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                            //NEWS_GROUP = n.NEWS_GROUP
                        }).OrderByDescending(p => p.NEWS_ID)).ToList();
                    }

                    if (Utils.CIntDef(advancedSearch.SEARCHBEDROOM) != 0)
                    {
                        NewsList = (NewsList.Where(x => x.NEWS_HTML_EN2 == advancedSearch.SEARCHBEDROOM).Select(n => new ESHOP_NEWS
                        {
                            NEWS_TITLE = n.NEWS_TITLE,
                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                            NEWS_URL = n.NEWS_URL,
                            NEWS_PRICE1 = n.NEWS_PRICE1,
                            NEWS_PRICE2 = n.NEWS_PRICE2,
                            NEWS_CODE = n.NEWS_CODE,
                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                            NEWS_ID = n.NEWS_ID,
                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                            NEWS_FIELD4 = n.NEWS_FIELD4,
                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                            NEWS_TYPE = n.NEWS_TYPE,
                            //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                            //NEWS_GROUP = n.NEWS_GROUP
                        }).OrderByDescending(p => p.NEWS_ID)).ToList();
                    }

                    if (Utils.CIntDef(advancedSearch.SEARCHACREAGE) != 0)
                    {
                        NewsList = (NewsList.Where(x => x.NEWS_HTML_EN3 == advancedSearch.SEARCHACREAGE).Select(n => new ESHOP_NEWS
                        {
                            NEWS_TITLE = n.NEWS_TITLE,
                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                            NEWS_URL = n.NEWS_URL,
                            NEWS_PRICE1 = n.NEWS_PRICE1,
                            NEWS_PRICE2 = n.NEWS_PRICE2,
                            NEWS_CODE = n.NEWS_CODE,
                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                            NEWS_ID = n.NEWS_ID,
                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                            NEWS_FIELD4 = n.NEWS_FIELD4,
                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                            NEWS_TYPE = n.NEWS_TYPE,
                            //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                            //NEWS_GROUP = n.NEWS_GROUP
                        }).OrderByDescending(p => p.NEWS_ID)).ToList();
                    }

                    if (Utils.CDecDef(advancedSearch.SEARCHPRICEMIN) != Utils.CDecDef(advancedSearch.SEARCHPRICEMAX))
                    {
                        NewsList = (NewsList.Where(x => x.NEWS_PRICE1 >= advancedSearch.SEARCHPRICEMIN && x.NEWS_PRICE1 <= advancedSearch.SEARCHPRICEMAX).Select(n => new ESHOP_NEWS
                        {
                            NEWS_TITLE = n.NEWS_TITLE,
                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                            NEWS_URL = n.NEWS_URL,
                            NEWS_PRICE1 = n.NEWS_PRICE1,
                            NEWS_PRICE2 = n.NEWS_PRICE2,
                            NEWS_CODE = n.NEWS_CODE,
                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                            NEWS_ID = n.NEWS_ID,
                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                            NEWS_FIELD4 = n.NEWS_FIELD4,
                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                            NEWS_TYPE = n.NEWS_TYPE,
                            //NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                            //NEWS_GROUP = n.NEWS_GROUP
                        }).OrderByDescending(p => p.NEWS_ID)).ToList();
                    }

                    switch (CLICK)
                    {
                        case "Tìm kiếm":
                            ViewBag.NEWSLIST = NewsList;
                            break;
                        case "Thêm thuộc tính":
                            foreach (var item in NewsList.ToList())
                            {
                                List<TreeViewNode> result = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                                if (result.ToList().Count() > 0)
                                {
                                    foreach (var itemcat in result.ToList())
                                    {
                                        int idPro = Utils.CIntDef(itemcat.id);
                                        var CatListNew = _context.ESHOP_NEWS_PROPERTIES.Where(m => m.NEWS_ID == item.NEWS_ID && m.PROP_ID == idPro);
                                        if (CatListNew.ToList().Count > 0)
                                        {
                                        }
                                        else
                                        {
                                            ESHOP_NEWS_PROPERTIES nc = new ESHOP_NEWS_PROPERTIES();
                                            nc.PROP_ID = idPro;
                                            nc.NEWS_ID = item.NEWS_ID;
                                            _context.Add(nc);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            ViewBag.Error = "Thêm thuộc tính thành công";
                            break;

                        case "Loại bỏ thuộc tính":

                            foreach (var item in NewsList.ToList())
                            {
                                List<TreeViewNode> result = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                                if (result.ToList().Count() > 0)
                                {
                                    foreach (var itemcat in result.ToList())
                                    {
                                        int idPro = Utils.CIntDef(itemcat.id);
                                        var CatListNew = _context.ESHOP_NEWS_PROPERTIES.SingleOrDefault(m => m.NEWS_ID == item.NEWS_ID && m.PROP_ID == idPro);
                                        if (CatListNew != null)
                                        {
                                            _context.ESHOP_NEWS_PROPERTIES.Remove(CatListNew);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //ESHOP_NEWS_PROPERTIES nc = new ESHOP_NEWS_PROPERTIES();
                                            //nc.PROP_ID = idPro;
                                            //nc.NEWS_ID = item.NEWS_ID;
                                            //_context.Add(nc);
                                            //await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            ViewBag.Error = "Gỡ bỏ thuộc tính thành công";
                            break;
                    }

                }

                var Categorieslist = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE != 5 && x.CAT_PARENT_ID == 0 && (x.CAT_TYPE == 1 || x.CAT_TYPE == 2 || x.CAT_TYPE == 7 || x.CAT_TYPE == 8)).Select(p => new ESHOP_CATEGORIES
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

                List<TreeViewNode> nodes = new List<TreeViewNode>();

                ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


                foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0 && x.PROP_ACTIVE == 2))
                {
                    nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = true } });
                }

                foreach (var itemOne in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0 && x.PROP_ACTIVE == 2))
                {
                    foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == itemOne.PROP_ID))
                    {
                        nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false } });
                    }
                }

                //Serialize to JSON string.
                ViewBag.Json = JsonConvert.SerializeObject(nodes);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(int? id, string CLASSName, string value)
        {
            try
            {
                var entity = await _context.ESHOP_NEWS.FindAsync(id);

                if (entity != null)
                {
                    if (CLASSName == "NEWS_CODE")
                    {
                        entity.NEWS_CODE = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_TITLE")
                    {
                        StringBuilder newStringBuilder = new StringBuilder();
                        newStringBuilder.Append(value.Normalize(NormalizationForm.FormKD).Where(x => x < 128).ToArray());
                        string valueName = newStringBuilder.ToString().Replace(" ", "-");
                        valueName = valueName.ToLower().Replace("--", "-").Replace("/", "");

                        entity.NEWS_TITLE = value;
                        entity.NEWS_SEO_URL = valueName;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();

                    }
                    else if (CLASSName == "NEWS_TITLE_EN")
                    {
                        StringBuilder newStringBuilder = new StringBuilder();
                        newStringBuilder.Append(value.Normalize(NormalizationForm.FormKD).Where(x => x < 128).ToArray());
                        string valueName = newStringBuilder.ToString().Replace(" ", "-");
                        valueName = valueName.ToLower().Replace("--", "-").Replace("/", "");

                        entity.NEWS_TITLE_EN = value;
                        entity.NEWS_SEO_URL_EN = valueName;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_FIELD4")
                    {
                        entity.NEWS_FIELD4 = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_TIME_AVALBLE")
                    {
                        entity.NEWS_TIME_AVALBLE = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_SEO_URL_JS")
                    {
                        entity.NEWS_SEO_URL_JS = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_HTML_EN3")
                    {
                        entity.NEWS_HTML_EN3 = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_PRICE1")
                    {
                        entity.NEWS_PRICE1 = decimal.Parse(value);
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_HTML_EN1")
                    {
                        entity.NEWS_HTML_EN1 = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    else if (CLASSName == "NEWS_HTML_EN2")
                    {
                        entity.NEWS_HTML_EN3 = value;
                        entity.NEWS_UPDATE = DateTime.Now;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }

                    return Ok(new { status = true, message = "Cập nhật thành công ! với :" + CLASSName + " và giá trị là : " + value });
                }
                else
                {
                    return Ok(new { status = false, message = "Không thành công !" });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public async Task<IActionResult> NewsProCheckPopup(int id)
        {
            var eSHOP_NEWS = await _context.ESHOP_NEWS.SingleOrDefaultAsync(m => m.NEWS_ID == id);
            if (eSHOP_NEWS == null)
            {
                return NotFound();
            }

            ViewBag.Title = eSHOP_NEWS.NEWS_TITLE;
            ViewBag.Code = eSHOP_NEWS.NEWS_CODE;
            ViewBag.NewsId = id;

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = false } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            return PartialView("~/Areas/Admin/Views/News/NewsProCheckPopup.cshtml");
        }


        [HttpGet]
        public async Task<JsonResult> GetNewsListSelle(string txtSearch, int? page, int trang, int catId, int CusId)
        {
            int pageSize = trang;

            List<ESHOP_NEWS> oditemAddList = new List<ESHOP_NEWS>();

            var NewsList = await (_context.ESHOP_NEWS.Where(x => x.NEWS_TYPE == 1).Select(p => new ESHOP_NEWS
            {
                NEWS_ID = p.NEWS_ID,
                NEWS_TITLE = p.NEWS_TITLE,
                NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                NEWS_ORDER = p.NEWS_ORDER,
                NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                NEWS_IMAGE1 = p.NEWS_IMAGE1,
                NEWS_CODE = p.NEWS_CODE,
                NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                NEWS_FIELD3 = p.NEWS_FIELD3,
                NEWS_FIELD4 = p.NEWS_FIELD4,
                NEWS_PRICE1 = p.NEWS_PRICE1,
                NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                NEWS_UPDATE = p.NEWS_UPDATE,
                NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                USER_ID = p.USER_ID,
                NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
            }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();

            NewsList = (from n in NewsList.ToList()
                        join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                            where cs.CUSTOMER_ID == CusId
                            select new
                            {
                                n.NEWS_TITLE,
                                n.NEWS_PUBLISHDATE,
                                n.NEWS_SEO_URL,
                                n.NEWS_URL,
                                n.NEWS_PRICE1,
                                n.NEWS_PRICE2,
                                n.NEWS_CODE,
                                n.NEWS_IMAGE1,
                                n.NEWS_IMAGE2,
                                n.NEWS_ID,
                                n.NEWS_FILEHTML,
                                n.NEWS_FIELD4,
                                n.NEWS_HTML_EN1,
                                n.NEWS_HTML_EN2,
                                n.NEWS_HTML_EN3,
                                n.NEWS_TYPE,
                                n.NEWS_TITLE_EN,
                                n.NEWS_TITLE_JS,
                                n.NEWS_SHOWTYPE,
                                n.NEWS_FIELD3,
                                n.NEWS_SEO_URL_JS,
                                n.NEWS_UPDATE,
                                n.USER_ID,
                                n.NEWS_TIME_AVALBLE
                            }).Select(n => new ESHOP_NEWS
                            {
                                NEWS_TITLE = n.NEWS_TITLE,
                                NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                NEWS_SEO_URL = n.NEWS_SEO_URL,
                                NEWS_URL = n.NEWS_URL,
                                NEWS_PRICE1 = n.NEWS_PRICE1,
                                NEWS_PRICE2 = n.NEWS_PRICE2,
                                NEWS_CODE = n.NEWS_CODE,
                                NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                NEWS_ID = n.NEWS_ID,
                                NEWS_FILEHTML = n.NEWS_FILEHTML,
                                NEWS_FIELD4 = n.NEWS_FIELD4,
                                NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                NEWS_TYPE = n.NEWS_TYPE,
                                NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                NEWS_FIELD3 = n.NEWS_FIELD3,
                                NEWS_UPDATE = n.NEWS_UPDATE,
                                NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                USER_ID = n.USER_ID,
                                NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                            }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                if (Utils.CIntDef(catId) != 0)
                {
                    var NewsListTitle = (from n in NewsList
                                         join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                         where nc.CAT_ID == catId && (n.NEWS_TITLE.ToLower().Contains(txtSearch.ToLower()))
                                         select new
                                         {
                                             n.NEWS_TITLE,
                                             n.NEWS_PUBLISHDATE,
                                             n.NEWS_SEO_URL,
                                             n.NEWS_URL,
                                             n.NEWS_PRICE1,
                                             n.NEWS_PRICE2,
                                             n.NEWS_CODE,
                                             n.NEWS_IMAGE1,
                                             n.NEWS_IMAGE2,
                                             n.NEWS_ID,
                                             n.NEWS_FILEHTML,
                                             n.NEWS_FIELD4,
                                             n.NEWS_HTML_EN1,
                                             n.NEWS_HTML_EN2,
                                             n.NEWS_HTML_EN3,
                                             n.NEWS_TYPE,
                                             n.NEWS_TITLE_EN,
                                             n.NEWS_TITLE_JS,
                                             n.NEWS_SHOWTYPE,
                                             n.NEWS_FIELD3,
                                             n.NEWS_SEO_URL_JS,
                                             n.NEWS_UPDATE,
                                             n.USER_ID,
                                             n.NEWS_TIME_AVALBLE
                                         }).Select(n => new ESHOP_NEWS
                                         {
                                             NEWS_TITLE = n.NEWS_TITLE,
                                             NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                             NEWS_SEO_URL = n.NEWS_SEO_URL,
                                             NEWS_URL = n.NEWS_URL,
                                             NEWS_PRICE1 = n.NEWS_PRICE1,
                                             NEWS_PRICE2 = n.NEWS_PRICE2,
                                             NEWS_CODE = n.NEWS_CODE,
                                             NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                             NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                             NEWS_ID = n.NEWS_ID,
                                             NEWS_FILEHTML = n.NEWS_FILEHTML,
                                             NEWS_FIELD4 = n.NEWS_FIELD4,
                                             NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                             NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                             NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                             NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                             NEWS_TYPE = n.NEWS_TYPE,
                                             NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                             NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                             NEWS_FIELD3 = n.NEWS_FIELD3,
                                             NEWS_UPDATE = n.NEWS_UPDATE,
                                             NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                             USER_ID = n.USER_ID,
                                             NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                         }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsListTitle != null)
                    {
                        oditemAddList.AddRange(NewsListTitle);
                    }

                    var NewsListCode = (from n in NewsList
                                        join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                        where nc.CAT_ID == catId && (n.NEWS_CODE.ToLower().Contains(txtSearch.ToLower()))
                                        select new
                                        {
                                            n.NEWS_TITLE,
                                            n.NEWS_PUBLISHDATE,
                                            n.NEWS_SEO_URL,
                                            n.NEWS_URL,
                                            n.NEWS_PRICE1,
                                            n.NEWS_PRICE2,
                                            n.NEWS_CODE,
                                            n.NEWS_IMAGE1,
                                            n.NEWS_IMAGE2,
                                            n.NEWS_ID,
                                            n.NEWS_FILEHTML,
                                            n.NEWS_FIELD4,
                                            n.NEWS_HTML_EN1,
                                            n.NEWS_HTML_EN2,
                                            n.NEWS_HTML_EN3,
                                            n.NEWS_TYPE,
                                            n.NEWS_TITLE_EN,
                                            n.NEWS_TITLE_JS,
                                            n.NEWS_SHOWTYPE,
                                            n.NEWS_FIELD3,
                                            n.NEWS_SEO_URL_JS,
                                            n.NEWS_UPDATE,
                                            n.USER_ID,
                                            n.NEWS_TIME_AVALBLE
                                        }).Select(n => new ESHOP_NEWS
                                        {
                                            NEWS_TITLE = n.NEWS_TITLE,
                                            NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                            NEWS_SEO_URL = n.NEWS_SEO_URL,
                                            NEWS_URL = n.NEWS_URL,
                                            NEWS_PRICE1 = n.NEWS_PRICE1,
                                            NEWS_PRICE2 = n.NEWS_PRICE2,
                                            NEWS_CODE = n.NEWS_CODE,
                                            NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                            NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                            NEWS_ID = n.NEWS_ID,
                                            NEWS_FILEHTML = n.NEWS_FILEHTML,
                                            NEWS_FIELD4 = n.NEWS_FIELD4,
                                            NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                            NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                            NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                            NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                            NEWS_TYPE = n.NEWS_TYPE,
                                            NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                            NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                            NEWS_FIELD3 = n.NEWS_FIELD3,
                                            NEWS_UPDATE = n.NEWS_UPDATE,
                                            NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                            USER_ID = n.USER_ID,
                                            NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                        }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsListCode != null)
                    {
                        oditemAddList.AddRange(NewsListCode);
                    }
                }
                else
                {
                    var NewsListtitle = (NewsList.Where(n => (n.NEWS_TITLE.ToLower().Contains(txtSearch.ToLower()))).Select(p => new ESHOP_NEWS
                    {
                        NEWS_ID = p.NEWS_ID,
                        NEWS_TITLE = p.NEWS_TITLE,
                        NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                        NEWS_ORDER = p.NEWS_ORDER,
                        NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                        NEWS_IMAGE1 = p.NEWS_IMAGE1,
                        NEWS_CODE = p.NEWS_CODE,
                        NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                        NEWS_FIELD3 = p.NEWS_FIELD3,
                        NEWS_FIELD4 = p.NEWS_FIELD4,
                        NEWS_PRICE1 = p.NEWS_PRICE1,
                        NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                        NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                        NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                        NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                        NEWS_UPDATE = p.NEWS_UPDATE,
                        USER_ID = p.USER_ID,
                        NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
                    }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToList();
                    if (NewsListtitle != null)
                    {
                        oditemAddList.AddRange(NewsListtitle);
                    }

                    var NewsListcode = (NewsList.Where(x => x.NEWS_CODE != null).ToList().Where(n => (n.NEWS_CODE.ToLower().Contains(txtSearch.ToLower()))).Select(p => new ESHOP_NEWS
                    {
                        NEWS_ID = p.NEWS_ID,
                        NEWS_TITLE = p.NEWS_TITLE,
                        NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                        NEWS_ORDER = p.NEWS_ORDER,
                        NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                        NEWS_IMAGE1 = p.NEWS_IMAGE1,
                        NEWS_CODE = p.NEWS_CODE,
                        NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                        NEWS_FIELD3 = p.NEWS_FIELD3,
                        NEWS_FIELD4 = p.NEWS_FIELD4,
                        NEWS_PRICE1 = p.NEWS_PRICE1,
                        NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                        NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                        NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                        NEWS_SEO_URL_JS = p.NEWS_SEO_URL_JS,
                        NEWS_UPDATE = p.NEWS_UPDATE,
                        USER_ID = p.USER_ID,
                        NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE
                    }).AsEnumerable().OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToList();
                    if (NewsListcode != null)
                    {
                        oditemAddList.AddRange(NewsListcode);
                    }
                }
            }
            else
            {
                if (Utils.CIntDef(catId) != 0)
                {
                    NewsList = (from n in NewsList
                                join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                where nc.CAT_ID == catId
                                select new
                                {
                                    n.NEWS_TITLE,
                                    n.NEWS_PUBLISHDATE,
                                    n.NEWS_SEO_URL,
                                    n.NEWS_URL,
                                    n.NEWS_PRICE1,
                                    n.NEWS_PRICE2,
                                    n.NEWS_CODE,
                                    n.NEWS_IMAGE1,
                                    n.NEWS_IMAGE2,
                                    n.NEWS_ID,
                                    n.NEWS_FILEHTML,
                                    n.NEWS_FIELD4,
                                    n.NEWS_HTML_EN1,
                                    n.NEWS_HTML_EN2,
                                    n.NEWS_HTML_EN3,
                                    n.NEWS_TYPE,
                                    n.NEWS_TITLE_EN,
                                    n.NEWS_TITLE_JS,
                                    n.NEWS_SHOWTYPE,
                                    n.NEWS_FIELD3,
                                    n.NEWS_SEO_URL_JS,
                                    n.NEWS_UPDATE,
                                    n.USER_ID,
                                    n.NEWS_TIME_AVALBLE
                                }).Select(n => new ESHOP_NEWS
                                {
                                    NEWS_TITLE = n.NEWS_TITLE,
                                    NEWS_PUBLISHDATE = n.NEWS_PUBLISHDATE,
                                    NEWS_SEO_URL = n.NEWS_SEO_URL,
                                    NEWS_URL = n.NEWS_URL,
                                    NEWS_PRICE1 = n.NEWS_PRICE1,
                                    NEWS_PRICE2 = n.NEWS_PRICE2,
                                    NEWS_CODE = n.NEWS_CODE,
                                    NEWS_IMAGE1 = n.NEWS_IMAGE1,
                                    NEWS_IMAGE2 = n.NEWS_IMAGE2,
                                    NEWS_ID = n.NEWS_ID,
                                    NEWS_FILEHTML = n.NEWS_FILEHTML,
                                    NEWS_FIELD4 = n.NEWS_FIELD4,
                                    NEWS_HTML_EN1 = n.NEWS_HTML_EN1,
                                    NEWS_HTML_EN2 = n.NEWS_HTML_EN2,
                                    NEWS_HTML_EN3 = n.NEWS_HTML_EN3,
                                    NEWS_TITLE_EN = n.NEWS_TITLE_EN,
                                    NEWS_TYPE = n.NEWS_TYPE,
                                    NEWS_TITLE_JS = n.NEWS_TITLE_JS,
                                    NEWS_SHOWTYPE = n.NEWS_SHOWTYPE,
                                    NEWS_FIELD3 = n.NEWS_FIELD3,
                                    NEWS_UPDATE = n.NEWS_UPDATE,
                                    NEWS_SEO_URL_JS = n.NEWS_SEO_URL_JS,
                                    USER_ID = n.USER_ID,
                                    NEWS_TIME_AVALBLE = n.NEWS_TIME_AVALBLE
                                }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();
                    if (NewsList != null)
                    {
                        oditemAddList.AddRange(NewsList);
                    }
                }
                else
                {
                    oditemAddList.AddRange(NewsList);
                }
            }


            var result = oditemAddList.GroupBy(x => x.NEWS_ID).Select(y => y.First());
            if (groupTyoe == 0)
            {

            }
            else
            {
                result = result.Where(x => x.USER_ID == IdUser);
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

            int totalPage = result.Count();

            float totalNumsize = (totalPage / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);

            ViewBag.numSize = numSize;

            var dataPost = result.OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(start).Take(pageSize);

            // return Json(listPost);
            //return Json(new { data = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            return Json(new { data = dataPost, pageCurrent = page, numSize = numSize });
        }

        public IActionResult IndexSeller(int id)
        {
            ViewBag.ID = id;
            return View();
        }
        #endregion
    }
}
