using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KuaforYonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }

        // Diğer admin işlemleri burada tanımlanabilir
    }
}