using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class PaketController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Uye> _userManager;

    public PaketController(ApplicationDbContext context, UserManager<Uye> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ✅ BU YOKTU → 404'ÜN SEBEBİ BU
    public IActionResult Index()
    {
        var paketler = _context.Paketler
            .Include(p => p.Hizmet)
            .ToList();

        return View(paketler);
    }

    // Paketi seç
    public async Task<IActionResult> Sec(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // Eski aktif paketleri kapat
        var aktifler = _context.UyePaketler
            .Where(x => x.UyeId == user.Id && x.AktifMi)
            .ToList();

        foreach (var p in aktifler)
            p.AktifMi = false;

        var paket = _context.Paketler.Find(id);
        if (paket == null) return NotFound();

        var uyePaket = new UyePaket
        {
            UyeId = user.Id,
            PaketId = paket.Id,
            BaslangicTarihi = DateTime.Now,
            BitisTarihi = DateTime.Now.AddDays(paket.SureGun),
            AktifMi = true
        };

        _context.UyePaketler.Add(uyePaket);
        _context.SaveChanges();

        return RedirectToAction("Dashboard", "Uye");
    }
}
