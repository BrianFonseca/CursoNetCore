using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class PeliculaCreacionDTO : PeliculaPatchDTO
    {
        [ImageSizeValidation(MaxSizeInMegaBytes: 4)]
        [FileTypeValidation(typeGroupFile: TypeGroupFile.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIDs { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO>  Actores { get; set; }
    }
}
