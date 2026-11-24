# Araçlar, sürümler ve lisanslar

Pera'nın kendi paketi yalnızca Pera'yı, .NET çalışma zamanını ve `windows/THIRD-PARTY-NOTICES.txt` içinde listelenen açık kaynak kütüphaneleri içerir. Büyük medya ve ofis araçları ilk kurulumda **doğrudan kendi resmi kaynaklarından** indirilir. Her dosyanın SHA-256 özeti kurulumdan önce kodda sabitlenmiş değerle karşılaştırılır; eşleşmezse kurulmaz.

İlk açılışta **Kurulumu tamamla → Eksik araçları indir** ile Medya ve Ofis araç grupları sırayla kurulur. Bilgisayarında zaten bulunan araçlar yeniden indirilmez. Görsel, PDF ve ZIP işlemleri araç kurmadan da çalışır. Kurulum yönetici izni istemez ve Windows güvenlik ayarlarını değiştirmez.

## Medya araçları

Video ve ses dönüştürme, medya önizleme, bazı görsel biçimleri ve web indirmeleri için gerekir.

- [FFmpeg ve ffprobe 9.0.2 full build, gyan.dev](https://github.com/GyanD/codexffmpeg/releases/tag/9.0.2) — GPL. gyan.dev, [FFmpeg'in resmi indirme sayfasında](https://ffmpeg.org/download.html) listelenir. [Derleme bilgisi ve kaynaklar](https://www.gyan.dev/ffmpeg/builds/).
- [yt-dlp 2026.08.19](https://github.com/yt-dlp/yt-dlp/releases/tag/2026.08.19) — GPLv3+ (tek dosya derlemesi). [Kaynaklar ve üçüncü taraf lisansları](https://github.com/yt-dlp/yt-dlp/tree/2026.08.19).
- [Deno 2.9.7](https://github.com/denoland/deno/releases/tag/v2.9.7) — MIT ve içerdiği bağımlılıkların lisansları. [Kaynaklar](https://github.com/denoland/deno/tree/v2.9.7).

Tam adresler ve SHA-256 değerleri: [`Toolchain.cs`](../windows/Nashi.Core/Toolchain.cs).

## Ofis araçları

Office belgelerini (Word, Excel, PowerPoint) dönüştürmek için gerekir.

- [LibreOffice 26.8.1, The Document Foundation](https://www.libreoffice.org/download/) — MPL 2.0 ve içerdiği üçüncü taraf lisansları. Resmi Windows x64 MSI paketi, Pera'nın kullanıcı araç klasörüne açılır.

Tam adresler ve SHA-256 değerleri: [`OfficeRuntime.cs`](../windows/Nashi.Windows/OfficeRuntime.cs).

## Meta veri aracı

Meta veri düzenleme ilk kullanıldığında ayrıca indirilir.

- [ExifTool 13.59](https://exiftool.org/) — Perl Artistic License / GPL. Resmi SourceForge dağıtımından indirilir. Lisans metni: [`Resources/licenses/ExifTool-LICENSE.txt`](../Resources/licenses/ExifTool-LICENSE.txt).

Tam adres ve SHA-256 değeri: [`MetadataService.cs`](../windows/Nashi.Core/MetadataService.cs).

## Pera ile birlikte gelen kütüphaneler

- Windows kütüphanelerinin bildirimleri: [`windows/THIRD-PARTY-NOTICES.txt`](../windows/THIRD-PARTY-NOTICES.txt) ve [`windows/licenses/`](../windows/licenses/)
- Diğer lisans metinleri: [`Resources/licenses/`](../Resources/licenses/)

Pera, Orkun Işıtmak'ın [Nashi](https://github.com/orkunisitmak/nashi-app) projesine dayanır ve MIT lisansıyla kullanılır. Bu sayfa Nashi'nin araç listesinden uyarlanmıştır.
