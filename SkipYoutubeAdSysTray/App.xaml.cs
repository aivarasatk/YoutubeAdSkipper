using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Image = Google.Cloud.Vision.V1.Image;
using System.Windows;
using NHotkey.Wpf;
using System.Windows.Input;
using NHotkey;
using SkipYoutubeAdSysTray.Helpers;

namespace SkipYoutubeAdSysTray
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notifyIcon;

        private readonly GoogleApi _apiClient;
        private readonly MouseController _mouseController;

        private const string ScreenshotImageUri = "<full path to a .png image>";

        private const int Screen1Width = 1680;//my setup uses two screens
        private const int Screen2Width = 1920;
        private const int MaxScreenHeight = 1080;
        public App()
        {
            _apiClient = new GoogleApi();
            _mouseController = new MouseController();
            SetupSkipAdHotKey();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        private void SetupSkipAdHotKey()
        {
            HotkeyManager.Current.AddOrReplace("SkipAd", Key.S, ModifierKeys.Alt, OnSkipAd);
        }

        private async void OnSkipAd(object sender, HotkeyEventArgs e)
        {
            try
            {
                TakeAndSaveScreenshot(ScreenshotImageUri);

                var skipAdLocation = await _apiClient.SkipAdImageLocationAsync(ScreenshotImageUri);

                if (skipAdLocation is null)
                    return;//do not proceed with clicking
                else
                    _mouseController.SimulateClick(new System.Drawing.Point(DesktopLocationX(skipAdLocation.X), skipAdLocation.Y));
            }
            catch { }         
        }

        private int DesktopLocationX(int skipAdLocationX) => skipAdLocationX - Screen1Width;

        private void TakeAndSaveScreenshot(string imageLocation)
        {
            var captureBmp = new Bitmap(Screen1Width + Screen2Width, MaxScreenHeight, PixelFormat.Format32bppArgb);
            using var captureGraphic = Graphics.FromImage(captureBmp);

            captureGraphic.CopyFromScreen(-Screen1Width, sourceY: 0, destinationX: 0, destinationY: 0, captureBmp.Size);
            captureBmp.Save(imageLocation, ImageFormat.Jpeg);
        }
    }
}
