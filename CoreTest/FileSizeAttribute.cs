using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileSizeAttribute : ValidationAttribute, IClientModelValidator
    {
        private string errorMessageString;
        private long val_file_size;

        public FileSizeAttribute(string fileExtensions, string errorMessage) : base(errorMessage)
        {
            val_file_size = Convert.ToInt64(fileExtensions);
            errorMessageString = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            IFormFile file = value as IFormFile;

            if (val_file_size < file.Length)
            {
                 validationResult = new ValidationResult(errorMessageString);
            }
            return validationResult;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
           // context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-filesize", errorMessageString);
            context.Attributes.Add("data-val-filesize-data", val_file_size.ToString());
        }
    }

}
