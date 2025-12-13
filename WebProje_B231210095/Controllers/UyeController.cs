using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Models;

[Authorize] // giriş yapmadan girilemesin
public class UyeController : Controller
{
    private readonly UserManager<Uye> _userManager;

    public UyeController(UserManager<Uye> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user);
    }

    // GET: Profil düzenleme
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    // POST: Profil düzenleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Uye model)
    {
        var user = await _userManager.GetUserAsync(User);

        user.AdSoyad = model.AdSoyad;
        user.PhoneNumber = model.PhoneNumber;
        user.Yas = model.Yas;
        user.Boy = model.Boy;
        user.Kilo = model.Kilo;

        await _userManager.UpdateAsync(user);

        return RedirectToAction("Dashboard");
    }
}
