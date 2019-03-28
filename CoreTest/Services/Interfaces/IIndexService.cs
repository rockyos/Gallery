using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CoreTest.Models.Entity;

namespace CoreTest.Services.Interfaces
{
    public interface IIndexService
    {
        Task GetIndexServiceAsync(Photo photoFromClient, ISession session, string sessionkey);
    }
}
