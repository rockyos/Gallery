using AutoMapper;
using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class GetPhotoService : BaseService, IGetPhotoService
    {
        private readonly IMapper _mapper;
        public GetPhotoService(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<PhotoDto>> GetPhotoDBandSessionAsync( ISession session, string sessionkey)
        {
            var photosFromSession = session.Get<List<Photo>>(sessionkey);
            var photoFromDb = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosFromSession != null)
            {
                var hidePhotoFromSession = new List<Photo>();
                foreach (var item in photosFromSession)
                {
                    var photo = photoFromDb.Find(c => c.Guid == item.Guid);
                    if (photo != null)
                    {
                        photoFromDb.Remove(photo);
                        hidePhotoFromSession.Add(item);
                    }
                }
                photosFromSession.RemoveAll(i => hidePhotoFromSession.Contains(i));
                photoFromDb.AddRange(photosFromSession);
            }
            var photosDto = _mapper.Map<List<Photo>, List<PhotoDto>>(photoFromDb);
            return photosDto;
        }
    }
}
