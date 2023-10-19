using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Utilidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    //[Route("api/v1/autores")]  // el placeholder /[controller] reemplazan el "controller" por el nombre del controlador
    [Route("api/autores")]
    [CabeceraEstaPresente("x-version", "1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    [ApiConventionType(typeof(DefaultApiConventions))]//Documenta los tipos de respuesta de los endpoints en swagger
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "obtenerAutoresV1")]   // api/autores
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var autores = await queryable.OrderBy(autor => autor.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name = "obtenerAutorV1")] // para agregar mas parametros o para que sea opcional es => /{param2?} o valor fijo => {param2=persona}
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
                return NotFound();

            var dto = mapper.Map<AutorDTOConLibros>(autor);
            return dto;
        }

        [HttpGet("{nombre}", Name = "obtenerAutoresPorNombreV1")]
        public async Task<List<AutorDTO>> GetByName(string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name = "crearAutorV1")]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
        {

            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutorConElMismoNombre) return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name = "actualizarAutorV1")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCracionDTO, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            var autor = mapper.Map<Autor>(autorCracionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Borra un autor
        /// </summary>
        /// <param name="id">Id del autor a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "borrarAutorV1")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
