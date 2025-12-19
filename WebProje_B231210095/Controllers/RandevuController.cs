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
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        var randevu = await _context.Randevular
            .Include(r => r.Hizmet)
            .FirstOrDefaultAsync(r => r.Id == id && r.UyeId == user.Id);

        if (randevu == null)
            return NotFound();

        if (randevu.Durum != "Bekliyor")
            return RedirectToAction("Dashboard", "Uye");

        var antrenorler = _context.AntrenorHizmetler
            .Where(x => x.HizmetId == randevu.HizmetId)
            .Select(x => x.Antrenor)
            .Distinct()
            .ToList();

        ViewBag.Antrenorler = new SelectList(antrenorler, "Id", "AdSoyad");
        ViewBag.RandevuId = randevu.Id;

        var model = new RandevuCreateVM
        {
            AntrenorId = randevu.AntrenorId,
            Tarih = randevu.TarihSaat.Date,
            Saat = randevu.TarihSaat.TimeOfDay
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RandevuCreateVM model)
    {
        var user = await _userManager.GetUserAsync(User);

        var randevu = await _context.Randevular
            .FirstOrDefaultAsync(r => r.Id == id && r.UyeId == user.Id);

        if (randevu == null)
            return NotFound();

        //  Tarih + Saat birleştir
        var yeniTarihSaat = model.Tarih.Date + model.Saat;

        //  geçmiş tarih
        if (yeniTarihSaat < DateTime.Now)
            ModelState.AddModelError("", "Geçmiş tarih seçilemez.");

        //  10 dk kuralı
        if (model.Saat.Minutes % 10 != 0)
            ModelState.AddModelError("", "Saatler 10 dakikalık aralıklarla olmalıdır.");

        //  çalışma saati
        if (model.Saat.Hours < 9 || model.Saat.Hours >= 22)
            ModelState.AddModelError("", "Çalışma saatleri 09:00 - 22:00.");

        if (!ModelState.IsValid)
            return View(model);

     
        randevu.AntrenorId = model.AntrenorId;
        randevu.TarihSaat = yeniTarihSaat;
        randevu.Durum = "Bekliyor"; // tekrar onaya düşer

        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard", "Uye");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        var randevu = await _context.Randevular
            .Include(r => r.Antrenor)
            .Include(r => r.Hizmet)
            .FirstOrDefaultAsync(r => r.Id == id && r.UyeId == user.Id);

        if (randevu == null)
            return NotFound();

        // Sadece bekleyen randevu silinebilir
        if (randevu.Durum != "Bekliyor")
            return RedirectToAction("Dashboard", "Uye");

        return View(randevu);
    }





    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        var randevu = await _context.Randevular
            .FirstOrDefaultAsync(r => r.Id == id && r.UyeId == user.Id);

        if (randevu == null)
            return NotFound();

        // Soft delete
        randevu.Durum = "İptal";

        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard", "Uye");
    }
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
