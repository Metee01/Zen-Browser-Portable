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
  <a href="https://github.com/zen-browser/desktop/blob/dev/LICENSE"><img src="https://img.shields.io/badge/Lisans-MPL--2.0-blue?style=for-the-badge" alt="Lisans"></a>
  <a href="#"><img src="https://img.shields.io/badge/Platform-Windows%2010%2F11-0078d4?style=for-the-badge&logo=windows&logoColor=white" alt="Platform"></a>
</p>

---

## 📖 Hakkında

Bu proje, [Zen Browser](https://zen-browser.app)'ın Windows için **taşınabilir (portable) olarak yapılandırılmış** halidir. Zen Browser'ı USB bellek, harici disk veya bilgisayarınızdaki herhangi bir klasörden — **kurulum yapmadan ve sistemde iz bırakmadan** — doğrudan çalıştırmanızı sağlar.

> [!NOTE]
> Zen Browser; verimlilik odaklı, açık kaynaklı ve Firefox tabanlı bir tarayıcıdır. Bu taşınabilir yapılandırma orijinal tarayıcıda **hiçbir değişiklik yapmaz**; yalnızca resmi dosyaları, profil dizinini yeniden yönlendiren taşınabilir bir başlatıcı ile sarar.

### ✨ Temel Özellikler

| Özellik | Açıklama |
|---|---|
| 🔒 **Sıfır Kurulum** | Yönetici izni gerekmez. Kayıt defteri girdisi oluşturmaz. Artık dosya bırakmaz. |
| 💾 **Tamamen Bağımsız** | Tarayıcı dosyaları, profiller, yer imleri, eklentiler — her şey tek bir klasörde yaşar. |
| 👤 **Çoklu Profil Desteği** | Kutudan çıktığı gibi 5 adet önceden yapılandırılmış profil yuvası. İş, kişisel ve diğer bağlamlar arasında kolayca geçiş yapın. |
| 🔄 **Taşınabilir** | Tüm klasörü başka bir bilgisayara taşıyın, takın ve kaldığınız yerden devam edin. |
| 🛡️ **Varsayılan Gizlilik** | `-no-remote` bayrağı ile çalışarak, sistemde kurulu olan herhangi bir Firefox/Zen örneğinden tamamen izole edilir. |

---

## 🚀 Hızlı Başlangıç

### Gereksinimler

- **İşletim Sistemi:** Windows 10 / 11 (x64)
- **Disk Alanı:** ~500 MB

### Kurulum

1. **En son sürümü indirin:** [📦 Releases](https://github.com/Metee01/Zen-Browser-Portable/releases) sayfasından ZIP dosyasını indirin ve istediğiniz bir konuma çıkartın — masaüstü, USB bellek, harici disk, nereye isterseniz.

2. **Zen Browser'ı indirin:** [Resmi indirme sayfasından](https://zen-browser.app/download) indirin:
   - **Windows (x64)** yükleyicisini veya arşivini indirin.
   - Tarayıcı dosyalarını çıkartın ve `app/win/` dizinine yerleştirin (`app/win/zen.exe` dosyasının mevcut olduğundan emin olun).

   > [!TIP]
   > Bilgisayarınızda zaten kurulu bir Zen Browser varsa, `C:\Program Files\Zen Browser\` içindeki dosyaları doğrudan `app/win/` klasörüne kopyalayabilirsiniz.

3. **`PortableZen.exe`** dosyasına çift tıklayın ve gezinmeye başlayın!

Hepsi bu — taşınabilir Zen Browser'ınız kullanıma hazır. 🎉

> [!TIP]
> Komut satırından başlatmayı tercih ederseniz `zenbrowser.bat` dosyasını da doğrudan çalıştırabilirsiniz.

---

## 🔍 Güvenlik ve Yanlış Pozitifler

Bazı antivirüs motorları `PortableZen.exe` dosyasını şüpheli olarak işaretleyebilir. **Bu bir yanlış pozitiftir (false positive).** İşte nedeni:

`PortableZen.exe`, Windows ile birlikte gelen .NET C# derleyicisi (`csc.exe`) ile derlenmiş minimal bir C# sarmalayıcısıdır. Yalnızca `zen.exe` dosyasını `app/win/` dizininde bulup doğru profil yoluyla başlatır — harici bağımlılık yok, gizli davranış yok.

```
src\launcher.cs  →  csc.exe /target:winexe  →  PortableZen.exe
```

Konsol penceresini gizleme ve komut çalıştırma mantığı, bazı zararlı yazılımların da kullandığı bir teknik olduğu için sezgisel (heuristic) tabanlı antivirüs motorları bu dosyayı zaman zaman işaretler. **Dosya herhangi bir zararlı kod içermemektedir.**

> [!TIP]
> `.exe` dosyasına güvenmiyorsanız, doğrudan `zenbrowser.bat` dosyasını çalıştırabilirsiniz — tamamen aynı işi yapar. `.exe` yalnızca kolaylık sağlamak için sunulmuştur (PortableApps.com gibi programlar aracılığıyla Zen Browser'ı bir arayüzde görmenizi sağlar).

---

## 📁 Dizin Yapısı

```
Zen-Browser-Portable/
│
├── app/
│   └── win/                    # Zen Browser dosyaları (Git tarafından takip edilmez)
│       ├── zen.exe             # Ana tarayıcı çalıştırılabilir dosyası
│       └── ...                 # Diğer tüm tarayıcı dosyaları
│
├── data/
│   ├── profile.ini             # Profil yöneticisi yapılandırması
│   ├── profile1/               # Varsayılan profil (aktif)
│   ├── profile2/               # Ek profil yuvası
│   ├── profile3/               # Ek profil yuvası
│   ├── profile4/               # Ek profil yuvası
│   └── profile5/               # Ek profil yuvası
│
├── src/
│   └── launcher.cs             # C# sarmalayıcı kaynağı (CI'da PortableZen.exe olarak derlenir)
│
├── PortableZen.exe             # Taşınabilir başlatıcı (CI'da üretilir, Git tarafından takip edilmez)
├── zenbrowser.bat              # Başlatıcı batch betiği (kaynak)
```

> [!IMPORTANT]
> Tarayıcı dosyalarını [Kurulum](#kurulum) talimatlarını takip ederek kendiniz eklemeniz gerekmektedir. Böylece her zaman doğrudan resmi kaynaktan en güncel sürümü kullanırsınız.

---

## ⚙️ Nasıl Çalışır?

Taşınabilir başlatıcı, Zen Browser'ı taşınabilir bir profil ile başlatan minimal bir C# sarmalayıcısı tarafından çalıştırılır:

```csharp
string dir = Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().Location);
string zenExe = Path.Combine(dir, "app", "win", "zen.exe");
string profile = Path.Combine(dir, "data", "profile1");
Process.Start(new ProcessStartInfo {
    FileName = zenExe,
    Arguments = "-profile \"" + profile + "\" -no-remote",
    UseShellExecute = false,
    CreateNoWindow = true
});
```

**Her parçanın görevi:**

| Bileşen | Amaç |
|---|---|
| `GetExecutingAssembly()` | Başlatıcının bulunduğu dizine çözümlenir ve tüm yolları **göreceli** hale getirerek gerçek taşınabilirlik sağlar. |
| `-profile` | Zen/Firefox'a, tüm profil verileri için varsayılan `%APPDATA%` konumu yerine belirtilen dizini kullanmasını söyler. |
| `-no-remote` | Taşınabilir örneğin, ana makinede zaten çalışan herhangi bir Firefox veya Zen Browser oturumuna bağlanmasını önler. |
| `CreateNoWindow = true` | Konsol penceresini gizleyerek daha temiz bir deneyim sağlar. |

`PortableZen.exe`, CI sırasında `src/launcher.cs` kaynak dosyasından yerleşik Windows C# derleyicisi (`csc.exe`) kullanılarak derlenir ve özel Zen ikonuyla birlikte temiz bir çift tıklama deneyimi sunar. Alternatif olarak `zenbrowser.bat` dosyasını da doğrudan çalıştırabilirsiniz — aynı sonucu verir.

---

## 👤 Çoklu Profil Yönetimi

Bu taşınabilir yapılandırma, `data/profile.ini` dosyasında tanımlanmış **5 adet önceden yapılandırılmış profil yuvası** ile birlikte gelir:

| Profil | Yol | Durum |
|---|---|---|
| Profile1 | `data/profile1/` | ✅ Varsayılan |
| Profile2 | `data/profile2/` | Kullanılabilir |
| Profile3 | `data/profile3/` | Kullanılabilir |
| Profile4 | `data/profile4/` | Kullanılabilir |
| Profile5 | `data/profile5/` | Kullanılabilir |

### Profil Değiştirme

Farklı bir profil ile başlatmak için `zenbrowser.bat` dosyasını düzenleyin ve `profile1` yerine istediğiniz profili yazın:

```batch
"%APP_DIR%\win\zen.exe" -profile "%DATA_DIR%\profile2" -no-remote
```

Alternatif olarak, `-ProfileManager` bayrağı ile Zen Browser'ın yerleşik **Profil Yöneticisi**'ni kullanabilirsiniz:

```batch
"%APP_DIR%\win\zen.exe" -ProfileManager
```

---

## 🔄 Zen Browser'ı Güncelleme

Zen Browser'ı daha yeni bir sürüme güncellemek için:

1. En son Zen Browser'ı [zen-browser.app/download](https://zen-browser.app/download) adresinden indirin.
2. `app/win/` klasörünün içeriğini silin.
3. Yeni tarayıcı dosyalarını `app/win/` klasörüne çıkartın.
4. Profilleriniz ve verileriniz `data/` klasöründe dokunulmadan kalır.

> [!CAUTION]
> Güncelleme yaparken `data/` dizinini **silmeyin**. Bu dizin tüm yer imlerinizi, şifrelerinizi, eklentilerinizi ve gezinme geçmişinizi içerir.

---

## 🛡️ Güvenlik ve Gizlilik Notları

- **İzole Çalışma:** `-no-remote` bayrağı, bu örneğin tamamen bağımsız çalışmasını sağlar.
- **Sistem İzi Bırakmaz:** Profiller taşınabilir klasör içinde saklandığından, Windows kayıt defterine veya `%APPDATA%` dizinine hiçbir şey yazılmaz.
- **USB Güvenli:** Bu yapılandırmayı bir USB bellekten çalıştırabilir ve ana bilgisayarda herhangi bir veri bırakmadan çıkarabilirsiniz.
- **Sizin Sorumluluğunuz:** Bu yapılandırma taşınabilir olsa da, değiştirilmiş sürümlerden kaçınmak için Zen Browser dosyalarını her zaman [resmi kaynaktan](https://zen-browser.app) indirdiğinizden emin olun.

---

## 🙏 Atıf ve Teşekkürler

Bu proje bir **topluluk taşınabilir yapılandırmasıdır** ve Zen Browser ekibinin **resmi bir ürünü değildir**.

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

Bu taşınabilir yapılandırma (başlatıcı betikleri, dizin yapısı ve dokümantasyon) kişisel kullanım için olduğu gibi sunulmaktadır.

Zen Browser'ın kendisi **[Mozilla Public License 2.0 (MPL-2.0)](https://github.com/zen-browser/desktop/blob/dev/LICENSE)** lisansı altındadır. Zen Browser kaynak koduna ve dosyalarına ait tüm haklar ilgili yazar ve katkıda bulunanlara aittir.

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
