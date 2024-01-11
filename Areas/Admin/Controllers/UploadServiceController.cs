using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UploadServiceController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public UploadServiceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[HttpPost, DisableRequestSizeLimit]
        public JsonResult UploadFile()
        {
            string fileName = "";
            string folderName = "CMS/Content/UploadService/";
            Microsoft.AspNetCore.Http.IFormFile file;
            try
            {
                file = Request.Form.Files[0];
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return Json(new { location = $"{fileName}" });
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
                //return Json(@"HTTP / 1.1 500 Server Error + ex.Message");
            }
        }
    }
}
