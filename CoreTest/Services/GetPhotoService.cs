using CoreTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IGetPhotoService
    {
        List<Photo> GetPhotoDBandSession(List<Photo> photo, string datasession);
    }

    public class GetPhotoService : IGetPhotoService
    {
        public List<Photo> GetPhotoDBandSession(List<Photo> photo, string datasession)
        {
            if (datasession != null)
            {
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
                foreach (var item in photosfromsession)
                {
                    item.ImageContent = null;
                }
                photo.AddRange(photosfromsession);
            }
            return photo;
        }      
    }
}
