# YTV Depo Otomasyon Yazılımı

**YTV Yazılım** (Arda) tarafından geliştirilmiştir.

## 📌 Proje Hakkında

YTV Depo Otomasyon, küçük ve orta ölçekli depolar için geliştirilmiş masaüstü tabanlı bir stok ve ürün yönetim yazılımıdır. Ürün ekleme, stok takibi, kategori yönetimi ve istatistik görüntüleme gibi temel depo işlemlerini kapsar.

## 🚀 Özellikler

- 🔐 Kullanıcı girişi ve kimlik doğrulama (Yakında personel ve kullanıcı girişi ile ilgili güncellemeler gelecektir.)
- 📦 Ürün ekleme, silme, güncelleme ve listeleme
- 📷 Webcam ile barkod okuma (fiziksel barkod cihazı olmadan)
- 📊 Stok hareketi takibi (giriş/çıkış)
- 📄 PDF stok raporu oluşturma
- 🗂️ Kategori yönetimi
- 📈 İstatistik ekranı (toplam ürün, en düşük stok, kar marjı grafiği)

## 🛠️ Kullanılan Teknolojiler

- C# — Windows Forms (.NET Framework)
- Microsoft SQL Server
- AForge.Video — kamera entegrasyonu
- ZXing.Net — barkod okuma
- iTextSharp — PDF oluşturma

## ⚙️ Kurulum

1. Repoyu klonlayın
2. `./schema.sql` dosyasını SQL Server Management Studio'da çalıştırın.
3. `sqlbaglanti.cs` dosyasını kendiniz oluşturun:

```csharp
internal class sqlbaglanti
{
    public SqlConnection baglanti()
    {
        SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DBDepoOtomasyon;Integrated Security=True;");
        conn.Open();
        return conn;
    }
}
```

4. Visual Studio'da projeyi açın ve NuGet paketlerini yükleyin
5. Uygulamayı çalıştırın

## 📋 Gereksinimler

- Windows işletim sistemi
- Visual Studio 2019 veya üzeri
- Microsoft SQL Server (Express yeterli)
- Webcam (barkod okuma için) veya Barkod Cihazı

## 👨‍💻 Geliştirici
Arda
**YTV Yazılım**  
[GitHub](https://github.com/ytvyazilim)
[YTV Mağaza](https://www.itemsatis.com/p/YTVMAGAZA)
[İnstagram](https://www.instagram.com/qarda078/)
[Threads](https://www.threads.com/@qarda078)
Discord : ytvmagaza

## Hakkımda
Merhaba, ben Arda. 17 yaşındayım ve dijital dünyada sadece kod yazan değil, değer üreten bir geliştirici olma yolunda ilerleyen bir öğrenciyim. Yazılım mühendisliği hedefimle, günümü hem kendimi geliştirmeye hem de topluluğa fayda sağlayacak işler yapmaya adıyorum.

Teknik dünyamda C#, SQL ve ASP.NET MVC mimarisi ile güçlü backend çözümleri üretirken; aynı zamanda Unity ile oyun dünyasının kapılarını aralıyorum. Masaüstü uygulamaları geliştirme ve oyun tasarımı üzerine yoğunlaşan çalışmalarımı, öğrendiğim her yeni bilgiyi kendi projelerimde "uygula ve geliştir" disipliniyle harmanlayarak pekiştiriyorum.

"YTV Yazılım" çatısı altında, merakımı üretimle birleştiriyor ve teknoloji ekosistemine kalıcı katkılar sağlamayı hedefliyorum.

Benimle teknik projeler, oyun geliştirme fikirleri veya dijital ekosistemler üzerine konuşmak isterseniz, bana aşağıdaki kanallardan ulaşabilirsiniz.
