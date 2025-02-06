using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Renta
{
    public int Id { get; set; }

    public int? TipoPago { get; set; }

    public string? IdPagoPayPal { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? Paquete { get; set; }

    public string? HoraInicial { get; set; }

    public int? HorasTour { get; set; }

    public string? HoraFinal { get; set; }

    public int? IdCliente { get; set; }

    public decimal? Monto { get; set; }

    public double? TipoCambio { get; set; }

    public double? Descuento { get; set; }

    public DateOnly? FechaCaptura { get; set; }

    public string? Estatus { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public int? Adicional { get; set; }

    public decimal? CostoAdicional { get; set; }
}
