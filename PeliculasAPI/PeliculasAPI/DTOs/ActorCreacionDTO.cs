using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreacionDTO : ActorPatchDTO
    {
        [ImageSizeValidation(MaxSizeInMegaBytes:4)]
        [FileTypeValidation(typeGroupFile: TypeGroupFile.Image)]
        public IFormFile Foto { get; set; }
    }
}
