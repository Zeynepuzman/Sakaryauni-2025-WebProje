using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Data;
using System.Linq;

namespace WebProje_B231210095.Controllers.Api
{
    [ApiController]
    [Route("api/antrenorler")]
    public class AntrenorApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AntrenorApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/antrenorler
        [HttpGet]
        public IActionResult GetAntrenorler()
        {
            var antrenorler = _context.Antrenorler
                .Select(a => new
                {
                    a.Id,
                    a.AdSoyad,
                    a.Uzmanlik,
                    a.Email
                })
                .ToList();

            return Ok(antrenorler);
        }
    }
}
