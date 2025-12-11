using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UyelerController : Controller
    {
        private readonly UserManager<Uye> _userManager;

        public UyelerController(UserManager<Uye> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: Üye Düzenleme
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Üye Düzenleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Uye model)
        {
            if (id != model.Id) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.AdSoyad = model.AdSoyad;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Yas = model.Yas;
            user.Boy = model.Boy;
            user.Kilo = model.Kilo;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        // GET: Üye Silme
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Silme Onayı
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
