using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class IndexService : IIndexService
    {
        public async Task GetIndexServiceAsync(Photo photoFromClient, ISession Session, string sessionkey)
        {
            List<Photo> photosInSession = Session.Get<List<Photo>>(sessionkey);
            Photo photo = new Photo();
            using (var reader = new BinaryReader(photoFromClient.FormFile.OpenReadStream()))
            {
                byte[] img = reader.ReadBytes((int)photoFromClient.FormFile.Length);
                photo.PhotoName = photoFromClient.FormFile.FileName;
                photo.ImageContent = img;
                photo.Guid = Guid.NewGuid().ToString();
            }

            if (photosInSession != null)
            {
                photosInSession.Add(photo);
                Session.Set(sessionkey, photosInSession);
            } else
            {
                Session.Set(sessionkey, new List<Photo>() { photo });
            }
        }
    }
}
