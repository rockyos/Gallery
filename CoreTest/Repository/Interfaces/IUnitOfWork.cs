using System.Threading.Tasks;

namespace CoreTest.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IPhotoRepository PhotoRepository { get; }
        IUserRepository UserRepository { get; }
        Task SubmitChangesAsync();
    }
}