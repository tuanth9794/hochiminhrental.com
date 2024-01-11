using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreCnice.Models;
using CoreCnice.Domain;
using CoreCnice.Connect;
using Microsoft.AspNetCore.Http;
using CoreCnice.UtilsCs;
using CoreCnice.Helpers;

namespace BulSoftCMS.Controllers
{
    public class CatalogController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        string SessionKeyName = "CustomerSesion1";
        string SessionKeyName1 = "CustomerSesion";
        string SessionKeyName2 = "CustomerSesion2";
        string SessionKeyName3 = "CustomerSesion3";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryTemplateListOne(int id)
        {
            //if(seourl != null)
            //{
            //    var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
            //                       join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
            //                       join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
            //                       where c.CAT_SEO_URL == seourl
            //                       select  n
            //                       );
            //    //var Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == seourl);

            //    //ViewBag.Title = Listcat.ToList()[0].CAT_NAME;

            //    return View(ListNewsCat);
            //}

            if (id != 0)
            {
                var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                   join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                   join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                   where nc.CAT_ID == id
                                   select n
                                   );
                var Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID == id);

                ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE;

                ViewBag.Id = Listcat.ToList()[0].CAT_ID;

                ViewBag.IMAGE2 = Listcat.ToList()[0].CAT_IMAGE2;

                ViewBag.Type = Listcat.ToList()[0].CAT_TYPE;

                ViewBag.Alt = Listcat.ToList()[0].CAT_FIELD1;

