using CoreTest.Models;
using CoreTest.Repository.Interfaces;
using System.Threading.Tasks;

namespace CoreTest.Repository
{


    public class UnitOfWork : IUnitOfWork
    {
        private IPhotoRepository _photoRepository;
        private IUserRepository _userRepository;
        public PhotoContext ApplicationContext { get; }

        public UnitOfWork(PhotoContext сontext)
        {
            ApplicationContext = сontext;
        }

        public IPhotoRepository PhotoRepository => _photoRepository ?? (_photoRepository = new PhotoRepository(ApplicationContext));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(ApplicationContext));

        public async Task SubmitChangesAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }

    }
}
