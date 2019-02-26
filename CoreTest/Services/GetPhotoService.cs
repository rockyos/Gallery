using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IGetPhotoService
    {
       Task<List<Photo>> GetPhotoDBandSessionAsync(List<Photo> datasession);
    }

    public class GetPhotoService : IGetPhotoService
    {
        private readonly IRepository<Photo> _repository;
        public GetPhotoService(IRepository<Photo> repository)
        {
            _repository = repository;
        }

        public async Task<List<Photo>> GetPhotoDBandSessionAsync(List<Photo> photosfromsession)
        {
            List<Photo> photo = (List<Photo>) await _repository.GetAll();

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
