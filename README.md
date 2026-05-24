# YTV Depo Otomasyon Yazılımı

**YTV Yazılım** tarafından geliştirilmiştir.

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

**YTV Yazılım**  
[GitHub](https://github.com/ytvyazilim) • [YTV Mağaza](https://www.itemsatis.com/p/YTVMAGAZA)