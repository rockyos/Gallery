using AutoMapper;
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
       Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(List<Photo> datasession);
    }

    public class GetPhotoService : IGetPhotoService
    {
        UnitOFWork _uow { get; set; }
        public GetPhotoService(UnitOFWork uow)
        {
            _uow = uow;
        }

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(List<Photo> photosfromsession)
        {

            Mapper.Initialize(cfg => cfg.CreateMap<Photo, PhotoDTO>());
            var photos = Mapper.Map<List<Photo>, List<PhotoDTO>>(await _uow.PhotoRepository.GetList().ToListAsync());
            
            if (photosfromsession != null)
            {
                var photoDTOfromsession = Mapper.Map<List<Photo>, List<PhotoDTO>>(photosfromsession);
                photos.AddRange(photoDTOfromsession);
            }
            return photos;
        }
    }
}
