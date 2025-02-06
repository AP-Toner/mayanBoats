using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Password { get; set; }

    public DateOnly? FechaCaptura { get; set; }

    public string? Estatus { get; set; }
}
