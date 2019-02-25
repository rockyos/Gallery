using CoreTest.Models;
using CoreTest.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IPhotolistService
    {
        List<Photo> GetPhotolistAsync(Photo model, IRepository<Photo> _repository);
    }

    public class PhotolistService : IPhotolistService
    {
        public List<Photo> GetPhotolistAsync(Photo model, IRepository<Photo> _repository)
        {
            List<Photo> photolist = new List<Photo>();
            foreach (var item in model.FormFile)
            {
                byte[] img;
                using (var reader = new BinaryReader(item.OpenReadStream()))
                {
                    img = reader.ReadBytes((int)item.Length);
                }
                photolist.Add(new Photo { PhotoName = item.FileName, ImageContent = img });
            }
            foreach (var item in photolist)
            {
                _repository.Add(item);
            }
            return photolist;
        }
    }
}
