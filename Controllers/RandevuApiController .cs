using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KuaforYonetim.Data;
using KuaforYonetim.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("api/Randevu")]
[ApiController]
public class RandevuApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RandevuApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("uygunluklar/{calisanId}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetCalisanUygunluklari(int calisanId)
    {
        try
        {
            // Çalışanın uygunluk saatlerini getir
            var uygunluklar = await _context.CalisanUygunluklar
                .Where(u => u.CalisanId == calisanId)
                .ToListAsync();

            if (!uygunluklar.Any())
            {
                return NotFound("Bu çalışan için uygunluk bilgisi bulunamadı.");
            }

            // Çalışanın onaylanmış randevularını al
            var doluRandevular = await _context.Randevular
                .Where(r => r.CalisanId == calisanId && r.Durum == RandevuDurumu.Onaylandi)
                .Select(r => r.Tarih)
                .ToListAsync();

            var uygunSaatler = new List<object>();

            foreach (var uygunluk in uygunluklar)
            {
                // Çalışanın uygun olduğu saat aralığını hesapla
                var uygunSaatAraligi = Enumerable.Range((int)uygunluk.BaslangicSaati.TotalHours,
                                                         (int)(uygunluk.BitisSaati - uygunluk.BaslangicSaati).TotalHours)
                                                 .Select(s => uygunluk.BaslangicSaati.Add(TimeSpan.FromHours(s)));

                // O günün dolu saatlerini al
                var doluSaatler = doluRandevular
                    .Where(r => r.DayOfWeek == uygunluk.Gun)
                    .Select(r => r.TimeOfDay)
                    .ToHashSet();

                // Uygun ve dolu olmayan saatleri hesapla
                var musaitSaatler = uygunSaatAraligi
                    .Where(saat => !doluSaatler.Contains(saat)) // Sadece dolu olmayan saatler
                    .Select(saat => new
                    {
                        Gun = uygunluk.Gun.ToString(),
                        Saat = saat.ToString(@"hh\:mm")
                    });

                uygunSaatler.AddRange(musaitSaatler);
            }

            return Ok(uygunSaatler);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return StatusCode(500, "Sunucu hatası.");
        }
    }
}
