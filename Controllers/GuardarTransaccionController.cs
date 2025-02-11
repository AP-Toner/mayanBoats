using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

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
        public async Task<IActionResult> Post()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string ipnData = await reader.ReadToEndAsync();
                    Console.WriteLine($"Received IPN data: {ipnData}");

                    var datosTransaccion = JsonSerializer.Deserialize<DatosTransaccion>(ipnData);

                    if (datosTransaccion == null || datosTransaccion.transaccion == null || datosTransaccion.reservacion == null)
                    {
                        Console.WriteLine("IPN data is incomplete, aborting...");
                        return BadRequest(new { success = false, message = "Invalid IPN data format" });
                    }

                    var ipnValid = await ValidarPayPalIPN(datosTransaccion.transaccion);

                    if (ipnValid)
                    {
                        var renta = new Renta
                        {
                            Nombre = datosTransaccion.reservacion.nombre,
                            Apellido = datosTransaccion.reservacion.apellido,
                            CorreoElectronico = datosTransaccion.reservacion.correo,
                            Telefono = datosTransaccion.reservacion.telefono,
                            Fecha = DateOnly.Parse(datosTransaccion.reservacion.fecha),
                            Paquete = int.Parse(datosTransaccion.reservacion.paquete),
                            HoraInicial = datosTransaccion.reservacion.horaInicial,
                            HorasTour = int.Parse(datosTransaccion.reservacion.horasTour),
                            HoraFinal = datosTransaccion.reservacion.horaFinal,
                            IdCliente = int.Parse(datosTransaccion.reservacion.idCliente),
                            Monto = decimal.Parse(datosTransaccion.reservacion.totalDespuesDescuento.Replace("$", "")),
                            TipoCambio = double.Parse(datosTransaccion.reservacion.tipoCambio),
                            Descuento = double.Parse(datosTransaccion.reservacion.descuentoAplicable.Replace("$", "")),
                            TipoPago = int.Parse(datosTransaccion.reservacion.tipoPago),
                            Estatus = datosTransaccion.reservacion.estatus,
                            IdPagoPayPal = datosTransaccion.reservacion.idPagoPaypal,
                            Adicional = int.Parse(datosTransaccion.reservacion.personaAdicional),
                            CostoAdicional = decimal.Parse(datosTransaccion.reservacion.costoAdicional.Replace("$", "")),
                            FechaCaptura = DateOnly.Parse(datosTransaccion.reservacion.fechaCaptura),
                        };

                        _context.Rentas.Add(renta);

                        var transaccionPaypal = new TransaccionPaypal
                        {
                            IdPaypal = datosTransaccion.transaccion.idPaypal,
                            FechaCreacion = DateOnly.Parse(datosTransaccion.transaccion.fechaCreacion),
                            CompradorNombre = datosTransaccion.transaccion.compradorNombre,
                            CompradorApellido = datosTransaccion.transaccion.compradorApellido,
                            CompradorCorreo = datosTransaccion.transaccion.compradorCorreo,
                            CompraMoneda = datosTransaccion.transaccion.compraMoneda,
                            CompraMonto = float.Parse(datosTransaccion.transaccion.compraMonto),
                            PagoEstatus = datosTransaccion.transaccion.pagoEstatus,
                            Accion = datosTransaccion.transaccion.accion,
                            CompradorCodigoPais = datosTransaccion.transaccion.compradorCodigoPais,
                            CompradorId = datosTransaccion.transaccion.compradorId,
                            BeneficiarioCorreo = datosTransaccion.transaccion.beneficiarioCorreo,
                            BeneficiarioId = datosTransaccion.transaccion.beneficiarioId,
                            FechaCaptura = DateOnly.Parse(datosTransaccion.transaccion.fechaCaptura),
                            Estatus = datosTransaccion.transaccion.estatus,
                        };

                        _context.TransaccionPaypals.Add(transaccionPaypal);
                        await _context.SaveChangesAsync();

                        return Ok(new { success = true });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Transaction could not be verified with PayPal" });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { success = false, message = "Error processing transaction.", error = ex.Message });
            }
        }

        private async Task<bool> ValidarPayPalIPN(DetallesTransaccion datosTransaccion)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "cmd", "_notify-validate" },
                    { "txn_id", datosTransaccion.idPaypal },
                    { "payment_date", Uri.EscapeDataString(DateTime.Parse(datosTransaccion.fechaCreacion).ToString("ddd, dd MM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture)) }, // Encode date
                    { "first_name", Uri.EscapeDataString(datosTransaccion.compradorNombre) },
                    { "last_name", Uri.EscapeDataString(datosTransaccion.compradorApellido) },
                    { "payer_email", Uri.EscapeDataString(datosTransaccion.compradorCorreo) },
                    { "mc_currency", datosTransaccion.compraMoneda },
                    { "mc_gross", datosTransaccion.compraMonto },
                    { "payment_status", datosTransaccion.pagoEstatus.ToUpper() }, // Ensure it's uppercase
                    { "payer_id", datosTransaccion.compradorId },
                    { "receiver_email", Uri.EscapeDataString(datosTransaccion.beneficiarioCorreo) },
                    { "receiver_id", datosTransaccion.beneficiarioId }
                };

                var requestBody = new FormUrlEncodedContent(values);

                Console.WriteLine("Sending verification request to PayPal...");
                var queryString = string.Join("&", values.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                Console.WriteLine($"Formatted Validation String: {queryString}");

                var verificationResponse = await client.PostAsync(
                    "https://ipnpb.sandbox.paypal.com/cgi-bin/webscr",
                    requestBody
                );

                var verificationResult = await verificationResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"IPN Verification Response: {verificationResult}");

                return verificationResult == "VERIFIED";
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