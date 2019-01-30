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
    public class FileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        private List<string> AllowedExtensions { get; set; }
        string errorMessageString;
        string ext;
        public FileExtensionsAttribute(string fileExtensions, string errorMessage) : base(errorMessage)
        {
            ext = fileExtensions;
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            errorMessageString = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            IFormFile file = value as IFormFile;
            if (file != null)
            {
                var fileName = file.FileName;
                bool validateres = AllowedExtensions.Any(y => fileName.EndsWith(y));
                if (validateres == false)
                {
                    validationResult = new ValidationResult(errorMessageString);
                } 
            }
            return validationResult;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-fileextensions", errorMessageString);
            context.Attributes.Add("data-val-fileextensions-data", ext);
        }
    }
}
