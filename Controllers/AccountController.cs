using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;
using KuaforYonetim.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace KuaforYonetim.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;

        public AccountController(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // Kullanıcı Giriş Sayfası
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Kullanıcıya AdSoyad bilgisini Claim olarak ekle
                        var claims = new List<Claim>
                {
                    new Claim("AdSoyad", user.AdSoyad ?? "Kullanıcı")
                };

                        // Yeni Claims ile kullanıcıyı yeniden oturum açtır
                        var claimsIdentity = new ClaimsIdentity(claims, "Identity.Application");
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("Identity.Application", claimsPrincipal);

                        return RedirectToAction("Index", "Home");
                    }
                }

                // Giriş başarısızsa hata göster
                ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
            }

            return View(model);
        }

        // Kullanıcı Kayıt Sayfası
        public IActionResult Register()
        {
            return View();
        }

        // Kayıt Olma İşlemi
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Kullanici { UserName = model.Email, Email = model.Email, AdSoyad = model.AdSoyad };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kullanıcıyı hemen giriş yaptır
                    await _userManager.AddClaimAsync(user, new Claim("AdSoyad", user.AdSoyad));
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                // Kayıt başarısız
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // Çıkış Yapma
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Kullanıcıyı çıkış yaptırır
            return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendirir
        }
    }
}