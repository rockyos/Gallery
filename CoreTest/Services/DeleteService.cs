using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IDeleteService
    {
        Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos, IRepository _repository);
    }

    public class DeleteService : IDeleteService
    {
        public async Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos, IRepository _repository)
        {
            //photos = new List<Photo>();
            Photo photo = await _repository.GetOne(guid);
            if (photo != null)
            {
                if(photos == null)
                {
                    photos = new List<Photo>();
                }
                photos.Add(photo);
            } else
            {
                foreach (var item in photos)
                {
                    if (item.Guid == guid)
                    {
                        photos.Remove(item);
                        break;
                    }
                }
            }
            return photos;
        }
    }
}
