﻿using CoreTest.Extensions;
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
        string sessionkey = "photos";
        readonly IRepository _repository;
        readonly IResizeService _resizer;
        readonly IPhotolistService _photolistService;
        readonly IGetPhotoService _getPhotoService;
        readonly IIndexService _indexService;
        readonly ISavePhotoService _savePhotoService;
        readonly IDeleteService _deleteService;

        protected ISession Session => HttpContext.Session;

        public PhotosController(IRepository repository, IResizeService resizer, ISavePhotoService savePhotoService,
            IPhotolistService photolistService, IGetPhotoService getPhotoService, IIndexService getindexService, IDeleteService deleteService) 
        {
            _repository = repository;
            _resizer = resizer;
            _photolistService = photolistService;
            _getPhotoService = getPhotoService;
            _indexService = getindexService;
            _savePhotoService = savePhotoService;
            _deleteService = deleteService;
        }

       //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> ImageResize(string id, int width)
        {
            Photo photo = await _repository.GetOne(id);
            var datasession = Session.GetString(sessionkey);
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
            var datasession = Session.GetString(sessionkey);
            var photos = _getPhotoService.GetPhotoDBandSession(photo, datasession);
            return Json(photos);
        }

        public async Task<IActionResult> SavePhoto()
        {
            var datasession = Session.GetString(sessionkey);
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
                var datasession = Session.GetString(sessionkey);
                string data = _indexService.GetIndexService(model, datasession);
                Session.SetString(sessionkey, data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string guid)
        {
            var sessionPhotos = Session.Get<List<Photo>>(sessionkey);
            var data = await _deleteService.DeleteAsync(guid, sessionPhotos, _repository);
            Session.Set(sessionkey, data);
            return RedirectToAction(nameof(Index));
        }
    }
}
