using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mayanBoats.Pages
{
    public class ContactoModel : PageModel
    {
        private readonly MayanDbContext _context;

        public ContactoModel(MayanDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Correo { get; set; }

        [BindProperty]
        public string Asunto { get; set; }

        [BindProperty]
        public string Mensaje { get; set; }

        public void OnGet()
        { 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var pregunta = new Pregunta
            {
                Nombre = Nombre,
                Correo = Correo,
                Asunto = Asunto,
                Mensaje = Mensaje,
                FechaCaptura = DateOnly.FromDateTime(DateTime.Now),
                Activo = 1
            };

            _context.Preguntas.Add(pregunta);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Contacto");
        }
    }
}
