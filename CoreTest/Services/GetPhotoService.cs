using AutoMapper;
using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(List<Photo> photosFromSession, ISession Session, string sessionkey)
        {
            List<Photo> photoFromDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosFromSession != null)
            {
                List<Photo> equalPhoto = new List<Photo>(); // find dublicate Photo in session and DB and delete their from session
                foreach (var item in photosFromSession)
                {
                    if (photoFromDB.Find(c => c.Guid == item.Guid) != null)
                    {
                        equalPhoto.Add(item);
                    }
                }
                photosFromSession.RemoveAll(i => equalPhoto.Contains(i));
                if (equalPhoto.Capacity != 0) // if find dublicate Photo save to session
                {
                    Session.Set(sessionkey, photosFromSession);
                }

                if (photosFromSession != null)
                {
                    photoFromDB.AddRange(photosFromSession);
                }                  
            }
            List<PhotoDTO> photosDTO = _mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDB);
            return photosDTO;
        }
    }
}
