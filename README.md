<p align="center">
  <a href="README.md">🇬🇧 English</a> · <a href="README_TR.md">🇹🇷 Türkçe</a>
</p>

<p align="center">
  <img src="zen-browser.ico" alt="Zen Browser Portable" width="80" />
</p>

<h1 align="center">Zen Browser — Windows Portable</h1>

<p align="center">
  <strong>Carry your browser, your profiles, and your privacy — anywhere.</strong>
</p>

<p align="center">
  <a href="https://zen-browser.app"><img src="https://img.shields.io/badge/Zen%20Browser-Official%20Site-8338ec?style=for-the-badge&logo=firefox&logoColor=white" alt="Zen Browser"></a>
  <a href="https://github.com/zen-browser/desktop"><img src="https://img.shields.io/github/stars/zen-browser/desktop?style=for-the-badge&logo=github&label=Upstream%20Stars&color=ff6f61" alt="Stars"></a>
  <a href="https://github.com/zen-browser/desktop/blob/dev/LICENSE"><img src="https://img.shields.io/badge/License-MPL--2.0-blue?style=for-the-badge" alt="License"></a>
  <a href="#"><img src="https://img.shields.io/badge/Platform-Windows%2010%2F11-0078d4?style=for-the-badge&logo=windows&logoColor=white" alt="Platform"></a>
</p>

---

## 📖 About

