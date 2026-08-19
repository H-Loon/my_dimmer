# Dimmer

A lightweight, modern Windows desktop utility that allows you to dim your screen(s) globally below their hardware minimums using the native Windows Magnification API. 

## Features

- 🖥️ **Global Fullscreen Dimming**: Uses the native Windows Magnification API (`MagSetFullscreenColorEffect`) to adjust the screen matrix color scale. This provides smooth, hardware-independent soft dimming that applies globally across all screens.
- 📺 **Full Web Video Compatibility**: Works flawlessly with hardware-accelerated video playback on the web (e.g., YouTube, Netflix, Twitch). Many overlay-based screen dimmers fail to dim or flicker when videos are played in fullscreen; since this utility uses the native Windows Magnification API to transform the desktop display pipe directly, it dims web videos perfectly.
- 🎨 **Premium Modern UI**: Built with a sleek, dark-themed WPF interface featuring custom control templates, a smooth slider, and responsive interactive elements.
- 📥 **System Tray Integration**: Runs quietly in the background. Minimizing or closing the control panel hides it to the Windows Notification Area (System Tray).
- 💾 **Settings Persistence**: Saves your brightness settings automatically to `%AppData%\DimmerClone\settings.json` and restores them when the application starts.
- 🔌 **Clean Restoration**: Automatically restores your monitor brightness to 100% and uninitializes the magnification effect on exit so your screen is never left dim.

## Tech Stack

- **Framework**: .NET 9.0 (WPF for GUI, Windows Forms for NotifyIcon)
- **API**: Windows Magnification API (P/Invoke to `magnification.dll`)
- **OS**: Windows 10 / 11

## Getting Started

### Prerequisites

- [Windows OS](https://www.microsoft.com/windows)
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Clone & Build

To run and build the application from source:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/my_dimmer.git
   cd my_dimmer
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run --project DimmerClone
   ```

## Usage

1. **Adjust Brightness**: Open the Dimmer Control Panel and drag the slider to dim your screen (range: 10% - 100%).
2. **Minimize to Tray**: Click the close (`X`) or minimize button to hide the panel. The application will continue running in the background.
3. **Tray Controls**: Double-click the tray icon to reopen the control panel, or right-click it to access settings or exit the app.
