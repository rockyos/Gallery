using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteAsync(string guid, ISession Session, string sessionkey);
    }
}
