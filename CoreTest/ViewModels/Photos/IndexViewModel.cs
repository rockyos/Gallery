using CoreTest.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTest.ViewModels.Photos
{
    public class IndexViewModel
    {
        public List<Photo> Photos { get; set; }
        [Required(ErrorMessage = "File not selected!")]
        [DataType(DataType.Upload)]
        [FileExtensions("jpg,jpeg,png,gif","File must be image!")]
        [FileSize("2000000", "File is too big!")]
        public IFormFile FormFile { get; set; }
    }
}