using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreTest.Models.Entity;

namespace CoreTest.Controllers
{
    public class PhotosController : Controller
    {
        private readonly string _sessionkey = "photos";
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

        public async Task<IActionResult> ImageResize(string id, int width)
        {
            var sessionPhotos = Session.Get<List<Photo>>(_sessionkey);
            var resizedImage = await _resizer.GetImageAsync(sessionPhotos, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }


        public async Task<IActionResult> GetListfromDb()
        {
            var photos = await _getPhotoService.GetPhotoDBandSessionAsync(Session, _sessionkey);
            return Json(photos);
        }

        public async Task<IActionResult> SavePhoto()
        {
            await _savePhotoService.SavePhotoAsync(Session, _sessionkey);
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Index(Photo model)
        {
            if (ModelState.IsValid)
            {
                await _indexService.GetIndexServiceAsync(model, Session, _sessionkey);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string guid)
        {
            await _deleteService.DeleteAsync(guid, Session, _sessionkey);
            return RedirectToAction(nameof(Index));
        }
    }
}
