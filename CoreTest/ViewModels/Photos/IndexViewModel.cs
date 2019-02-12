using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTest.ViewModels.Photos
{
    public class IndexViewModel
    {
        public List<Photo> Photos { get; set; }
        [DataType(DataType.Upload)]
        [JsonIgnore]
        public List<IFormFile> FormFile { get; set; }
    }
}