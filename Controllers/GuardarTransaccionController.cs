using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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

        // Método POST modificado para manejar la respuesta del IPN
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DatosTransaccion datos)
        {
            try
            {
                // Validar si la transacción de PayPal es válida
                if (datos.transaccion != null && !string.IsNullOrEmpty(datos.transaccion.idPaypal))
                {
                    // Validar la transacción a través de IPN de PayPal
                    var ipnValido = await ValidarPayPalIPN(datos.transaccion.idPaypal, datos.transaccion.fechaCreacion, datos.transaccion.beneficiarioCorreo, datos.transaccion.compradorCorreo);

                    if (ipnValido)
                    {
                        // Si la validación de IPN es exitosa, proceder a guardar los datos
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

                        // Guardar la transacción de PayPal si la validación es exitosa
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

                        // Guardar los cambios en la base de datos
                        await _context.SaveChangesAsync();

                        return Ok(new { success = true });
                    }
                    else
                    {
                        // Si la validación del IPN falla, devolver una respuesta de error
                        return BadRequest(new { success = false, message = "Transaction could not be verified with PayPal" });
                    }
                }

                // Si los datos de la transacción son inválidos, devolver un error
                return BadRequest(new { success = false, message = "Invalid transaction data" });
            }
            catch (Exception ex)
            {
                // Loguear el error para facilitar la depuración
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { success = false, message = "Error al guardar transacción y reservación.", error = ex.Message });
            }
        }

        // Método auxiliar para validar la transacción de PayPal a través de IPN
        // SANDBOX: https://ipnpb.sandbox.paypal.com/cgi-bin/webscr
        // LIVE: https://ipnpb.paypal.com/cgi-bin/webscr
        private async Task<bool> ValidarPayPalIPN(string transactionId, string paymentDate, string receiverEmail, string payerEmail)
        {
            using (var client = new HttpClient())
            {
                // Preparar datos para validación
                var verificationData = new Dictionary<string, string>
        {
            { "cmd", "_notify-validate" },
            { "txn_id", transactionId },
            { "payment_date", paymentDate },
            { "receiver_email", "paypal@placara.com" },
            { "payer_email", payerEmail }
        };

                // Log the request before sending it
                Console.WriteLine("Sending verification request to PayPal:");
                foreach (var item in verificationData)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }

                // Enviar datos a PayPal para validación
                var verificationResponse = await client.PostAsync(
                    "https://ipnpb.sandbox.paypal.com/cgi-bin/webscr",  // Ensure you're using the sandbox/live URL accordingly
                    new FormUrlEncodedContent(verificationData)
                );

                var verificationResult = await verificationResponse.Content.ReadAsStringAsync();

                // Log the response from PayPal for debugging
                Console.WriteLine($"IPN Verification Response: {verificationResult}");

                // Verificar si la validación fue exitosa
                return verificationResult == "VERIFIED";
            }
        }



        // Modelos de datos (mantenerlos como están)
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
