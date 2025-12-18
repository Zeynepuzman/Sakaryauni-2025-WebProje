using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

[Authorize(Roles = "Antrenor")]
public class AntrenorController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Uye> _userManager;

    public AntrenorController(
        ApplicationDbContext context,
        UserManager<Uye> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ================= DASHBOARD =================
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);

        var antrenor = await _context.Antrenorler
            .FirstOrDefaultAsync(a => a.UserId == user.Id);

        if (antrenor == null)
            return Unauthorized();

        var randevular = await _context.Randevular
            .Include(r => r.Uye)
            .Include(r => r.Hizmet)
            .Where(r => r.AntrenorId == antrenor.Id)
            .OrderBy(r => r.TarihSaat)
            .ToListAsync();

        ViewBag.Antrenor = antrenor;
        return View(randevular);
    }

    // ================= ONAYLA =================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Onayla(int id)
    {
        var randevu = await _context.Randevular.FindAsync(id);
        if (randevu == null)
            return NotFound();

        randevu.Durum = "Onaylandı";
        randevu.ReddetmeSebebi = null; // güvenlik için
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Dashboard));
    }

    // ================= REDDET (GET) =================
    // Sebep yazma ekranını açar
    [HttpGet]
    public async Task<IActionResult> Reddet(int id)
    {
        var randevu = await _context.Randevular
            .Include(r => r.Uye)
            .Include(r => r.Hizmet)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (randevu == null)
            return NotFound();

        return View(randevu); // Views/Antrenor/Reddet.cshtml
    }

    // ================= REDDET (POST) =================
    // Sebebi kaydeder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reddet(int id, string reddetmeSebebi)
    {
        var randevu = await _context.Randevular.FindAsync(id);
        if (randevu == null)
            return NotFound();

        randevu.Durum = "Reddedildi";
        randevu.ReddetmeSebebi = reddetmeSebebi;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Dashboard));
    }
}
