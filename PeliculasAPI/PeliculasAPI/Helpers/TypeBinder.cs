using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PeliculasAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var proveedorDeValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if(proveedorDeValores == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var valorDeserealizado = JsonConvert.DeserializeObject<T>(proveedorDeValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserealizado);
            }
            catch (Exception)
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor invalido para tipo List<int>");
            }

            return Task.CompletedTask;
        }
    }
}
