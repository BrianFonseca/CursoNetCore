namespace WebApiAutores.DTOs
{
    public class ColeccionDeRecursos<T>: Recurso where T : class
    {
        public List<T> Valores { get; set; }
    }
}
