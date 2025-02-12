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
                    Apellido = datos.reservacion.apellido,
                    CorreoElectronico = datos.reservacion.correo,
                    Telefono = datos.reservacion.telefono,
                    Fecha = DateOnly.Parse(datos.reservacion.fecha),
                    Paquete = int.Parse(datos.reservacion.paquete),
                    HoraInicial = datos.reservacion.horaInicial,
                    HorasTour = int.Parse(datos.reservacion.horasTour),
                    HoraFinal = datos.reservacion.horaFinal,
                    IdCliente = int.Parse(datos.reservacion.idCliente),
                    Monto = decimal.Parse(datos.reservacion.totalDespuesDescuento.Replace("$", "")),
                    TipoCambio = double.Parse(datos.reservacion.tipoCambio),
                    Descuento = double.Parse(datos.reservacion.descuentoAplicable.Replace("$", "")),
                    TipoPago = int.Parse(datos.reservacion.tipoPago),
                    Estatus = datos.reservacion.estatus,
                    IdPagoPayPal = datos.reservacion.idPagoPaypal,
                    Adicional = int.Parse(datos.reservacion.personaAdicional),
                    CostoAdicional = decimal.Parse(datos.reservacion.costoAdicional.Replace("$", "")),
                    FechaCaptura = DateOnly.Parse(datos.reservacion.fechaCaptura),
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
                    Accion = datos.transaccion.accion,
                    CompradorCodigoPais = datos.transaccion.compradorCodigoPais,
                    CompradorId = datos.transaccion.compradorId,
                    BeneficiarioCorreo = datos.transaccion.beneficiarioCorreo,
                    BeneficiarioId = datos.transaccion.beneficiarioId,
                    FechaCaptura = DateOnly.Parse(datos.transaccion.fechaCaptura),
                    Estatus = datos.transaccion.estatus,
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
            public string apellido { get; set; }
            public string correo { get; set; }
            public string telefono { get; set; }
            public string fecha { get; set; }
            public string horaInicial { get; set; }
            public string horasTour { get; set; }
            public string horaFinal { get; set; }
            public string personaAdicional { get; set; }
            public string descuentoAplicable { get; set; }
            public string totalDespuesDescuento { get; set; }
            public string tipoPago { get; set; }
            public string paquete { get; set; }
            public string idCliente { get; set; }
            public string tipoCambio { get; set; }
            public string costoAdicional { get; set; }
            public string estatus { get; set; }
            public string fechaCaptura { get; set; }
            public string idPagoPaypal { get; set; }
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
            public string accion { get; set; }
            public string compradorCodigoPais { get; set; }
            public string compradorId { get; set; }
            public string beneficiarioCorreo { get; set; }
            public string beneficiarioId { get; set; }
            public string fechaCaptura { get; set; }
            public string estatus { get; set; }
        }
    }
}
