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

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync( ISession Session, string sessionkey)
        {
            List<Photo> photosFromSession = Session.Get<List<Photo>>(sessionkey);
            if (photosFromSession != null)
            {
                photosFromSession = photosFromSession.GroupBy(x => x.Guid).Select(y => y.First()).ToList();
            }

            List<Photo> photoFromDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosFromSession != null)
            {
                List<Photo> equalPhoto = new List<Photo>(); // find dublicate Photo in session and DB 
                foreach (var item in photosFromSession)
                {
                    Photo photo = photoFromDB.Find(c => c.Guid == item.Guid);
                    if (photo != null)
                    {
                        equalPhoto.Add(photo);
                    }
                }
                photoFromDB.RemoveAll(i => equalPhoto.Contains(i));
                photoFromDB.AddRange(photosFromSession);
               
            }
            List<PhotoDTO> photosDTO = _mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDB);
            return photosDTO;
        }
    }
}
