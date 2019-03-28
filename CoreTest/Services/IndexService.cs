using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class IndexService : IIndexService
    {
        public async Task GetIndexServiceAsync(Photo photoFromClient, ISession session, string sessionkey)
        {
            var photosInSession = session.Get<List<Photo>>(sessionkey);
            var photo = new Photo();
            using (var reader = new BinaryReader(photoFromClient.FormFile.OpenReadStream()))
            {
                var img = reader.ReadBytes((int)photoFromClient.FormFile.Length);
                photo.PhotoName = photoFromClient.FormFile.FileName;
                photo.ImageContent = img;
                photo.Guid = Guid.NewGuid().ToString();
            }

            if (photosInSession != null)
            {
                photosInSession.Add(photo);
                session.Set(sessionkey, photosInSession);
            } else
            {
                session.Set(sessionkey, new List<Photo>() { photo });
            }
        }
    }
}
