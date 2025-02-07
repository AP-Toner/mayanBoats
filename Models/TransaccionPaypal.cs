using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class TransaccionPaypal
{
    public int Id { get; set; }

    public string IdPaypal { get; set; } = null!;

    public DateOnly? FechaCreacion { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public string? CompradorCodigoPais { get; set; }

    public string? CompradorCorreo { get; set; }

    public string? CompradorNombre { get; set; }

    public string? CompradorApellido { get; set; }

    public string? CompradorId { get; set; }

    public string? CompraMoneda { get; set; }

    public float? CompraMonto { get; set; }

    public string? BeneficiarioCorreo { get; set; }

    public string? BeneficiarioId { get; set; }

    public string? PagoEstatus { get; set; }

    public DateOnly? FechaCaptura { get; set; }

    public string? Estatus { get; set; }
}
