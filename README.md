# Pera

Pera, Windows için sürükle ve çark tabanlı bir dosya dönüştürme ve düzenleme uygulamasıdır. Şu an geliştirme aşamasında.

Pera, Orkun Işıtmak'ın açık kaynak [Nashi](https://github.com/orkunisitmak/nashi-app) projesinden türetilmiştir ve ondan esinlenmiştir. Nashi ile resmi bir bağlantısı yoktur. Bu depo, Nashi'nin Windows sürümünün kişisel tercihlere göre değiştirilmiş bir kopyasıdır; macOS sürümü, web sitesi ve Nashi'ye özel yayın otomasyonu çıkarılmıştır.

> Not: Uygulamanın adı Pera; kod klasörleri ve iç adlar (ör. `Nashi.Core`) şimdilik Nashi adıyla duruyor.

## Derleme

Gerekenler: Windows 10/11 x64 ve [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
dotnet publish windows/Nashi.Windows -c Release -o build/windows/Pera
```

Çıktı kendi kendine yeten bir klasördür; `build/windows/Pera/Pera.exe` ile açılır.

Motor ve güvenlik kontrolleri:

```powershell
dotnet run --project windows/Nashi.Verify -c Release
```

## Klasörler

- `windows/` — uygulama (`Nashi.Windows`), ortak motor (`Nashi.Core`) ve kontroller (`Nashi.Verify`)
- `Resources/` — simgeler, ses, biçim kataloğu, Pages dönüştürücü betiği ve lisanslar
- `Sources/NashiCore/Resources/` — Türkçe arayüz metinleri
- `Reference/` — test için arşiv ve belge örnekleri

## Lisans

Kaynak kod [MIT](LICENSE) lisanslıdır. Orijinal telif hakkı Orkun Işıtmak ve Nashi katkıcılarına aittir. Üçüncü taraf bileşenlerin lisansları `Resources/licenses/`, `windows/licenses/` ve `windows/THIRD-PARTY-NOTICES.txt` içindedir.
