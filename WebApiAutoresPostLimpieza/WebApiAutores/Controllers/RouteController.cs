using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RouteController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RouteController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DatoHATEOAS>>> Get()
        {
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            var datosHateoas = new List<DatoHATEOAS>
            {
                new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self", metodo: "GET"),
                new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }), descripcion: "autores", metodo: "GET")
            };

            if(esAdmin.Succeeded)
            {
                datosHateoas.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));
                datosHateoas.Add(new DatoHATEOAS(enlace: Url.Link("crearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));
            }

            return datosHateoas;
        }
    }
}
