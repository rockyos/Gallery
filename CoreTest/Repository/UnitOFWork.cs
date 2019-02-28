using CoreTest.Models;
using CoreTest.Repository.Interfaces;
using System.Threading.Tasks;

namespace CoreTest.Repository
{


    public class UnitOfWork : IUnitOfWork
    {
        private IPhotoRepository _photoRepository;

        public UnitOfWork(PhotoContext сontext)
        {
            ApplicationContext = сontext;
        }

        public PhotoContext ApplicationContext { get; }

        public IPhotoRepository PhotoRepository => _photoRepository ?? (_photoRepository = new PhotoRepository(ApplicationContext));


        public async Task SubmitChangesAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }

    }
}
