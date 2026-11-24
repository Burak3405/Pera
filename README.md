# Pera

Pera, Windows için sürükle ve çark tabanlı bir dosya dönüştürme ve düzenleme uygulamasıdır. Şu an geliştirme aşamasında.

Pera, Orkun Işıtmak'ın açık kaynak [Nashi](https://github.com/orkunisitmak/nashi-app) projesinden türetilmiştir ve ondan esinlenmiştir. Nashi ile resmi bir bağlantısı yoktur.

## Neler yapar

27 araç ve 46 girdi biçimi: görsel, video, ses, PDF, Office belgeleri, arşiv ve altyazı dönüştürme; kırpma, işaretleme, sansürleme, QR okuma, PDF düzenleme ve web'den medya indirme. Dönüştürme ve düzenleme bilgisayarında çalışır; orijinal dosyaların değişmez, Pera yeni bir kopya kaydeder.

Medya ve ofis araçları (FFmpeg, yt-dlp, Deno, LibreOffice) ilk açılışta **Kurulumu tamamla → Eksik araçları indir** ile kendi resmi kaynaklarından indirilir ve doğrulanır. Ayrıntılar: [araçlar, sürümler ve lisanslar](docs/dependencies.md).

## Kısayollar

- **Sürüklerken:** Bir dosyayı herhangi bir uygulamada sürüklerken **Ctrl** araçlar, **Shift** dönüştürme, **Alt** dosya işlemleri çarkını açar. Tuşlar Ayarlar → Genel'den değiştirilebilir. Her seçeneğin açıklaması uygulamadaki **Araçlar ve kısayollar → Çark rehberi** bölümündedir.
- **Pera içinde:** **Ctrl+O** dosya seç, **Ctrl+V** dosya/metin/görsel/bağlantı yapıştır, **Ctrl+Space** seçili dosyaların çarkını aç, **Ctrl+1 / Ctrl+2** Ana Sayfa / Kitaplık, **Ctrl+F** ara, **Ctrl+Shift+D** kopyalanmış bağlantıyı indir. Ana sayfadaki listede **Delete**, seçili dosyaları asıllarını silmeden listeden kaldırır.
- **Medya düzenleyicide:** **Space** oynat/duraklat, **Sol/Sağ** bir saniye, **Shift+Sol/Sağ** bir kare, **I/O** aralık başı/sonu, **Shift+Space** seçili aralığı oynat.
- **Kırpmada:** Sürükleyerek seç, **Shift** oranı kilitler, **Enter** kaydeder.

Ayarlar → Genel'de, çark ve kısayolların geçici olarak durmasını istediğin uygulamaları virgülle yazabilirsin (ör. `Photoshop, CalculatorApp`). Pera sistem tepsisinde çalışmaya devam edebilir.

## Verilerin

Ayarlar, kitaplık ve indirilen araçlar `%LOCALAPPDATA%\Pera` klasöründe, uygulama klasörünün dışında tutulur. Kitaplık son 50 işlemi ve yapıştırılan dosyaları saklar. Pera arka planda panoyu izlemez ve analiz verisi göndermez.

## Derleme

Gerekenler: Windows 10/11 x64 ve [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
dotnet publish windows/Pera.Windows -c Release -o build/windows/Pera
```

Çıktı kendi kendine yeten bir klasördür; `build/windows/Pera/Pera.exe` ile açılır. Ayrıca .NET kurmak gerekmez.

Motor ve güvenlik kontrolleri:

```powershell
dotnet run --project windows/Pera.Verify -c Release
```

## Klasörler

- `windows/` — uygulama (`Pera.Windows`), ortak motor (`Pera.Core`) ve kontroller (`Pera.Verify`)
- `Resources/` — simgeler, ses, biçim kataloğu, Türkçe arayüz metinleri (`Localization/`), Pages dönüştürücü betiği ve lisanslar
- `Reference/` — test için arşiv ve belge örnekleri
- `docs/` — araç ve lisans bilgileri

## Lisans

Kaynak kod [MIT](LICENSE) lisanslıdır. Orijinal telif hakkı Orkun Işıtmak ve Nashi katkıcılarına aittir. Üçüncü taraf bileşenlerin lisansları `Resources/licenses/`, `windows/licenses/` ve `windows/THIRD-PARTY-NOTICES.txt` içindedir.
