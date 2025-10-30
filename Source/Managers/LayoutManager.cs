using DT.Source.Globals;
using DT.Source.Graphics;
using System;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.System;

namespace DT.Source.Managers;
class LayoutManager
{
    public struct IconSize
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IconSize(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// DT Button class. Initialize it via Initialize() and add inside some window as a child.
    /// </summary>
    public class Button
    {
        StackPanel  Body        { get; set; }
        Border      Root        { get; set; }
        Path        Icon        { get; set; }

        /// <summary>
        /// Becomes true after pointer left button to prevent "after exited clicks".
        /// </summary>
        bool IsExited { get; set; }

        public IconSize IconSize { get; set; }

        /// <summary>
        /// Root node of the element to get it after Initialze() and draw somewhere.
        /// </summary>
        public Border GetRoot()
        {
            return Root;
        }
        /// <summary>
        /// Icon node of the element.
        /// </summary>
        public Path GetIcon()
        {
            return Icon;
        }

        public PointerEventHandler OnPointerReleased { get; set; }

        public RoutedEventHandler OnCreated { get; set; }

        /// <summary>
        /// Icon svg path received from  DT.Source.Graphics.Svg class
        /// </summary>
        public string SvgPath { get; set; }

        public bool IsActive { get; set; }

        public void Initialize()
        {
            Root = new Border()
            {
                Margin          = new Thickness(10),
                CornerRadius    = new CornerRadius(10),

                Background = Colors.SoftGrey
            };

            Body = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment   = VerticalAlignment.Center,
            };

            Icon = new Path
            {
                Width   = IconSize.X,
                Height  = IconSize.Y,
                Data    = SvgPath.ToGeometry(),
                Fill    = Colors.Grey
            };

            Root.Loaded += OnCreated;

            Root.PointerPressed += (s, a) =>
            {
                Animation.ButtonPressed(Root);
                IsExited = false;
            };
            Root.PointerReleased += (s, a) =>
            {
                if (!IsExited)
                {
                    Animation.ButtonReleased(Root);
                    OnPointerReleased(s, a);
                }
            };
            Root.PointerExited += (s, a) =>
            {
                Animation.ButtonReleased(Root);
                IsExited = true;
            };

            Body.Children.Add(Icon);
            Root.Child = Body;
        }
        public void Update()
        {
            if (!IsActive)
            {
                Icon.Fill       = Colors.Grey;
                Root.Background = Colors.SoftGrey;
            }
            else
            {
                Icon.Fill       = Colors.SoftYellow;
                Root.Background = Colors.Yellow;
            }
        }
    }
    /// <summary>
    /// DT Window class. Show it with Show() and use it's Root node to add children inside.
    /// </summary>
    public class Window
    {
        public string Title { get; set; }

        AppWindow ThisWindow { get; set; }

        Grid Root { get; set; }
        Grid Body { get; set; }

        PointerEventHandler OnPopupPointerReleased { get; set; }

        Border PopUpRoot  { get; set; }
        Grid PopUpRootGrid      { get; set; }

        Border PopUpInputBorder         { get; set; }
        StackPanel PopUpInputStackPanel { get; set; }

        TextBlock PopUpInputContent { get; set; }
        TextBox PopUpTextBox        { get; set; }

        Button PopUpButton      { get; set; }

        Border LoadingRoot { get; set; }
        StackPanel LoadingRootStackPanel { get; set; }
        Path LoadingCircle { get; set; }
        SolidColorBrush LoadingCircleColor { get; set; }
        RotateTransform LoadingCircleRotate { get; set; }

