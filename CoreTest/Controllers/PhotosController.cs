﻿using CoreTest.Models;
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
        readonly IGetPhotoService _getPhotoService;
        readonly IIndexService _indexService;
        readonly ISavePhotoService _savePhotoService;
        public PhotosController(IRepository repository, IResizeService resizer, ISavePhotoService savePhotoService,
            IPhotolistService photolistService, IGetPhotoService getPhotoService, IIndexService getindexService) 
        {
            _repository = repository;
            _resizer = resizer;
            _photolistService = photolistService;
            _getPhotoService = getPhotoService;
            _indexService = getindexService;
            _savePhotoService = savePhotoService;
        }

       //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> ImageResize(string id, int width)
        {
            Photo photo = await _repository.GetOne(id);
            var datasession = HttpContext.Session.GetString(sessionkey);
            byte[] resizedImage = _resizer.GetImage(photo, datasession, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
        }


        public async Task<IActionResult> Index()
        {
            return View("Index");
        }
        

        public async Task<IActionResult> GetListfromDB()
        {
            List<Photo> photo = await _repository.GetAll();
            var datasession = HttpContext.Session.GetString(sessionkey);
            var photos = _getPhotoService.GetPhotoDBandSession(photo, datasession);
            return Json(photos);
        }

        public async Task<IActionResult> SavePhoto()
        {
            var datasession = HttpContext.Session.GetString(sessionkey);
            await _savePhotoService.SavePhotoAsync(datasession, _repository);
            HttpContext.Session.Clear();
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
                string data = _indexService.GetIndexService(model, datasession);
                HttpContext.Session.SetString(sessionkey, data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string guid)
        {
            var datasession = HttpContext.Session.GetString(sessionkey);
            List<Photo> photosfromsession = new List<Photo>();
            if (datasession != null)
            {
                photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
            }
            Photo photo = new Photo();
            photo = await _repository.GetOne(guid);

            if (photo != null)
            {
                photosfromsession.Add(photo);
            } else
            {
                foreach (var item in photosfromsession)
                {
                    if(item.Guid == guid)
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
