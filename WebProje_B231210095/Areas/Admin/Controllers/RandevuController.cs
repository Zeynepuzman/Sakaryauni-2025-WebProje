using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  SADECE BUGÜNKÜ RANDEVULAR
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var randevular = await _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Where(r => r.TarihSaat >= today && r.TarihSaat < tomorrow)
                .OrderBy(r => r.TarihSaat)
                .ToListAsync();

            return View(randevular);
        }
    }
}
