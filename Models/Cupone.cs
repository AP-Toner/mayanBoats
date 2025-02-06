using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Cupone
{
    public int Id { get; set; }

    public string? Cupon { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? Activo { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public decimal? Descuento { get; set; }

    public DateOnly? FechaCaptura { get; set; }

    public string? Estatus { get; set; }
}
