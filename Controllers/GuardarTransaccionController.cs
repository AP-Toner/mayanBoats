using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;

namespace mayanBoats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuardarTransaccionController : Controller
    {
        private readonly MayanDbContext _context;
        private readonly string _paypalIdentityToken = "YOUR_PAYPAL_IDENTITY_TOKEN"; // Your PDT identity token

        public GuardarTransaccionController(MayanDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionRequest request)
        {
            try
            {
                // Map data to Renta model
                var renta = new Renta
                {
                    TipoPago = int.Parse(request.Reservacion.TipoPago),
                    IdPagoPayPal = request.Reservacion.IdPagoPaypal,
                    Fecha = DateOnly.Parse(request.Reservacion.Fecha),
                    Paquete = int.Parse(request.Reservacion.Paquete),
                    HoraInicial = request.Reservacion.HoraInicial,
                    HorasTour = int.Parse(request.Reservacion.HorasTour),
                    HoraFinal = request.Reservacion.HoraFinal,
                    IdCliente = int.Parse(request.Reservacion.IdCliente),
                    Monto = decimal.Parse(request.Reservacion.TotalDespuesDescuento),
                    TipoCambio = double.Parse(request.Reservacion.TipoCambio),
                    Descuento = double.Parse(request.Reservacion.DescuentoAplicable),
                    FechaCaptura = DateOnly.Parse(request.Reservacion.FechaCaptura),
                    Estatus = request.Reservacion.Estatus,
                    CorreoElectronico = request.Reservacion.Correo,
                    Nombre = request.Reservacion.Nombre,
                    Apellido = request.Reservacion.Apellido,
                    Telefono = request.Reservacion.Telefono,
                    Adicional = int.Parse(request.Reservacion.PersonaAdicional),
                    CostoAdicional = decimal.Parse(request.Reservacion.CostoAdicional)
                };

                _context.Rentas.Add(renta); // Add Renta record to the database

                // Map data to TransaccionPaypal model
                var transaccionPaypal = new TransaccionPaypal
                {
                    IdPaypal = request.Transaccion.IdPaypal,
                    FechaCreacion = DateOnly.Parse(request.Transaccion.FechaCreacion),
                    Accion = request.Transaccion.Accion,
                    CompradorCodigoPais = request.Transaccion.CompradorCodigoPais,
                    CompradorCorreo = request.Transaccion.CompradorCorreo,
                    CompradorNombre = request.Transaccion.CompradorNombre,
                    CompradorApellido = request.Transaccion.CompradorApellido,
                    CompradorId = request.Transaccion.CompradorId,
                    CompraMoneda = request.Transaccion.CompraMoneda,
                    CompraMonto = float.Parse(request.Transaccion.CompraMonto),
                    BeneficiarioCorreo = request.Transaccion.BeneficiarioCorreo,
                    BeneficiarioId = request.Transaccion.BeneficiarioId,
                    PagoEstatus = request.Transaccion.PagoEstatus,
                    FechaCaptura = DateOnly.Parse(request.Transaccion.FechaCaptura),
                    Estatus = request.Transaccion.Estatus
                };

                _context.TransaccionPaypals.Add(transaccionPaypal); // Add TransaccionPaypal record to the database
                await _context.SaveChangesAsync(); // Save both records to the database

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Error processing transaction.", error = ex.Message });
            }
        }

        // PDT Callback handler
        [HttpGet("PdtCallback")]
        public async Task<IActionResult> PdtCallback(string tx, string token)
        {
            try
            {
                // Step 1: Validate the PDT token with PayPal
                var validationResponse = await ValidatePdtToken(token);

                // Step 2: If validated, process the transaction
                if (validationResponse != null && validationResponse.Contains("SUCCESS"))
                {
                    // Fetch transaction details
                    var details = await GetTransactionDetails(tx);

                    // Step 3: Process the Renta and TransaccionPaypal data
                    var renta = new Renta
                    {
                        TipoPago = 1,  // Assuming '1' represents PayPal
                        IdPagoPayPal = tx,
                        Fecha = DateOnly.Parse(details["payment_date"]),
                        Paquete = int.Parse(details["package_id"]),
                        HoraInicial = details["start_time"],
                        HorasTour = int.Parse(details["tour_duration"]),
                        HoraFinal = details["end_time"],
                        IdCliente = int.Parse(details["client_id"]),
                        Monto = decimal.Parse(details["total_after_discount"]),
                        TipoCambio = 0, // Assuming no exchange rate used
                        Descuento = double.Parse(details["discount"]),
                        FechaCaptura = DateOnly.Parse(details["capture_date"]),
                        Estatus = "A", // Assuming 'A' is the active status
                        CorreoElectronico = details["payer_email"],
                        Nombre = details["payer_name"],
                        Apellido = details["payer_surname"],
                        Telefono = details["payer_phone"],
                        Adicional = int.Parse(details["additional_persons"]),
                        CostoAdicional = decimal.Parse(details["additional_cost"])
                    };

                    _context.Rentas.Add(renta);

                    var transaccionPaypal = new TransaccionPaypal
                    {
                        IdPaypal = tx,
                        FechaCreacion = DateOnly.Parse(details["payment_date"]),
                        Accion = "Sale",
                        CompradorCodigoPais = details["payer_country_code"],
                        CompradorCorreo = details["payer_email"],
                        CompradorNombre = details["payer_name"],
                        CompradorApellido = details["payer_surname"],
                        CompradorId = details["payer_id"],
                        CompraMoneda = details["currency"],
                        CompraMonto = float.Parse(details["total_amount"]),
                        BeneficiarioCorreo = details["merchant_email"],
                        BeneficiarioId = details["merchant_id"],
                        PagoEstatus = details["payment_status"],
                        FechaCaptura = DateOnly.Parse(details["capture_date"]),
                        Estatus = "A"
                    };

                    _context.TransaccionPaypals.Add(transaccionPaypal);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true });
                }

                return BadRequest(new { success = false, message = "Payment verification failed." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Error processing PDT response.", error = ex.Message });
            }
        }

        // Validate the PDT token by sending a request to PayPal
        private async Task<string> ValidatePdtToken(string token)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("cmd", "_notify-synch"),
                new KeyValuePair<string, string>("tx", token),
                new KeyValuePair<string, string>("at", _paypalIdentityToken)
            });

            var response = await client.PostAsync("https://www.paypal.com/cgi-bin/webscr", content);
            return await response.Content.ReadAsStringAsync();
        }

        // Get transaction details using PayPal's `tx`
        private async Task<Dictionary<string, string>> GetTransactionDetails(string tx)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("cmd", "_notify-synch"),
                new KeyValuePair<string, string>("tx", tx),
                new KeyValuePair<string, string>("at", _paypalIdentityToken)
            });

            var response = await client.PostAsync("https://www.paypal.com/cgi-bin/webscr", content);
            var responseData = await response.Content.ReadAsStringAsync();

            // Parse the response from PayPal into a dictionary
            var details = new Dictionary<string, string>();
            foreach (var line in responseData.Split('\n'))
            {
                if (line.StartsWith("L_"))
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        details[parts[0]] = parts[1];
                    }
                }
            }

            return details;
        }

        // Define a class to match the JSON structure sent from the frontend
        public class TransactionRequest
        {
            public ReservacionModel Reservacion { get; set; }
            public TransaccionModel Transaccion { get; set; }
        }

        // Define the structures for Reservacion and Transaccion to map the data
        public class ReservacionModel
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Correo { get; set; }
            public string Telefono { get; set; }
            public string Fecha { get; set; }
            public string Paquete { get; set; }
            public string HoraInicial { get; set; }
            public string HorasTour { get; set; }
            public string HoraFinal { get; set; }
            public string IdCliente { get; set; }
            public string TotalDespuesDescuento { get; set; }
            public string DescuentoAplicable { get; set; }
            public string TipoPago { get; set; }
            public string Estatus { get; set; }
            public string IdPagoPaypal { get; set; }
            public string PersonaAdicional { get; set; }
            public string CostoAdicional { get; set; }
            public string FechaCaptura { get; set; }
            public string TipoCambio { get; set; }
        }

        public class TransaccionModel
        {
            public string IdPaypal { get; set; }
            public string FechaCreacion { get; set; }
            public string CompradorNombre { get; set; }
            public string CompradorApellido { get; set; }
            public string CompradorCorreo { get; set; }
            public string CompraMoneda { get; set; }
            public string CompraMonto { get; set; }
            public string PagoEstatus { get; set; }
            public string Accion { get; set; }
            public string CompradorCodigoPais { get; set; }
            public string CompradorId { get; set; }
            public string BeneficiarioCorreo { get; set; }
            public string BeneficiarioId { get; set; }
            public string FechaCaptura { get; set; }
            public string Estatus { get; set; }
        }
    }
}
