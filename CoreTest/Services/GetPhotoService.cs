using CoreTest.Models;
using CoreTest.Repository;
using Microsoft.EntityFrameworkCore;
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
        UnitOFWork _uow { get; set; }
        public GetPhotoService(UnitOFWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Photo>> GetPhotoDBandSessionAsync(List<Photo> photosfromsession)
        {
            var photos = await _uow.PhotoRepository.GetList().Select(x => new Photo()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    PhotoName = x.PhotoName
                })
                .ToListAsync();
            
            if (photosfromsession != null)
            {
                photosfromsession.ForEach(x => x.ImageContent = null);
                photos.AddRange(photosfromsession);
            }
            return photos;
        }
    }
}
