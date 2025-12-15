using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

[Authorize]
public class UyeController : Controller
{
    private readonly UserManager<Uye> _userManager;
    private readonly ApplicationDbContext _context;

    public UyeController(UserManager<Uye> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);

        var aktifPaket = _context.UyePaketler
            .Where(x => x.UyeId == user.Id && x.AktifMi)
            .Select(x => new
            {
                PaketAdi = x.Paket.Ad,
                BitisTarihi = x.BitisTarihi,
                KalanGun = Math.Max(0, (x.BitisTarihi - DateTime.Now).Days)
            })
            .FirstOrDefault();

        ViewBag.AktifPaket = aktifPaket;

        return View(user);
    }

    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Uye model)
    {
        var user = await _userManager.GetUserAsync(User);

        user.AdSoyad = model.AdSoyad;
        user.PhoneNumber = model.PhoneNumber;
        user.Yas = model.Yas;
        user.Boy = model.Boy;
        user.Kilo = model.Kilo;

        await _userManager.UpdateAsync(user);

        return RedirectToAction("Dashboard");
    }
}
