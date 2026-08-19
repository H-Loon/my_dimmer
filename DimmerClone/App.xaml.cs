using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using Application = System.Windows.Application;

namespace DimmerClone
{
    public partial class App : Application
    {
        public static DimmerService DimmerService { get; private set; } = new DimmerService();
        private NotifyIcon? _notifyIcon;
        private bool _isExiting = false;
        private MainWindow? _mainWindow;

        private void Log(string message)
        {
            try { System.IO.File.AppendAllText(@"c:\Projects\my_dimmer\DimmerClone\debug.log", $"{DateTime.Now}: {message}\n"); } catch {}
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Log("Application_Startup started");
            try
            {
                // Shutdown when MainWindow closes
                // Keep running when window closes
                ShutdownMode = ShutdownMode.OnExplicitShutdown;

                _notifyIcon = new NotifyIcon();
                
                try 
                {
                    // Convert content PNG to Icon for the Tray
                    string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icon.png");
                    if (System.IO.File.Exists(iconPath))
                    {
                        using (var bitmap = new Bitmap(iconPath))
                        {
                            // GetHicon creates an unmanaged handle, we must use it carefully. 
                            // Icon.FromHandle creates a managed wrapper.
                            _notifyIcon.Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                        }
                    }
                    else
                    {
                        _notifyIcon.Icon = SystemIcons.Application;
                    }
                }
                catch (Exception iconEx)
                {
                    Log($"Icon error: {iconEx.Message}");
                    _notifyIcon.Icon = SystemIcons.Application; 
                }

                _notifyIcon.Visible = true;
                _notifyIcon.Text = "Dimmer Clone";
                _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
                
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Settings", null, (s, args) => ShowMainWindow());
                contextMenu.Items.Add("-");
                contextMenu.Items.Add("Exit", null, (s, args) => {
                    _isExiting = true;
                    Shutdown();
                });
                _notifyIcon.ContextMenuStrip = contextMenu;
                Log("NotifyIcon setup done");

                DimmerService.InitializeOverlays();
                Log("Overlays initialized");
                
                // Open settings on start so user knows it's running
                ShowMainWindow();
                Log("ShowMainWindow called");
            }
            catch (Exception ex)
            {
                Log($"Startup fatal error: {ex}");
                try { System.Windows.MessageBox.Show($"Startup Error: {ex.Message}\nCheck debug.log for details.", "Dimmer Error"); } catch {}
                Shutdown();
            }
        }

        private void ShowMainWindow()
        {
            Log("Enter ShowMainWindow");
            if (_mainWindow == null)
            {
                Log("Creating new MainWindow");
                _mainWindow = new MainWindow();
                _mainWindow.Closing += MainWindow_Closing;
            }
            
            // Force visibility and state
            _mainWindow.Show();
            Log("MainWindow.Show() called");

            if (_mainWindow.Visibility != Visibility.Visible)
            {
                _mainWindow.Visibility = Visibility.Visible;
                Log("Forced Visibility=Visible");
            }

            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                _mainWindow.WindowState = WindowState.Normal;
                Log("Restored from Minimized");
            }
            
            _mainWindow.Activate();
            _mainWindow.Topmost = true;  // Briefly toggle topmost to force to front if stuck behind
            _mainWindow.Topmost = false;
            _mainWindow.Focus();
            Log("Window activation steps done");
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExiting)
            {
                e.Cancel = true; // Prevent closing
                _mainWindow?.Hide(); // Hide instead
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DimmerService.SaveState();
            _notifyIcon?.Dispose();
            base.OnExit(e);
        }
    }
}
