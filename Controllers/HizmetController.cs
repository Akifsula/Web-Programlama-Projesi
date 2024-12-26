using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;

namespace KuaforYonetim.Controllers
{
    public class HizmetController : Controller
    {
        // Örnek hizmet listesi
        private static List<Hizmet> hizmetler = new List<Hizmet>
        {
            new Hizmet { HizmetId = 1, Ad = "Saç Kesimi", Ucret = 250, TahminiSure = TimeSpan.FromMinutes(30) },
            new Hizmet { HizmetId = 2, Ad = "Sakal Traşı", Ucret = 100, TahminiSure = TimeSpan.FromMinutes(10) },
            new Hizmet { HizmetId = 3, Ad = "Saç&Sakal", Ucret = 300, TahminiSure = TimeSpan.FromMinutes(40) },
            new Hizmet { HizmetId = 4, Ad = "Saç Boyama", Ucret = 150, TahminiSure = TimeSpan.FromMinutes(60) }
        };

        public IActionResult Index()
        {
            return View(hizmetler);
        }

        public IActionResult Detay(int id)
        {
            var hizmet = hizmetler.FirstOrDefault(h => h.HizmetId == id);
            if (hizmet == null) return NotFound();
            return View(hizmet);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                hizmet.HizmetId = hizmetler.Count + 1; // Örnek için ID ataması
                hizmetler.Add(hizmet);
                return RedirectToAction("Index");
            }
            return View(hizmet);
        }
    }
}