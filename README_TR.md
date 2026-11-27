<p align="center">
  <a href="README.md">🇬🇧 English</a> · <a href="README_TR.md">🇹🇷 Türkçe</a>
</p>

<p align="center">
  <img src="zen-browser.ico" alt="Zen Browser Portable" width="80" />
</p>

<h1 align="center">Zen Browser — Windows Taşınabilir</h1>

<p align="center">
  <strong>Tarayıcını, profillerini ve gizliliğini yanında taşı — her yere.</strong>
</p>

<p align="center">
  <a href="https://zen-browser.app"><img src="https://img.shields.io/badge/Zen%20Browser-Resmi%20Site-8338ec?style=for-the-badge&logo=firefox&logoColor=white" alt="Zen Browser"></a>
  <a href="https://github.com/zen-browser/desktop"><img src="https://img.shields.io/github/stars/zen-browser/desktop?style=for-the-badge&logo=github&label=Upstream%20Stars&color=ff6f61" alt="Stars"></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/Lisans-MPL--2.0-blue?style=for-the-badge" alt="Lisans"></a>
  <a href="#"><img src="https://img.shields.io/badge/Platform-Windows%2010%2F11-0078d4?style=for-the-badge&logo=windows&logoColor=white" alt="Platform"></a>
</p>

---

## 📖 Hakkında

