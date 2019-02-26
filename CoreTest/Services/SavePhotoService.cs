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
        Task SavePhotoAsync(List<Photo> photosfromsession);
    }

    public class SavePhotoService : ISavePhotoService
    {
        private readonly IRepository<Photo> _repository;
        public SavePhotoService(IRepository<Photo> repository)
        {
            _repository = repository;
        }
        public async Task SavePhotoAsync(List<Photo> photosfromsession)
        {

            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await _repository.GetOne(m => m.Guid == item.Guid);
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
                _repository.SaveToDB();
            }
        }
    }
}
