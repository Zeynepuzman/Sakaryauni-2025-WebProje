using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Uye> _signInManager;
        private readonly UserManager<Uye> _userManager;
        private readonly IUserStore<Uye> _userStore;
        private readonly IUserEmailStore<Uye> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Uye> userManager,
            IUserStore<Uye> userStore,
            SignInManager<Uye> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // 🔥 BURAYA EKSTRA FIELDS EKLİYORUZ
        public class InputModel
        {
            [Required]
            [Display(Name = "Ad Soyad")]
            public string AdSoyad { get; set; }

            [Display(Name = "Yaş")]
            public int? Yas { get; set; }

            [Display(Name = "Boy (cm)")]
            public float? Boy { get; set; }

            [Display(Name = "Kilo (kg)")]
            public float? Kilo { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Şifre en az {2}, en fazla {1} karakter olabilir.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Şifre")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Şifre Tekrar")]
            [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                // Email ve Username alanlarını doldurur
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // BURADA EKSTRA ALANLARI USER OBJESİNE ATIYORUZ
                user.AdSoyad = Input.AdSoyad;
                user.Yas = Input.Yas;
                user.Boy = Input.Boy;
                user.Kilo = Input.Kilo;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Yeni kullanıcı başarıyla oluşturuldu.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }


        private Uye CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Uye>();
            }
            catch
            {
                throw new InvalidOperationException($"'{nameof(Uye)}' sınıfının bir örneği oluşturulamadı.");
            }
        }

        private IUserEmailStore<Uye> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Email desteklenmiyor.");
            }
            return (IUserEmailStore<Uye>)_userStore;
        }
    }
}
