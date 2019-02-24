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
        List<Photo> GetPhotoDBandSession(List<Photo> photo, List<Photo> datasession);
    }

    public class GetPhotoService : IGetPhotoService
    {
        public List<Photo> GetPhotoDBandSession(List<Photo> photo, List<Photo> photosfromsession)
        {
            if (photosfromsession != null)
            {
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
