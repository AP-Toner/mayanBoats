using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;

namespace mayanBoats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuardarTransaccionController : Controller
    {
        private readonly MayanDbContext _context;

        public GuardarTransaccionController(MayanDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DatosTransaccion datos)
        {
            try
            {
                // Guardar Renta
                var renta = new Renta
                {
                    Nombre = datos.reservacion.nombre,
                    CorreoElectronico = datos.reservacion.correo,
                    Telefono = datos.reservacion.telefono,
                    Fecha = DateOnly.Parse(datos.reservacion.fecha),
                    HoraInicial = datos.reservacion.hora,
                    Monto = decimal.Parse(datos.reservacion.totalDespuesDescuento.Replace("$", "")),
                    Estatus = "A", // Activo
                    IdPagoPayPal = datos.transaccion.idPaypal
                };

                _context.Rentas.Add(renta);

                // Guardar Transacción PayPal
                var transaccionPaypal = new TransaccionPaypal
                {
                    IdPaypal = datos.transaccion.idPaypal,
                    FechaCreacion = DateOnly.Parse(datos.transaccion.fechaCreacion),
                    CompradorNombre = datos.transaccion.compradorNombre,
                    CompradorApellido = datos.transaccion.compradorApellido,
                    CompradorCorreo = datos.transaccion.compradorCorreo,
                    CompraMoneda = datos.transaccion.compraMoneda,
                    CompraMonto = float.Parse(datos.transaccion.compraMonto),
                    PagoEstatus = datos.transaccion.pagoEstatus,
                    Estatus = "A", // Activo
                    Accion = "CAPTURE" // Captura
                };

                _context.TransaccionPaypals.Add(transaccionPaypal);

                // Save both entities in a single transaction
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { success = false, message = "Error al guardar transacción y reservación.", error = ex.Message });
            }
        }

        public class DatosTransaccion
        {
            public DatosReservacion reservacion { get; set; }
            public DetallesTransaccion transaccion { get; set; }
        }

        public class DatosReservacion
        {
            public string nombre { get; set; }
            public string correo { get; set; }
            public string telefono { get; set; }
            public string fecha { get; set; }
            public string hora { get; set; }
            public string precioBase { get; set; }
            public string personaAdicional { get; set; }
            public string subtotal { get; set; }
            public string descuentoAplicable { get; set; }
            public string totalDespuesDescuento { get; set; }
        }

        public class DetallesTransaccion
        {
            public string idPaypal { get; set; }
            public string fechaCreacion { get; set; }
            public string compradorNombre { get; set; }
            public string compradorApellido { get; set; }
            public string compradorCorreo { get; set; }
            public string compraMoneda { get; set; }
            public string compraMonto { get; set; }
            public string pagoEstatus { get; set; }
        }
    }
}
