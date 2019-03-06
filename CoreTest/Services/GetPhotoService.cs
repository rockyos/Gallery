using AutoMapper;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class GetPhotoService : BaseService, IGetPhotoService
    {
        private IMapper _mapper;
        public GetPhotoService(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(List<Photo> photosFromSession)
        {
            List<Photo> photoFromDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosFromSession != null)
            {
                photoFromDB.AddRange(photosFromSession);
            }
            List<PhotoDTO> photosDTO = _mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDB);
            return photosDTO;
        }
    }
}
