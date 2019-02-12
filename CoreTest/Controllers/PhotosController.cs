using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        readonly IRepository _repository;
        readonly IResizeService _resizer;
        readonly IPhotolistService _photolistService;
        public PhotosController(IRepository repository, IResizeService resizer, IPhotolistService photolistService) 
        {
            _repository = repository;
            _resizer = resizer;
            _photolistService = photolistService;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> ImageResize(int id, int width)
        {
            Photo photo = await _repository.GetOne(id);
            byte[] resizedImage = _resizer.GetImage(photo.ImageContent, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
        }


        public async Task<IActionResult> Index()
        {
            return View("Index");
        }
        

        public async Task<IActionResult> GetListfromDB()
        {
            List<Photo> photos = await _repository.GetAll();
            var json = JsonConvert.SerializeObject(photos);
            return Json(json);
        }


        [HttpPost]
        public async Task<IActionResult> Index(Photo model)
        {
            if (ModelState.IsValid)
            {
                List<Photo> photolist = _photolistService.GetPhotolistAsync(model, _repository);
                await _repository.SaveChanges(); 
                var json = JsonConvert.SerializeObject(photolist);
                return Json(json);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] id)
        {
            foreach (var item in id)
            {
                var photo = await _repository.GetOne(item);
                _repository.Remove(photo);
                await _repository.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
