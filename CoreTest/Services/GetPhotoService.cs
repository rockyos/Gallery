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

    public class GetPhotoService : BaseService, IGetPhotoService
    {
        public GetPhotoService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(List<Photo> photosfromsession)
        {
            List<Photo> photoFromDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosfromsession != null)
            {
                photoFromDB.AddRange(photosfromsession);
            }
            List<PhotoDTO> photosDTO = Mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDB);
            return photosDTO;
        }
    }
}
