using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.ViewModels.Photos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        readonly IRepository _repository;

        public PhotosController(IRepository repository) 
        {
            _repository = repository;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> ImageResize(int id, int width)
        {
            Photo photo = await _repository.GetOne(id);
            Bitmap bmp;

            MemoryStream memoryStream = new MemoryStream();
            const long quality = 50;
            using (var ms = new MemoryStream(photo.ImageContent))
            {
                bmp = new Bitmap(ms);
                int imageHeight = bmp.Height;
                int imageWidth = bmp.Width;
                if (imageWidth > width)
                {
                    float ratio = (float)imageWidth / (float)imageHeight;
                    var resized_Bitmap = new Bitmap((int)width, (int)(width / ratio));
                    using (var graphics = Graphics.FromImage(resized_Bitmap))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(bmp, 0, 0, width, width / ratio);

                        var qualityParamId = Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                        var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        resized_Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                    }
                }
                else {
                    bmp.Save(memoryStream, ImageFormat.Jpeg);
                }
            }
            return new FileContentResult(memoryStream.ToArray(), "binary/octet-stream");
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
                List<Photo> photolist = new List<Photo>();
                foreach (var item in model.FormFile)
                {
                    byte[] img;
                    using (var reader = new BinaryReader(item.OpenReadStream()))
                    {
                        img = reader.ReadBytes((int)item.Length);
                    }                  
                    photolist.Add(new Photo { PhotoName = item.FileName, ImageContent = img });
                }
                foreach (var item in photolist)
                {
                    _repository.Add(item);
                }
                await _repository.SaveChanges();
                return PartialView("Mypart", photolist);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var photo = await _repository.GetOne(id);
            _repository.Remove(photo);    
            await _repository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //private bool PhotoExists(int id)
        //{
        //    return db.Photos.Any(e => e.Id == id);
        //}
    }
}
