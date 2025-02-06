using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Producto
{
    public int Id { get; set; }

    public int IdTipoProducto { get; set; }

    public string? Descripcion { get; set; }

    public int Cantidad { get; set; }
}
