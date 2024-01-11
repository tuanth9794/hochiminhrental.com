using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreCnice.Areas.Admin.Models;
using CoreCnice.Connect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LazZiya.ImageResize;
using CoreCnice.UtilsCs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImagesSeoController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        clsFormat cl = new clsFormat();

        private readonly IHostingEnvironment he;

        public ImagesSeoController(IHostingEnvironment e)
        {
            he = e;
        }

        public IActionResult Index()
        {
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string chieudaiwh, string chieucaowh)
        {
            string idnew = "";
            try
            {
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

                string[] ListCat = Request.Form["Employee[]"];
                int chieudai = Utils.CIntDef(chieudaiwh);
                int chieucao = Utils.CIntDef(chieucaowh);

                string imgEror = "";

                if (ListCat.ToList().Count() > 0)
                {
                    foreach (var itemcat in ListCat.ToList())
                    {
                        int idcat = int.Parse(itemcat.ToString());
                        var listcat = from c in _context.ESHOP_CATEGORIES
                                      join nc in _context.ESHOP_NEWS_CAT on c.CAT_ID equals nc.CAT_ID
                                      join n in _context.ESHOP_NEWS on nc.NEWS_ID equals n.NEWS_ID
                                      where nc.CAT_ID == idcat
                                      select n;
                        if (listcat.ToList().Count > 0)
                        {
                            foreach (var item in listcat.ToList())
                            {
                                if (item.NEWS_IMAGE1.Contains(".gif") != true)
                                {
                                    string linkimg = "wwwroot\\UploadImages\\Data\\News\\" + item.NEWS_ID + "\\" + item.NEWS_IMAGE1;
                                    idnew = linkimg;
                                    if (CheckExitFile(linkimg) == true)
                                    {
                                        var img = Image.FromFile(linkimg);
                                        string imgnew = "wwwroot\\UploadImages\\Data\\News\\" + item.NEWS_ID + "\\thumb-" + item.NEWS_IMAGE1;
                                        if (CheckExitFile(imgnew) == false)
                                        {
                                            try
                                            {
                                                var scaleImage = ImageResize.Scale(img, chieudai, chieucao);
                                                scaleImage.SaveAs(imgnew);
                                            }
                                            catch (Exception ex)
                                            {
                                                imgEror += linkimg + "</br>";
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    if (imgEror != "")
                    {
                        ViewBag.Error = imgEror;
                    }
                    else
                    {
                        ViewBag.Error = "Tạo hình đại diện nhỏ thành công";
                    }
                }
                else
                {
                    ViewBag.Error = "Vui lòng lựa chọn chuyên mục";
                }


             

                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bị lỗi ở hình " + idnew + ex.Message;
                return View();
            }
        }

        public string CheckToExitFileRead(string pt2)
        {
            try
            {
                bool existsab = System.IO.File.Exists(pt2);
                if (existsab == true)
                {
                    return cl.ReadFile(pt2);
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


        public bool CheckExitFile(string pt2)
        {
            try
            {
                bool existsab = System.IO.File.Exists(pt2);
                if (existsab == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public void CheckToExitFile(string fulpath, string noidung)
        {
            try
            {
                System.IO.File.Delete(fulpath);

                using (StreamWriter writer = new StreamWriter(fulpath))
                {
                    writer.WriteLine(noidung);
                }

            }
            catch (IOException ex)
            {
                throw ex;
            }
        }


        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadFile(string chieudaiwh, string chieucaowh)
        {
            string idnew = "";
            try
            {
                string imgEror = "";

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string path = String.Empty;
                    string pathfodel = String.Empty;

                    if (file != null && file.FileName.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        pathfodel = Path.Combine(he.WebRootPath);
                        path = Path.Combine(he.WebRootPath, fileName);
                        AppendToFile(pathfodel, path, file);
                        imgEror = "Upload File Thành Công";
                    }
                }
                else
                {
                    imgEror = "Vui lòng chọn file up load";
                }


                ViewBag.Error = imgEror;


                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bị lỗi ở hình " + idnew + ex.Message;
                return View();
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