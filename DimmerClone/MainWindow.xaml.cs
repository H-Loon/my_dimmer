using System;
using System.Windows;
using System.Windows.Controls;
using Application = System.Windows.Application;

namespace DimmerClone
{
    public partial class MainWindow : Window
    {
        private bool _isInitializing = true;

        private void Log(string message)
        {
#if DEBUG
            try { System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log"), $"{DateTime.Now}: MainWindow: {message}\n"); } catch {}
#endif
        }

        public MainWindow()
        {
            Log("Constructor started");
            InitializeComponent();
            Log("InitializeComponent done");
            
            // Initial Value from Service
            int current = App.DimmerService.GetGlobalBrightness();
            GlobalSlider.Value = current;
            BrightnessValueText.Text = $"{current}%";

            _isInitializing = false;
        }

        private void GlobalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isInitializing) return;

            int val = (int)e.NewValue;
            if (BrightnessValueText != null)
                BrightnessValueText.Text = $"{val}%";
            
            App.DimmerService.SetGlobalBrightness(val);
            
            if (StatusText != null)
                StatusText.Text = $"Brightness set to {val}%";
        }

        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // This triggers Closing event which we intercept in App.xaml.cs to Hide instead.
            this.Close();
        }
    }
}
