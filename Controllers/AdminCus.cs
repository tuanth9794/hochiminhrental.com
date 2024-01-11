using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreCnice.Connect;
using CoreCnice.Domain;
using CoreCnice.UtilsCs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using CoreCnice.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LazZiya.ImageResize;
using CoreCnice.Areas.Admin.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulSoftCMS.Controllers
{
    [Authorize]
    public class AdminCusController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        string SessionKeyName = "CustomerSesion1";
        string SessionKeyName1 = "CustomerSesion";
        clsFormat fm = new clsFormat();
        private readonly IHostingEnvironment he;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AdminCusController(IHostingEnvironment e, IHttpContextAccessor httpContextAccessor)
        {
            he = e;
            this.httpContextAccessor = httpContextAccessor;
        }

        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([Bind("USER_UN,USER_PW")] ESHOP_USER eSHOP_USER)
        {
            if (String.IsNullOrEmpty(eSHOP_USER.USER_UN))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản";
                ViewBag.Display = "display:block";
            }
            else if (String.IsNullOrEmpty(eSHOP_USER.USER_PW))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu";
                ViewBag.Display = "display:block";
            }
            else
            {
                var LogResult = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_UN == eSHOP_USER.USER_UN.Trim() && m.CUSTOMER_PW == fm.Encrypt(eSHOP_USER.USER_PW.Trim()));
                if (LogResult == null)
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không tồn tại";
                    ViewBag.Display = "display:block";
                    return View();
                }
                else
                {
                    ViewBag.Error = "TÀI KHOẢN VÀ MẬT KHẨU TỒN TẠI";
                    //set the key value in Cookie  
                    Set("CustomerName", eSHOP_USER.USER_UN, 100);
                    Set("CustomerID", LogResult.CUSTOMER_ID.ToString(), 100);
                    Response.Redirect("/AdminCus/CustomerInfo", true);
                    ////Delete the cookie object  
                    //Remove("Key");          

                }
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "cookie");

            return RedirectToAction("/quantriweb");
        }

        [AllowAnonymous]
        public IActionResult IndexSeller()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            return View();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            return View();
        }

        [AllowAnonymous]
        public IActionResult PostNewsCus()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);

            var Customer = _context.ESHOP_CUSTOMER.SingleOrDefault(x => x.CUSTOMER_ID == cookieValueId);

            return View(Customer);
        }

        [AllowAnonymous]
        public IActionResult PostNewsCusReview(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            string cookieValueId = Request.Cookies["CustomerID"];

            ViewBag.idsl = Utils.CIntDef(cookieValueId);

            var ListCustomer_Request = _context.Customer_Request.FirstOrDefault(x => x.Customer_request_id == id);

            return View(ListCustomer_Request);
        }

        [AllowAnonymous]
        public IActionResult PostNewsCusEdit(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            var ListCustomer_Request = _context.Customer_Request.FirstOrDefault(x => x.Customer_request_id == id);

            return View(ListCustomer_Request);
        }


        [AllowAnonymous]
        public IActionResult CustomerSetting()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);

            var Customer = _context.ESHOP_CUSTOMER.SingleOrDefault(x => x.CUSTOMER_ID == cookieValueId);

            return View(Customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerSetting([Bind("CUSTOMER_ID,CUSTOMER_FULLNAME,CUSTOMER_UN,CUSTOMER_PW,CUSTOMER_SEX,CUSTOMER_ADDRESS,CUSTOMER_PHONE1,CUSTOMER_NEWSLETTER,CUSTOMER_PUBLISHDATE,CUSTOMER_UPDATE,CUSTOMER_OID,CUSTOMER_SHOWTYPE,CUSTOMER_TOTAL_POINT,CUSTOMER_REMAIN,CUSTOMER_FIELD1,CUSTOMER_IP,CUSTOMER_NGANSACH,CUSTOMER_LOAICANHO,CUSTOMER_THOIGIANTHUE,CUSTOMER_SONGUOIO,CUSTOMER_PHONE2,CUSTOMER_EMAIL")] ESHOP_CUSTOMER eSHOP_CUSTOMER)
        {
            int id = Utils.CIntDef(Request.Cookies["CustomerID"]);

            if (id != eSHOP_CUSTOMER.CUSTOMER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CUSTOMER);
                    await _context.SaveChangesAsync();
                    ViewBag.Error = "Cập nhật thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Error = "Cập nhật không thành công";
                    if (!ESHOP_CUSTOMERExists(eSHOP_CUSTOMER.CUSTOMER_ID))
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

            return View(eSHOP_CUSTOMER);
        }

        private bool ESHOP_CUSTOMERExists(int id)
        {
            return _context.ESHOP_CUSTOMER.Any(e => e.CUSTOMER_ID == id);
        }


        [AllowAnonymous]
        public IActionResult CustomerListRequest()
        {
            string email = "";
            int idCustomer = 0;
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;
            idCustomer = Utils.CIntDef(Request.Cookies["CustomerID"]);
            if (idCustomer != 0)
            {
                var ListCustomer_Request = _context.Customer_Request.Where(x => x.CUSTOMER_ID == idCustomer);

                return View(ListCustomer_Request);
            }
            else
            {
                var ListCustomer_Request = _context.Customer_Request.Where(x => x.Customer_Email == email);

                return View(ListCustomer_Request);
            }

        }

        [AllowAnonymous]
        public IActionResult SellerList()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            var CusList = _context.ESHOP_CUSTOMER.Where(x => x.CUSTOMER_SHOWTYPE == 2).Select(p => new ESHOP_CUSTOMER
            {
                CUSTOMER_ID = p.CUSTOMER_ID,
                CUSTOMER_FULLNAME = p.CUSTOMER_FULLNAME,
                CUSTOMER_ADDRESS = p.CUSTOMER_ADDRESS,
                CUSTOMER_PHONE1 = p.CUSTOMER_PHONE1,
                CUSTOMER_PUBLISHDATE = p.CUSTOMER_PUBLISHDATE,
            }).OrderByDescending(p => p.CUSTOMER_PUBLISHDATE);

            return View(CusList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SelerListProject()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            int idsaler = Utils.CIntDef(Request.Cookies["CustomerID"]);
            ViewBag.idsl = idsaler;
            if (idsaler != 0)
            {
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
                    NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                    NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();


                var NewsListTitle = (from n in NewsList
                                     join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                     join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                     join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                     where cs.CUSTOMER_ID == idsaler
                                     select new
                                     {
                                         n.NEWS_ID,
                                         n.NEWS_CODE,
                                         n.NEWS_TITLE,
                                         n.NEWS_DESC,
                                         n.NEWS_TARGET,
                                         n.NEWS_SEO_KEYWORD,
                                         n.NEWS_SEO_DESC,
                                         n.NEWS_SEO_TITLE,
                                         n.NEWS_SEO_URL,
                                         n.NEWS_FILEHTML,
                                         n.NEWS_PUBLISHDATE,
                                         n.NEWS_UPDATE,
                                         n.NEWS_SHOWTYPE,
                                         n.NEWS_SHOWINDETAIL,
                                         n.NEWS_FEEDBACKTYPE,
                                         n.NEWS_TYPE,
                                         n.NEWS_PERIOD,
                                         n.NEWS_ORDER_PERIOD,
                                         n.NEWS_ORDER,
                                         n.NEWS_PRICE1,
                                         n.NEWS_PRICE2,
                                         n.NEWS_PRICE3,
                                         n.NEWS_IMAGE1,
                                         n.NEWS_IMAGE2,
                                         n.NEWS_TITLE_EN,
                                         n.NEWS_DESC_EN,
                                         n.NEWS_URL,
                                         c.CAT_SEO_URL,
                                         n.NEWS_FILEHTML_EN,
                                         n.NEWS_HTML_EN1,
                                         n.NEWS_HTML_EN2,
                                         n.NEWS_HTML_EN3,
                                         n.NEWS_FIELD4,
                                         n.NEWS_TITLE_JS,
                                         n.NEWS_SEO_URL_EN,
                                         c.CAT_SEO_URL_EN
                                     }).Select(p => new NewsModelCat
                                     {
                                         NEWS_ID = p.NEWS_ID,
                                         NEWS_CODE = p.NEWS_CODE,
                                         NEWS_TITLE = p.NEWS_TITLE,
                                         NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                         NEWS_DESC_EN = p.NEWS_DESC_EN,
                                         NEWS_DESC = p.NEWS_DESC,
                                         NEWS_URL = p.NEWS_URL,
                                         NEWS_TARGET = p.NEWS_TARGET,
                                         NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                         NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                         NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                         NEWS_SEO_URL = p.NEWS_SEO_URL,
                                         NEWS_FILEHTML = p.NEWS_FILEHTML,
                                         NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                         NEWS_UPDATE = p.NEWS_UPDATE,
                                         NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                         NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                         NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                         NEWS_TYPE = p.NEWS_TYPE,
                                         NEWS_PERIOD = p.NEWS_PERIOD,
                                         NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                         NEWS_ORDER = p.NEWS_ORDER,
                                         NEWS_PRICE1 = p.NEWS_PRICE1,
                                         NEWS_PRICE2 = p.NEWS_PRICE2,
                                         NEWS_PRICE3 = p.NEWS_PRICE3,
                                         NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                         NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                         CAT_SEO_URL = p.CAT_SEO_URL,
                                         NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                         NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                         NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                         NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                         NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                         NEWS_FIELD4 = p.NEWS_FIELD4,
                                         NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                         CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                     }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                return View(NewsListTitle);
            }
            else
            {
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
                    NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                    NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();


                var NewsListTitle = (from n in NewsList
                                     join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                     join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                     join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                     select new
                                     {
                                         n.NEWS_ID,
                                         n.NEWS_CODE,
                                         n.NEWS_TITLE,
                                         n.NEWS_DESC,
                                         n.NEWS_TARGET,
                                         n.NEWS_SEO_KEYWORD,
                                         n.NEWS_SEO_DESC,
                                         n.NEWS_SEO_TITLE,
                                         n.NEWS_SEO_URL,
                                         n.NEWS_FILEHTML,
                                         n.NEWS_PUBLISHDATE,
                                         n.NEWS_UPDATE,
                                         n.NEWS_SHOWTYPE,
                                         n.NEWS_SHOWINDETAIL,
                                         n.NEWS_FEEDBACKTYPE,
                                         n.NEWS_TYPE,
                                         n.NEWS_PERIOD,
                                         n.NEWS_ORDER_PERIOD,
                                         n.NEWS_ORDER,
                                         n.NEWS_PRICE1,
                                         n.NEWS_PRICE2,
                                         n.NEWS_PRICE3,
                                         n.NEWS_IMAGE1,
                                         n.NEWS_IMAGE2,
                                         n.NEWS_TITLE_EN,
                                         n.NEWS_DESC_EN,
                                         n.NEWS_URL,
                                         c.CAT_SEO_URL,
                                         n.NEWS_FILEHTML_EN,
                                         n.NEWS_HTML_EN1,
                                         n.NEWS_HTML_EN2,
                                         n.NEWS_HTML_EN3,
                                         n.NEWS_FIELD4,
                                         n.NEWS_TITLE_JS,
                                         n.NEWS_SEO_URL_EN,
                                         c.CAT_SEO_URL_EN
                                     }).Select(p => new NewsModelCat
                                     {
                                         NEWS_ID = p.NEWS_ID,
                                         NEWS_CODE = p.NEWS_CODE,
                                         NEWS_TITLE = p.NEWS_TITLE,
                                         NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                         NEWS_DESC_EN = p.NEWS_DESC_EN,
                                         NEWS_DESC = p.NEWS_DESC,
                                         NEWS_URL = p.NEWS_URL,
                                         NEWS_TARGET = p.NEWS_TARGET,
                                         NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                         NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                         NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                         NEWS_SEO_URL = p.NEWS_SEO_URL,
                                         NEWS_FILEHTML = p.NEWS_FILEHTML,
                                         NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                         NEWS_UPDATE = p.NEWS_UPDATE,
                                         NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                         NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                         NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                         NEWS_TYPE = p.NEWS_TYPE,
                                         NEWS_PERIOD = p.NEWS_PERIOD,
                                         NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                         NEWS_ORDER = p.NEWS_ORDER,
                                         NEWS_PRICE1 = p.NEWS_PRICE1,
                                         NEWS_PRICE2 = p.NEWS_PRICE2,
                                         NEWS_PRICE3 = p.NEWS_PRICE3,
                                         NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                         NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                         CAT_SEO_URL = p.CAT_SEO_URL,
                                         NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                         NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                         NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                         NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                         NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                         NEWS_FIELD4 = p.NEWS_FIELD4,
                                         NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                         CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                     }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                return View(NewsListTitle);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ProjectListAprove(int idsaler)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);

            if (idsaler != 0)
            {
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
                    NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                    NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();


                var NewsListTitle = (from n in NewsList
                                     join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                     join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                     join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                     where cs.CUSTOMER_ID == idsaler
                                     select new
                                     {
                                         n.NEWS_ID,
                                         n.NEWS_CODE,
                                         n.NEWS_TITLE,
                                         n.NEWS_DESC,
                                         n.NEWS_TARGET,
                                         n.NEWS_SEO_KEYWORD,
                                         n.NEWS_SEO_DESC,
                                         n.NEWS_SEO_TITLE,
                                         n.NEWS_SEO_URL,
                                         n.NEWS_FILEHTML,
                                         n.NEWS_PUBLISHDATE,
                                         n.NEWS_UPDATE,
                                         n.NEWS_SHOWTYPE,
                                         n.NEWS_SHOWINDETAIL,
                                         n.NEWS_FEEDBACKTYPE,
                                         n.NEWS_TYPE,
                                         n.NEWS_PERIOD,
                                         n.NEWS_ORDER_PERIOD,
                                         n.NEWS_ORDER,
                                         n.NEWS_PRICE1,
                                         n.NEWS_PRICE2,
                                         n.NEWS_PRICE3,
                                         n.NEWS_IMAGE1,
                                         n.NEWS_IMAGE2,
                                         n.NEWS_TITLE_EN,
                                         n.NEWS_DESC_EN,
                                         n.NEWS_URL,
                                         c.CAT_SEO_URL,
                                         n.NEWS_FILEHTML_EN,
                                         n.NEWS_HTML_EN1,
                                         n.NEWS_HTML_EN2,
                                         n.NEWS_HTML_EN3,
                                         n.NEWS_FIELD4,
                                         n.NEWS_TITLE_JS,
                                         n.NEWS_SEO_URL_EN,
                                         c.CAT_SEO_URL_EN
                                     }).Select(p => new NewsModelCat
                                     {
                                         NEWS_ID = p.NEWS_ID,
                                         NEWS_CODE = p.NEWS_CODE,
                                         NEWS_TITLE = p.NEWS_TITLE,
                                         NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                         NEWS_DESC_EN = p.NEWS_DESC_EN,
                                         NEWS_DESC = p.NEWS_DESC,
                                         NEWS_URL = p.NEWS_URL,
                                         NEWS_TARGET = p.NEWS_TARGET,
                                         NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                         NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                         NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                         NEWS_SEO_URL = p.NEWS_SEO_URL,
                                         NEWS_FILEHTML = p.NEWS_FILEHTML,
                                         NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                         NEWS_UPDATE = p.NEWS_UPDATE,
                                         NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                         NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                         NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                         NEWS_TYPE = p.NEWS_TYPE,
                                         NEWS_PERIOD = p.NEWS_PERIOD,
                                         NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                         NEWS_ORDER = p.NEWS_ORDER,
                                         NEWS_PRICE1 = p.NEWS_PRICE1,
                                         NEWS_PRICE2 = p.NEWS_PRICE2,
                                         NEWS_PRICE3 = p.NEWS_PRICE3,
                                         NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                         NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                         CAT_SEO_URL = p.CAT_SEO_URL,
                                         NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                         NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                         NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                         NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                         NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                         NEWS_FIELD4 = p.NEWS_FIELD4,
                                         NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                         CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                     }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                return View(NewsListTitle);
            }
            else
            {
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
                    NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                    NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();


                var NewsListTitle = (from n in NewsList
                                     join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                     join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                     join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                     select new
                                     {
                                         n.NEWS_ID,
                                         n.NEWS_CODE,
                                         n.NEWS_TITLE,
                                         n.NEWS_DESC,
                                         n.NEWS_TARGET,
                                         n.NEWS_SEO_KEYWORD,
                                         n.NEWS_SEO_DESC,
                                         n.NEWS_SEO_TITLE,
                                         n.NEWS_SEO_URL,
                                         n.NEWS_FILEHTML,
                                         n.NEWS_PUBLISHDATE,
                                         n.NEWS_UPDATE,
                                         n.NEWS_SHOWTYPE,
                                         n.NEWS_SHOWINDETAIL,
                                         n.NEWS_FEEDBACKTYPE,
                                         n.NEWS_TYPE,
                                         n.NEWS_PERIOD,
                                         n.NEWS_ORDER_PERIOD,
                                         n.NEWS_ORDER,
                                         n.NEWS_PRICE1,
                                         n.NEWS_PRICE2,
                                         n.NEWS_PRICE3,
                                         n.NEWS_IMAGE1,
                                         n.NEWS_IMAGE2,
                                         n.NEWS_TITLE_EN,
                                         n.NEWS_DESC_EN,
                                         n.NEWS_URL,
                                         c.CAT_SEO_URL,
                                         n.NEWS_FILEHTML_EN,
                                         n.NEWS_HTML_EN1,
                                         n.NEWS_HTML_EN2,
                                         n.NEWS_HTML_EN3,
                                         n.NEWS_FIELD4,
                                         n.NEWS_TITLE_JS,
                                         n.NEWS_SEO_URL_EN,
                                         c.CAT_SEO_URL_EN
                                     }).Select(p => new NewsModelCat
                                     {
                                         NEWS_ID = p.NEWS_ID,
                                         NEWS_CODE = p.NEWS_CODE,
                                         NEWS_TITLE = p.NEWS_TITLE,
                                         NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                         NEWS_DESC_EN = p.NEWS_DESC_EN,
                                         NEWS_DESC = p.NEWS_DESC,
                                         NEWS_URL = p.NEWS_URL,
                                         NEWS_TARGET = p.NEWS_TARGET,
                                         NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                         NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                         NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                         NEWS_SEO_URL = p.NEWS_SEO_URL,
                                         NEWS_FILEHTML = p.NEWS_FILEHTML,
                                         NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                         NEWS_UPDATE = p.NEWS_UPDATE,
                                         NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                         NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                         NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                         NEWS_TYPE = p.NEWS_TYPE,
                                         NEWS_PERIOD = p.NEWS_PERIOD,
                                         NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                         NEWS_ORDER = p.NEWS_ORDER,
                                         NEWS_PRICE1 = p.NEWS_PRICE1,
                                         NEWS_PRICE2 = p.NEWS_PRICE2,
                                         NEWS_PRICE3 = p.NEWS_PRICE3,
                                         NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                         NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                         CAT_SEO_URL = p.CAT_SEO_URL,
                                         NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                         NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                         NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                         NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                         NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                         NEWS_FIELD4 = p.NEWS_FIELD4,
                                         NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                         CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                     }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                return View(NewsListTitle);
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> ProjectList(int idsaler)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);

            ViewBag.CustomerId = cookieValueId;

            if (idsaler != 0)
            {
                return View();
            }
            else
            {
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
                    NEWS_TIME_AVALBLE = p.NEWS_TIME_AVALBLE,
                    NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN
                }).OrderByDescending(p => p.NEWS_PUBLISHDATE)).ToListAsync();


                var NewsListTitle = (from n in NewsList
                                     join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                     join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                     join cs in _context.NEWS_CUSTOMER_POST on n.NEWS_ID equals cs.NEWS_ID
                                     select new
                                     {
                                         n.NEWS_ID,
                                         n.NEWS_CODE,
                                         n.NEWS_TITLE,
                                         n.NEWS_DESC,
                                         n.NEWS_TARGET,
                                         n.NEWS_SEO_KEYWORD,
                                         n.NEWS_SEO_DESC,
                                         n.NEWS_SEO_TITLE,
                                         n.NEWS_SEO_URL,
                                         n.NEWS_FILEHTML,
                                         n.NEWS_PUBLISHDATE,
                                         n.NEWS_UPDATE,
                                         n.NEWS_SHOWTYPE,
                                         n.NEWS_SHOWINDETAIL,
                                         n.NEWS_FEEDBACKTYPE,
                                         n.NEWS_TYPE,
                                         n.NEWS_PERIOD,
                                         n.NEWS_ORDER_PERIOD,
                                         n.NEWS_ORDER,
                                         n.NEWS_PRICE1,
                                         n.NEWS_PRICE2,
                                         n.NEWS_PRICE3,
                                         n.NEWS_IMAGE1,
                                         n.NEWS_IMAGE2,
                                         n.NEWS_TITLE_EN,
                                         n.NEWS_DESC_EN,
                                         n.NEWS_URL,
                                         c.CAT_SEO_URL,
                                         n.NEWS_FILEHTML_EN,
                                         n.NEWS_HTML_EN1,
                                         n.NEWS_HTML_EN2,
                                         n.NEWS_HTML_EN3,
                                         n.NEWS_FIELD4,
                                         n.NEWS_TITLE_JS,
                                         n.NEWS_SEO_URL_EN,
                                         c.CAT_SEO_URL_EN
                                     }).Select(p => new NewsModelCat
                                     {
                                         NEWS_ID = p.NEWS_ID,
                                         NEWS_CODE = p.NEWS_CODE,
                                         NEWS_TITLE = p.NEWS_TITLE,
                                         NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                         NEWS_DESC_EN = p.NEWS_DESC_EN,
                                         NEWS_DESC = p.NEWS_DESC,
                                         NEWS_URL = p.NEWS_URL,
                                         NEWS_TARGET = p.NEWS_TARGET,
                                         NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                         NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                         NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                         NEWS_SEO_URL = p.NEWS_SEO_URL,
                                         NEWS_FILEHTML = p.NEWS_FILEHTML,
                                         NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                         NEWS_UPDATE = p.NEWS_UPDATE,
                                         NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                         NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                         NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                         NEWS_TYPE = p.NEWS_TYPE,
                                         NEWS_PERIOD = p.NEWS_PERIOD,
                                         NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                         NEWS_ORDER = p.NEWS_ORDER,
                                         NEWS_PRICE1 = p.NEWS_PRICE1,
                                         NEWS_PRICE2 = p.NEWS_PRICE2,
                                         NEWS_PRICE3 = p.NEWS_PRICE3,
                                         NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                         NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                         CAT_SEO_URL = p.CAT_SEO_URL,
                                         NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                         NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                         NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                         NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                         NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                         NEWS_FIELD4 = p.NEWS_FIELD4,
                                         NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                         CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                     }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                NewsListTitle = (from n in NewsListTitle
                                 join cus in _context.CustomerConfirm on n.NEWS_ID equals cus.NEWS_ID
                                 where cus.CUSTOMER_ID == cookieValueId
                                 select new
                                 {
                                     n.NEWS_ID,
                                     n.NEWS_CODE,
                                     n.NEWS_TITLE,
                                     n.NEWS_DESC,
                                     n.NEWS_TARGET,
                                     n.NEWS_SEO_KEYWORD,
                                     n.NEWS_SEO_DESC,
                                     n.NEWS_SEO_TITLE,
                                     n.NEWS_SEO_URL,
                                     n.NEWS_FILEHTML,
                                     n.NEWS_PUBLISHDATE,
                                     n.NEWS_UPDATE,
                                     n.NEWS_SHOWTYPE,
                                     n.NEWS_SHOWINDETAIL,
                                     n.NEWS_FEEDBACKTYPE,
                                     n.NEWS_TYPE,
                                     n.NEWS_PERIOD,
                                     n.NEWS_ORDER_PERIOD,
                                     n.NEWS_ORDER,
                                     n.NEWS_PRICE1,
                                     n.NEWS_PRICE2,
                                     n.NEWS_PRICE3,
                                     n.NEWS_IMAGE1,
                                     n.NEWS_IMAGE2,
                                     n.NEWS_TITLE_EN,
                                     n.NEWS_DESC_EN,
                                     n.NEWS_URL,
                                     n.CAT_SEO_URL,
                                     n.NEWS_FILEHTML_EN,
                                     n.NEWS_HTML_EN1,
                                     n.NEWS_HTML_EN2,
                                     n.NEWS_HTML_EN3,
                                     n.NEWS_FIELD4,
                                     n.NEWS_TITLE_JS,
                                     n.NEWS_SEO_URL_EN,
                                     n.CAT_SEO_URL_EN
                                 }).Select(p => new NewsModelCat
                                 {
                                     NEWS_ID = p.NEWS_ID,
                                     NEWS_CODE = p.NEWS_CODE,
                                     NEWS_TITLE = p.NEWS_TITLE,
                                     NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                     NEWS_DESC_EN = p.NEWS_DESC_EN,
                                     NEWS_DESC = p.NEWS_DESC,
                                     NEWS_URL = p.NEWS_URL,
                                     NEWS_TARGET = p.NEWS_TARGET,
                                     NEWS_SEO_KEYWORD = p.NEWS_SEO_KEYWORD,
                                     NEWS_SEO_DESC = p.NEWS_SEO_DESC,
                                     NEWS_SEO_TITLE = p.NEWS_SEO_TITLE,
                                     NEWS_SEO_URL = p.NEWS_SEO_URL,
                                     NEWS_FILEHTML = p.NEWS_FILEHTML,
                                     NEWS_PUBLISHDATE = p.NEWS_PUBLISHDATE,
                                     NEWS_UPDATE = p.NEWS_UPDATE,
                                     NEWS_SHOWTYPE = p.NEWS_SHOWTYPE,
                                     NEWS_SHOWINDETAIL = p.NEWS_SHOWINDETAIL,
                                     NEWS_FEEDBACKTYPE = p.NEWS_FEEDBACKTYPE,
                                     NEWS_TYPE = p.NEWS_TYPE,
                                     NEWS_PERIOD = p.NEWS_PERIOD,
                                     NEWS_ORDER_PERIOD = p.NEWS_ORDER_PERIOD,
                                     NEWS_ORDER = p.NEWS_ORDER,
                                     NEWS_PRICE1 = p.NEWS_PRICE1,
                                     NEWS_PRICE2 = p.NEWS_PRICE2,
                                     NEWS_PRICE3 = p.NEWS_PRICE3,
                                     NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                     NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                     CAT_SEO_URL = p.CAT_SEO_URL,
                                     NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                     NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                     NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                     NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                     NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                     NEWS_FIELD4 = p.NEWS_FIELD4,
                                     NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                     CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                                 }).Distinct().Take(100).OrderByDescending(x => x.NEWS_PUBLISHDATE).ToList();

                return View(NewsListTitle);
            }

        }



        [AllowAnonymous]
        public IActionResult RegisterSeller()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;
            ViewBag.Display = "display:none";
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(string hovaten, string sodienthoai, string gioitinh, string loaitk, string diachiemail, string diachi, string quanhuyen, string tinhthanh, string magioithieu, string tentaikhoan, string matkhaudn, string taikhoandk, string matkhaudk, string CLICK)
        {
            if (ModelState.IsValid)
            {
                tentaikhoan = diachiemail;
                if (String.IsNullOrEmpty(tentaikhoan))
                {
                    ViewBag.Error = "Vui lòng nhập tài khoản";
                    ViewBag.Display = "display:block";
                }
                else if (String.IsNullOrEmpty(matkhaudn))
                {
                    ViewBag.Error = "Vui lòng nhập mật khẩu";
                    ViewBag.Display = "display:block";
                }
                else
                {
                    var LogResult = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_UN == tentaikhoan.Trim());
                    if (LogResult != null)
                    {
                        ViewBag.Error = "Tài khoản đăng ký đã tồn tại";
                        ViewBag.Display = "display:block";
                    }
                    else
                    {
                        var LogResultEmail = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_EMAIL == tentaikhoan.Trim());
                        if (LogResult != null)
                        {
                            ViewBag.Error = "Email đăng ký đã tồn tại";
                            ViewBag.Display = "display:block";
                        }
                        else
                        {
                            int id = 1;
                            ESHOP_CUSTOMER cs = new ESHOP_CUSTOMER();
                            cs.CUSTOMER_PW = fm.Encrypt(matkhaudn);
                            cs.CUSTOMER_UN = tentaikhoan;
                            cs.CUSTOMER_FULLNAME = hovaten;
                            cs.CUSTOMER_PHONE1 = sodienthoai;
                            cs.CUSTOMER_SEX = Utils.CIntDef(gioitinh);
                            cs.CUSTOMER_ADDRESS = diachi;
                            cs.CUSTOMER_KHUVU = quanhuyen;
                            cs.CUSTOMER_SHOWTYPE = Utils.CIntDef(loaitk);
                            cs.CUSTOMER_FIELD3 = tinhthanh;
                            cs.CUSTOMER_FIELD4 = magioithieu;
                            cs.CUSTOMER_EMAIL = diachiemail;
                            cs.CUSTOMER_PUBLISHDATE = DateTime.Now;
                            _context.Add(cs);
                            await _context.SaveChangesAsync();
                            var LogResultDK = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_UN == tentaikhoan.Trim() && m.CUSTOMER_PW == fm.Encrypt(matkhaudn.Trim()));
                            if (LogResultDK == null)
                            {
                                ViewBag.Error = "Tài khoản hoặc mật khẩu không tồn tại";
                                ViewBag.Display = "display:block";
                                return View();
                            }
                            else
                            {
                                Set("CustomerName", tentaikhoan, 100);
                                Set("CustomerID", LogResultDK.CUSTOMER_ID.ToString(), 100);
                                string Body = EmailDangKy(taikhoandk, "hochiminhrental.com");

                                var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == id);

                                string emailgui = "";
                                string emailSend = "";
                                var ListEmail = _context.ESHOP_EMAIL.SingleOrDefault(x => x.EMAIL_ID == 2);
                                if (ListEmail != null)
                                {
                                    emailgui = ListEmail.EMAIL_TO;
                                    emailSend = ListEmail.EMAIL_CC;
                                }

                                SendEmailSMTP("Thông tin liên hệ từ EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, "Đăng ký tài khoản", diachiemail, sodienthoai, emailgui, emailSend, Body, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);

                                if (Utils.CIntDef(loaitk) == 1)
                                {
                                    Response.Redirect("/AdminCus/Index", true);
                                }
                                else
                                {
                                    Response.Redirect("/AdminCus/IndexSeller", true);
                                }
                                //Redirect("/Customer/CustomerInfo");

                            }
                        }    
                     
                    }
                }
            }

            return View();
        }


        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void Set(string CustomerName, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(CustomerName, value, option);
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }

        public void SendEmail(string receiver, string subject, string message)
        {
            var senderEmail = new MailAddress("jamilmoughal786@gmail.com", "Jamil");
            var receiverEmail = new MailAddress(receiver, "Receiver");
            var password = "Your Email Password here";
            var sub = subject;
            var body = message;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }

        public void SendEmailSMTP(string EmailDisplay, string EmailSend, string emailPass, string strSubject, string toAddress, string sodt, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL, string port, string sv)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toAddress);

                if (ccAddress != "")
                {
                    mail.CC.Add(ccAddress);
                }

                mail.From = new MailAddress(EmailSend);

                mail.Subject = EmailDisplay;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailSend, emailPass);
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {

            }
        }

        public string EmailDangKy(string username, string link)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_container' style='background-color:#ffffff;border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='top'>	";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_header' style='background-color:#96588a;border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:36px 48px;display:block'>";
            src += "<h1 style='color:#ffffff;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:30px;font-weight:300;line-height:150%;margin:0;text-align:left'>Chào mừng tới HOCHIMINHRENTAL</h1>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top' style='padding:48px 48px 0'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<p style='margin:0 0 16px'>Cảm ơn bạn đã tạo một tài khoản trên hochiminhrental. Tên người dùng của bạn là <strong>" + username + "</strong></p>";
            src += "<p style='margin:0 0 16px'>Bạn có thể truy cập trang tài khoản để xem các đơn hàng và thay đổi mật khẩu tại đây: <a href='" + link + "' rel='nofollow' style='color:#96588a;font-weight:normal;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://hochiminhrental.com/tai-khoan/&amp;source=gmail&amp;ust=1553785064514000&amp;usg=AFQjCNH9cNbvKVx8sKoP3b_BumplCS_O4A'>" + link + "</a>.</p>";
            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";
            src += "<p>© 2020 hochiminhrental<br>";
            src += "Hotline: 0898.303.929<br>";
            src += "Email: <a href='mailto:support@eurekahome.vn' target='_blank'>support@eurekahome.vn</a><br>";
            src += "Địa chỉ: 21 Vo Truong Toan - An Phu - Thu Duc City - HCMC.</p>";
            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }

        public string EmailQuenMK(string username, string link,string mk)
        {
            string src = "";
            src += "<table border = '0' cellpadding = '0' cellspacing = '0' height = '100%' width = '100%'><tbody><tr><td align='center' valign='top'>";
            src += " <table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_container' style='background-color:#ffffff;border:1px solid #dedede;border-radius:3px!important'>";
            src += "<tbody><tr>";
            src += "<td align='center' valign='top'>	";
            src += "<table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_header' style='background-color:#96588a;border-radius:3px 3px 0 0!important;color:#ffffff;border-bottom:0;font-weight:bold;line-height:100%;vertical-align:middle;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif'>";
            src += "<tbody><tr>";
            src += "<td id='m_1851103133717073586header_wrapper' style='padding:36px 48px;display:block'>";
            src += "<h1 style='color:#ffffff;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:30px;font-weight:300;line-height:150%;margin:0;text-align:left'>Chào mừng tới HOCHIMINHRENTAL</h1>";
            src += "</td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='600' id='m_1851103133717073586template_body'>";
            src += "<tbody><tr><td valign='top' id='m_1851103133717073586body_content' style='background-color:#ffffff'>";
            src += "<table border='0' cellpadding='20' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td valign='top' style='padding:48px 48px 0'>";
            src += "<div id='m_1851103133717073586body_content_inner' style='color:#636363;font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif;font-size:14px;line-height:150%;text-align:left'>";
            src += "<p style='margin:0 0 16px'>Cảm ơn bạn đã tạo một tài khoản trên hochiminhrental. Tên người dùng của bạn là <strong>" + username + "</strong></p>";
            src += "<p style='margin:0 0 16px'>Mật khẩu người dùng của bạn là <strong>" + mk + "</strong></p>";
            src += "<p style='margin:0 0 16px'>Bạn có thể truy cập trang tài khoản để xem các đơn hàng và thay đổi mật khẩu tại đây: <a href='" + link + "' rel='nofollow' style='color:#96588a;font-weight:normal;text-decoration:underline' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://hochiminhrental.com/tai-khoan/&amp;source=gmail&amp;ust=1553785064514000&amp;usg=AFQjCNH9cNbvKVx8sKoP3b_BumplCS_O4A'>" + link + "</a>.</p>";
            src += "</div></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
            src += "<tr><td align='center' valign='top'>";
            src += "<table border='0' cellpadding='10' cellspacing='0' width='600' id='m_1851103133717073586template_footer'><tbody><tr>";
            src += "<td valign='top' style='padding:0'><table border='0' cellpadding='10' cellspacing='0' width='100%'><tbody><tr>";
            src += "<td colspan='2' valign='middle' id='m_1851103133717073586credit' style='padding:0 48px 48px 48px;border:0;color:#c09bb9;font-family:Arial;font-size:12px;line-height:125%;text-align:center'>";
            src += "<p>© 2020 hochiminhrental<br>";
            src += "Hotline: 0898.303.929<br>";
            src += "Email: <a href='mailto:support@eurekahome.vn' target='_blank'>support@eurekahome.vn</a><br>";
            src += "Địa chỉ: 21 Vo Truong Toan - An Phu - Thu Duc City - HCMC.</p>";
            src += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            src += "</td></tr></tbody></table>";
            return src;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SellerSetting()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            string cookieValueId = Request.Cookies["CustomerID"];
            if (!String.IsNullOrEmpty(cookieValueId))
            {
                int id = Utils.CIntDef(cookieValueId);

                var eSHOP_CUSTOMER = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_ID == id);
                if (eSHOP_CUSTOMER == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(eSHOP_CUSTOMER);
                }
            }
            else
            {

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SellerSetting([Bind("CUSTOMER_ID,CUSTOMER_FULLNAME,CUSTOMER_UN,CUSTOMER_PW,CUSTOMER_SEX,CUSTOMER_ADDRESS,CUSTOMER_PHONE1,CUSTOMER_NEWSLETTER,CUSTOMER_PUBLISHDATE,CUSTOMER_UPDATE,CUSTOMER_OID,CUSTOMER_SHOWTYPE,CUSTOMER_TOTAL_POINT,CUSTOMER_REMAIN,CUSTOMER_FIELD1,CUSTOMER_IP,CUSTOMER_NGANSACH,CUSTOMER_LOAICANHO,CUSTOMER_THOIGIANTHUE,CUSTOMER_SONGUOIO,CUSTOMER_PHONE2,CUSTOMER_EMAIL")] ESHOP_CUSTOMER eSHOP_CUSTOMER)
        {
            int id = Utils.CIntDef(Request.Cookies["CustomerID"]);

            if (id != eSHOP_CUSTOMER.CUSTOMER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CUSTOMER);
                    await _context.SaveChangesAsync();
                    ViewBag.Error = "Cập nhật thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Error = "Cập nhật không thành công";
                    if (!ESHOP_CUSTOMERExists(eSHOP_CUSTOMER.CUSTOMER_ID))
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

            return View(eSHOP_CUSTOMER);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Singin()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            await HttpContext.SignOutAsync(
                 scheme: "cookie");

            ViewBag.Display = "display:none";
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SinginCustomer()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            await HttpContext.SignOutAsync(
                 scheme: "cookie");

            ViewBag.Display = "display:none";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Singin(string taikhoan, string matkhau)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản";
                ViewBag.Display = "display:block";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu";
                ViewBag.Display = "display:block";
            }
            else
            {
                var LogResult = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_UN == taikhoan.Trim() && m.CUSTOMER_PW == fm.Encrypt(matkhau.Trim())&& m.CUSTOMER_ACTIVE ==1);
                if (LogResult == null)
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không tồn tại";
                    ViewBag.Display = "display:block";
                    return View();
                }
                else
                {
                    ViewBag.Error = "TÀI KHOẢN VÀ MẬT KHẨU TỒN TẠI";
                    if (LogResult.CUSTOMER_SHOWTYPE == 2)
                    {
                        //set the key value in Cookie  
                        Set("CustomerName", LogResult.CUSTOMER_UN, 100);
                        Set("CustomerID", LogResult.CUSTOMER_ID.ToString(), 100);
                        Response.Redirect("/AdminCus/IndexSeller", true);
                        ////Delete the cookie object  
                        //Remove("Key");   
                    }
                    else
                    {
                        //set the key value in Cookie  
                        Set("CustomerName", LogResult.CUSTOMER_UN, 100);
                        Set("CustomerID", LogResult.CUSTOMER_ID.ToString(), 100);
                        Response.Redirect("/AdminCus/Index", true);
                        ////Delete the cookie object  
                        //Remove("Key");   
                    }


                }
            }
            return View();
        }


        [AllowAnonymous]
        public IActionResult SellerListRequest()
        {
            string email = "anhluong3evn@gmail.com";
            int idCustomer = 0;
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            if (idCustomer != 0)
            {
                var ListCustomer_Request = _context.Customer_Request.Where(x => x.Customer_Active == 1);

                return View(ListCustomer_Request);
            }
            else
            {
                var ListCustomer_Request = _context.Customer_Request.Where(x => x.Customer_Active == 1);

                return View(ListCustomer_Request);
            }

        }

        [AllowAnonymous]
        public IActionResult SellerPost()
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            var Cats = new List<SelectListItem>();
            //Çalışanlarımızı listemize aktarıyorum    
            foreach (var item in _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE == 1))
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SellerPost([Bind("NEWS_ID,NEWS_CODE,NEWS_TITLE,NEWS_DESC,NEWS_URL,NEWS_TARGET,NEWS_SEO_KEYWORD,NEWS_SEO_DESC,NEWS_SEO_TITLE,NEWS_SEO_URL,NEWS_FILEHTML,NEWS_PUBLISHDATE,NEWS_UPDATE,NEWS_SHOWTYPE,NEWS_SHOWINDETAIL,NEWS_FEEDBACKTYPE,NEWS_TYPE,NEWS_PERIOD,NEWS_ORDER_PERIOD,NEWS_ORDER,NEWS_PRINTTYPE,NEWS_COUNT,NEWS_SENDEMAIL,NEWS_SENDDATE,NEWS_PRICE1,NEWS_PRICE2,NEWS_PRICE3,NEWS_IMAGE1,NEWS_IMAGE2,NEWS_IMAGE3,NEWS_IMAGE4,NEWS_FIELD3,NEWS_FIELD4,NEWS_FIELD1,NEWS_FIELD5,NEWS_FIELD2,NEWS_FILEHTML_EN,NEWS_TITLE_EN,NEWS_DESC_EN,NEWS_HTML_EN1,NEWS_HTML_EN2,NEWS_HTML_EN3,NEWS_TITLE_JS,NEWS_SEO_DESC_JS,NEWS_SEO_URL_JS,NEWS_SEO_URL_EN,NEWS_SEO_META_CANONICAL,NEWS_SEO_META_DESC_EN,NEWS_LIENKET_EN,NEWS_TIME_AVALBLE")] ESHOP_NEWS eSHOP_NEWS)
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

                        int cookieValueId = Utils.CIntDef(Request.Cookies["CustomerID"]);

                        NEWS_CUSTOMER_POST cusPost = new NEWS_CUSTOMER_POST();
                        cusPost.CUSTOMER_ID = cookieValueId;
                        cusPost.NEWS_ID = eSHOP_NEWS.NEWS_ID;
                        _context.Add(cusPost);
                        await _context.SaveChangesAsync();


                        //return RedirectToAction(nameof(Index));
                        return RedirectToAction("SellerPostEdit", "AdminCus", new { id = eSHOP_NEWS.NEWS_ID });
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

        [AllowAnonymous]
        public async Task<IActionResult> SellerPostEdit(int? id)
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
            foreach (var item in _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID != 65 && x.CAT_TYPE == 1))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerPostEdit(int id, [Bind("NEWS_ID,NEWS_CODE,NEWS_TITLE,NEWS_DESC,NEWS_URL,NEWS_TARGET,NEWS_SEO_KEYWORD,NEWS_SEO_DESC,NEWS_SEO_TITLE,NEWS_SEO_URL,NEWS_FILEHTML,NEWS_PUBLISHDATE,NEWS_UPDATE,NEWS_SHOWTYPE,NEWS_SHOWINDETAIL,NEWS_FEEDBACKTYPE,NEWS_TYPE,NEWS_PERIOD,NEWS_ORDER_PERIOD,NEWS_ORDER,NEWS_PRINTTYPE,NEWS_COUNT,NEWS_SENDEMAIL,NEWS_SENDDATE,NEWS_PRICE1,NEWS_PRICE2,NEWS_PRICE3,NEWS_IMAGE1,NEWS_IMAGE2,NEWS_IMAGE3,NEWS_IMAGE4,NEWS_FIELD3,NEWS_FIELD4,NEWS_FIELD1,NEWS_FIELD5,NEWS_FIELD2,NEWS_FILEHTML_EN,NEWS_TITLE_EN,NEWS_DESC_EN,NEWS_HTML_EN1,NEWS_HTML_EN2,NEWS_HTML_EN3,NEWS_TITLE_JS,NEWS_SEO_DESC_JS,NEWS_SEO_URL_JS,NEWS_SEO_URL_EN,NEWS_SEO_META_CANONICAL,NEWS_SEO_META_DESC_EN,NEWS_LIENKET_EN,NEWS_TIME_AVALBLE")] ESHOP_NEWS eSHOP_NEWS)
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

        public string CheckToExitFileRead(string fulpath)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(fulpath);
                if (!exists)
                {
                    return fm.ReadFile(fulpath);
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

        private bool ESHOP_NEWSExists(int id)
        {
            return _context.ESHOP_NEWS.Any(e => e.NEWS_ID == id);
        }


        [AllowAnonymous]
        public async Task<IActionResult> SellerPostImages(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SellerPostImages(List<IFormFile> files, int id, [Bind("NEWS_IMG_ID,NEWS_IMG_IMAGE1,NEWS_IMG_DESC,NEWS_IMG_ORDER,NEWS_IMG_SHOWTYPE,NEWS_ID")] ESHOP_NEWS_IMAGE eSHOP_NEWS_IMAGE, string submit)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

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

        [HttpPost]
        [AllowAnonymous]
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

        [HttpGet]
        [AllowAnonymous]
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


        [AllowAnonymous]
        public async Task<IActionResult> SellerPosProCheck(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

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
        [AllowAnonymous]
        public async Task<IActionResult> SellerPosProCheck(string selectedItems, int id, [Bind("NEWS_PROP_ID,NEWS_ID,PROP_ID")] ESHOP_NEWS_PROPERTIES eSHOP_NEWS_PROPERTIES)
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


        [AllowAnonymous]
        public IActionResult ListSugget(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            var ListCustomer_Request = (from n in _context.Customer_Request
                                        join nc in _context.CustomerConfirm on int.Parse(n.Customer_request_id.ToString()) equals nc.Customer_request_id
                                        join ne in _context.ESHOP_NEWS on nc.NEWS_ID equals ne.NEWS_ID
                                        where nc.NEWS_ID == id
                                        select new
                                        {
                                            ne.NEWS_TITLE,
                                            ne.NEWS_TITLE_EN,
                                            nc.CustomerConfirm_Id,
                                            n.Customer_Name,
                                            n.Customer_Phone,
                                            nc.CustomerConfirm_Comment,
                                            nc.CustomerConfirm_Date,
                                            nc.CustomerConfirm_Active,
                                            nc.CustomerConfirm_PublishDate
                                        }).Select(p => new ConfirmModelCustomer
                                        {
                                            CustomerConfirm_Id = p.CustomerConfirm_Id,
                                            Customer_Name = p.Customer_Name,
                                            Customer_Phone = p.Customer_Phone,
                                            CustomerConfirm_Comment = p.CustomerConfirm_Comment,
                                            CustomerConfirm_Date = p.CustomerConfirm_Date,
                                            CustomerConfirm_Active = p.CustomerConfirm_Active,
                                            CustomerConfirm_PublishDate = p.CustomerConfirm_PublishDate,
                                            NEWS_TITLE_EN = p.NEWS_TITLE_EN,
                                            NEWS_TITLE = p.NEWS_TITLE
                                        }).Distinct().Take(100).OrderByDescending(x => x.CustomerConfirm_PublishDate).ToList();


            return View(ListCustomer_Request);

        }



        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(int id)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string diachiemail)
        {
            int LangId = 2;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                LangId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName));
            }

            ViewBag.Langue = LangId;

            int MoneyId = 1;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName1)))
            {
                MoneyId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName1));
            }

            ViewBag.Money = MoneyId;

            var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
            ViewData["Title"] = eshop_Meta.CONFIG_TITLE;
            ViewData["Desc"] = eshop_Meta.CONFIG_NAME_US;
            ViewData["Key"] = eshop_Meta.CONFIG_KEYWORD;
            ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
            ViewBag.USD = eshop_Meta.CONFIG_PORT;

            if(IsEmail(diachiemail) == true)
            {
                var LogResult = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_EMAIL == diachiemail && m.CUSTOMER_ACTIVE == 1);

                if (LogResult == null)
                {
                    ViewBag.Error = "Email không tồn tại";
                    ViewBag.Display = "display:block";
                    return View();
                }
                else
                {

                    string mkmoi = fm.TaoChuoiTuDong(10);

                    LogResult.CUSTOMER_PW = fm.Encrypt(mkmoi);

                    _context.Update(LogResult);
                    await _context.SaveChangesAsync();

                    string Body = EmailQuenMK(diachiemail, "hochiminhrental.com", mkmoi);

                    var eSHOP_CONFIG = await _context.ESHOP_CONFIG.SingleOrDefaultAsync(m => m.CONFIG_ID == 1);

                    string emailgui = "";
                    string emailSend = "";
                    var ListEmail = _context.ESHOP_EMAIL.SingleOrDefault(x => x.EMAIL_ID == 2);
                    if (ListEmail != null)
                    {
                        emailgui = ListEmail.EMAIL_TO;
                        emailSend = ListEmail.EMAIL_CC;
                    }

                    SendEmailSMTP("Gửi lại mật khẩu từ EUREKA", eSHOP_CONFIG.CONFIG_EMAIL, eSHOP_CONFIG.CONFIG_PASSWORD, "Đăng ký tài khoản", diachiemail, "", emailgui, emailSend, Body, true, true, eSHOP_CONFIG.CONFIG_PORT, eSHOP_CONFIG.CONFIG_SMTP);
                }
            }   
            else
            {
                ViewBag.Error = "Email không đúng định dạng";
                ViewBag.Display = "display:block";
                return View();
            }    
            

            return View();
        }

        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
    }
}
