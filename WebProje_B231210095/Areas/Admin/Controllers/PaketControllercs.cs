using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var paketler = _context.Paketler
                .Include(p => p.Hizmet)
                .ToList();

            return View(paketler);
        }

        public IActionResult Create()
        {
            ViewBag.Hizmetler = new SelectList(
                _context.Hizmetler,
                "Id",
                "Ad"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paket paket)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = new SelectList(
                    _context.Hizmetler,
                    "Id",
                    "Ad",
                    paket.HizmetId
                );
                return View(paket);
            }

            _context.Paketler.Add(paket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var paket = _context.Paketler.Find(id);
            if (paket == null) return NotFound();

            ViewBag.Hizmetler = new SelectList(
                _context.Hizmetler,
                "Id",
                "Ad",
                paket.HizmetId
            );

            return View(paket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Paket model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = new SelectList(
                    _context.Hizmetler,
                    "Id",
                    "Ad",
                    model.HizmetId
                );
                return View(model);
            }

            _context.Paketler.Update(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var paket = _context.Paketler
                .Include(p => p.Hizmet)
                .FirstOrDefault(p => p.Id == id);

            if (paket == null) return NotFound();

            return View(paket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var paket = _context.Paketler.Find(id);
            if (paket != null)
            {
                _context.Paketler.Remove(paket);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
