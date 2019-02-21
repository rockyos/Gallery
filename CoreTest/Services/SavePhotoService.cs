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
        Task SavePhotoAsync(string datasession, IRepository _repository);
    }

    public class SavePhotoService : ISavePhotoService
    {
        public async Task SavePhotoAsync(string datasession, IRepository _repository)
        {
            if (datasession != null)
            {
                List<Photo> photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
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