        /// <summary>
        /// Returns current PopUp input text.
        /// </summary>
        /// <returns></returns>
        public string GetPopUpInput()
        {
            return PopUpTextBox.Text;
        }
        /// <summary>
        /// Shows PopUp with specified input text inside.
        /// </summary>
        /// <returns></returns>
        public void ShowPopUp(string content, PointerEventHandler onCloseEvent)
        {
            PopUpTextBox.Text = content;
            PopUpInputContent.Text = content;
            OnPopupPointerReleased = onCloseEvent;

            PopUpRoot.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Adds a child to the window.
        /// </summary>
        /// <param name="element"></param>
        public void Add(UIElement element)
        {
            Body.Children.Add(element);
            Body.ColumnDefinitions.Add(new());
            element.SetValue(Grid.ColumnProperty, Body.Children.Count - 1);
        }
        /// <summary>
        /// Initializes and opens the window.
        /// </summary>
        public async Task Initialize()
        {
            ThisWindow = await AppWindow.TryCreateAsync();
            ThisWindow.Title = Title;

            Root = new();

            Root.ColumnDefinitions.Add(new());

            Body = new() 
            {
                Padding = new Thickness(10)
            };

            Root.Children.Add(Body);

            // Init window's popup
            PopUpInitialize();

            // Init window's loading screen
            LoadingInitialize();

            Root.Children.Add(PopUpRoot);
            Root.Children.Add(LoadingRoot);

            var PageElement = new Page
            {
                Background = Colors.Black,
                Content = Root
            };

            ElementCompositionPreview.SetAppWindowContent(ThisWindow, PageElement);
            await ThisWindow.TryShowAsync();
        }
        public void ShowLoading()
        {
            Animation.Show(LoadingRoot);
            Animation.ColorRamp(LoadingCircleColor);
        }
        public void HideLoading()
        {
            Animation.Disolve(LoadingRoot);
        }
        void LoadingInitialize()
        {
            LoadingCircleRotate = new()
            {
                CenterX = 70,
                CenterY = 70
            };
            LoadingCircleColor = Colors.White;

            LoadingCircle = new()
            {
                Fill = LoadingCircleColor,
                Data = Svg.Logo.ToGeometry(),
                Stretch = Stretch.Fill,
                Width = 140,
                Height = 140,
                RenderTransform = LoadingCircleRotate
            };

            Animation.ColorRamp(LoadingCircleColor);
            Animation.Rotate(LoadingCircleRotate);

            LoadingRoot = new()
            {
                Background = Colors.Black
            };

            LoadingRootStackPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            LoadingRootStackPanel.Children.Add(LoadingCircle);
            LoadingRoot.Child = LoadingRootStackPanel;
        }

        void PopUpInitialize()
        {
            PopUpRoot = new()
            {
                Background = Colors.Black,
                Margin = new Thickness(10),
                Visibility = Visibility.Collapsed // hide popup until invoked
            };
            PopUpRootGrid = new();

            PopUpRootGrid.ColumnDefinitions.Add(new());
            PopUpRootGrid.ColumnDefinitions.Add(new());

            PopUpRoot.Child = PopUpRootGrid;

            // Confirmation button
            PopUpButton = new()
            {
                SvgPath = Svg.Logo,
                IconSize = new(200, 200),
                OnPointerReleased = (s, a) => { OnPopupPointerReleased(s, a); PopUpRoot.Visibility = Visibility.Collapsed; }
            };
            PopUpButton.Initialize();


            PopUpInputBorder = new()
            {
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10),
                Padding = new Thickness(20),
                Background = Colors.SoftGreen,
                AllowFocusOnInteraction = false,
            };

            PopUpInputBorder.GotFocus += (s, a) =>
            {
                PopUpTextBox.Focus(FocusState.Programmatic);
                PopUpTextBox.SelectionStart = PopUpTextBox.Text.Length;
                PopUpTextBox.SelectionLength = 0;

                PopUpInputBorder.Background = Colors.Green;
            };
            PopUpInputBorder.LostFocus += (s, a) =>
            {
                PopUpInputBorder.Background = Colors.SoftGreen;
            };

            PopUpInputBorder.PointerReleased += (s, a) =>
            {
                PopUpTextBox.Focus(FocusState.Programmatic);
                PopUpTextBox.SelectionStart = PopUpTextBox.Text.Length;
                PopUpTextBox.SelectionLength = 0;
            };

            // Move text fields here
            PopUpInputStackPanel = new();

            // Hide this text box and only invoke it focus to handle use input
            PopUpTextBox = new()
            {
                Text = "some input text",
                Width = 1,
                Height = 1
            };

            // This text clones content from PopUpTextBox field
            PopUpInputContent = new()
            {
                Text = "...",
                Foreground = Colors.White,
                FontFamily = new("/Assets/Fonts/mononoki.otf#mononoki"),
                FontSize = 18
            };

            PopUpTextBox.TextChanged += (s, a) =>
            {
                PopUpInputContent.Text = PopUpTextBox.Text;
            };
            PopUpTextBox.KeyUp += (s, a) =>
            {
                if (a.Key == VirtualKey.Enter)
                {
                    OnPopupPointerReleased(s, default); 
                    PopUpRoot.Visibility = Visibility.Collapsed;
                }
            };

            PopUpInputStackPanel.Children.Add(PopUpTextBox);
            PopUpInputStackPanel.Children.Add(PopUpInputContent);

            PopUpInputBorder.Child = PopUpInputStackPanel;

            PopUpRootGrid.Children.Add(PopUpInputBorder);
            PopUpRootGrid.Children.Add(PopUpButton.GetRoot());

            PopUpButton.GetRoot().SetValue(Grid.ColumnProperty, 0);
            PopUpInputBorder.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
