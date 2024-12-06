using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;

namespace KuaforYonetim.Controllers
{
    public class HizmetController : Controller
    {
        // Örnek hizmet listesi
        private static List<Hizmet> hizmetler = new List<Hizmet>
        {
            new Hizmet { HizmetId = 1, Ad = "Saç Kesimi", Ucret = 50, TahminiSure = TimeSpan.FromMinutes(30) },
            new Hizmet { HizmetId = 2, Ad = "Sakal Traşı", Ucret = 30, TahminiSure = TimeSpan.FromMinutes(20) }
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