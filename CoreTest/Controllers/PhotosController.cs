using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        string sessionkey = "WebSession";
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
            Photo photo = new Photo();
            if (id > 999)
            {
                photo = await _repository.GetOne(id);
            }
            else
            {
                var datasession = HttpContext.Session.GetString(sessionkey);
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                foreach (var item in photosfromsession)
                {
                    if(photosfromsession.IndexOf(item) == id)
                    {
                        photo = item;
                        break;
                    }
                }
            }
           
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
            var datasession = HttpContext.Session.GetString(sessionkey);         
            if(datasession != null)
            {
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                photos.AddRange(photosfromsession);
            }
            return Json(photos);
        }


        [HttpPost]
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

                var datasession = HttpContext.Session.GetString(sessionkey);

                if (datasession != null)
                {
                    List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                    photosfromsession.AddRange(photolist);
                    foreach (var item in photosfromsession)
                    {
                        item.Id = photosfromsession.IndexOf(item);
                    }
                    var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
                    HttpContext.Session.SetString(sessionkey, serialisedDate);
                } else
                {
                    var serialisedDate = JsonConvert.SerializeObject(photolist);
                    HttpContext.Session.SetString(sessionkey, serialisedDate);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var datasession = HttpContext.Session.GetString(sessionkey);
            List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
            if (id > 999)
            {
                Photo photo = await _repository.GetOne(id);
                photosfromsession.Add(photo);
            }
            else
            {
                foreach (var item in photosfromsession)
                {
                    if(photosfromsession.IndexOf(item) == id)
                    {
                        photosfromsession.RemoveAt(id);
                        break;
                    }
                }
                var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
                HttpContext.Session.SetString(sessionkey, serialisedDate);
            }

            //foreach (var item in id)
            //{
            //    var photo = await _repository.GetOne(item);
            //    _repository.Remove(photo);
            //    await _repository.SaveChanges();
            //}
            return RedirectToAction(nameof(Index));
        }
    }
}
