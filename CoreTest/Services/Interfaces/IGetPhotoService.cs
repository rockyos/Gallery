using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreTest.Services.Interfaces
{
    public interface IGetPhotoService
    {
        Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(ISession session, string sessionkey);
    }
}
