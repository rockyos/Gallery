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
        Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos);
    }

    public class DeleteService : IDeleteService
    {
        private readonly IRepository<Photo> _repository;
        public DeleteService(IRepository<Photo> repository)
        {
            _repository = repository;
        }

        public async Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos)
        {
            Photo photo = await _repository.GetOneAsync(m => m.Guid == guid);
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
