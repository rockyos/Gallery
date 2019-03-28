using CoreTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreTest.Models.Entity;

namespace CoreTest.Services.Interfaces
{
    public interface IResizeService
    {
        Task<byte[]> GetImageAsync(List<Photo> photosInSession, string id, int photoWidthInPixel);
    }
}