Bu proje, [Zen Browser](https://zen-browser.app)'ın Windows için **taşınabilir (portable) paketidir**. Zen Browser'ı USB bellekten, harici diskten veya bilgisayarınızdaki herhangi bir klasörden **kurulum yapmadan** çalıştırmanızı sağlar; tarayıcı verileriniz de o klasörün içinde kalır.

> [!NOTE]
> Zen Browser; verimlilik odaklı, açık kaynaklı ve Firefox tabanlı bir tarayıcıdır. Bu paket tarayıcıda **hiçbir değişiklik yapmaz**: resmi dosyaları kullanır ve Zen'i taşınabilir klasördeki profile yönlendiren küçük bir başlatıcı ekler.

### ✨ Temel Özellikler

| Özellik | Açıklama |
|---|---|
| 🔒 **Kurulum Yok** | Yükleyici yok, yönetici izni gerekmez. Çıkartın ve çalıştırın. |
| 💾 **Tek Klasör** | Tarayıcı dosyaları, profiller, yer imleri, eklentiler — her şey tek bir klasörde. |
| 👤 **Çoklu Profil** | Hazır 5 profil yuvası ve istediğiniz kadar adlandırılmış profil. Başlatırken <kbd>Shift</kbd>'e basılı tutarak seçebilirsiniz. |
| 🔗 **Bağlantı ve Dosya Açar** | Bağlantılar, `.html` dosyaları ve "Birlikte aç" çalışır. Açıkken tekrar başlatmak yalnızca yeni bir pencere açar. |
| 🧹 **Arkasını Toplar** | Tarayıcı kapanınca `PortableZen.exe`, Zen'in bu klasör için kayıt defterine yazdığı küçük değerleri siler. |
| 🔄 **Otomatik Sürümler** | GitHub Actions her yeni Zen sürümünü genellikle birkaç saat içinde paketler. |

---

## 🚀 Hızlı Başlangıç

### Gereksinimler

- **İşletim Sistemi:** Windows 10 / 11 (x64)
- **Disk Alanı:** ~500 MB

### Kurulum

1. [📦 Releases](https://github.com/Metee01/Zen-Browser-Portable/releases) sayfasından en son `Zen-Portable-v<sürüm>.zip` dosyasını indirin. İçinde resmi Zen Browser dosyaları zaten var.
2. ZIP'i istediğiniz yere çıkartın — masaüstü, USB bellek, harici disk.
3. **`PortableZen.exe`** dosyasına çift tıklayın ve gezinmeye başlayın! 🎉

> [!TIP]
> `.exe` yerine okuyup düzenleyebileceğiniz bir başlatıcı isterseniz `zenbrowser.bat` dosyasını kullanabilirsiniz. Aşağıda anlatılan kayıt defteri temizliğini yapmaz.

<details>
<summary>Release yerine depodan kurmak</summary>

1. Bu depoyu klonlayın veya indirin.
2. Zen Browser dosyalarını `app/win/` içine koyun; `app/win/zen.exe` bulunmalı. Dosyaları resmi yükleyiciden 7-Zip ile çıkartabilir (dosyalar `core` klasöründedir) ya da `C:\Program Files\Zen Browser\` klasöründen kopyalayabilirsiniz.
3. Başlatıcıyı derleyin (SDK gerekmez, derleyici Windows ile gelir):

   ```bat
   C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /nologo /target:winexe /win32icon:zen-browser.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:PortableZen.exe src\launcher.cs
   ```

   Ya da doğrudan `zenbrowser.bat` kullanın.
</details>

---

## 👤 Profiller

ZIP beş profil klasörüyle gelir: `data/profile1` … `data/profile5`. Varsayılan `profile1`'dir.

| Nasıl | Ne olur |
|---|---|
| `PortableZen.exe`'ye çift tıklamak | `data/profile1` açılır. |
| Başlatırken <kbd>Shift</kbd>'e basılı tutmak | `data/` içindeki tüm klasörleri listeleyen küçük bir pencere açılır; birini seçersiniz. |
| `PortableZen.exe --profile 2` | `data/profile2` açılır. Masaüstü kısayolu için kullanışlıdır. |
| `PortableZen.exe --profile is` | `data/is` açılır; yoksa taşınabilir varsayılan ayarlarla oluşturulur. |

Farklı profiller aynı anda açık olabilir. `zenbrowser.bat` içinde `set "PROFILE=profile1"` satırındaki `profile1`'i değiştirin.

> [!WARNING]
> Zen'in `-ProfileManager` seçeneğini kullanmayın. Profil listesini taşınabilir klasörün dışına, `%APPDATA%` içine kaydeder.

---

## 🔄 Güncelleme

1. Zen Browser'ı kapatın.
2. [Releases](https://github.com/Metee01/Zen-Browser-Portable/releases) sayfasından en yeni ZIP'i indirin.
3. Taşınabilir klasörünüzdeki **`app\win`** klasörünü silin.
4. Yeni ZIP'teki **`app`** klasörünü ve **`PortableZen.exe`** dosyasını taşınabilir klasörünüze kopyalayın.

Yer imleriniz, şifreleriniz, eklentileriniz ve geçmişiniz `data/` içinde durur ve etkilenmez.

> [!CAUTION]
> Güncelleme yaparken `data/` klasörünü asla silmeyin — tüm tarayıcı verileriniz oradadır.

---

## ⚙️ Nasıl Çalışır?

`PortableZen.exe`, Windows ile gelen C# derleyicisiyle derlenmiş küçük bir C# programıdır ([`src/launcher.cs`](src/launcher.cs)). Başlattığınızda:

1. Yanındaki `app\win\zen.exe` dosyasını bulur (tüm yollar görecelidir, klasör taşınabilir).
2. Zen'i `-profile data\<profil>` ile başlatır; böylece tüm tarayıcı verileri taşınabilir klasörde kalır.
3. Birlikte açtığınız bağlantı veya dosyaları doğrudan Zen'e iletir. Bu taşınabilir Zen zaten açıksa Zen bunları ikinci bir kopya başlatmak yerine açık pencereye verir; ayrıca kurulu bir Zen'e asla bağlanmaz.
4. Tarayıcı kapanana kadar arka planda bekler, sonra Zen'in bu klasör için kayıt defterine yazdığı değerleri siler (aşağıya bakın).

Taşınabilir profiller ayrıca bir `user.js` dosyasıyla gelir: otomatik güncellemeleri, varsayılan tarayıcı sorusunu, çökme raporlayıcısını, telemetriyi ve deneyleri kapatır; USB belleği yormamak için önbelleği diskte değil bellekte tutar. Dilediğiniz gibi düzenleyebilirsiniz; tarayıcı her başladığında uygulanır.

---

## 🛡️ Gizlilik Notları

- **Tarayıcı verileri:** Profiller, geçmiş, şifreler, eklentiler ve önbellek `data/` içinde kalır. Testlerimizde `%APPDATA%\zen` veya `%LOCALAPPDATA%\zen` içine hiçbir şey yazılmadı.
- **Kayıt defteri:** Zen (Firefox gibi) çalışırken `HKCU\Software\Mozilla\Zen` altına pencere boyutu ve son açılış zamanı gibi birkaç küçük değer yazar. Bunlarda gezinme verisi bulunmaz. `PortableZen.exe` tarayıcı kapanınca kendi klasörüne ait olanları siler; kurulu bir Zen'in değerlerine dokunmaz. `zenbrowser.bat` bu temizliği yapmaz.
- **Windows'un kendisi** bir programın çalıştığını yine de hatırlayabilir (örneğin son kullanılan dosyalar listesi veya prefetch verisi). Bu, taşınabilir bir uygulamanın kontrolü dışındadır.
- **Kaynaktan indirin:** Bu deponun resmi Releases sayfasını kullanın; paketler resmi Zen Browser yükleyicisinden otomatik olarak üretilir.

---

## 🔍 Antivirüs Uyarıları

Bazı antivirüs motorları veya Windows SmartScreen, `PortableZen.exe` küçük, yeni ve dijital olarak imzasız olduğu için uyarı verebilir. **Dosya herhangi bir zararlı kod içermez.** Kaynağın tamamı [`src/launcher.cs`](src/launcher.cs) dosyasındadır ve release iş akışı exe'yi doğrudan bu dosyadan derler:

```
src\launcher.cs  →  csc.exe /target:winexe  →  PortableZen.exe
```

İsterseniz bunun yerine `zenbrowser.bat` dosyasını çalıştırabilirsiniz.

---

## 📁 Dizin Yapısı

```
Zen-Browser-Portable/
├── app/
│   └── win/              # Zen Browser dosyaları (release ZIP'inde hazır gelir)
│       ├── zen.exe
│       └── ...
├── data/
│   ├── profile1/         # Varsayılan profil
│   ├── profile2/ … profile5/
│   └── <ad>/             # --profile <ad> ile oluşturduğunuz profiller
├── src/
│   └── launcher.cs       # PortableZen.exe'nin kaynağı
├── PortableZen.exe       # Başlatıcı (release iş akışı derler)
└── zenbrowser.bat        # Düz metin alternatif başlatıcı
```

---

## 🙏 Atıf ve Teşekkürler

Bu proje bir **topluluk taşınabilir paketidir** ve Zen Browser ekibinin **resmi bir ürünü değildir**.

<table>
  <tr>
    <td><strong>Zen Browser</strong></td>
    <td>
      Güzel tasarlanmış, Firefox tabanlı, açık kaynaklı bir tarayıcı.<br>
      🌐 <a href="https://zen-browser.app">zen-browser.app</a><br>
      💻 <a href="https://github.com/zen-browser/desktop">github.com/zen-browser/desktop</a><br>
      📄 <a href="https://docs.zen-browser.app">docs.zen-browser.app</a><br>
      📜 <a href="https://github.com/zen-browser/desktop/blob/dev/LICENSE">MPL-2.0</a> lisansı altında
    </td>
  </tr>
  <tr>
    <td><strong>Mozilla Firefox</strong></td>
    <td>
      Zen Browser'ın arkasındaki motor.<br>
      🌐 <a href="https://www.mozilla.org/firefox/">mozilla.org/firefox</a>
    </td>
  </tr>
</table>

> Zen Browser'ı beğendiyseniz, projeyi desteklemeyi düşünün:
> - ☕ [Ko-fi](https://ko-fi.com/zen_browser)
> - 🎁 [Patreon](https://patreon.com/zen_browser)

---

## 📜 Lisans

Bu depodaki dosyalar (başlatıcı, betikler, yapılandırma ve dokümantasyon) **[Mozilla Public License 2.0](LICENSE)** lisansı altındadır.

Zen Browser'ın kendisi de **[MPL-2.0](https://github.com/zen-browser/desktop/blob/dev/LICENSE)** lisansı altındadır. Zen Browser kaynak koduna ve dosyalarına ait tüm haklar ilgili yazar ve katkıda bulunanlara aittir.

---

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Taşınabilir yapılandırmayı geliştirmek için fikirleriniz varsa:

1. Bu depoyu **Fork**'layın
2. Bir **özellik dalı** oluşturun (`git checkout -b feature/iyilestirmem`)
3. Değişikliklerinizi **commit**'leyin (`git commit -m "feat: iyileştirmemi ekle"`)
4. Dalı **push**'layın (`git push origin feature/iyilestirmem`)
5. Bir **Pull Request** açın

### Sorun Bildirme

**Taşınabilir başlatıcı** ile ilgili sorunlarla karşılaşırsanız, lütfen bu depoda bir issue açın.
**Zen Browser'ın kendisi** ile ilgili sorunlar için lütfen [resmi issue takipçisine](https://github.com/zen-browser/desktop/issues) başvurun.

---

<p align="center">
  <sub>Taşınabilir tarayıcı tutkunları için ❤️ ile yapılmıştır</sub>
</p>
