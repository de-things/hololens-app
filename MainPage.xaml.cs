using DT.Source.Graphics;
using DT.Source.Windows;
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


            Button MainButton = new();

            // Set button properties
            MainButton.SvgPath = Svg.Logo;
            MainButton.IconSize = new(200, 200);
            MainButton.OnPointerReleased = (e, a) =>
            {
                new ExampleLampWindow().Initialize();
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
    }
}
