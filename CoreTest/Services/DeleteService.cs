using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreTest.Services
{
    public interface IDeleteService
    {
        Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos);
    }

    public class DeleteService : IDeleteService
    {
        UnitOfWork _uow { get; set; }
        public DeleteService(UnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Photo>> DeleteAsync(string guid, List<Photo> photos)
        {
            Photo photo = await (await _uow.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == guid);
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