This project is a **portable repackaging** of [Zen Browser](https://zen-browser.app) for Windows. It allows you to run Zen Browser directly from a USB drive, an external hard drive, or any folder on your computer — **without installation and without leaving traces on the host system**.

> [!NOTE]
> Zen Browser is an open-source, Firefox-based browser focused on productivity and a calmer internet experience. This portable build does **not** modify the original browser in any way; it simply wraps the official binaries with a portable launcher that redirects the profile directory.

### ✨ Key Features

| Feature | Description |
|---|---|
| 🔒 **Zero Installation** | No admin rights needed. No registry entries. No leftover files. |
| 💾 **Fully Self-Contained** | Browser binaries, profiles, bookmarks, extensions — everything lives inside one folder. |
| 👤 **Multi-Profile Support** | 5 pre-configured profile slots out of the box. Switch between work, personal, and other contexts effortlessly. |
| 🔄 **Sync-Friendly** | Move the entire folder to another machine, plug in, and continue exactly where you left off. |
| 🛡️ **Privacy by Default** | Runs with `-no-remote` flag, ensuring complete isolation from any locally installed Firefox/Zen instance. |

---

## 🚀 Quick Start

### Prerequisites

- **Operating System:** Windows 10 / 11 (x64)
- **Disk Space:** ~500 MB

### Setup

1. **Download the latest release** from the [📦 Releases](https://github.com/Metee01/Zen-Browser-Portable/releases) page and extract the ZIP to any location — your desktop, a USB drive, an external hard drive, anywhere you like.

2. **Download Zen Browser** from the [official download page](https://zen-browser.app/download):
   - Download the **Windows (x64)** installer or archive.
   - Extract the browser files and place them inside the `app/win/` directory so that `app/win/zen.exe` exists.

   > [!TIP]
   > You can also copy the contents of an existing Zen Browser installation from `C:\Program Files\Zen Browser\` directly into `app/win/`.

3. **Double-click `PortableZen.exe`** and start browsing!

That's it — your portable Zen Browser is ready to use. 🎉

> [!TIP]
> You can also run `zenbrowser.bat` directly if you prefer launching from the command line.

---

## 🔍 Security & False Positives

Some antivirus engines may flag `PortableZen.exe` as suspicious. **This is a false positive.** Here's why:

`PortableZen.exe` is nothing more than the following 3-line batch script compiled into an `.exe` using a Bat-to-Exe converter:

```batch
@echo off
set APP_DIR=%~dp0app
set DATA_DIR=%~dp0data
"%APP_DIR%\win\zen.exe" -profile "%DATA_DIR%\profile1" -no-remote
```

Bat-to-Exe tools work by embedding a script inside a wrapper that launches a hidden console process. This pattern — hiding a console window and executing commands — is the same technique used by some malware, which is why heuristic-based antivirus engines sometimes flag it. **The file does not contain any malicious code.**

🔬 **VirusTotal scan results:** [View full report](https://www.virustotal.com/gui/file/4f7f75cea134a04cc4cca38cf8dbb7824c8e0f20258537e93458fb5a58aa91cd?nocache=1)

> [!TIP]
> If you don't trust the `.exe` file, you can simply run `zenbrowser.bat` directly — it does the exact same thing. The `.exe` is provided purely for convenience (allows you to see Zen Browser in a UI through programs like PortableApps.com).

---

## 📁 Directory Structure

```
Zen-Browser-Portable/
│
├── app/
│   └── win/                    # Zen Browser binaries (not tracked by Git)
│       ├── zen.exe             # Main browser executable
│       └── ...                 # All other browser files
│
├── data/
│   ├── profile.ini             # Profile manager configuration
│   ├── profile1/               # Default profile (active)
│   ├── profile2/               # Additional profile slot
│   ├── profile3/               # Additional profile slot
│   ├── profile4/               # Additional profile slot
│   └── profile5/               # Additional profile slot
│
├── PortableZen.exe             # Portable launcher (compiled from zenbrowser.bat)
├── zenbrowser.bat              # Launcher batch script (source)
```

> [!IMPORTANT]
> You need to supply the Zen Browser binaries yourself by following the [Setup](#setup) instructions. This ensures you always use the latest version directly from the official source.

---

## ⚙️ How It Works

The portable launcher is powered by a simple but effective batch script:

```batch
@echo off
set APP_DIR=%~dp0app
set DATA_DIR=%~dp0data
"%APP_DIR%\win\zen.exe" -profile "%DATA_DIR%\profile1" -no-remote
```

**What each part does:**

| Component | Purpose |
|---|---|
| `%~dp0` | Resolves to the directory where the launcher lives, making all paths **relative** and truly portable. |
| `-profile` | Tells Zen/Firefox to use the specified directory for all profile data instead of the default `%APPDATA%` location. |
| `-no-remote` | Prevents the portable instance from connecting to any already-running Firefox or Zen Browser session on the host machine. |

The `PortableZen.exe` file is simply this batch script converted to a standalone `.exe` using a bat-to-exe compiler, providing a cleaner double-click experience with the custom Zen icon.

---

## 👤 Multi-Profile Management

This portable build comes with **5 pre-configured profile slots** defined in `data/profile.ini`:

| Profile | Path | Status |
|---|---|---|
| Profile1 | `data/profile1/` | ✅ Default |
| Profile2 | `data/profile2/` | Available |
| Profile3 | `data/profile3/` | Available |
| Profile4 | `data/profile4/` | Available |
| Profile5 | `data/profile5/` | Available |

### Switching Profiles

To launch a different profile, edit the `zenbrowser.bat` file and change `profile1` to your desired profile:

```batch
"%APP_DIR%\win\zen.exe" -profile "%DATA_DIR%\profile2" -no-remote
```

Alternatively, you can use Zen Browser's built-in **Profile Manager** by launching with the `-ProfileManager` flag:

```batch
"%APP_DIR%\win\zen.exe" -ProfileManager
```

---

## 🔄 Updating Zen Browser

To update to a newer version of Zen Browser:

1. Download the latest Zen Browser from [zen-browser.app/download](https://zen-browser.app/download).
2. Delete the contents of the `app/win/` folder.
3. Extract the new browser files into `app/win/`.
4. Your profiles and data remain untouched in the `data/` folder.

> [!CAUTION]
> Do **not** delete the `data/` directory when updating. It contains all your bookmarks, passwords, extensions, and browsing history.

---

## 🛡️ Security & Privacy Notes

- **Isolated Execution:** The `-no-remote` flag ensures this instance runs completely independently.
- **No System Traces:** Since profiles are stored within the portable folder, nothing is written to the Windows registry or `%APPDATA%`.
- **USB Safe:** You can run this from a USB drive and remove it without leaving any data on the host computer.
- **Your Responsibility:** While this setup is portable, always ensure you're downloading Zen Browser binaries from the [official source](https://zen-browser.app) to avoid tampered builds.

---

## 🙏 Attribution & Credits

This project is a **community portable repackaging** and is **not** an official product of the Zen Browser team.

<table>
  <tr>
    <td><strong>Zen Browser</strong></td>
    <td>
      A beautifully designed, Firefox-based, open-source browser.<br>
      🌐 <a href="https://zen-browser.app">zen-browser.app</a><br>
      💻 <a href="https://github.com/zen-browser/desktop">github.com/zen-browser/desktop</a><br>
      📄 <a href="https://docs.zen-browser.app">docs.zen-browser.app</a><br>
      📜 Licensed under <a href="https://github.com/zen-browser/desktop/blob/dev/LICENSE">MPL-2.0</a>
    </td>
  </tr>
  <tr>
    <td><strong>Mozilla Firefox</strong></td>
    <td>
      The engine behind Zen Browser.<br>
      🌐 <a href="https://www.mozilla.org/firefox/">mozilla.org/firefox</a>
    </td>
  </tr>
</table>

> If you enjoy Zen Browser, consider supporting the project:
> - ☕ [Ko-fi](https://ko-fi.com/zen_browser)
> - 🎁 [Patreon](https://patreon.com/zen_browser)

---

## 📜 License

This portable configuration (launcher scripts, directory structure, and documentation) is provided as-is for personal use.

Zen Browser itself is licensed under the **[Mozilla Public License 2.0 (MPL-2.0)](https://github.com/zen-browser/desktop/blob/dev/LICENSE)**. All rights to the Zen Browser source code and binaries belong to their respective authors and contributors.

---

## 🤝 Contributing

Contributions are welcome! If you have ideas for improving the portable setup:

1. **Fork** this repository
2. Create a **feature branch** (`git checkout -b feature/my-improvement`)
3. **Commit** your changes (`git commit -m "feat: add my improvement"`)
4. **Push** to the branch (`git push origin feature/my-improvement`)
5. Open a **Pull Request**

### Reporting Issues

If you encounter problems with the **portable launcher**, please open an issue on this repository.
For issues with **Zen Browser itself**, please report them on the [official issue tracker](https://github.com/zen-browser/desktop/issues).

---

<p align="center">
  <sub>Made with ❤️ for portable browser enthusiasts</sub>
</p>
