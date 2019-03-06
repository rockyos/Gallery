using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services.Interfaces
{
    public interface IResizeService
    {
        Task<byte[]> GetImageAsync(List<Photo> photosInSession, string id, int photoWidthInPixel);
    }
}
