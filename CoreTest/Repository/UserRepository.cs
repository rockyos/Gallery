using CoreTest.Models;
using CoreTest.Repository.Interfaces;

namespace CoreTest.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PhotoContext context) : base(context)
        {
        }
    }
}
