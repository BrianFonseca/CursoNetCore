using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers.V1;
using WebApiAutores.Tests.Mocks;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtenemos4Links()
        {
            //Preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(4, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Links_UsandoMoq()
        {
            //Preparacion
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var rootController = new RootController(mockAuthorizationService.Object);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(
                It.IsAny<string>(),
                It.IsAny<object>()
                )).Returns(string.Empty);
            rootController.Url = mockUrlHelper.Object;

            //Ejecucion
            var resultado = await rootController.Get();

            //Verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}
