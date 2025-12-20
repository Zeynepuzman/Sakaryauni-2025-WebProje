# PowerZone – Spor & Wellness Yönetim Sistemi

Bu proje, **ASP.NET Core MVC** kullanılarak geliştirilmiş bir spor salonu ve wellness yönetim sistemidir.  
Kullanıcıların spor ve beslenme ihtiyaçlarına yönelik **yapay zekâ destekli öneriler**, paket yönetimi ve randevu sistemi sunar.

---

##  Projenin Amacı

- Spor salonu yönetimini dijital ortama taşımak  
- Üyelere kişiselleştirilmiş spor & beslenme önerileri sunmak  
- Admin, Antrenör ve Üye rollerini ayrı ayrı yönetmek  
- Gerçek hayatta kullanılabilir bir web uygulaması geliştirmek

---

##  Kullanıcı Rolleri

###  Üye
- Kayıt olma ve giriş yapma
- Paket görüntüleme
- Randevu alma
- Yapay zekâ destekli egzersiz & beslenme önerisi alma

###  Antrenör
- Kendi randevularını görüntüleme
- Hizmet verdiği alanları yönetme

###  Admin
- Üye, antrenör ve hizmet yönetimi
- Paket ekleme / düzenleme
- Randevuları görüntüleme
- İstatistikleri görüntüleme

---

##  Yapay Zekâ Özelliği

Projede yapay zekâ, kullanıcıdan alınan bilgilere göre:

- Egzersiz planı
- Beslenme önerisi
- Tahmini vücut etkisi

oluşturmak için kullanılmıştır.


---

##  Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- Bootstrap
- JavaScript
- Unsplash API
- Groq AI API

---

##  Veritabanı Yapısı (Özet)

Projede **Code First** yaklaşımı kullanılmıştır.

### Temel Tablolar:
- **AspNetUsers** → Kullanıcı bilgileri
- **AspNetRoles** → Rol bilgileri
- **Uyeler**
- **Antrenorler**
- **Hizmetler**
- **Paketler**
- **Randevular**
- **UyePaketler**
- **AntrenorHizmetler**

Tablolar arasında **birden-çoğa** ve **çoktan-çoğa** ilişkiler bulunmaktadır.

---

## Veri Doğrulama (Validation)

- **Sunucu tarafı doğrulama**:  
  Data Annotation (`[Required]`, `[Range]`, `[StringLength]`) kullanılmıştır.
- **İstemci tarafı doğrulama**:  
  ASP.NET MVC’nin yerleşik validation mekanizması ile sağlanmıştır.

Bu sayede hatalı veri girişi hem kullanıcı tarafında hem sunucu tarafında engellenmiştir.

---

##  Session & Cookie Kullanımı

- **ASP.NET Identity** cookie tabanlı kimlik doğrulama kullanır
- Giriş yapan kullanıcının bilgileri **Session** üzerinden tutulur
- Rol bazlı yönlendirme yapılmaktadır

---

##  Kurulum Adımları

1. Projeyi klonlayın
2. `appsettings.json` dosyasını düzenleyin
3. Veritabanı migration işlemlerini çalıştırın
4. Projeyi çalıştırın

---

##  Güvenlik Notu
- API anahtarları **GitHub’a eklenmemiştir**
- `appsettings.Development.json` dosyası `.gitignore` ile korunmaktadır

---

##  Sonuç

Bu proje, gerçek hayatta kullanılabilecek bir spor salonu yönetim sistemi olacak şekilde tasarlanmıştır.  
ASP.NET Core MVC mimarisi, katmanlı yapı ve modern web teknolojileri kullanılarak geliştirilmiştir.
