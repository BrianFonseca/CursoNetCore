using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApiAutores.Utilidades
{
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var nameSpaceControlador = controller.ControllerType.Namespace; // Controllers.V1
            var versionAPI = nameSpaceControlador.Split('.').Last().ToLower(); // v1
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
