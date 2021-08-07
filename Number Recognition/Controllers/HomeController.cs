using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Number_Recognition.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Number_Recognition.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    byte[] bytes = new byte[file.Length];
                    using (var stream = file.OpenReadStream())
                    {
                        stream.Read(bytes, 0, (int)file.Length);
                        stream.Close();
                    }
                    ViewBag.bytes = Convert.ToBase64String(bytes);

                    ViewBag.Message = "File Uploaded Successfully!!";
                    return View("Index");
                }
            }
            catch { }

            ViewBag.Message = "File upload failed!!";
            ViewBag.filePath = null;
            return View("Index");
        }
    }
}
