using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]  // el placeholder /[controller] reemplazan el "controller" por el nombre del controlador
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio, ILogger <AutoresController> logger) 
        {
            this.context = context;
            this.servicio = servicio;
            this.logger = logger;
        }

        [HttpGet]   // api/autores
        [HttpGet("listado")] // api/autores/listado
        [HttpGet("/listado")] // /listado
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            //throw new NotImplementedException();
            logger.LogInformation("Estamos obteniendo los autores");
            logger.LogWarning("Este es un mensaje de prueba");
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("primero")]  // api/autores/primero?nombre=felipe&apellido=gavilan
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")] // para agregar mas parametros o para que sea opcional es => /{param2?} o valor fijo => {param2=persona}
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if(autor == null)
                return NotFound();

            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
                return NotFound();

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {

            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if(existeAutorConElMismoNombre) return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x  => x.Id == id);

            if (!existe)
                return NotFound();

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
