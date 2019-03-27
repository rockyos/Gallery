using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IResizeService _resizer;
        private readonly IGetPhotoService _getPhotoService;
        private readonly IIndexService _indexService;
        private readonly ISavePhotoService _savePhotoService;
        private readonly IDeleteService _deleteService;

        protected ISession Session => HttpContext.Session;

        public PhotosController(IResizeService resizer, ISavePhotoService savePhotoService,
            IGetPhotoService getPhotoService, IIndexService getindexService, IDeleteService deleteService) 
        {
            _resizer = resizer;
            _getPhotoService = getPhotoService;
            _indexService = getindexService;
            _savePhotoService = savePhotoService;
            _deleteService = deleteService;
        }

       //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> ImageResize(string id, int width)
        {
            List<Photo> sessionPhotos = Session.Get<List<Photo>>(sessionkey);
            byte[] resizedImage = await _resizer.GetImageAsync(sessionPhotos, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }
        

        public async Task<IActionResult> GetListfromDB()
        {
            List<PhotoDTO> photos = await _getPhotoService.GetPhotoDBandSessionAsync(Session, sessionkey);
            return Json(photos);
        }

        public async Task<IActionResult> SavePhoto()
        {
            await _savePhotoService.SavePhotoAsync(Session, sessionkey);
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
                await _indexService.GetIndexServiceAsync(model, Session, sessionkey);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string guid)
        {
            await _deleteService.DeleteAsync(guid, Session, sessionkey);
            return RedirectToAction(nameof(Index));
        }
    }
}
