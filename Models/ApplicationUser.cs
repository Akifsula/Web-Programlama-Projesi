using Microsoft.AspNetCore.Identity;

namespace KuaforYonetim.Models
{
    // IdentityUser'den türetilen ApplicationUser sınıfı
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } // Kullanıcının tam adı
    }
}

// kullanılmadıgı icin bu dosyayı silecegim