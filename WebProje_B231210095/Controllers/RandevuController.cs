using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;
using WebProje_B231210095.Models.ViewModels;

[Authorize]
public class RandevuController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Uye> _userManager;

    public RandevuController(ApplicationDbContext context, UserManager<Uye> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Randevu/Create
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);

        // Kullanıcının aktif paketi
        var aktifPaket = _context.UyePaketler
            .Include(x => x.Paket)
                .ThenInclude(p => p.Hizmet)
            .FirstOrDefault(x => x.UyeId == user.Id && x.AktifMi);

        if (aktifPaket == null)
        {
            TempData["Error"] = "Randevu alabilmek için aktif paketiniz olmalıdır.";
            return RedirectToAction("Dashboard", "Uye");
        }

        var hizmetId = aktifPaket.Paket.HizmetId;

        // Seçilen hizmeti veren antrenörleri getir
        var antrenorler = _context.AntrenorHizmetler
            .Where(x => x.HizmetId == hizmetId)
            .Select(x => x.Antrenor)
            .Distinct()
            .ToList();

        ViewBag.PaketAdi = aktifPaket.Paket.Ad;
        ViewBag.HizmetAdi = aktifPaket.Paket.Hizmet.Ad;
        ViewBag.Antrenorler = new SelectList(antrenorler, "Id", "AdSoyad");

        return View();
    }

    // POST: Randevu/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RandevuCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        var aktifPaket = _context.UyePaketler
            .Include(x => x.Paket)
            .ThenInclude(p => p.Hizmet)
            .FirstOrDefault(x => x.UyeId == user.Id && x.AktifMi);

        if (aktifPaket == null)
            return RedirectToAction("Dashboard", "Uye");

        //  Hizmet bilgisi (süre için)
        var hizmet = aktifPaket.Paket.Hizmet;
        if (hizmet == null)
        {
            ModelState.AddModelError("", "Hizmet bilgisi bulunamadı.");
            return View(model);
        }

        var baslangic = model.Tarih.Date + model.Saat;
        var bitis = baslangic.AddMinutes(hizmet.SureDakika);

        //  ASIL ÇAKIŞMA KONTROLÜ
    
        bool doluMu = _context.Randevular.Any(r =>
    r.AntrenorId == model.AntrenorId &&

    // İptal olmayan randevular doludur
    r.Durum != "İptal" &&

    // Zaman çakışması
    r.TarihSaat < bitis &&
    r.TarihSaat.AddMinutes(r.SureDakika) > baslangic
);


        if (doluMu)
        {
            ModelState.AddModelError("",
                "Seçilen antrenör bu saat aralığında doludur. Lütfen başka bir saat seçiniz.");

            var antrenorler = _context.AntrenorHizmetler
                .Where(x => x.HizmetId == aktifPaket.Paket.HizmetId)
                .Select(x => x.Antrenor)
                .ToList();

            ViewBag.PaketAdi = aktifPaket.Paket.Ad;
            ViewBag.HizmetAdi = aktifPaket.Paket.Hizmet.Ad;
            ViewBag.Antrenorler = new SelectList(antrenorler, "Id", "AdSoyad");

            return View(model);
        }


        //  Randevu oluştur
        var randevu = new Randevu
        {
            UyeId = user.Id,
            AntrenorId = model.AntrenorId,
            HizmetId = aktifPaket.Paket.HizmetId,
            TarihSaat = baslangic,
            SureDakika = hizmet.SureDakika,
            Ucret = aktifPaket.Paket.Ucret,
            Durum = "Bekliyor"
        };

        _context.Randevular.Add(randevu);
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard", "Uye");
    }

}
