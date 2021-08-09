using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Number_Recognition.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
                    Bitmap png = null;
                    using (var stream = file.OpenReadStream())
                    {
                        stream.Read(bytes, 0, (int)file.Length);
                        png = (Bitmap)Image.FromStream(stream);
                        stream.Close();
                    }
                    var pixels = getPixels(png);
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

        private byte[] getPixels(Bitmap png)
        {
            int width = png.Width;
            int height = png.Height;
            int numPixels = width * height;
            byte[] pixels = new byte[numPixels];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    var color = png.GetPixel(x, y);
                    var brightness = (byte)(color.GetBrightness()*256);
                    pixels[y * width + x] = brightness;
                }
            return pixels;
        }
    }
}
