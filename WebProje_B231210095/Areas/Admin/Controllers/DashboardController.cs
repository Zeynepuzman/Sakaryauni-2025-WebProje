using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;
using Microsoft.EntityFrameworkCore;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Uye> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<Uye> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // LINQ sorguları – Admin Dashboard için veri çekiyoruz
            var toplamUye = await _context.Users.CountAsync();
            var toplamAntrenor = await _context.Antrenorler.CountAsync();
            var toplamHizmet = await _context.Hizmetler.CountAsync();
            var bugunRandevu = await _context.Randevular
                                            .Where(x => x.TarihSaat.Date == DateTime.Today)
                                            .CountAsync();

            // ViewBag ile ekrana aktarıyoruz
            ViewBag.ToplamUye = toplamUye;
            ViewBag.ToplamAntrenor = toplamAntrenor;
            ViewBag.ToplamHizmet = toplamHizmet;
            ViewBag.BugunRandevu = bugunRandevu;

            return View();
        }
    }
}