                return View(ListNewsCat);
            }
            return View();
        }

        public IActionResult PostTemplateList(int id)
        {
            //if(seourl != null)
            //{
            //    var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
            //                       join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
            //                       join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
            //                       where c.CAT_SEO_URL == seourl
            //                       select  n
            //                       );
            //    //var Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == seourl);

            //    //ViewBag.Title = Listcat.ToList()[0].CAT_NAME;

            //    return View(ListNewsCat);
            //}

            if (id != 0)
            {
                var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                   join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                   join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                   where nc.CAT_ID == id
                                   select n
                                   );
                var Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_ID == id);

                ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE;

                ViewBag.Id = Listcat.ToList()[0].CAT_ID;

                ViewBag.IMAGE2 = Listcat.ToList()[0].CAT_IMAGE2;

                ViewBag.Type = Listcat.ToList()[0].CAT_TYPE;

                ViewBag.Alt = Listcat.ToList()[0].CAT_FIELD1;

                return View(ListNewsCat);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult ContactPost()
        {
            return View();
        }

        //url rewriter
        public IActionResult UrlReturn(string resource, string resource1, string resource2)
        {
            try
            {
                string listSearchNews = "";
                bool flag = true;
                bool flagloop = false;
                //string listremove = "";

                int TypePrice = 2;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName3)))
                {
                    TypePrice = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName3));
                }

                ViewBag.ChonNgay = TypePrice;

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

                int SapXepId = 0;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName2)))
                {
                    SapXepId = Utils.CIntDef(HttpContext.Session.GetString(SessionKeyName2));
                }

                ViewBag.SapXep = SapXepId;
                var eshop_Meta = _context.ESHOP_CONFIG.SingleOrDefault();
                ViewData["FAVICON"] = eshop_Meta.CONFIG_FAVICON;
                ViewBag.USD = eshop_Meta.CONFIG_PORT;
                string urlSearch = HttpContext.Request.Query["s"].ToString();
                decimal Priceto = Utils.CDecDef(HttpContext.Request.Query["priceto"].ToString());
                decimal priceform = Utils.CDecDef(HttpContext.Request.Query["priceform"].ToString());
                if (MoneyId == 2)
                {
                    Priceto = Priceto * Utils.CDecDef(eshop_Meta.CONFIG_PORT);
                    priceform = priceform * Utils.CDecDef(eshop_Meta.CONFIG_PORT);
                }
                if (String.IsNullOrEmpty(urlSearch) != true)
                {
                    string rep = "?s=";
                    string idPro = urlSearch.Replace(rep, "").Replace("cat=", "");
                    string cat_seo = resource;
                    List<string> list = idPro.Split(',').ToList();
                    //string[] list = idPro.Split(',');
                    string itemload = "";
                    int cat_id = 0;
                    int? cat_type = 0;
                    List<ESHOP_CATEGORIES> Listcat = new List<ESHOP_CATEGORIES>();
                    Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource).ToList();                   
                    
                    if (Listcat.ToList().Count > 0)
                    {
                        cat_id = Listcat.ToList()[0].CAT_ID;
                        ViewBag.Id = cat_id;
                        cat_type = Listcat.ToList()[0].CAT_TYPE;
                    }
                    else
                    {
                        Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL_EN == resource).ToList();
                        if (Listcat.ToList().Count > 0)
                        {
                            cat_id = Listcat.ToList()[0].CAT_ID;
                            ViewBag.Id = cat_id;
                            cat_type = Listcat.ToList()[0].CAT_TYPE;
                        }
                    }    

                    List<NewsModelCat> oditemAddList = new List<NewsModelCat>();
                    List<NewsModelCat> oditemAddListAnd = new List<NewsModelCat>();
                    if (list.Count() > 0)
                    {
                        ///lọc thuộc tính cùng loại
                        for (int i = 0; i < list.Count(); i++)
                        {
                            int idprop = Utils.CIntDef(list[i]);
                            if (idprop > 0)
                            {
                                if (listSearchNews.IndexOf(idprop.ToString()) > -1)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                }
                                if (flag == false)
                                {
                                    var listPro = _context.ESHOP_PROPERTIES.SingleOrDefault(x => x.PROP_ID == idprop);
                                    if (listPro != null)
                                    {
                                        if (String.IsNullOrEmpty(listSearchNews) == true)
                                        {
                                            listSearchNews += "[" + idprop + ",";
                                        }
                                        else
                                        {
                                            listSearchNews += "[" + idprop + ",";
                                        }

                                        int idCha = listPro.PROP_PARENT_ID;
                                        if (idCha != 0)
                                        {
                                            for (int j = 0; j < list.Count(); j++)
                                            {
                                                if (Utils.CIntDef(list[j]) != idprop)
                                                {
                                                    int idpropNews = Utils.CIntDef(list[j]);
                                                    var listProParent = _context.ESHOP_PROPERTIES.SingleOrDefault(x => x.PROP_ID == idpropNews);
                                                    if (listProParent != null)
                                                    {
                                                        int idChaNews = listProParent.PROP_PARENT_ID;
                                                        if (idCha == idChaNews)
                                                        {
                                                            listSearchNews += Utils.CIntDef(list[j]) + ",";
                                                            //list.RemoveAt(j);                                                 
                                                            //listremove += j + ",";
                                                        }
                                                    }
                                                }
                                                else
                                                {

                                                }
                                                if (j + 1 == list.Count())
                                                {
                                                    listSearchNews += "];";
                                                    //List<string> listrm = listremove.Split(',').ToList();
                                                    //for (int kj = 0; kj < list.Count(); kj++)
                                                    //{
                                                    //    if (Utils.CIntDef(listrm[kj]) != 0)
                                                    //    {
                                                    //        list.RemoveAt(Utils.CIntDef(listrm[kj]));
                                                    //    }
                                                    //}

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        List<string> listSearchCha = listSearchNews.Split(';').ToList();
                        if (listSearchCha.Count() > 2)
                        {
                            for (int k = 0; k < listSearchCha.Count; k++)
                            {
                                list = listSearchCha[k].Replace("[", " ").Replace("]", " ").TrimEnd().TrimStart().Split(",").ToList();
                                if (flagloop == false)
                                {
                                    if (oditemAddListAnd.ToList().Count > 0)
                                    {
                                        oditemAddList = new List<NewsModelCat>();
                                        oditemAddList.AddRange(oditemAddListAnd);
                                    }
                                }
                                else
                                {
                                    oditemAddList = new List<NewsModelCat>();
                                    oditemAddList.AddRange(oditemAddListAnd);
                                }
                                oditemAddListAnd = new List<NewsModelCat>();

                                for (int i = 0; i < list.Count(); i++)
                                {
                                    int idprop = Utils.CIntDef(list[i]);
                                    var listPro = _context.ESHOP_PROPERTIES.SingleOrDefault(x => x.PROP_ID == idprop);
                                    if (listPro != null)
                                    {
                                        if (LangId == 1)
                                        {
                                            itemload += "<p id='xoachon" + listPro.PROP_ID + "'><a  href='javascript: void(0)' onclick='clickSearchRemove(" + listPro.PROP_ID + ")' class='prdctfltr_title_remove' data-key='price'>" + listPro.PROP_NAME + "<span> <i class='fas fa-times'></i></span> </a></p>";
                                        }
                                        else
                                        {
                                            itemload += "<p id='xoachon" + listPro.PROP_ID + "'><a  href='javascript: void(0)' onclick='clickSearchRemove(" + listPro.PROP_ID + ")' class='prdctfltr_title_remove' data-key='price'>" + listPro.PRO_NAME_EN + "<span> <i class='fas fa-times'></i></span> </a></p>";
                                        }
                                    }


                                    if (idprop != 0)
                                    {
                                        if (idprop == 77)
                                        {
                                            ViewBag.Timtheongay = 1;
                                        }

                                        if (k >= 1)
                                        {
                                            if (oditemAddList.ToList().Count == 0)
                                            {
                                                if (Priceto == priceform)
                                                {
                                                    if (cat_type != 2)
                                                    {
                                                        var _vNewsList = (from n in oditemAddList.ToList()
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              n.NEWS_PRICE4,
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
                                                                              n.CAT_SEO_URL_EN,                                                                           
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                            NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddListAnd.AddRange(listNewModels);
                                                        flagloop = true;
                                                    }
                                                    else
                                                    {
                                                        var _vNewsList = (from n in oditemAddList.ToList()
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              n.NEWS_PRICE4,
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
                                                                              n.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                            NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddListAnd.AddRange(listNewModels);
                                                        flagloop = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (cat_type != 2)
                                                    {
                                                        var _vNewsList = (from n in oditemAddList.ToList()
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              n.NEWS_IMAGE1,
                                                                              n.NEWS_PRICE3,
                                                                              n.NEWS_PRICE4,
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
                                                                              n.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                            NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddListAnd.AddRange(listNewModels);
                                                        flagloop = true;
                                                    }
                                                    else
                                                    {
                                                        var _vNewsList = (from n in oditemAddList.ToList()
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              n.NEWS_PRICE4,
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
                                                                              n.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                            NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddListAnd.AddRange(listNewModels);
                                                        flagloop = true;
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                if (cat_type != 2)
                                                {
                                                    var _vNewsList = (from n in oditemAddList.ToList()
                                                                      join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                      join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                      join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                      where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                                      orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                          n.NEWS_PRICE4,
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
                                                                          n.NEWS_TITLE_JS,
                                                                          n.NEWS_FIELD4,
                                                                          n.NEWS_SEO_URL_EN,
                                                                          n.CAT_SEO_URL_EN,
                                                                      }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                    var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                    //oditemAddList = new List<NewsModelCat>();

                                                    var listNewModels = result.Select(p => new NewsModelCat
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
                                                        NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                    }).OrderByDescending(p => p.NEWS_ID);

                                                    //foreach (var item in listNewModels)
                                                    //{
                                                    //    oditemAddList.Add(item);
                                                    //}
                                                    oditemAddListAnd.AddRange(listNewModels);
                                                    flagloop = true;
                                                }
                                                else
                                                {
                                                    var _vNewsList = (from n in oditemAddList.ToList()
                                                                      join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                      join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                      join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                      where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                                      orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                          n.NEWS_PRICE4,
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
                                                                          n.NEWS_TITLE_JS,
                                                                          n.NEWS_FIELD4,
                                                                          n.NEWS_SEO_URL_EN,
                                                                          n.CAT_SEO_URL_EN,
                                                                      }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                    var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                    //oditemAddList = new List<NewsModelCat>();

                                                    var listNewModels = result.Select(p => new NewsModelCat
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
                                                        NEWS_PRICE4 = p.NEWS_PRICE4,
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
                                                    }).OrderByDescending(p => p.NEWS_ID);

                                                    //foreach (var item in listNewModels)
                                                    //{
                                                    //    oditemAddList.Add(item);
                                                    //}
                                                    oditemAddListAnd.AddRange(listNewModels);
                                                    flagloop = true;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (oditemAddList.ToList().Count == 0)
                                            {
                                                if (Priceto == priceform)
                                                {
                                                    if (cat_type != 2)
                                                    {
                                                        var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              c.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddList.AddRange(listNewModels);

                                                    }
                                                    else
                                                    {
                                                        var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              c.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddList.AddRange(listNewModels);
                                                    }

                                                }
                                                else
                                                {
                                                    if (cat_type != 2)
                                                    {
                                                        var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              n.NEWS_IMAGE1,
                                                                              n.NEWS_PRICE3,                                                                            
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
                                                                              c.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddList.AddRange(listNewModels);
                                                    }
                                                    else
                                                    {
                                                        var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                          where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
                                                                          orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                              c.CAT_SEO_URL_EN,
                                                                          }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                        var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                        var listNewModels = result.Select(p => new NewsModelCat
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
                                                        }).OrderByDescending(p => p.NEWS_ID);

                                                        //foreach (var item in listNewModels)
                                                        //{
                                                        //    oditemAddList.Add(item);
                                                        //}
                                                        oditemAddList.AddRange(listNewModels);
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                if (cat_type != 2)
                                                {
                                                    var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                      join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                      join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                      join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                      where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                                      orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                          n.NEWS_TITLE_JS,
                                                                          n.NEWS_FIELD4,
                                                                          n.NEWS_SEO_URL_EN,
                                                                          c.CAT_SEO_URL_EN,
                                                                      }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                    var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                    //oditemAddList = new List<NewsModelCat>();

                                                    var listNewModels = result.Select(p => new NewsModelCat
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
                                                    }).OrderByDescending(p => p.NEWS_ID);

                                                    //foreach (var item in listNewModels)
                                                    //{
                                                    //    oditemAddList.Add(item);
                                                    //}
                                                    oditemAddList.AddRange(listNewModels);
                                                }
                                                else
                                                {
                                                    var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                      join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                      join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                      join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                      where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                                      orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                          n.NEWS_TITLE_JS,
                                                                          n.NEWS_FIELD4,
                                                                          n.NEWS_SEO_URL_EN,
                                                                          c.CAT_SEO_URL_EN,
                                                                      }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                    var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                    //oditemAddList = new List<NewsModelCat>();

                                                    var listNewModels = result.Select(p => new NewsModelCat
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
                                                    }).OrderByDescending(p => p.NEWS_ID);

                                                    //foreach (var item in listNewModels)
                                                    //{
                                                    //    oditemAddList.Add(item);
                                                    //}
                                                    oditemAddList.AddRange(listNewModels);
                                                }
                                            }
                                        }
                                    }
                                }
                                //if(oditemAddListAnd.ToList().Count > 0)
                                //{

                                //}              
                                //else
                                //{

                                //}
                            }
                        }
                        else
                        {
                            list = listSearchCha[0].Replace("[", " ").Replace("]", " ").TrimEnd().TrimStart().Split(",").ToList();
                            for (int i = 0; i < list.Count(); i++)
                            {
                                int idprop = Utils.CIntDef(list[i]);
                                var listPro = _context.ESHOP_PROPERTIES.SingleOrDefault(x => x.PROP_ID == idprop);
                                if (listPro != null)
                                {
                                    if (LangId == 1)
                                    {
                                        itemload += "<p id='xoachon" + listPro.PROP_ID + "'><a  href='javascript: void(0)' onclick='clickSearchRemove(" + listPro.PROP_ID + ")' class='prdctfltr_title_remove' data-key='price'>" + listPro.PROP_NAME + "<span> <i class='fas fa-times'></i></span> </a></p>";
                                    }
                                    else
                                    {
                                        itemload += "<p id='xoachon" + listPro.PROP_ID + "'><a  href='javascript: void(0)' onclick='clickSearchRemove(" + listPro.PROP_ID + ")' class='prdctfltr_title_remove' data-key='price'>" + listPro.PRO_NAME_EN + "<span> <i class='fas fa-times'></i></span> </a></p>";
                                    }
                                }


                                if (idprop != 0)
                                {
                                    if (idprop == 77)
                                    {
                                        ViewBag.Timtheongay = 1;
                                    }

                                    if (oditemAddList.ToList().Count == 0)
                                    {
                                        if (Priceto == priceform)
                                        {
                                            if (cat_type != 2)
                                            {
                                                var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                  join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                  join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                  join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                  where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                                  orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                      c.CAT_SEO_URL_EN,
                                                                  }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                var listNewModels = result.Select(p => new NewsModelCat
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
                                                }).OrderByDescending(p => p.NEWS_ID);

                                                //foreach (var item in listNewModels)
                                                //{
                                                //    oditemAddList.Add(item);
                                                //}
                                                oditemAddList.AddRange(listNewModels);

                                            }
                                            else
                                            {
                                                var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                  join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                  join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                  join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                  where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                                  orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                      c.CAT_SEO_URL_EN,
                                                                  }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                var listNewModels = result.Select(p => new NewsModelCat
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
                                                }).OrderByDescending(p => p.NEWS_ID);

                                                //foreach (var item in listNewModels)
                                                //{
                                                //    oditemAddList.Add(item);
                                                //}
                                                oditemAddList.AddRange(listNewModels);
                                            }

                                        }
                                        else
                                        {
                                            if (cat_type != 2)
                                            {
                                                var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                  join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                  join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                  join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                  where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
                                                                  orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                      n.NEWS_IMAGE1,
                                                                      n.NEWS_PRICE3,                                                                   
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
                                                                      c.CAT_SEO_URL_EN,
                                                                  }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                var listNewModels = result.Select(p => new NewsModelCat
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
                                                }).OrderByDescending(p => p.NEWS_ID);

                                                //foreach (var item in listNewModels)
                                                //{
                                                //    oditemAddList.Add(item);
                                                //}
                                                oditemAddList.AddRange(listNewModels);
                                            }
                                            else
                                            {
                                                var _vNewsList = (from n in _context.ESHOP_NEWS
                                                                  join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                                  join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                                  join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                                  where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
                                                                  orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                      c.CAT_SEO_URL_EN,
                                                                  }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                                var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                                var listNewModels = result.Select(p => new NewsModelCat
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
                                                }).OrderByDescending(p => p.NEWS_ID);

                                                //foreach (var item in listNewModels)
                                                //{
                                                //    oditemAddList.Add(item);
                                                //}
                                                oditemAddList.AddRange(listNewModels);
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (cat_type != 2)
                                        {
                                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                              where n.NEWS_PRINTTYPE > 0 && (nc.CAT_ID == cat_id) && nd.PROP_ID == idprop
                                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                  n.NEWS_TITLE_JS,
                                                                  n.NEWS_FIELD4,
                                                                  n.NEWS_SEO_URL_EN,
                                                                  c.CAT_SEO_URL_EN,
                                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                            //oditemAddList = new List<NewsModelCat>();

                                            var listNewModels = result.Select(p => new NewsModelCat
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
                                            }).OrderByDescending(p => p.NEWS_ID);

                                            //foreach (var item in listNewModels)
                                            //{
                                            //    oditemAddList.Add(item);
                                            //}
                                            oditemAddList.AddRange(listNewModels);
                                        }
                                        else
                                        {
                                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                                              where n.NEWS_PRINTTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
                                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                                  n.NEWS_TITLE_JS,
                                                                  n.NEWS_FIELD4,
                                                                  n.NEWS_SEO_URL_EN,
                                                                  c.CAT_SEO_URL_EN,
                                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                                            //oditemAddList = new List<NewsModelCat>();

                                            var listNewModels = result.Select(p => new NewsModelCat
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
                                            }).OrderByDescending(p => p.NEWS_ID);

                                            //foreach (var item in listNewModels)
                                            //{
                                            //    oditemAddList.Add(item);
                                            //}
                                            oditemAddList.AddRange(listNewModels);
                                        }

                                    }
                                }
                            }
                        }

                        ViewBag.HTMLLOC = itemload;
                    }


                    if (LangId == 1)
                    {
                        ViewBag.CAT_NAME = Listcat.ToList()[0].CAT_NAME;
                        ViewBag.CAT_ID = cat_id;
                        ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE;
                        ViewBag.CATDESC = Listcat.ToList()[0].CAT_DESC;
                    }
                    else
                    {
                        ViewBag.CAT_NAME = Listcat.ToList()[0].CAT_NAME_EN;
                        ViewBag.CAT_ID = cat_id;
                        ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE_EN;
                        ViewBag.CATDESC = Listcat.ToList()[0].CAT_DESC_EN;
                    }
                    var Group = oditemAddList.GroupBy(x => x.NEWS_ID).Select(g => g.First()).OrderByDescending(p => p.NEWS_PUBLISHDATE);

                    if (SapXepId == 1)
                    {
                        return View("SearchFind", Group.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                    }
                    else
                    {
                        return View("SearchFind", Group.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                    }
                }
                else if (Priceto != priceform)
                {
                    int cat_id = 0;
                    int? cat_type = 0;
                    var Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource);
                    if (Listcat.ToList().Count > 0)
                    {
                        cat_id = Listcat.ToList()[0].CAT_ID;
                        ViewBag.Id = cat_id;
                        cat_type = Listcat.ToList()[0].CAT_TYPE;
                    }
                    List<NewsModelCat> oditemAddList = new List<NewsModelCat>();

                    if (oditemAddList.ToList().Count == 0)
                    {
                        if (cat_type != 2)
                        {
                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                              where n.NEWS_SHOWTYPE > 0 && (nc.CAT_ID == cat_id) && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                  c.CAT_SEO_URL_EN,
                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                            var listNewModels = result.Select(p => new NewsModelCat
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
                                NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                            }).OrderByDescending(p => p.NEWS_ID);

                            //foreach (var item in listNewModels)
                            //{
                            //    oditemAddList.Add(item);
                            //}
                            oditemAddList.AddRange(listNewModels);
                        }
                        else
                        {
                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                              where n.NEWS_SHOWTYPE > 0 && n.NEWS_TYPE != 0 && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                  c.CAT_SEO_URL_EN,
                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                            var listNewModels = result.Select(p => new NewsModelCat
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
                                NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                            }).OrderByDescending(p => p.NEWS_ID);

                            //foreach (var item in listNewModels)
                            //{
                            //    oditemAddList.Add(item);
                            //}
                            oditemAddList.AddRange(listNewModels);
                        }
                    }
                    else
                    {

                        if (cat_type != 2)
                        {
                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                              where n.NEWS_SHOWTYPE > 0 && (nc.CAT_ID == cat_id) && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                  n.NEWS_TITLE_JS,
                                                  n.NEWS_SEO_URL_EN,
                                                  c.CAT_SEO_URL_EN,
                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                            //oditemAddList = new List<NewsModelCat>();

                            var listNewModels = result.Select(p => new NewsModelCat
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
                                NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                            }).OrderByDescending(p => p.NEWS_ID);

                            //foreach (var item in listNewModels)
                            //{
                            //    oditemAddList.Add(item);
                            //}
                            oditemAddList.AddRange(listNewModels);
                        }
                        else
                        {
                            var _vNewsList = (from n in _context.ESHOP_NEWS
                                              join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                              join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                              join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                              where n.NEWS_SHOWTYPE > 0 && n.NEWS_TYPE != 0 && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
                                              orderby n.NEWS_ORDER, n.NEWS_PUBLISHDATE descending, n.NEWS_TITLE
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
                                                  n.NEWS_TITLE_JS,
                                                  n.NEWS_SEO_URL_EN,
                                                  c.CAT_SEO_URL_EN,
                                              }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(1000);

                            var result = _vNewsList.GroupBy(x => x.NEWS_ID).Select(y => y.First());

                            //oditemAddList = new List<NewsModelCat>();

                            var listNewModels = result.Select(p => new NewsModelCat
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
                                NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                CAT_SEO_URL_EN = p.CAT_SEO_URL_EN
                            }).OrderByDescending(p => p.NEWS_ID);

                            //foreach (var item in listNewModels)
                            //{
                            //    oditemAddList.Add(item);
                            //}
                            oditemAddList.AddRange(listNewModels);
                        }

                    }

                    if (LangId == 1)
                    {
                        ViewBag.CAT_NAME = Listcat.ToList()[0].CAT_NAME;
                        ViewBag.CAT_ID = cat_id;
                        ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE;
                        ViewBag.CATDESC = Listcat.ToList()[0].CAT_DESC;
                    }
                    else
                    {
                        ViewBag.CAT_NAME = Listcat.ToList()[0].CAT_NAME_EN;
                        ViewBag.CAT_ID = cat_id;
                        ViewBag.Title = Listcat.ToList()[0].CAT_SEO_TITLE_EN;
                        ViewBag.CATDESC = Listcat.ToList()[0].CAT_DESC_EN;
                    }

                    var Group = oditemAddList.GroupBy(x => x.NEWS_ID).Select(g => g.First()).OrderByDescending(p => p.NEWS_PUBLISHDATE);

                    if (SapXepId == 1)
                    {
                        return View("SearchFind", Group.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                    }
                    else
                    {
                        return View("SearchFind", Group.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(resource1) == true)
                    {
                        if (string.IsNullOrEmpty(resource) == false)
                        {
                            if (resource == "lien-he")
                            {
                                List<ESHOP_CATEGORIES> Listcat = new List<ESHOP_CATEGORIES>();
                                Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource).ToList();
                                if (Listcat.ToList().Count > 0)
                                {

                                }
                                else
                                {
                                    Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL_EN == resource).ToList();
                                }
                                if (Listcat.ToList().Count > 0)
                                {
                                    ViewData["Title"] = Listcat.ToList()[0].CAT_SEO_TITLE;
                                    ViewData["Desc"] = Listcat.ToList()[0].CAT_SEO_DESC;
                                    ViewData["Key"] = Listcat.ToList()[0].CAT_SEO_KEYWORD;
                                    return View("Contact", Listcat.SingleOrDefault());
                                }
                                else
                                {
                                    ViewData["Title"] = "Thông tin liên hệ Eureka";
                                    ViewData["Desc"] = "Eureka";
                                    ViewData["Key"] = "Eureka";
                                    return View("Contact");
                                }
                            }
                            else if (resource == "favorites-list")
                            {
                                var cart = SessionHelper.GetObjectFromJson<List<NewsModelCat>>(HttpContext.Session, "cart");

                                if (LangId == 1)
                                {

                                    ViewData["Title"] = "favorites list";

                                    ViewData["Desc"] = "favorites list";

                                    ViewData["Key"] = "favorites list";

                                    ViewBag.DescDai = "favorites list";
                                }
                                else
                                {
                                    ViewData["Title"] = "Danh sách sản phẩm yêu thích";

                                    ViewData["Desc"] = "Danh sách sản phẩm yêu thích";

                                    ViewData["Key"] = "Danh sách sản phẩm yêu thích";

                                    ViewBag.DescDai = "Danh sách sản phẩm yêu thích";
                                }
                                return View("CategoryTemplateListLike", cart);
                            }
                            else if (resource == "dang-ky-dai-ly")
                            {
                                List<ESHOP_CATEGORIES> Listcat = new List<ESHOP_CATEGORIES>();
                                Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource).ToList();
                                if (Listcat.ToList().Count > 0)
                                {

                                }
                                else
                                {
                                    Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL_EN == resource).ToList();
                                }
                                if (Listcat.ToList().Count > 0)
                                {
                                    ViewData["Title"] = Listcat.ToList()[0].CAT_SEO_TITLE;
                                    ViewData["Desc"] = Listcat.ToList()[0].CAT_SEO_DESC;
                                    ViewData["Key"] = Listcat.ToList()[0].CAT_SEO_KEYWORD;
                                    return View("Contact", Listcat);
                                }
                                else
                                {
                                    ViewData["Title"] = "Thông tin liên hệ Eureka";
                                    ViewData["Desc"] = "Eureka";
                                    ViewData["Key"] = "Eureka";
                                    return View("Contact");
                                }
                            }
                            else if (resource == "contact")
                            {
                                List<ESHOP_CATEGORIES> Listcat = new List<ESHOP_CATEGORIES>();
                                Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource).ToList();
                                if (Listcat.ToList().Count > 0)
                                {

                                }
                                else
                                {
                                    Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL_EN == resource).ToList();
                                }
                                if (Listcat.ToList().Count > 0)
                                {
                                    ViewData["Title"] = Listcat.ToList()[0].CAT_SEO_TITLE;
                                    ViewData["Desc"] = Listcat.ToList()[0].CAT_SEO_DESC;
                                    ViewData["Key"] = Listcat.ToList()[0].CAT_SEO_KEYWORD;
                                    return View("Contact", Listcat.SingleOrDefault());
                                }
                                else
                                {
                                    ViewData["Title"] = "Thông tin liên hệ Eureka";
                                    ViewData["Desc"] = "Eureka";
                                    ViewData["Key"] = "Eureka";
                                    return View("Contact");
                                }
                            }
                            else
                            {
                                List<ESHOP_CATEGORIES> Listcat = new List<ESHOP_CATEGORIES>();
                                Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL == resource).ToList();
                                if (Listcat.ToList().Count > 0)
                                {

                                }
                                else
                                {
                                    Listcat = _context.ESHOP_CATEGORIES.Where(x => x.CAT_SEO_URL_EN == resource).ToList();
                                }

                                if (Listcat != null)
                                {
                                    int id = Listcat.ToList()[0].CAT_ID;

                                    var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                                       join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                                       join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                                       where nc.CAT_ID == id
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
                                                           c.CAT_SEO_URL_EN,
                                                           n.NEWS_TIME_AVALBLE
                                                       }
                                                  );

                                    if (LangId == 1)
                                    {
                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_SEO_TITLE))
                                        {
                                            ViewData["Title"] = Listcat.ToList()[0].CAT_SEO_TITLE;
                                        }
                                        else
                                        {
                                            ViewData["Title"] = Listcat.ToList()[0].CAT_NAME + eshop_Meta.CONFIG_TITLE;
                                        }

                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_SEO_DESC))
                                        {
                                            ViewData["Desc"] = Listcat.ToList()[0].CAT_SEO_DESC;
                                        }
                                        else
                                        {
                                            ViewData["Desc"] = Listcat.ToList()[0].CAT_NAME + Listcat.ToList()[0].CAT_DESC + eshop_Meta.CONFIG_NAME_US;
                                        }

                                        ViewData["Key"] = Listcat.ToList()[0].CAT_SEO_KEYWORD;

                                        ViewBag.DescDai = Listcat.ToList()[0].CAT_DESC_EN;
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_TARGET))
                                        {
                                            ViewData["Title"] = Listcat.ToList()[0].CAT_TARGET;
                                        }
                                        else
                                        {
                                            ViewData["Title"] = Listcat.ToList()[0].CAT_NAME_EN + eshop_Meta.CONFIG_TITLE;
                                        }

                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_SEO_META_DESC_EN))
                                        {
                                            ViewData["Desc"] = Listcat.ToList()[0].CAT_SEO_META_DESC_EN;
                                        }
                                        else
                                        {
                                            ViewData["Desc"] = Listcat.ToList()[0].CAT_NAME_EN + Listcat.ToList()[0].CAT_DESC_JS + eshop_Meta.CONFIG_NAME_US;
                                        }

                                        ViewData["Key"] = Listcat.ToList()[0].CAT_SEO_KEYWORD;
                                        ViewBag.DescDai = Listcat.ToList()[0].CAT_NAME_JS;
                                    }

                                    ViewBag.Id = Listcat.ToList()[0].CAT_ID;

                                    if (Utils.CIntDef(Listcat.ToList()[0].CAT_PARENT_ID) == 0)
                                    {
                                        ViewBag.IdPe = Listcat.ToList()[0].CAT_ID;
                                    }
                                    else if (Utils.CIntDef(Listcat.ToList()[0].CAT_PARENT_ID) == 2444)
                                    {
                                        ViewBag.IdPe = Listcat.ToList()[0].CAT_ID;
                                    }
                                    else
                                    {
                                        ViewBag.IdPe = Listcat.ToList()[0].CAT_PARENT_ID;
                                    }

                                    ViewBag.Type = Listcat.ToList()[0].CAT_TYPE;

                                    ViewBag.Alt = Listcat.ToList()[0].CAT_FIELD1;

                                    ViewBag.IMAGE2 = Listcat.ToList()[0].CAT_IMAGE2;

                                    ViewBag.IMAGE3 = Listcat.ToList()[0].CAT_IMAGE3;

                                    if (LangId == 1)
                                    {
                                        ViewBag.NAME = Listcat.ToList()[0].CAT_NAME;
                                    }
                                    else
                                    {
                                        ViewBag.NAME = Listcat.ToList()[0].CAT_NAME_EN;
                                    }

                                    ViewBag.CODE = Listcat.ToList()[0].CAT_CODE;

                                    //ViewBag.DESC = Listcat.ToList()[0].CAT_DESC;

                                    if (LangId == 1)
                                    {
                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_SEO_TITLE_EN))
                                        {
                                            ViewBag.UrlCal = Listcat.ToList()[0].CAT_SEO_TITLE_EN;
                                        }
                                        else
                                        {
                                            ViewBag.UrlCal = "/" + Listcat.ToList()[0].CAT_SEO_URL;
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(Listcat.ToList()[0].CAT_SEO_META_CANONICAL))
                                        {
                                            ViewBag.UrlCal = Listcat.ToList()[0].CAT_SEO_META_CANONICAL;
                                        }
                                        else
                                        {
                                            ViewBag.UrlCal = "/" + Listcat.ToList()[0].CAT_SEO_URL_EN;
                                        }
                                    }


                                    ViewBag.Url = "/" + Listcat.ToList()[0].CAT_SEO_URL;

                                    ViewBag.TitleHead = Listcat.ToList()[0].CAT_FIELD2;

                                    ViewBag.Type = Listcat.ToList()[0].CAT_TYPE;

                                    ViewBag.urlVnRef = "/" + Listcat.ToList()[0].CAT_SEO_URL;
                                    ViewBag.urlEnref = "/" + Listcat.ToList()[0].CAT_SEO_URL_EN;

                                    var listNewModels = ListNewsCat.Select(p => new NewsModelCat
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
                                        NEWS_IMAGE1 = p.NEWS_IMAGE1,
                                        NEWS_IMAGE2 = p.NEWS_IMAGE2,
                                        CAT_SEO_URL = p.CAT_SEO_URL,
                                        NEWS_FILEHTML_EN = p.NEWS_FILEHTML_EN,
                                        NEWS_HTML_EN1 = p.NEWS_HTML_EN1,
                                        NEWS_HTML_EN2 = p.NEWS_HTML_EN2,
                                        NEWS_HTML_EN3 = p.NEWS_HTML_EN3,
                                        NEWS_FIELD4 = p.NEWS_FIELD4,
                                        NEWS_TITLE_JS = p.NEWS_TITLE_JS,
                                        NEWS_SEO_URL_EN = p.NEWS_SEO_URL_EN,
                                        CAT_SEO_URL_EN = p.CAT_SEO_URL_EN,
                                        NEWS_TIME_AVALBLE= p.NEWS_TIME_AVALBLE
                                    });

                                    if (Listcat.ToList()[0].CAT_TYPE == 6)
                                    {
                                        return View("About", listNewModels);
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 3)
                                    {
                                        return View("Advisory", listNewModels);
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 2)
                                    {
                                        return View("CategoryTemplateListCat", Listcat.SingleOrDefault());
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 4)
                                    {
                                        return View("CustomerTemplateList", listNewModels);
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 0)
                                    {
                                        return View("PostTemplateList", listNewModels);
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 5)
                                    {
                                        return View("HistoriesListTeamplate", Listcat.SingleOrDefault());
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 10)
                                    {
                                        if (SapXepId == 1)
                                        {
                                            return View("CategoryTemplateListOneMap", listNewModels.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(51));
                                        }
                                        else
                                        {
                                            return View("CategoryTemplateListOneMap", listNewModels.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(51));
                                        }
                                    }
                                    else if (Listcat.ToList()[0].CAT_TYPE == 11)
                                    {
                                        var ListCatChild = _context.ESHOP_CATEGORIES.Where(x => x.CAT_PARENT_ID == id);
                                        return View("CategoryTemplateListCatPer", ListCatChild);
                                    }
                                    else
                                    {
                                        string page = HttpContext.Request.Query["page"].ToString();
                                        int take = 51;
                                        int skip = 0;
                                        if (Utils.CIntDef(page) != 0)
                                        {
                                            skip = ((Utils.CIntDef(page) - 1) * take);
                                            ViewBag.page = page;
                                            ViewBag.pageDangchon = Utils.CIntDef(page);
                                        }
                                        else
                                        {
                                            ViewBag.page = 1;
                                            ViewBag.pageDangchon = Utils.CIntDef(1);
                                        }

                                        if (SapXepId == 1)
                                        {
                                            return View("CategoryTemplateListOne", listNewModels.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(skip).Take(take));
                                        }
                                        else
                                        {
                                            return View("CategoryTemplateListOne", listNewModels.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(skip).Take(take));
                                        }

                                    }
                                }
                                else
                                {
                                    var eSHOP_CATEGORIES = _context.ESHOP_NEWS.SingleOrDefault(m => m.NEWS_SEO_URL == resource);
                                    if (eSHOP_CATEGORIES != null)
                                    {
                                        var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                                           join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                                           join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                                           where nc.NEWS_ID == eSHOP_CATEGORIES.NEWS_ID
                                                           select c
                                                          );

                                        //ViewBag.Title = eSHOP_CATEGORIES.NEWS_SEO_TITLE;

                                        Response.Redirect(ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL);
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(resource1) == false)
                        {
                            var eSHOP_CATEGORIES = _context.ESHOP_NEWS.SingleOrDefault(m => m.NEWS_SEO_URL == resource1);
                            if (eSHOP_CATEGORIES != null)
                            {
                                var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                                   join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                                   join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                                   where nc.NEWS_ID == eSHOP_CATEGORIES.NEWS_ID
                                                   select c
                                                  );

                                //ViewBag.Title = eSHOP_CATEGORIES.NEWS_SEO_TITLE;

                                if (LangId == 1)
                                {
                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_SEO_TITLE))
                                    {
                                        ViewBag.Title = eSHOP_CATEGORIES.NEWS_SEO_TITLE;
                                    }
                                    else
                                    {
                                        ViewBag.Title = eSHOP_CATEGORIES.NEWS_TITLE + eshop_Meta.CONFIG_TITLE;
                                    }

                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_SEO_DESC))
                                    {
                                        ViewData["Desc"] = eSHOP_CATEGORIES.NEWS_SEO_DESC;
                                    }
                                    else
                                    {
                                        ViewData["Desc"] =eSHOP_CATEGORIES.NEWS_TITLE + eSHOP_CATEGORIES.NEWS_SEO_DESC + eshop_Meta.CONFIG_NAME_US;
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_TARGET))
                                    {
                                        ViewBag.Title = eSHOP_CATEGORIES.NEWS_TARGET;
                                    }
                                    else
                                    {
                                        ViewBag.Title = eSHOP_CATEGORIES.NEWS_TITLE_EN;
                                    }

                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_SEO_META_DESC_EN))
                                    {
                                        ViewData["Desc"] = eSHOP_CATEGORIES.NEWS_SEO_META_DESC_EN;
                                    }
                                    else
                                    {
                                        ViewData["Desc"] = eSHOP_CATEGORIES.NEWS_TITLE_EN + eSHOP_CATEGORIES.NEWS_DESC_EN + eshop_Meta.CONFIG_NAME_US;
                                    }
                                }

                                ViewData["Key"] = eSHOP_CATEGORIES.NEWS_SEO_KEYWORD;

                                ViewBag.CatId = ListNewsCat.ToList()[0].CAT_ID;

                                ViewBag.Type = ListNewsCat.ToList()[0].CAT_TYPE;

                                ViewBag.Alt = eSHOP_CATEGORIES.NEWS_IMAGE4;

                                if (LangId == 1)
                                {
                                    ViewBag.NameCat = ListNewsCat.ToList()[0].CAT_NAME;
                                }
                                else
                                {
                                    ViewBag.NameCat = ListNewsCat.ToList()[0].CAT_NAME_EN;
                                }

                                ViewBag.Url = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + resource1;

                                ViewBag.UrlCat = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL;

                                if (LangId == 1)
                                {
                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_IMAGE4))
                                    {
                                        ViewBag.UrlCal = eSHOP_CATEGORIES.NEWS_IMAGE4;
                                    }
                                    else
                                    {
                                        ViewBag.UrlCal = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + resource1;
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(eSHOP_CATEGORIES.NEWS_SEO_META_CANONICAL))
                                    {
                                        ViewBag.UrlCal = eSHOP_CATEGORIES.NEWS_SEO_META_CANONICAL;
                                    }
                                    else
                                    {
                                        ViewBag.UrlCal = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL_EN + "/" + resource1;
                                    }
                                }

                                ViewBag.TitleHead = eSHOP_CATEGORIES.NEWS_FIELD5;
                                ViewBag.urlVnRef = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL;
                                ViewBag.urlEnref = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL_EN + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL_EN;

                                if (eSHOP_CATEGORIES.NEWS_TYPE == 0)
                                {
                                    return View("PostDetail", eSHOP_CATEGORIES);
                                }
                                else if (eSHOP_CATEGORIES.NEWS_TYPE == 1 || eSHOP_CATEGORIES.NEWS_TYPE == 2 || eSHOP_CATEGORIES.NEWS_TYPE == 3)
                                {
                                    if (SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart") == null)
                                    {
                                        List<NewsCatModel> NewsCat = new List<NewsCatModel>();
                                        NewsCat.Add(new NewsCatModel
                                        {
                                            NEWS_ID = eSHOP_CATEGORIES.NEWS_ID,
                                            NEWS_TITLE = eSHOP_CATEGORIES.NEWS_TITLE,
                                            NEWS_IMAGE1 = eSHOP_CATEGORIES.NEWS_IMAGE1,
                                            NEWS_PRICE1 = eSHOP_CATEGORIES.NEWS_PRICE1,
                                            NEWS_PRICE2 = eSHOP_CATEGORIES.NEWS_PRICE2,
                                            NEWS_TYPE = eSHOP_CATEGORIES.NEWS_TYPE,
                                            NEWS_CODE = eSHOP_CATEGORIES.NEWS_CODE,
                                            NEWS_FIELD4 = eSHOP_CATEGORIES.NEWS_FIELD4,
                                            NEWS_HTML_EN1 = eSHOP_CATEGORIES.NEWS_HTML_EN1,
                                            NEWS_HTML_EN2 = eSHOP_CATEGORIES.NEWS_HTML_EN2,
                                            NEWS_HTML_EN3 = eSHOP_CATEGORIES.NEWS_HTML_EN3,
                                            CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL,
                                        });
                                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                                    }
                                    else
                                    {
                                        List<NewsCatModel> NewsCat = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
                                        int index = isExist(Utils.CIntDef(eSHOP_CATEGORIES.NEWS_ID));
                                        if (index != -1)
                                        {

                                        }
                                        else
                                        {
                                            NewsCat.Add(new NewsCatModel
                                            {
                                                NEWS_ID = eSHOP_CATEGORIES.NEWS_ID,
                                                NEWS_TITLE = eSHOP_CATEGORIES.NEWS_TITLE,
                                                NEWS_IMAGE1 = eSHOP_CATEGORIES.NEWS_IMAGE1,
                                                NEWS_PRICE1 = eSHOP_CATEGORIES.NEWS_PRICE1,
                                                NEWS_PRICE2 = eSHOP_CATEGORIES.NEWS_PRICE2,
                                                NEWS_TYPE = eSHOP_CATEGORIES.NEWS_TYPE,
                                                NEWS_CODE = eSHOP_CATEGORIES.NEWS_CODE,
                                                NEWS_FIELD4 = eSHOP_CATEGORIES.NEWS_FIELD4,
                                                NEWS_HTML_EN1 = eSHOP_CATEGORIES.NEWS_HTML_EN1,
                                                NEWS_HTML_EN2 = eSHOP_CATEGORIES.NEWS_HTML_EN2,
                                                NEWS_HTML_EN3 = eSHOP_CATEGORIES.NEWS_HTML_EN3,
                                                CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIES.NEWS_SEO_URL,
                                            });
                                        }
                                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                                    }
                                    //addSpView(eSHOP_CATEGORIES);
                                    var cart = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
                                    ViewBag.cart = cart;
                                    return View("ProductDetail", eSHOP_CATEGORIES);
                                }

                            }
                            else
                            {
                                var eSHOP_CATEGORIESONE = _context.ESHOP_NEWS.SingleOrDefault(m => m.NEWS_SEO_URL_EN == resource1);
                                if (eSHOP_CATEGORIESONE != null)
                                {
                                    var ListNewsCat = (from c in _context.ESHOP_CATEGORIES
                                                       join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                                       join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                                       where nc.NEWS_ID == eSHOP_CATEGORIESONE.NEWS_ID
                                                       select c
                                                  );

                                    //ViewBag.Title = eSHOP_CATEGORIES.NEWS_SEO_TITLE;
                                    if (LangId == 1)
                                    {
                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_SEO_TITLE))
                                        {
                                            ViewBag.Title = eSHOP_CATEGORIESONE.NEWS_SEO_TITLE;
                                        }
                                        else
                                        {
                                            ViewBag.Title = eSHOP_CATEGORIESONE.NEWS_TITLE + eshop_Meta.CONFIG_TITLE;
                                        }

                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_SEO_DESC))
                                        {
                                            ViewData["Desc"] = eSHOP_CATEGORIESONE.NEWS_SEO_DESC;
                                        }
                                        else
                                        {
                                            ViewData["Desc"] = eSHOP_CATEGORIESONE.NEWS_TITLE + eSHOP_CATEGORIESONE.NEWS_SEO_DESC + eshop_Meta.CONFIG_NAME_US;
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_TARGET))
                                        {
                                            ViewBag.Title = eSHOP_CATEGORIESONE.NEWS_TARGET;
                                        }
                                        else
                                        {
                                            ViewBag.Title = eSHOP_CATEGORIESONE.NEWS_TITLE_EN;
                                        }

                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_SEO_META_DESC_EN))
                                        {
                                            ViewData["Desc"] = eSHOP_CATEGORIESONE.NEWS_SEO_META_DESC_EN;
                                        }
                                        else
                                        {
                                            ViewData["Desc"] = eSHOP_CATEGORIESONE.NEWS_TITLE_EN + eSHOP_CATEGORIESONE.NEWS_DESC_EN + eshop_Meta.CONFIG_NAME_US;
                                        }
                                    }

                                    ViewData["Key"] = eSHOP_CATEGORIESONE.NEWS_SEO_KEYWORD;

                                    ViewBag.CatId = ListNewsCat.ToList()[0].CAT_ID;

                                    ViewBag.Type = ListNewsCat.ToList()[0].CAT_TYPE;

                                    ViewBag.Alt = eSHOP_CATEGORIESONE.NEWS_IMAGE4;

                                    if (LangId == 1)
                                    {
                                        ViewBag.NameCat = ListNewsCat.ToList()[0].CAT_NAME;
                                    }
                                    else
                                    {
                                        ViewBag.NameCat = ListNewsCat.ToList()[0].CAT_NAME_EN;
                                    }

                                    ViewBag.Url = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + resource1;

                                    ViewBag.UrlCat = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL;

                                    ViewBag.UrlCat = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL;

                                    if (LangId == 1)
                                    {
                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_IMAGE4))
                                        {
                                            ViewBag.UrlCal = eSHOP_CATEGORIESONE.NEWS_IMAGE4;
                                        }
                                        else
                                        {
                                            ViewBag.UrlCal = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + resource1;
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(eSHOP_CATEGORIESONE.NEWS_SEO_META_CANONICAL))
                                        {
                                            ViewBag.UrlCal = eSHOP_CATEGORIESONE.NEWS_SEO_META_CANONICAL;
                                        }
                                        else
                                        {
                                            ViewBag.UrlCal = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL_EN + "/" + resource1;
                                        }
                                    }

                                    ViewBag.TitleHead = eSHOP_CATEGORIESONE.NEWS_FIELD5;
                                    ViewBag.urlVnRef = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIESONE.NEWS_SEO_URL;
                                    ViewBag.urlEnref = "/" + ListNewsCat.ToList()[0].CAT_SEO_URL_EN + "/" + eSHOP_CATEGORIESONE.NEWS_SEO_URL_EN;

                                    if (eSHOP_CATEGORIESONE.NEWS_TYPE == 0)
                                    {
                                        return View("PostDetail", eSHOP_CATEGORIESONE);
                                    }
                                    else if (eSHOP_CATEGORIESONE.NEWS_TYPE == 1 || eSHOP_CATEGORIESONE.NEWS_TYPE == 2 || eSHOP_CATEGORIESONE.NEWS_TYPE == 3)
                                    {
                                        if (SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart") == null)
                                        {
                                            List<NewsCatModel> NewsCat = new List<NewsCatModel>();
                                            NewsCat.Add(new NewsCatModel
                                            {
                                                NEWS_ID = eSHOP_CATEGORIESONE.NEWS_ID,
                                                NEWS_TITLE = eSHOP_CATEGORIESONE.NEWS_TITLE,
                                                NEWS_IMAGE1 = eSHOP_CATEGORIESONE.NEWS_IMAGE1,
                                                NEWS_PRICE1 = eSHOP_CATEGORIESONE.NEWS_PRICE1,
                                                NEWS_PRICE2 = eSHOP_CATEGORIESONE.NEWS_PRICE2,
                                                NEWS_TYPE = eSHOP_CATEGORIESONE.NEWS_TYPE,
                                                NEWS_CODE = eSHOP_CATEGORIESONE.NEWS_CODE,
                                                NEWS_FIELD4 = eSHOP_CATEGORIESONE.NEWS_FIELD4,
                                                NEWS_HTML_EN1 = eSHOP_CATEGORIESONE.NEWS_HTML_EN1,
                                                NEWS_HTML_EN2 = eSHOP_CATEGORIESONE.NEWS_HTML_EN2,
                                                NEWS_HTML_EN3 = eSHOP_CATEGORIESONE.NEWS_HTML_EN3,
                                                CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIESONE.NEWS_SEO_URL,
                                            });
                                            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                                        }
                                        else
                                        {
                                            List<NewsCatModel> NewsCat = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
                                            int index = isExist(Utils.CIntDef(eSHOP_CATEGORIESONE.NEWS_ID));
                                            if (index != -1)
                                            {

                                            }
                                            else
                                            {
                                                NewsCat.Add(new NewsCatModel
                                                {
                                                    NEWS_ID = eSHOP_CATEGORIESONE.NEWS_ID,
                                                    NEWS_TITLE = eSHOP_CATEGORIESONE.NEWS_TITLE,
                                                    NEWS_IMAGE1 = eSHOP_CATEGORIESONE.NEWS_IMAGE1,
                                                    NEWS_PRICE1 = eSHOP_CATEGORIESONE.NEWS_PRICE1,
                                                    NEWS_PRICE2 = eSHOP_CATEGORIESONE.NEWS_PRICE2,
                                                    NEWS_TYPE = eSHOP_CATEGORIESONE.NEWS_TYPE,
                                                    NEWS_CODE = eSHOP_CATEGORIESONE.NEWS_CODE,
                                                    NEWS_FIELD4 = eSHOP_CATEGORIESONE.NEWS_FIELD4,
                                                    NEWS_HTML_EN1 = eSHOP_CATEGORIESONE.NEWS_HTML_EN1,
                                                    NEWS_HTML_EN2 = eSHOP_CATEGORIESONE.NEWS_HTML_EN2,
                                                    NEWS_HTML_EN3 = eSHOP_CATEGORIESONE.NEWS_HTML_EN3,
                                                    CAT_SEO_URL = ListNewsCat.ToList()[0].CAT_SEO_URL + "/" + eSHOP_CATEGORIESONE.NEWS_SEO_URL,
                                                });
                                            }
                                            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", NewsCat);
                                        }
                                        //addSpView(eSHOP_CATEGORIES);
                                        var cart = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
                                        ViewBag.cart = cart;
                                        return View("ProductDetail", eSHOP_CATEGORIESONE);
                                    }
                                }
                            }
                        }


                    }
                }
                Response.Redirect("/thong-bao-loi");
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private int isExist(int id)
        {
            List<NewsCatModel> cart = SessionHelper.GetObjectFromJson<List<NewsCatModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (Utils.CIntDef(cart[i].NEWS_ID) == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public IActionResult Eror404()
        {
            ViewData["Title"] = "Lỗi 404";
            ViewData["Desc"] = "Lỗi 404";
            ViewData["Key"] = "Lỗi 404";
            return View();
        }
    }
}
