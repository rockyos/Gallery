using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class SavePhotoService : BaseService, ISavePhotoService
    {
        public SavePhotoService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task SavePhotoAsync(List<Photo> photosInSession)
        {
            if (photosInSession != null)
            {
                foreach (var item in photosInSession)
                {
                    Photo photoDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == item.Guid);
                    if (photoDB != null)
                    {
                        await UnitOfWork.PhotoRepository.RemoveAsync(photoDB);
                    }
                    else
                    {
                        await UnitOfWork.PhotoRepository.InsertAsync(item);
                    }
                }
                await UnitOfWork.SubmitChangesAsync();
            }
        }
    }
}
