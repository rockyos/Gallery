using CoreTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreTest.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace CoreTest.Services.Interfaces
{
    public interface IGetPhotoService
    {
        Task<List<PhotoDto>> GetPhotoDBandSessionAsync(ISession session, string sessionkey);
    }
}
