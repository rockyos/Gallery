using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(ISession session, string sessionkey);
    }
}
