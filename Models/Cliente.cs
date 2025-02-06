using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Telefono { get; set; }

    public DateOnly? FechaRegistro { get; set; }
}
