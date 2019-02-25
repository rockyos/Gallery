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
        Task SavePhotoAsync(List<Photo> photosfromsession, IRepository<Photo> repository, IUnitOfWork unitOfWork);
    }

    public class SavePhotoService : ISavePhotoService
    {
        public async Task SavePhotoAsync(List<Photo> photosfromsession, IRepository<Photo> repository, IUnitOfWork unitOfWork)
        {
            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await repository.GetOne(m => m.Guid == item.Guid);
                    if (photo != null)
                    {
                        repository.Remove(photo);
                    }
                    else
                    {
                        item.Id = 0;
                        repository.Add(item);
                    }
                }
                unitOfWork.SaveToDB();
            }
        }
    }
}
