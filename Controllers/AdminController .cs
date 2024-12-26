using KuaforYonetim.Data;
using KuaforYonetim.Models;
using KuaforYonetim.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforYonetim.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece admin erişimi
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult UygunlukSil(int id)
        {
            var uygunluk = _context.CalisanUygunluklar.FirstOrDefault(u => u.UygunlukId == id);
            if (uygunluk == null)
            {
                TempData["ErrorMessage"] = "Uygunluk bulunamadı.";
                return RedirectToAction("Uygunluklar", new { calisanId = uygunluk.CalisanId });
            }

            _context.CalisanUygunluklar.Remove(uygunluk);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Uygunluk başarıyla silindi.";
            return RedirectToAction("Uygunluklar", new { calisanId = uygunluk.CalisanId });
        }

        [HttpGet]
        public IActionResult UygunlukDuzenle(int id)
        {
            var uygunluk = _context.CalisanUygunluklar.Find(id);
            if (uygunluk == null)
            {
                return NotFound();
            }

            var viewModel = new UygunlukViewModel
            {
                UygunlukId = uygunluk.UygunlukId,
                CalisanId = uygunluk.CalisanId,
                Gun = (int)uygunluk.Gun, // Enum'dan int'e dönüştürme
                BaslangicSaati = uygunluk.BaslangicSaati,
                BitisSaati = uygunluk.BitisSaati
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult UygunlukDuzenle(UygunlukViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mevcutUygunluk = _context.CalisanUygunluklar.Find(model.UygunlukId);
                if (mevcutUygunluk != null)
                {
                    mevcutUygunluk.Gun = (DayOfWeek)model.Gun; // int'ten Enum'a dönüştürme
                    mevcutUygunluk.BaslangicSaati = model.BaslangicSaati;
                    mevcutUygunluk.BitisSaati = model.BitisSaati;

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Uygunluk başarıyla güncellendi.";
                    return RedirectToAction("Uygunluklar", new { calisanId = model.CalisanId });
                }
            }

            return View(model);
        }


        public IActionResult Takvim(int calisanId)
        {
            var uygunluklar = _context.CalisanUygunluklar
                .Where(u => u.CalisanId == calisanId)
                .OrderBy(u => u.Gun) // Günlere göre sıralama
                .ThenBy(u => u.BaslangicSaati) // Başlangıç saatine göre sıralama
                .Select(u => new
        {
            title = $"Uygunluk: {u.BaslangicSaati} - {u.BitisSaati}",
            start = DateTime.Today.AddDays((int)u.Gun).Add(u.BaslangicSaati),
            end = DateTime.Today.AddDays((int)u.Gun).Add(u.BitisSaati)
        })
        .ToList();

            ViewBag.CalisanId = calisanId;
            ViewBag.Uygunluklar = uygunluklar;
            return View();
        }

        public IActionResult Uygunluklar(int calisanId)
        {
            var uygunluklar = _context.CalisanUygunluklar
                .Where(u => u.CalisanId == calisanId)
                .ToList();

            ViewBag.CalisanId = calisanId;
            return View(uygunluklar);
        }



        [HttpGet]
        public IActionResult UygunlukEkle(int calisanId)
        {
            if (calisanId == 0)
            {
                TempData["ErrorMessage"] = "Geçerli bir çalışan ID'si sağlanmadı.";
                return RedirectToAction("Calisanlar");
            }

            ViewBag.CalisanId = calisanId;
            return View();
        }

        [HttpPost]
        public IActionResult UygunlukEkle(CalisanUygunluk uygunluk)
        {
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını konsola yazdır
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model Hatası: {error.ErrorMessage}");
                }

                TempData["ErrorMessage"] = "Tüm alanları doldurun.";
                // Formu hatalarla birlikte yeniden yükle
                return View(uygunluk);
            }

            // Çalışan doğrulaması
            uygunluk.Calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanId == uygunluk.CalisanId);
            if (uygunluk.Calisan == null)
            {
                TempData["ErrorMessage"] = "Geçerli bir çalışan bulunamadı.";
                // Çalışan bulunamazsa formu yeniden yükle
                return View(uygunluk);
            }

            // Uygunluğu ekle
            _context.CalisanUygunluklar.Add(uygunluk);
            try
            {
                // Veritabanına kaydet
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Uygunluk başarıyla eklendi.";
                // Başarılı işlem sonrası uygunluklar sayfasına yönlendir
                return RedirectToAction("Uygunluklar", new { calisanId = uygunluk.CalisanId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                // Hata durumunda formu yeniden yükle
                return View(uygunluk);
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calisanlar()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanHizmetler)
                .ThenInclude(ch => ch.Hizmet)
                .ToList();


            // Her çalışanın verimliliğini hesaplayalım
            var verimlilikListesi = _context.Calisanlar
                .Select(c => new
                {
                    CalisanId = c.CalisanId,
                    Verimlilik = _context.Randevular
                        .Where(r => r.CalisanId == c.CalisanId && r.Durum == RandevuDurumu.Onaylandi)
                        .Sum(r => r.Hizmet.Ucret)
                })
                .ToDictionary(x => x.CalisanId, x => x.Verimlilik);

            ViewBag.VerimlilikListesi = verimlilikListesi;

            return View(calisanlar);
        }

        [HttpGet]
        public IActionResult CalisanEkle()
        {
            var viewModel = new CalisanEkleViewModel
            {
                Hizmetler = _context.Hizmetler.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CalisanEkle(CalisanEkleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model Hatası: {error.ErrorMessage}");
                }

                viewModel.Hizmetler = _context.Hizmetler.ToList();
                TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
                return View(viewModel);
            }

            var yeniCalisan = new Calisan
            {
                AdSoyad = viewModel.AdSoyad,
                CalisanHizmetler = viewModel.SelectedHizmetler.Select(hizmetId => new CalisanHizmet
                {
                    HizmetId = hizmetId
                }).ToList(),
                CalisanUygunluklar = viewModel.Uygunluklar.Select(uygunluk => new CalisanUygunluk
                {
                    Gun = uygunluk.Gun,
                    BaslangicSaati = uygunluk.BaslangicSaati,
                    BitisSaati = uygunluk.BitisSaati
                }).ToList()
            };

            _context.Calisanlar.Add(yeniCalisan);

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi.";
                return RedirectToAction("Calisanlar");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                viewModel.Hizmetler = _context.Hizmetler.ToList();
                return View(viewModel);
            }
        }


        [HttpPost]
        public IActionResult CalisanSil(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.CalisanUygunluklar)
                .Include(c => c.CalisanHizmetler)
                .FirstOrDefault(c => c.CalisanId == id);

            if (calisan == null)
            {
                TempData["ErrorMessage"] = "Çalışan bulunamadı.";
                return RedirectToAction("Calisanlar");
            }

            // İlişkili uygunlukları ve hizmetleri sil
            _context.CalisanUygunluklar.RemoveRange(calisan.CalisanUygunluklar);
            _context.CalisanHizmetler.RemoveRange(calisan.CalisanHizmetler);

            // Çalışanı sil
            _context.Calisanlar.Remove(calisan);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Çalışan başarıyla silindi.";
            return RedirectToAction("Calisanlar");
        }

        public IActionResult Randevular()
        {
            var bekleyenRandevular = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Hizmet)
                .Include(r => r.Kullanici)
                .Where(r => r.Durum == RandevuDurumu.Bekliyor)
                .ToList();

            var aktifRandevular = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Hizmet)
                .Include(r => r.Kullanici)
                .Where(r => r.Durum == RandevuDurumu.Onaylandi)
                .ToList();

            ViewBag.AktifRandevular = aktifRandevular;

            return View(bekleyenRandevular); // Sadece bekleyen randevular model olarak gönderiliyor
        }

        public IActionResult Onayla(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Durum = RandevuDurumu.Onaylandi;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Randevu başarıyla onaylandı.";
            }
            return RedirectToAction("Randevular");
        }

        public IActionResult Reddet(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Durum = RandevuDurumu.Reddedildi;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Randevu başarıyla reddedildi.";
            }
            return RedirectToAction("Randevular");
        }
    }
}