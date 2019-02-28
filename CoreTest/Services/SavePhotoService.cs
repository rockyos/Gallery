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

    public class SavePhotoService : ISavePhotoService
    {
        private UnitOfWork _uow { get; set; }
        public SavePhotoService(UnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task SavePhotoAsync(List<Photo> photosfromsession)
        {

            if (photosfromsession != null)
            {
                foreach (var item in photosfromsession)
                {
                    Photo photo = await (await _uow.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == item.Guid);
                    if (photo != null)
                    {
                        await _uow.PhotoRepository.RemoveAsync(photo);
                    }
                    else
                    {
                        await _uow.PhotoRepository.InsertAsync(item);
                    }
                }
                await _uow.SubmitChangesAsync();
            }
        }
    }
}
