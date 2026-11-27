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
  <a href="LICENSE"><img src="https://img.shields.io/badge/License-MPL--2.0-blue?style=for-the-badge" alt="License"></a>
  <a href="#"><img src="https://img.shields.io/badge/Platform-Windows%2010%2F11-0078d4?style=for-the-badge&logo=windows&logoColor=white" alt="Platform"></a>
</p>

---

## 📖 About

This project is a **portable repackaging** of [Zen Browser](https://zen-browser.app) for Windows. It lets you run Zen Browser directly from a USB drive, an external hard drive, or any folder on your computer — **without installing it** and while keeping your browser data inside that folder.

> [!NOTE]
> Zen Browser is an open-source, Firefox-based browser focused on productivity and a calmer internet experience. This portable build does **not** modify the browser in any way; it uses the official binaries and a small launcher that points Zen at a profile folder inside the portable directory.

### ✨ Key Features

| Feature | Description |
|---|---|
| 🔒 **Zero Installation** | No installer, no admin rights. Extract and run. |
| 💾 **Self-Contained** | Browser files, profiles, bookmarks, extensions — everything lives inside one folder. |
| 👤 **Multiple Profiles** | 5 profile slots out of the box, plus any number of named profiles. Hold <kbd>Shift</kbd> while starting to pick one. |
| 🔗 **Opens Links and Files** | Works with links, `.html` files and "Open with". Starting it again while it is open simply opens a new window. |
| 🧹 **Cleans Up After Itself** | When the browser closes, `PortableZen.exe` removes the small registry values Zen wrote for this folder. |
| 🔄 **Auto-Built Releases** | A GitHub Actions workflow packages every new Zen release, usually within hours. |

---

## 🚀 Quick Start

### Prerequisites

- **Operating System:** Windows 10 / 11 (x64)
- **Disk Space:** ~500 MB

### Setup

1. Download the latest `Zen-Portable-v<version>.zip` from the [📦 Releases](https://github.com/Metee01/Zen-Browser-Portable/releases) page. It already contains the official Zen Browser files.
2. Extract the ZIP anywhere you like — your desktop, a USB drive, an external disk.
3. Double-click **`PortableZen.exe`** and start browsing! 🎉

> [!TIP]
> `zenbrowser.bat` is a plain-text alternative to the `.exe` if you prefer a launcher you can read and edit. It does not perform the registry cleanup described below.

<details>
<summary>Building from the repository instead of a release</summary>

1. Clone or download this repository.
2. Put the Zen Browser files into `app/win/` so that `app/win/zen.exe` exists. You can extract them from the official installer with 7-Zip (the files are in its `core` folder) or copy them from `C:\Program Files\Zen Browser\`.
3. Compile the launcher (no SDK needed, the compiler ships with Windows):

   ```bat
   C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /nologo /target:winexe /win32icon:zen-browser.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:PortableZen.exe src\launcher.cs
   ```

   Or just use `zenbrowser.bat`.
</details>

---

## 👤 Profiles

The ZIP comes with five profile folders, `data/profile1` … `data/profile5`. `profile1` is used by default.

| How | What happens |
|---|---|
| Double-click `PortableZen.exe` | Opens `data/profile1`. |
| Hold <kbd>Shift</kbd> while starting | A small window lists every folder in `data/`; pick one. |
| `PortableZen.exe --profile 2` | Opens `data/profile2`. Handy for a desktop shortcut. |
| `PortableZen.exe --profile work` | Opens `data/work`, creating it (with the portable defaults) if it does not exist. |

Different profiles can be open at the same time. In `zenbrowser.bat`, change `profile1` on the `set "PROFILE=profile1"` line.

> [!WARNING]
> Do not use Zen's `-ProfileManager`. It stores its profile list in `%APPDATA%`, outside the portable folder.

---

## 🔄 Updating

### From inside Zen

When a new version is out, Zen shows **Update available**. Click **Download**, then **Update and restart** when it is ready. Zen updates the files in `app\win` itself; your `data/` folder is not touched.

- Zen asks before downloading, so nothing is downloaded on a computer you only use briefly.
- The download is stored on the computer you are using (under `C:\ProgramData`), not on the USB drive. Finish the update with **Update and restart** before unplugging; otherwise just download it again on the next computer.
- `PortableZen.exe` leaves a pending update in place and removes Zen's update folder once the update is done.
- `PortableZen.exe` and `zenbrowser.bat` themselves are not updated this way. Grab them from a new release when you want the latest launcher.

### With a new release ZIP

1. Close Zen Browser.
2. Download the newest ZIP from [Releases](https://github.com/Metee01/Zen-Browser-Portable/releases).
3. In your portable folder, delete the **`app\win`** folder.
4. From the new ZIP, copy the **`app`** folder and **`PortableZen.exe`** into your portable folder.

Your bookmarks, passwords, extensions and history live in `data/` and are not touched. Copies from before November 2026 also have a `user.js` in each profile folder; it does no harm and can be deleted, since the same defaults now come from `policies.json`.

> [!CAUTION]
> Never delete the `data/` folder when updating — it holds all of your browser data.

---

## ⚙️ How It Works

`PortableZen.exe` is a small C# program ([`src/launcher.cs`](src/launcher.cs)) compiled with the C# compiler that ships with Windows. When you start it, it:

1. Finds `app\win\zen.exe` next to itself (all paths are relative, so the folder can move).
2. Starts Zen with `-profile data\<profile>`, which keeps all browser data inside the portable folder.
3. Passes any links or files you opened it with straight to Zen. If this portable Zen is already open, Zen hands them to the running window instead of starting a second copy, and it never connects to a separately installed Zen.
4. Waits in the background until the browser closes, then removes what Zen left on the computer for this folder (see below).

Portable defaults are set in [`app/win/distribution/policies.json`](distribution/policies.json), Zen's standard settings file for the whole installation, so they apply to every profile, including new ones. It turns off telemetry, studies, the default-browser prompt and agent, asks before downloading Zen updates, and keeps the cache in memory instead of on the disk to spare USB drives. The cache settings are only defaults: you can still change them in Zen's settings and your choice is kept. Open `about:policies` in Zen to see them.

---

## 🛡️ Privacy Notes

- **Browser data:** Profiles, history, passwords, extensions and cache stay inside `data/`. In our testing nothing was written to `%APPDATA%\zen` or `%LOCALAPPDATA%\zen`, and the crash reporter (which would save reports in `%APPDATA%`) is turned off by both launchers.
- **What Zen still leaves behind while it runs** (like Firefox, it contains no browsing data):
  - a few small values under `HKCU\Software\Mozilla\Zen`, such as the window size and the last launch time;
  - a small update folder under `C:\ProgramData\Mozilla-1de4eec8-1241-4177-a864-e594e8d1fb38\updates\`, named after the folder Zen runs from.

  `PortableZen.exe` removes both for its own folder when the browser closes; an installed Zen is left alone. `zenbrowser.bat` does not clean them.
- **Windows itself** may still remember that a program ran (for example in recent-files lists or prefetch data). That is outside of what a portable app can control.
- **Download from the source:** Use the official Releases page of this repository, which is built automatically from the official Zen Browser installer. Each release lists the SHA256 of the ZIP and of the official installer it was built from; check yours with `(Get-FileHash .\Zen-Portable-v<version>.zip).Hash` in PowerShell.

---

## 🔍 Antivirus Warnings

Some antivirus engines or Windows SmartScreen may warn about `PortableZen.exe` because it is small, new and not code-signed. **It does not contain any malicious code.** The full source is in [`src/launcher.cs`](src/launcher.cs) and the release workflow compiles it from that file:

```
src\launcher.cs  →  csc.exe /target:winexe  →  PortableZen.exe
```

If you prefer, run `zenbrowser.bat` instead.

---

## 📁 Directory Structure

```
Zen-Browser-Portable/
├── app/
│   └── win/              # Zen Browser files (included in the release ZIP)
│       ├── zen.exe
│       ├── distribution/
│       │   └── policies.json   # Portable defaults (copied from /distribution)
│       └── ...
├── data/
│   ├── profile1/         # Default profile
│   ├── profile2/ … profile5/
│   └── <name>/           # Any profile you create with --profile <name>
├── distribution/
│   └── policies.json     # Source of the portable defaults
├── src/
│   └── launcher.cs       # Source of PortableZen.exe
├── PortableZen.exe       # Launcher (built by the release workflow)
└── zenbrowser.bat        # Plain-text alternative launcher
```

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

The files in this repository (launcher, scripts, configuration and documentation) are licensed under the **[Mozilla Public License 2.0](LICENSE)**.

Zen Browser itself is also licensed under **[MPL-2.0](https://github.com/zen-browser/desktop/blob/dev/LICENSE)**. All rights to the Zen Browser source code and binaries belong to their respective authors and contributors.

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
