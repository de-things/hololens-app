using DT.Source.Configs;
using DT.Source.Graphics;
using DT.Source.Windows;
using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using static DT.Source.Managers.LayoutManager;

namespace DT
{
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        bool IsWelcomeScreenClosing {  get; set; }
        public static string Log { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            // Generate devices.json to store devices data locally
            Devices.CreateDevicesDataIsNotExist();

            Button MainButton = new();

            // Set button properties
            MainButton.SvgPath = Svg.Logo;
            MainButton.IconSize = new(200, 200);
            MainButton.OnPointerReleased = (e, a) =>
            {
                new LampDeviceWindow().Initialize();
                Log = "";
            };
            // Initialize button
            MainButton.Initialize();

            Root.Children.Add(MainButton.GetRoot());

            ApplicationView.GetForCurrentView().Title = "Home";

            Log += $"Welcome to de:things showcase for Hololens 2.\n\nLeft button creates existing UI components for IoT infrastructure and begins logging in this block.";

            WelcomeLogo.Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), Svg.Logo);
            WelcomeTitle.Text = "decentralized things";
            WelcomeHint.Text = "tap to continue";

            PlayLoginSound();
            Animation.WelcomeApproach(WelcomeLogo, WelcomeLogoRotator, WelcomeLogoBrush, WelcomeTitle, WelcomeHint);

            WelcomeScreen.PointerReleased += (s, a) =>
            {
                if (!IsWelcomeScreenClosing)
                {
                    Animation.Disolve(WelcomeScreen);
                    IsWelcomeScreenClosing = true;
                }
            };

            Update();
        }
        async void Update()
        {
            while (true)
            {
                Debug.Text = Log;
                await Task.Delay(500);
            }
        }
        async void PlayLoginSound()
        {
            var element = new Windows.UI.Xaml.Controls.MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Sounds");
            var file = await folder.GetFileAsync(@"Login.wav");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            element.SetSource(stream, "");
            element.Play();
        }
    }
}
