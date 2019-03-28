using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CoreTest.Models.Entity
{
    public class Photo
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        [JsonIgnore]
        public IFormFile FormFile { get; set; }
        public byte[] ImageContent { get; set; }
        public string PhotoName { get; set; }
    }
}
