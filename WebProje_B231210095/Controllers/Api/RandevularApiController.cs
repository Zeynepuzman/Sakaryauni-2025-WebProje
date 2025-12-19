using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Models;
using WebProje_B231210095.Data;

namespace WebProje_B231210095.Controllers.Api
{
    [Route("api/randevular")]
    [ApiController]
    public class RandevularApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RandevularApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/randevular
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .OrderByDescending(r => r.TarihSaat)
                .Select(r => new
                {
                    tarih = r.TarihSaat,
                    uye = r.Uye.AdSoyad,
                    antrenor = r.Antrenor.AdSoyad,
                    hizmet = r.Hizmet.Ad,
                    sure = r.SureDakika,
                    durum = r.Durum
                })
                .ToListAsync();

            return Ok(randevular);
        }
    }
}
