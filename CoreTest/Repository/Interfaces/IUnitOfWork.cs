using System.Threading.Tasks;

namespace CoreTest.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IPhotoRepository PhotoRepository { get; }

        Task SubmitChangesAsync();
    }
}