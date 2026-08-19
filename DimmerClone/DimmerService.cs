using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DimmerClone
{
    public class DimmerService
    {
        // Magnification API P/Invokes
        [DllImport("Magnification.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool MagInitialize();

        [DllImport("Magnification.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool MagUninitialize();

        [DllImport("Magnification.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool MagSetFullscreenColorEffect(ref MAGCOLOREFFECT pEffect);

        [StructLayout(LayoutKind.Sequential)]
        public struct MAGCOLOREFFECT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public float[] transform;
        }

        private bool _isInitialized = false;
        private int _currentBrightness = 100;

        public DimmerService()
        {
            // Initialize Magnification API
            // Note: MagInitialize must be called once.
            _isInitialized = MagInitialize();
        }

        public void InitializeOverlays()
        {
            // For Magnification API, we just restore the last state or default
            var settings = SettingsManager.LoadSettings();
            int saved = 100;
            // Use "Global" key or just take the first one found if legacy
            if (settings.MonitorBrightness.ContainsKey("Global"))
            {
                saved = settings.MonitorBrightness["Global"];
            }
            else if (settings.MonitorBrightness.Count > 0)
            {
                // Migration: just take first value
                foreach(var val in settings.MonitorBrightness.Values) { saved = val; break; }
            }
            
            SetGlobalBrightness(saved);
        }

        public string SetGlobalBrightness(int brightness)
        {
            if (!_isInitialized) return "Failed: MagInitialize error";

            // Clamp
            if (brightness > 100) brightness = 100;
            if (brightness < 10) brightness = 10; // Safety floor

            _currentBrightness = brightness;

            float scale = brightness / 100.0f;

            MAGCOLOREFFECT effect = new MAGCOLOREFFECT();
            effect.transform = new float[25];
            
            // Identity matrix scaled by brightness
            // [ R 0 0 0 0 ]
            // [ 0 G 0 0 0 ]
            // [ 0 0 B 0 0 ]
            // [ 0 0 0 A 0 ]
            // [ 0 0 0 0 1 ]
            
            effect.transform[0] = scale;  // R
            effect.transform[6] = scale;  // G
            effect.transform[12] = scale; // B
            effect.transform[18] = 1.0f;  // A
            effect.transform[24] = 1.0f;  // w

            bool success = MagSetFullscreenColorEffect(ref effect);
            
            return success ? "Success" : "Failed";
        }

        public int GetGlobalBrightness()
        {
            return _currentBrightness;
        }

        public void SaveState()
        {
            var settings = new AppSettings();
            settings.MonitorBrightness = new Dictionary<string, int>();
            settings.MonitorBrightness["Global"] = _currentBrightness;
            SettingsManager.SaveSettings(settings);
            
            // Restore ramps on exit so screen isn't dim after close
            SetGlobalBrightness(100); 
            MagUninitialize();
        }
        
        // Stub for compatibility if needed, though we will remove usage
        public System.Collections.Generic.IEnumerable<System.Windows.Forms.Screen> GetScreens() 
        {
             return System.Windows.Forms.Screen.AllScreens;
        }
    }
}
