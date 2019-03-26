using CoreTest.Extensions;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public class SavePhotoService : BaseService, ISavePhotoService
    {
        public SavePhotoService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task SavePhotoAsync(ISession Session, string sessionkey)
        {
            var log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.RollingFile("logs\\log-{Date}.txt").CreateLogger();
            
            List<Photo> photosInSession = Session.Get<List<Photo>>(sessionkey);
            if (photosInSession != null)
            {
                foreach (var item in photosInSession)
                {
                    Photo photoDB = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == item.Guid);
                    if (photoDB != null)
                    {
                        log.Information("Remove photos from DB: {@PhotoDB}", photoDB);
                        await UnitOfWork.PhotoRepository.RemoveAsync(photoDB);
                    }
                    else
                    {
                        log.Information("Add photos to DB: {@PhotoDB}", item);
                        await UnitOfWork.PhotoRepository.InsertAsync(item);
                     }
                }
                await UnitOfWork.SubmitChangesAsync();
            }     
        }
    }
}
