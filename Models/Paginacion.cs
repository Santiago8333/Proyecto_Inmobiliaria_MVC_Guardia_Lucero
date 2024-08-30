using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Paginacion<T> : List<T>
{
    public int PaginaInicio { get; private set; }
    public int PaginasTotales { get; private set; }

    public Paginacion(List<T> items, int contador, int paginaInicio, int cantidadregistros)
    {
        PaginaInicio = paginaInicio;
        PaginasTotales = (int)Math.Ceiling(contador / (double)cantidadregistros);
        this.AddRange(items);
    }

    public bool PaginasAnteriores => PaginaInicio > 1;
    public bool PaginasPosteriores => PaginaInicio < PaginasTotales;

    public static async Task<Paginacion<T>> CrearPaginacion(IQueryable<T> fuente, int paginaInicio, int cantidadregistros)
    {
        var contador = fuente.Count(); // Contar los elementos totales de la fuente
        var items = fuente.Skip((paginaInicio - 1) * cantidadregistros).Take(cantidadregistros).ToList();

        // Si necesitas que esto sea asincrónico, podrías convertirlo usando Task.FromResult
        return await Task.FromResult(new Paginacion<T>(items, contador, paginaInicio, cantidadregistros));
    }
}
