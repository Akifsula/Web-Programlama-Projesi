using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;

namespace KuaforYonetim.Controllers
{
    public class CalisanController : Controller
    {
        // Örnek çalışan listesi (ileride veritabanına bağlanacak)
        private static List<Calisan> calisanlar = new List<Calisan>
        {
            new Calisan { CalisanId = 1, AdSoyad = "Ahmet Yılmaz", UzmanlikAlanlari = "Saç Kesimi, Sakal Traşı" },
            new Calisan { CalisanId = 2, AdSoyad = "Mehmet Kaya", UzmanlikAlanlari = "Saç Boyama, Fön Çekimi" }
        };

        public IActionResult Index()
        {
            return View(calisanlar);
        }

        public IActionResult Detay(int id)
        {
            var calisan = calisanlar.FirstOrDefault(c => c.CalisanId == id);
            if (calisan == null) return NotFound();
            return View(calisan);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                calisan.CalisanId = calisanlar.Count + 1; // Örnek için ID ataması
                calisanlar.Add(calisan);
                return RedirectToAction("Index");
            }
            return View(calisan);
        }
    }
}