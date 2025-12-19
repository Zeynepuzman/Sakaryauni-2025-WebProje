using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;

[ApiController]
[Route("api/admin/randevular")]
[Authorize(Roles = "Admin")]
public class AdminRandevuController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminRandevuController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var randevular = await _context.Randevular
            .Include(r => r.Uye)
            .Include(r => r.Antrenor)
            .Include(r => r.Hizmet)
            .Select(r => new
            {
                r.Id,
                Uye = r.Uye.AdSoyad,
                Antrenor = r.Antrenor.AdSoyad,
                Hizmet = r.Hizmet.Ad,
                Tarih = r.TarihSaat.ToString("dd.MM.yyyy"),
                Saat = r.TarihSaat.ToString("HH:mm"),
                r.SureDakika,
                r.Durum
            })
            .ToListAsync();

        return Ok(randevular);
    }
}
