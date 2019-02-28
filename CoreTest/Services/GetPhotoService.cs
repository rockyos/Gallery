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
            List<Photo> photoFromDB = await _uow.PhotoRepository.GetList().ToListAsync();
            if (photosfromsession != null)
            {
                photoFromDB.AddRange(photosfromsession);
            }
            List<PhotoDTO> photosDTO = Mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDB);
            return photosDTO;
        }
    }
}
