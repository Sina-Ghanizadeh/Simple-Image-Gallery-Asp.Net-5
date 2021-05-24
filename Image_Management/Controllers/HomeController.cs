using Image_Management.Data;
using Image_Management.Models;
using Image_Management.Models.UploadImage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Image_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var item = _context.files.ToList();



            return View(item);
        }
        [Route("Create")]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost("Create")]
        public IActionResult Create(FileViewModel model)
        {
            if (!(Path.GetExtension(model.file.FileName) == ".jpg" || Path.GetExtension(model.file.FileName) == ".png" || Path.GetExtension(model.file.FileName) == ".gif"))
            {
                ModelState.AddModelError("file", "File Type Is Not Valid");
                return View(model);
            }

            var upload = UploadFile(model);


            var file = new FileModel
            {

                Name = Path.GetFileNameWithoutExtension(upload.FileName),
                FilePath = upload.FileName,
                ThumbPath = upload.ThumbName,
                UploadTime = DateTime.Now

            };
            _context.files.Add(file);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public UploadImage UploadFile(FileViewModel model)
        {
            string fileName = null;
            string thumbName = null;



            if (model.file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.file.FileName);
                thumbName = Guid.NewGuid().ToString() + "-thumb" + Path.GetExtension(model.file.FileName);

                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images", fileName);

                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images", thumbName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.file.CopyTo(stream);
                    Image image = Image.FromStream(stream);
                    Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                    thumb.Save(thumbPath);
                }
            }
            UploadImage upload = new UploadImage() { FileName = fileName, ThumbName = thumbName };
            return upload;

        }


        public IActionResult ShowImage(int id)
        {
            var item = _context.files.SingleOrDefault(f => f.Id == id);

            return View(item);
        }
        public IActionResult Delete(int id)
        {
            var item = _context.files.Find(id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images", item.FilePath);
            var thumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot",
                   "images", item.ThumbPath);
            if (System.IO.File.Exists(filePath) && System.IO.File.Exists(thumbPath))
            {

                System.IO.File.Delete(filePath);

                System.IO.File.Delete(thumbPath);


            }

            _context.files.Remove(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
