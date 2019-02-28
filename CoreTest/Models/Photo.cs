using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Models
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
