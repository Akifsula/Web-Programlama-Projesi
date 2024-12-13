using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;
using KuaforYonetim.ViewModels;
using System.Threading.Tasks;

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

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni bir kullanıcı oluştur
                var user = new Kullanici { UserName = model.Email, Email = model.Email, AdSoyad = model.AdSoyad };

                // Kullanıcıyı veritabanına kaydet
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kaydedildikten sonra giriş ekranına yönlendir
                    return RedirectToAction("Login");
                }

                // Hataları modele ekle
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model); // Model valid değilse, kullanıcıyı aynı sayfada bırak
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı doğrula ve giriş yap
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Giriş başarılı, ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }

                // Giriş başarısızsa hata göster
                ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
            }
            return View(model);
        }
    }
}