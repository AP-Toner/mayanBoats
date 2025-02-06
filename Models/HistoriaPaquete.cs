using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class HistoriaPaquete
{
    public int Id { get; set; }

    public string? Paquete { get; set; }

    public int? Horas { get; set; }

    public double? CostoPaquete { get; set; }

    public double? PersonaAdicional { get; set; }

    public string? FechaCaptura { get; set; }

    public int? IdAnterior { get; set; }
}
