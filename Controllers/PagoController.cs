using mayanBoats.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace mayanBoats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly MayanDbContext _context;

        public PagoController(MayanDbContext context)
        {
            _context = context;
        }

        [HttpPost("capture")]
        public async Task<IActionResult> CapturarTransaccion([FromBody] PaypalResponse response)
        {
            if (response == null)
            {
                return BadRequest("Respuesta inválida de PayPal.");
            }

            // Crear entidad de TransaccionPaypal y mapear los valores
            var transaccion = new TransaccionPaypal
            {
                IdPaypal = response.id,
                FechaCreacion = response.create_time,
                Accion = "CAPTURE",
                CompradorCodigoPais = response.comprador?.direccion?.country_code,
                CompradorCorreo = response.comprador?.email_address,
                CompradorNombre = response.comprador?.nombre?.given_name,
                CompradorApellido = response.comprador?.nombre?.surname,
                CompradorId = response.comprador?.payer_id,
                CompraMoneda = response.purchase_units?.FirstOrDefault()?.monto?.currency_code,
                CompraMonto = float.TryParse(response.purchase_units?.FirstOrDefault()?.monto?.valor, out var monto) ? monto : 0,
                BeneficiarioCorreo = response.purchase_units?.FirstOrDefault()?.payee?.email_address,
                BeneficiarioId = response.purchase_units?.FirstOrDefault()?.payee?.merchant_id,
                PagoEstatus = response.status,
                FechaCaptura = response.update_time,
                Estatus = "A"
            };

            // Guardar en la base de datos
            _context.TransaccionPaypals.Add(transaccion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Transacción almacenada exitosamente.", data = response });
        }
    }
}
