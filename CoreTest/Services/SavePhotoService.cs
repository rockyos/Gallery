using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(List<Photo> photosfromsession, IRepository _repository);
    }

    public class SavePhotoService : ISavePhotoService
    {
        public async Task SavePhotoAsync(List<Photo> photosfromsession, IRepository _repository)
        {
            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await _repository.GetOne(item.Guid);
                    if (photo != null)
                    {
                        _repository.Remove(photo);
                    }
                    else
                    {
                        item.Id = 0;
                        _repository.Add(item);
                    }
                }
                await _repository.SaveChanges();
            }
        }
    }
}
