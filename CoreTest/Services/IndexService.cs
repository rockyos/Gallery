using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IIndexService
    {
        List<Photo> GetIndexService(Photo photo, List<Photo> datasession);
    }

    public class IndexService : IIndexService
    {
        public List<Photo> GetIndexService(Photo photo, List<Photo> photosfromsession)
        {
            Photo photolist = new Photo();
            using (var reader = new BinaryReader(photo.FormFile[0].OpenReadStream()))
            {
                byte[] img = reader.ReadBytes((int)photo.FormFile[0].Length);
                photolist.PhotoName = photo.FormFile[0].FileName;
                photolist.ImageContent = img;
                photolist.Guid = Guid.NewGuid().ToString();
            }

            if (photosfromsession != null)
            {
                photosfromsession.Add(photolist);
                return photosfromsession;
            }
            else
            {
                return new List<Photo>() { photolist };
            }
        }
    }
}
