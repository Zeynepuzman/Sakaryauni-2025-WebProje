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

    [Authorize(Roles = "Antrenor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Onayla(int id)
    {
        var randevu = await _context.Randevular.FindAsync(id);
        if (randevu == null)
            return NotFound();

        randevu.Durum = "Onaylandı";
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

    [Authorize(Roles = "Antrenor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reddet(int id)
    {
        var randevu = await _context.Randevular.FindAsync(id);
        if (randevu == null)
            return NotFound();

        randevu.Durum = "Reddedildi";
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }
}
