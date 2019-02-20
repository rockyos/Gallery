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

       //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
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
                    if(item.Id == id)
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
            foreach (var item in photos)
            {
                item.ImageContent = null;
            }
            return Json(photos);
        }

        public async Task<IActionResult> SavePhoto()
        {
            var datasession = HttpContext.Session.GetString(sessionkey);
            if (datasession != null)
            {
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                foreach (var item in photosfromsession)
                {
                    if (item.Id > 999)
                    {
                        _repository.Remove(item);
                    } else
                    {
                        item.Id = 0;
                        _repository.Add(item);
                    }
                    //await _repository.SaveChanges();
                }
                await _repository.SaveChanges();
                HttpContext.Session.Clear();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Index(Photo model)
        {
            if (ModelState.IsValid)
            {
                var datasession = HttpContext.Session.GetString(sessionkey);
                Photo photolist = new Photo();
                using (var reader = new BinaryReader(model.FormFile[0].OpenReadStream()))
                {
                    byte[]  img = reader.ReadBytes((int)model.FormFile[0].Length);
                    photolist.PhotoName = model.FormFile[0].FileName;
                    photolist.ImageContent = img;
                }

                if (datasession != null)
                {
                    List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                    int idmax = 0;
                    foreach (var item in photosfromsession)
                    {
                        if(item.Id > idmax & item.Id < 1000)
                        {
                            idmax = item.Id;
                        }
                    }
                    photolist.Id = ++idmax;
                    photosfromsession.Add(photolist);

                    var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
                    HttpContext.Session.SetString(sessionkey, serialisedDate);
                } else
                {      
                    var serialisedDate = JsonConvert.SerializeObject(new List<Photo>(){photolist});
                    HttpContext.Session.SetString(sessionkey, serialisedDate);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var datasession = HttpContext.Session.GetString(sessionkey);
            List<Photo> photosfromsession = new List<Photo>();
            if (datasession != null)
            {
                photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
            }
            if (id > 999)
            {
                Photo photo = await _repository.GetOne(id);
                photosfromsession.Add(photo);
            } else
            {
                foreach (var item in photosfromsession)
                {
                    if(item.Id == id)
                    {
                        photosfromsession.Remove(item);
                        break;
                    }
                }             
            }
            var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
            HttpContext.Session.SetString(sessionkey, serialisedDate);
            return RedirectToAction(nameof(Index));
        }
    }
}
