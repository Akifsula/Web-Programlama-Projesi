using Microsoft.AspNetCore.Identity;

namespace KuaforYonetim.Models
{
    public class Kullanici : IdentityUser
    {
        public string AdSoyad { get; set; }
    }
}

// Identity oldugu icin diger degerleri otomatik olusturuyor. Bu yüzden Kullanıcıya sadece AdSoyad degeri atadım.