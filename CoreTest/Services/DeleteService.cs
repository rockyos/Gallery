using CoreTest.Models;
using CoreTest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Services
{
    public interface IDeleteService
    {
        Task<string> DeleteAsync(string guid, string datasession, IRepository _repository);
    }

    public class DeleteService : IDeleteService
    {
        public async Task<string> DeleteAsync(string guid, string datasession, IRepository _repository)
        {
            List<Photo> photosfromsession = new List<Photo>();
            if (datasession != null)
            {
                photosfromsession = JsonConvert.DeserializeObject<List<Photo>>(datasession);
            }
            Photo photo = await _repository.GetOne(guid);
            if (photo != null)
            {
                photosfromsession.Add(photo);
            }
            else
            {
                foreach (var item in photosfromsession)
                {
                    if (item.Guid == guid)
                    {
                        photosfromsession.Remove(item);
                        break;
                    }
                }
            }
            var serialisedDate = JsonConvert.SerializeObject(photosfromsession);
            return serialisedDate;
        }
    }
}
