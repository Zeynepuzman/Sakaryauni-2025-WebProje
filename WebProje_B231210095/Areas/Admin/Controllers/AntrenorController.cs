using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Data;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AntrenorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Antrenors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Antrenorler.Include(a => a.Salon);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Antrenors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler
                .Include(a => a.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }

        // GET: Admin/Antrenors/Create
        public IActionResult Create()
        {
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Antrenor antrenor)
        {
            // Tek salon olduğu için sabitliyoruz
            antrenor.SalonId = 1;

            if (!ModelState.IsValid)
            {
                // Hata varsa formu tekrar göster
                return View(antrenor);
            }

            _context.Antrenorler.Add(antrenor);
            await _context.SaveChangesAsync();

            // Kayıt başarılıysa Admin Dashboard'a dön
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }


        // GET: Admin/Antrenors/Edit/5zmanlik,Salon
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor == null)
            {
                return NotFound();
            }
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", antrenor.SalonId);
            return View(antrenor);
        }

        // POST: Admin/Antrenors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Antrenor antrenor)
        {
            if (id != antrenor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenorExists(antrenor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(antrenor);
        }

        // GET: Admin/Antrenors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler
                .Include(a => a.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }

        // POST: Admin/Antrenors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor != null)
            {
                _context.Antrenorler.Remove(antrenor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntrenorExists(int id)
        {
            return _context.Antrenorler.Any(e => e.Id == id);
        }
    }
}
