using CoreTest.Models;
using CoreTest.ViewModels.Photos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IRepository _repository;

        public PhotosController(IHostingEnvironment environment, IRepository repository)
        {
            _repository = repository;
            _hostingEnvironment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                Photos = await _repository.GetAll()
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Photo model)
        {
            if (ModelState.IsValid)
            {
                var file = model.FormFile;
                string fileName = Guid.NewGuid().ToString();
                string fileNameSmall = fileName + "_s";
                string file_ext = Path.GetExtension(file.FileName);
                fileName += file_ext;
                fileNameSmall += file_ext;
                string dir = @"\photo\";
                string path_f = _hostingEnvironment.WebRootPath + dir;
                if (!Directory.Exists(path_f))
                {
                    Directory.CreateDirectory(path_f);
                }
                string fullpathname = Path.Combine(path_f, fileName);
                string fullpathname_small = Path.Combine(path_f, fileNameSmall);
                model.PhotoName = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
                model.PhotoPath = Path.Combine(dir, fileName);
                model.PhotoPath_S = Path.Combine(dir, fileNameSmall);

                using (var stream = new FileStream(fullpathname, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                float pic_width = 480;
                if (!PhotoResizer.Image_resize(fullpathname, fullpathname_small, pic_width))
                {
                    model.PhotoPath_S = model.PhotoPath;
                }

                _repository.Add(model);
                await _repository.SaveChanges();
                return PartialView("Mypart", model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var photo = await _repository.GetOne(id);
            _repository.Remove(photo);
            try
            {
                string path = _hostingEnvironment.WebRootPath + photo.PhotoPath;
                string path_small = _hostingEnvironment.WebRootPath + photo.PhotoPath_S;
                System.IO.File.Delete(path_small);
                System.IO.File.Delete(path);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await _repository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //private bool PhotoExists(int id)
        //{
        //    return db.Photos.Any(e => e.Id == id);
        //}
    }
}
