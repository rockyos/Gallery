using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreTest.Services.Interfaces;

namespace CoreTest.Services
{
    public class DeleteService : BaseService, IDeleteService
    {
        public DeleteService(UnitOfWork unitOfWork) : base(unitOfWork)
        {   
        }

        public async Task<List<Photo>> DeleteAsync(string guid, List<Photo> photosInSession)
        {
            Photo photoDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == guid);
            if (photoDB != null)
            {
                if(photosInSession == null)
                {
                    photosInSession = new List<Photo>();
                }
                photosInSession.Add(photoDB);
            } else
            {
                foreach (var item in photosInSession)
                {
                    if (item.Guid == guid)
                    {
                        photosInSession.Remove(item);
                        break;
                    }
                }
            }
            return photosInSession;
        }
    }
}
