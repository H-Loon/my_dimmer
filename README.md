# Dimmer

A lightweight, modern Windows desktop utility that allows you to dim your screen(s) globally below their hardware minimums using the native Windows Magnification API. 

<p align="center">
  <!-- Replace the src URL below with your actual screenshot path/URL when uploaded (e.g. docs/screenshot.png) -->
  <img src="https://raw.githubusercontent.com/yourusername/my_dimmer/main/screenshot.png" alt="Dimmer Control Panel" width="450" />
</p>

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

## How to Use

1. **Launch the Application**: Run the executable `DimmerClone.exe`. The main control panel will open, and a custom icon will appear in your system tray (bottom-right of your taskbar).
2. **Set Brightness**: Drag the slider in the control panel to set the global brightness level:
   - Drag to the **left** to dim your screen (down to a safe software floor of 10%).
   - Drag to the **right** to increase brightness back to normal (100%).
3. **Minimize / Close to Tray**: 
   - Click the minimize button (`-`) to minimize the control panel.
   - Click the close button (`X`) to hide the control panel. The application will continue running in the background.
4. **Context Menu Controls**: Right-click the system tray icon to open the menu:
   - **Settings**: Restores and shows the control panel.
   - **Exit**: Restores screen brightness back to 100% and closes the application completely.
5. **Quick Restore**: Double-click the tray icon at any time to quickly open the control panel.

## Getting Started (For Developers)

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

2. Build and run the project locally (for development):
   ```bash
   dotnet build
   dotnet run --project DimmerClone
   ```

3. Publish as a **self-contained, single-file executable** (requires no .NET runtime installed on the target PC):
   ```bash
   cd DimmerClone
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
   ```
   The compiled executable `DimmerClone.exe` will be generated in `DimmerClone/bin/Release/net9.0-windows/win-x64/publish/`. Just copy that single file anywhere and run it!
