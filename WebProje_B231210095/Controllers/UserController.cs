using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Models;

public class UserController : Controller
{
    private readonly UserManager<Uye> _userManager;

    public UserController(UserManager<Uye> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }
}
