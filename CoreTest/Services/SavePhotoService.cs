using CoreTest.Models;
using CoreTest.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(List<Photo> photosfromsession);
    }

    public class SavePhotoService : BaseService, ISavePhotoService
    {
        public SavePhotoService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task SavePhotoAsync(List<Photo> photosfromsession)
        {

            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == item.Guid);
                    if (photo != null)
                    {
                        await UnitOfWork.PhotoRepository.RemoveAsync(photo);
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
