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
        public List<Photo> GetIndexService(Photo photoFromClient, List<Photo> photosInSession)
        {
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
                return photosInSession;
            } else
            {
                return new List<Photo>() { photo };
            }
        }
    }
}
