using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(ISession Session, string sessionkey);
    }
}
