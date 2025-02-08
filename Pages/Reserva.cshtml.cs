using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mayanBoats.Pages
{
    public class ReservaModel : PageModel
    {
        public string NombrePaquete { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string NombreCliente { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string PrecioBase { get; set; }
        public string PersonaAdicional { get; set; }
        public string Subtotal { get; set; }
        public string DescuentoAplicable { get; set; }
        public string TotalDespuesDescuento { get; set; }

        public void OnGet()
        {
            NombrePaquete = Request.Query["nombrePaquete"];
            Fecha = Request.Query["fecha"];
            Hora = Request.Query["hora"];
            NombreCliente = Request.Query["nombreCliente"];
            Correo = Request.Query["correo"];
            Telefono = Request.Query["telefono"];
            PrecioBase = Request.Query["precioBase"];
            PersonaAdicional = Request.Query["personaAdicional"];
            Subtotal = Request.Query["subtotal"];
            DescuentoAplicable = Request.Query["descuentoAplicable"];
            TotalDespuesDescuento = Request.Query["totalDespuesDescuento"];
        }
    }
}
