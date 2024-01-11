using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreCnice.Models;
using CoreCnice.Connect;
using Microsoft.EntityFrameworkCore;
using CoreCnice.UtilsCs;
using Microsoft.AspNetCore.Http;

namespace BulSoftCMS.Controllers
{
    public class SearchController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        string SessionKeyName = "CustomerSesion1";
        string SessionKeyName1 = "CustomerSesion";
        string SessionKeyName2 = "CustomerSesion2";
        string SessionKeyName3 = "CustomerSesion3";

        public IActionResult Index()
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
            string keystimkiem = HttpContext.Request.Query["keysearch"].ToString(); 
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
              
                List<string> list = idPro.Split(',').ToList();
                //string[] list = idPro.Split(',');
                string itemload = "";
                int cat_id = 0;
                int? cat_type = 0;    

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
                                                                      where  nd.PROP_ID == idprop
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
                                                                      where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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
                                                                      where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
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
                                                                      where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
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
                                                                  where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop
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
                                                                  where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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
                                                                      where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop
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
                                                                      where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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
                                                                      where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
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
                                                                      where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
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
                                                                  where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop
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
                                                                  where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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
                                                              where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop
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
                                                              where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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
                                                              where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop && (priceform < n.NEWS_PRICE1 && n.NEWS_PRICE1 < Priceto)
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
                                                              where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0 && (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
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
                                                          where n.NEWS_SHOWTYPE > 0 &&  nd.PROP_ID == idprop
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
                                                          where n.NEWS_SHOWTYPE > 0 && nd.PROP_ID == idprop && n.NEWS_TYPE != 0
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


            
                var Group = oditemAddList.GroupBy(x => x.NEWS_ID).Select(g => g.First()).Where(n => n.NEWS_TITLE_EN.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_CODE.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_TITLE.ToLower().Contains(keystimkiem.ToLower())).OrderByDescending(p => p.NEWS_PUBLISHDATE);

                if (SapXepId == 1)
                {
                    return View(Group.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
                else
                {
                    return View(Group.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
            }
            else if (Priceto != priceform)
            {
                int cat_id = 0;
                int? cat_type = 0;
          
                List<NewsModelCat> oditemAddList = new List<NewsModelCat>();

                if (oditemAddList.ToList().Count == 0)
                {
                    if (cat_type != 2)
                    {
                        var _vNewsList = (from n in _context.ESHOP_NEWS
                                          join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                          join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID
                                          join nd in _context.ESHOP_NEWS_PROPERTIES on n.NEWS_ID equals nd.NEWS_ID
                                          where n.NEWS_SHOWTYPE > 0 &&  (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
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
                                          where n.NEWS_SHOWTYPE > 0 &&  (priceform <= n.NEWS_PRICE1 && n.NEWS_PRICE1 <= Priceto)
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

                var Group = oditemAddList.GroupBy(x => x.NEWS_ID).Select(g => g.First()).Where(n => n.NEWS_TITLE_EN.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_CODE.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_TITLE.ToLower().Contains(keystimkiem.ToLower())).OrderByDescending(p => p.NEWS_PUBLISHDATE);

                if (SapXepId == 1)
                {
                    return View(Group.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
                else
                {
                    return View(Group.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
            }
            else
            {
                var _vNewsList = (from n in _context.ESHOP_NEWS
                                  join nc in _context.ESHOP_NEWS_CAT on n.NEWS_ID equals nc.NEWS_ID
                                  join c in _context.ESHOP_CATEGORIES on nc.CAT_ID equals c.CAT_ID                                 
                                  where n.NEWS_SHOWTYPE > 0 && n.NEWS_TYPE != 0 && (n.NEWS_TITLE_EN.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_CODE.ToLower().Contains(keystimkiem.ToLower()) || n.NEWS_TITLE.ToLower().Contains(keystimkiem.ToLower()))
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
                                  }).Distinct().OrderByDescending(x => x.NEWS_PUBLISHDATE).Skip(0).Take(10000);

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

                var Group = listNewModels.GroupBy(x => x.NEWS_ID).Select(g => g.First()).OrderByDescending(p => p.NEWS_PUBLISHDATE);

                if (SapXepId == 1)
                {
                    return View(Group.ToList().OrderByDescending(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
                else
                {
                    return View(Group.ToList().OrderBy(x => x.NEWS_PRICE1).Skip(0).Take(1000));
                }
            }    
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
    }
}
