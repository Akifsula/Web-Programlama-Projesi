using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;

namespace KuaforYonetim.Controllers
{
    public class CalisanUygunlukController : Controller
    {
        // Örnek veri 
        private static List<CalisanUygunluk> uygunluklar = new List<CalisanUygunluk>();

        public IActionResult Index(int calisanId)
        {
            // Belirli bir çalışanın uygunluklarını getir
            var calisanUygunluklar = uygunluklar.Where(u => u.CalisanId == calisanId).ToList();
            ViewBag.CalisanId = calisanId;
            return View(calisanUygunluklar);
        }

        public IActionResult Ekle(int calisanId)
        {
            ViewBag.CalisanId = calisanId;
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(CalisanUygunluk uygunluk)
        {
            if (ModelState.IsValid)
            {
                uygunluk.UygunlukId = uygunluklar.Count + 1; // Örnek için ID ataması
                uygunluklar.Add(uygunluk);
                return RedirectToAction("Index", new { calisanId = uygunluk.CalisanId });
            }
            return View(uygunluk);
        }
    }
}