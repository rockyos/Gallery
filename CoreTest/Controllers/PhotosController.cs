using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        private readonly PhotoContext db;
        private IHostingEnvironment _hostingEnvironment;

        public PhotosController(PhotoContext context, IHostingEnvironment environment)
        {
            db = context;
            _hostingEnvironment = environment;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Photos = await db.Photos.ToListAsync();
            return View(); 
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Index (Photo model)
        {
            ViewBag.Photos = await db.Photos.ToListAsync();
            if (ModelState.IsValid)
            {
                var file = model.FormFile;
                string file_name = Guid.NewGuid().ToString();
                string file_name_small = file_name + "_s";
                string file_ext = Path.GetExtension(file.FileName);
                file_name += file_ext;
                file_name_small += file_ext;
                string dir = @"\photo\";
                string path_f = _hostingEnvironment.WebRootPath + dir;
                if (!Directory.Exists(path_f))
                {
                    Directory.CreateDirectory(path_f);
                }
                string fullpathname = Path.Combine(path_f, file_name);
                string fullpathname_small = Path.Combine(path_f, file_name_small);
                model.PhotoName = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
                model.PhotoPath = Path.Combine(dir, file_name);
                model.PhotoPath_S = Path.Combine(dir, file_name_small);

                using (var stream = new FileStream(fullpathname, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                float pic_width = 480;
                if (!PhotoResizer.Image_resize(fullpathname, fullpathname_small, pic_width))
                {
                    model.PhotoPath_S = model.PhotoPath;
                }

                db.Add(model);
                await db.SaveChangesAsync();
                return PartialView("Mypart", model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var photo = await db.Photos.FindAsync(id);
            db.Photos.Remove(photo);
            try {
                string path = _hostingEnvironment.WebRootPath + photo.PhotoPath;
                string path_small = _hostingEnvironment.WebRootPath + photo.PhotoPath_S;
                System.IO.File.Delete(path_small);
                System.IO.File.Delete(path);
                
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return db.Photos.Any(e => e.Id == id);
        }
    }
}
