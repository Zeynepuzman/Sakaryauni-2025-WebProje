using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AntrenorHizmetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorHizmetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AntrenorHizmet/Create
        public IActionResult Create()
        {
            ViewBag.Antrenorler = new SelectList(
                _context.Antrenorler,
                "Id",
                "AdSoyad"
            );

            ViewBag.Hizmetler = new SelectList(
                _context.Hizmetler,
                "Id",
                "Ad"
            );

            return View();
        }

        // POST: Admin/AntrenorHizmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AntrenorHizmet model)
        {
            bool varMi = _context.AntrenorHizmetler.Any(x =>
                x.AntrenorId == model.AntrenorId &&
                x.HizmetId == model.HizmetId
            );

            if (varMi)
            {
                ModelState.AddModelError("", "Bu antrenör bu hizmete zaten atanmış.");
            }

            if (ModelState.IsValid)
            {
                _context.AntrenorHizmetler.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Antrenorler = new SelectList(_context.Antrenorler, "Id", "AdSoyad");
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad");

            return View(model);
        }
    }
}
