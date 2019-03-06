using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface IDeleteService
    {
        Task<List<Photo>> DeleteAsync(string guid, List<Photo> photosInSession);
    }
}
