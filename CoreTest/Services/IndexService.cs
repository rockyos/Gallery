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
        string GetIndexService(Photo photo, string datasession);
    }

    public class IndexService : IIndexService
    {
        public string GetIndexService(Photo photo, string datasession)
        {
            Photo photolist = new Photo();
            using (var reader = new BinaryReader(photo.FormFile[0].OpenReadStream()))
            {
                byte[] img = reader.ReadBytes((int)photo.FormFile[0].Length);
                photolist.PhotoName = photo.FormFile[0].FileName;
                photolist.ImageContent = img;
                photolist.Guid = Guid.NewGuid().ToString();
            }

            if (datasession != null)
            {
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                photosfromsession.Add(photolist);
                var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
                return serialisedDate;
            }
            else
            {
                var serialisedDate = JsonConvert.SerializeObject(new List<Photo>() { photolist });
                return serialisedDate;
            }
        }
    }
}
