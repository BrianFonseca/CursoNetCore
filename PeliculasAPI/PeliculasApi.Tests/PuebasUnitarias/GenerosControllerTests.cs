using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Controllers;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasApi.Tests.PuebasUnitarias
{
    [TestClass]
    public class GenerosControllerTests : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerTodosLosGeneros()
        {
            //Preparacion
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            await contexto.Generos.AddAsync(new Genero() { Nombre = "Genero1" });
            await contexto.Generos.AddAsync(new Genero() { Nombre = "Genero2" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);

            //Prueba
            var controller = new GenerosController(contexto2, mapper);
            var respuesta = await controller.Get();

            //Verificacion
            var generos = respuesta.Value;
            Assert.AreEqual(2, generos.Count);
        }

        [TestMethod]
        public async Task ObtenerGeneroPorIdNoExistente()
        {
            //Preparacion
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new GenerosController(contexto, mapper);
            var respuesta = await controller.Get(1);

            var resultado = respuesta.Result as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerGeneroPorIdExistente()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            await contexto.Generos.AddAsync(new Genero() { Nombre = "Genero1" });
            await contexto.Generos.AddAsync(new Genero() { Nombre = "Genero2" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new GenerosController(contexto2, mapper);

            var respuesta = await controller.Get(1);
            Assert.AreEqual(1, respuesta.Value.Id);
        }

        [TestMethod]
        public async Task CrearGenero()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var nuevoGenero = new GeneroCreacionDTO() { Nombre = "nuevo genero" };

            var controller = new GenerosController(contexto, mapper);

            var respuesta = await controller.Post(nuevoGenero);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.IsNotNull(resultado);

            var contexto2 = ConstruirContext(nombreBD);
            var cantidad = await contexto2.Generos.CountAsync();
            Assert.AreEqual(1, cantidad);
        }

        [TestMethod]
        public async Task ActualizarGenero()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            contexto.Generos.Add(new Genero { Nombre = "Genero 1" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new GenerosController(contexto2, mapper);

            var generoCreacionDTO = new GeneroCreacionDTO() { Nombre = "Nuevo Nombre" };

            var id = 1;
            var respuesta = await controller.Put(id, generoCreacionDTO);

            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreBD);
            var existe = await contexto3.Generos.AnyAsync(x => x.Nombre == "Nuevo Nombre");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task IntentaBorrarGeneroNoExistente()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new GenerosController(contexto,mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task BorrarGenero()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            contexto.Generos.Add(new Genero { Nombre = "Genero 1" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new GenerosController(contexto2, mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;

            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreBD);
            var existe = await contexto3.Generos.AnyAsync();
            Assert.IsFalse(existe);
        }
    }
}
