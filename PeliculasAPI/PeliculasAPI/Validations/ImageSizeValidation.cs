using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PeliculasAPI.Validations
{
    public class ImageSizeValidation : ValidationAttribute
    {
        private readonly int maxSizeInMegaBytes;

        public ImageSizeValidation(int MaxSizeInMegaBytes)
        {
            maxSizeInMegaBytes = MaxSizeInMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if(formFile == null) {
                return ValidationResult.Success;
            }

            if(formFile.Length > maxSizeInMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no puede superar los {maxSizeInMegaBytes}mb");
            }

            return ValidationResult.Success;
        }
    }
}
