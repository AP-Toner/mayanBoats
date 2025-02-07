using System;
using System.Collections.Generic;

namespace mayanBoats.Models;

public partial class Pregunta
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Asunto { get; set; }

    public string? Mensaje { get; set; }

    public DateOnly? FechaCaptura { get; set; }

    public int? Activo { get; set; }

    public DateOnly? FechaRespuesta { get; set; }

    public string? Respuesta { get; set; }
}
