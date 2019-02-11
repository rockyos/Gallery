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
        public List<IFormFile> FormFile { get; set; }
    }
}