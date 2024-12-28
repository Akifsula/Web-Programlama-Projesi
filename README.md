![image](https://github.com/user-attachments/assets/100de0bc-9b99-493a-bb23-5f53469d8cfb)
![image](https://github.com/user-attachments/assets/4503cd4b-4512-4e8f-88fd-1f54c4f47a42)

Kuaför Yönetim Uygulaması Proje Özeti 
Bu projede, bir kuaför/berber işletmesinin çalışma süreçlerini dijitalleştirmek 
amacıyla, kullanıcıların kayıt olup giriş yaptıktan sonra hizmetleri ve çalışanları 
inceleyip randevu alabilecekleri, admin olarak giriş yapan kişinin ise çalışanların 
uzmanlık ve uygunluk bilgilerini tanımlayabileceği  ve alınan randevuların 
yönetimini yapabileceği bir ASP.NET Core MVC tabanlı web uygulaması 
geliştirilmiştir. 
1.VERİTABANI BAĞLANTISI VE ENTİTY FRAMEWORK 
Entity Framework (EF), .NET platformunda kullanılan bir Object-Relational 
Mapping (ORM) aracıdır. Bu araç, uygulama kodu ile veritabanı arasında bir köprü 
görevi görerek, SQL sorguları yazmadan nesne tabanlı bir şekilde veritabanı işlemleri 
yapılmasına olanak sağlamıştır. Projede, Entity Framework'ü kullanmak, veri erişim 
ve yönetim işlemlerini kolaylaştırmıştır. 
Projede kullanılan veritabanı bağlantısı, `appsettings.json` dosyasındaki 
`DefaultConnection` ayarı ile sağlanmıştır. Bu bağlantı sayesinde ASP.NET Core 
üzerinden SQL Server veritabanına erişim sağlanmıştır. Bağlantı ayarlarının bir 
örneği aşağıda verilmiştir: 
{ 
"ConnectionStrings": { 
"DefaultConnection": 
"Server=(localdb)\MSSQLLocalDB;Database=KuaforYonetimDB;Trusted_Connecti
 on=True;MultipleActiveResultSets=true" 
} 
} 
2. VERİTABANI MODELİ 
Veritabanı, aşağıdaki ana tabloları içermektedir: - Kullanıcılar: Kullanıcı bilgilerini saklar (kayıtlı müşteriler ve adminler). Bu 
tablolardan bazıları Identity sayesinde otomatik oluşur. - Çalışanlar: Çalışanların uzmanlık alanlarını ve uygunluk saatlerini içerir. - Randevular: Kullanıcıların çalışanlardan aldığı randevuları yönetir. -Hizmetler: Kuaför sisteminin kullanıcılara sunduğu hizmetleri içerir. Bu tablodaki 
değerler aynı zamanda Çalışanlara atanabilen uzmanlık alanlarıdır ve bu iki tablo Id 
numaralarına göre eşleşerek çoka çok ilişkisiye sahip olur. Bu iliski sonucu 
CalisanHizmetler isimli bir ara tablo da sisteme dahil edilir. -CalisanUygunluklar: Calışanların çalışma saati aralıkları ve gün bilgilerini tutar.

![image](https://github.com/user-attachments/assets/601d2e3b-86e2-48cd-a0bd-671a5e158130)

Oluşturulan bu veri tabanı, Visual studio içerisinde Görünüm/Sql Server Object Explorer sekmesinden ya da Microsoft SQL Server Managemenet Studio üzerinden incelenebilmektedir. 
Aşağıdaki görüntüde bu veri tabanındaki bazı tabloları ve “Randevular” tablosundan sistemdeki mevcut randevu verilerini gösteren sorguyu inceleyebilirsiniz.

![image](https://github.com/user-attachments/assets/73c76f6e-343a-4722-bea3-2a2d95dba587)

3. CONTROLLER SINIFLARI
Aşağıda, projede bulunan bazı önemli Controller sınıfları ve bu sınıfların Model-View ilişkisiyle hangi görevleri yerine getirdikleri listelenmiştir:
- AccountController.cs: Kullanıcıların kayıt olma ve giriş işlemlerini yönetir. Şifrelerin hashlenerek saklanması ve kimlik doğrulama işlemleri bu sınıfta yapılır.
- AdminController.cs: Çalışan ekleme, silme, uygunluk saatlerini ve uzmanlık alanlarını tanımlama işlemleri yapılır.
- RandevuController.cs: Kullanıcıların randevu oluşturma, listeleme, iptal etme ve adminin bu randevuları onaylama/red etme işlemlerini yönetir.
- RandevuApiController.cs: REST API çağrılarını yönetir. Örneğin, çalışanların uygunluk bilgilerini ve randevu detaylarını sorgulama işlevlerini içerir.
4. KULLANICI VE ADMİN GİRİŞ MEKANİZMASI
Projede iki tür kullanıcı bulunmaktadır:
- Admin: “B211210030@sakarya.edu.tr” mail adresi ile ve “sau” şifresi ile giriş yapar. Admin, çalışan ekleme/çıkarma ve işlemleri düzenleme yetkisine sahiptir.
- Kayıtlı Kullanıcılar: Kullanıcılar, e-posta ve şifre ile sisteme kayıt olabilir ve giriş yapabilir. Şifreler güvenli bir şekilde hashlenerek saklanır.

Kayıt olduğu bilgilerle sisteme giriş yapan bir kullanıcıyı şöyle bir arayüz karşılar: 

![image](https://github.com/user-attachments/assets/9df2b544-4cb5-41c9-b326-d47fd7260a2a)

5. REST API KULLANIMI
REST API, veritabanı ile iletişim kurmak için kullanılmıştır. Örnek kullanım senaryoları:
- Randevu detaylarının alınması için GET metodu.
- Çalışanların uygunluk saatlerinin sorgulanması için GET metodu.
- Yeni bir randevu oluşturulması için POST metodu.
REST API'nin temel amacı, uygulamanın veri alışverişini hızlandırmak ve güvenli bir şekilde gerçekleştirmektir. 


6. PROJEDE REST API VE LINQ SORGULARI KULLANIM ÖRNEĞİ

![image](https://github.com/user-attachments/assets/308e3820-e74d-4c85-9d6a-4a41c5515e85)

Kullanıcı, uygulamanın https://localhost:7100/Randevu/Ekle  sayfasında randevu talebi oluştururken ilgili form elemanlarını doldurmadan önce “Uygunlukları Görüntüle”  butonuna tıklayıp randevu almak istediği çalışanı seçtiği zaman, “RandevuApiController.cs” sınıfındaki methotlar ve yönlendirmelerle API mekanizması çalışır ve “Uygunluk.js” sınıfındaki kodlar sayesinde ilgili fetch adresiyle eşleşip kullanıcıya seçilen çalışanın randevu alınmamış gün ve saatleri mini bir pencerede aşağıdaki gibi listelenir.

![image](https://github.com/user-attachments/assets/a1683654-011b-4224-864d-2c14d583cca7)


Gelen pencerede “13.00” değerinin olmadığı kolaylıkla görülebiliyor. Bunun sebebi: bir kullanıcının Rauf Sula adlı çalışandan Salı günü 13.00 saati için onaylanmış bir randevusu bulunmasıdır. Kullanıcılar bu değerleri  Çalışanları Gör butonuna tıkladıktan sonra İlgili çalışanın kartındaki Detay butonuyla yönlendirildiği https://localhost:7100/Calisan/Detay/2 sayfasından da aşağıdaki gibi gözlemleyebilir.

![image](https://github.com/user-attachments/assets/f8b1924c-ade8-49de-a8af-8b7c0df61416)


API nin döndürdüğü değeri kontrol etmek için tarayıcıya  https://localhost:7100/api/Randevu/uygunluklar/2  adresi de yazılabilir. 

![image](https://github.com/user-attachments/assets/1b082760-1f15-4e98-ae7c-d186c66cd024)


7. YAPAY ZEKA ENTEGRASYONU

Kullanıcının yüklediği bir fotoğraf üzerinde belirtilen saç stili ve renk değişikliklerini uygulayarak kullanıcıya yeni bir görsel sunmayı hedefler. Bu, kullanıcıların farklı saç modelleri ve renklerini görselleştirerek karar vermelerine yardımcı olur.
Nasıl Çalışır?
1.	Fotoğraf ve Kullanıcı Tercihlerini Alma:
o	Kullanıcı bir fotoğraf URL'si girer ve hangi düzenlemelerin yapılacağını (saç stili, saç rengi veya her ikisi) seçer.
o	Kullanıcı ayrıca istediği saç stili ve rengi hakkında açıklamalar sağlar.
2.	API'ye Bağlanma:
o	Kullanıcıdan alınan bilgiler, MagicAPI adındaki bir harici yapay zeka servisine REST API aracılığıyla gönderilir.
o	API, görüntüyü işler ve bir request_id döndürerek işlemin başlatıldığını bildirir.


3.	İşlem Durumu Kontrolü:
o	Uygulama, dönen request_id ile düzenleme işleminin tamamlanıp tamamlanmadığını kontrol eder.
o	İşlem devam ediyorsa, API'den durumu düzenli aralıklarla kontrol eder.
4.	Sonucun Gösterilmesi:
o	İşlem tamamlandığında API'den işlenmiş görüntü URL'si alınır.
o	Kullanıcıya işlenmiş görüntü ve önerilen saç modelleri gösterilir.


![image](https://github.com/user-attachments/assets/6b81205d-8145-4550-a82a-cec86c5ed45c)

![image](https://github.com/user-attachments/assets/18d23dc4-86d7-4f2d-8551-66ffab79512e)

8.PROJEDE KULLANILAN TEKNOLOJİLER:
−	Asp.Net Core 6 MVC veya daha yukarı sürümleri
−	Veritabanı olarak SQL Server
−	C#
−	Entity Framework Core ORM
−	Bootstrap Tema
−	HTML5, CSS3, Javascript ve JQUERY



NOT:    Yapay Zekadan saç önerisi fotoğrafı istenirken girilmesi gereken:

•	Fotoğraf formatı URL örneği: https://replicate.delivery/mgxm/b8be17a7-abcb-4421-80f2-e6a1e3fe38c7/MarkZuckerberg.jpg

•	Renk örnekleri:    "red",    "purple",    "blonde" …

•	Saç Stili örnekleri:   “afro hairstyle”    “bob cut hairstyle”    “bowl cut hairstyle”  “braid hairstyle”    “caesar cut hairstyle” …








