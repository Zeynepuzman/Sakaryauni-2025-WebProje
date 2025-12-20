using Microsoft.AspNetCore.Mvc;
using WebProje_B231210095.Models;
using WebProje_B231210095.Services;

namespace WebProje_B231210095.Controllers
{
    public class AiController : Controller
    {
        private readonly GroqAiService _grokAiService;
        private readonly UnsplashImageService _unsplashImageService;

        public AiController(
            GroqAiService grokAiService,
            UnsplashImageService unsplashImageService)
        {
            _grokAiService = grokAiService;
            _unsplashImageService = unsplashImageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AiInputViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(AiInputViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            HttpContext.Session.SetString("UserGender", model.Cinsiyet);
            HttpContext.Session.SetInt32("UserAge", model.Yas);

            var aiText = await _grokAiService.GeneratePlanAsync(model);

            // Unsplash'tan gerçek görsel
            var imageUrl = await _unsplashImageService.GetFitnessImageAsync(
                model.Cinsiyet,
                model.Hedef
            );

            var result = new AiResultViewModel
            {
                ResultText = aiText,
                BodyDescription =
                    "Bu programa 8–12 hafta düzenli devam edilirse vücut görünümünde olumlu değişimler beklenir.",
                SuggestedImage = imageUrl
            };

            return View("Result", result);
        }
    }
}
