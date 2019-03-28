using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteAsync(string guid, ISession session, string sessionkey);
    }
}
