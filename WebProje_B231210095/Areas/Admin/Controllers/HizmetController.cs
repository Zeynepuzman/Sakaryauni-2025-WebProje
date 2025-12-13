using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HizmetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HizmetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Hizmet
        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler
                                    .Include(h => h.Salon)
                                    .ToList();
            return View(hizmetler);
        }

        // GET: Admin/Hizmet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Hizmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hizmet hizmet)
        {
            // Tek salon olduğu için otomatik 1 (ÖNCE!)
            hizmet.SalonId = 1;

            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(hizmet);
        }
        // GET: Admin/Hizmet/Edit/5
        public IActionResult Edit(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }
        // POST: Admin/Hizmet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Hizmet hizmet)
        {
            if (id != hizmet.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                hizmet.SalonId = 1;

                _context.Update(hizmet);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(hizmet);
        }

        // GET: Admin/Hizmet/Delete/5
        public IActionResult Delete(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }
        // POST: Admin/Hizmet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet != null)
            {
                _context.Hizmetler.Remove(hizmet);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
