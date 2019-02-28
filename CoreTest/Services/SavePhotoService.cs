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
        UnitOFWork _uow { get; set; }
        public SavePhotoService(UnitOFWork uow)
        {
            _uow = uow;
        }
        public async Task SavePhotoAsync(List<Photo> photosfromsession)
        {

            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await _uow.PhotoRepository.GetOneAsync(m => m.Guid == item.Guid);
                    if (photo != null)
                    {
                        _uow.PhotoRepository.Remove(photo);
                    }
                    else
                    {
                        await _uow.PhotoRepository.AddAsync(item);
                    }
                }
                _uow.SaveAll();
            }
        }
    }
}
