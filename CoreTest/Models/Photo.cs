using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
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
        [NotMapped]
        [Required(ErrorMessage = "File not selected!")]
        [DataType(DataType.Upload)]
        [FileExtensions("jpg,jpeg,png,gif","File must be image!")]
        [FileSize("2000000", "File is too big!")]
        public List<IFormFile> FormFile { get; set; }

        public byte[] ImageContent { get; set; }
        public string PhotoName { get; set; }
    }
}
