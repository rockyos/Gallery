using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(List<Photo> photosInSession);
    }
}
