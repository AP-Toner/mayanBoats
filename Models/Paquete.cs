using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Paquete
{
    public int Id { get; set; }

    public int? Paquete1 { get; set; }

    public int? Horas { get; set; }

    public double? CostoPaquete { get; set; }

    public double? PersonaAdicional { get; set; }

    public string? FechaCaptura { get; set; }

    public int? Activo { get; set; }

    // Obtener paquetes de la base de datos
    public static async Task<IList<Paquete>> GetAllPaquetesAsync(MayanDbContext context)
    {
        return await context.Paquetes.ToListAsync();
    }
}
