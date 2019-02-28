using CoreTest.Repository;

namespace CoreTest.Services
{
    public class BaseService
    {
        protected UnitOfWork UnitOfWork { get; }

        public BaseService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}