using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTest
    {
        [TestMethod]
        public void PrimeraLetraMinisculaDevuelveError()
        {
            //Preparacion
            var PrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "felipe";
            var valContext = new ValidationContext(new { Nombre = valor});

            //Ejecucion
            var resultado = PrimeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.AreEqual("La primera letra debe ser mayúscula", resultado?.ErrorMessage);
        }

        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            //Preparacion
            var PrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = PrimeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorConPrimeraLetraMayuscula_NoDevuelveError()
        {
            //Preparacion
            var PrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "Felipe";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = PrimeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(resultado);
        }
    }
}