using mayanBoats.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mayanBoats.Pages
{
    public class PaquetesModel : PageModel
    {
        private readonly MayanDbContext _context;

        public PaquetesModel(MayanDbContext context)
        {
            _context = context;
        }

        public IList<Paquete> Paquetes { get; set; }
        public IList<Hora> HorasDisponibles { get; set; }

        public async Task OnGetAsync()
        {
            Paquetes = await _context.Paquetes.Where(p => p.Activo == 1).ToListAsync();
            HorasDisponibles = await _context.Horas.Where(h => h.Estatus == "A").ToListAsync();
        }
    }
}
