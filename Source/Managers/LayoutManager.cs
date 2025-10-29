using DT.Source.Globals;
using DT.Source.Graphics;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

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
        /// Root node of the element to get it after Initialze() and draw somewhere.
        /// </summary>
        public StackPanel GetBody()
        {
            return Body;
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

        Grid Root { get; set; }

        /// <summary>
        /// Root node of the window. Use it to draw something inside.
        /// </summary>
        public Grid GetRoot()
        {
            return Root;
        }
        /// <summary>
        /// Adds a child to the window.
        /// </summary>
        /// <param name="element"></param>
        public void Add(UIElement element)
        {
            Root.Children.Add(element);
            Root.ColumnDefinitions.Add(new());
            element.SetValue(Grid.ColumnProperty, Root.Children.Count - 1);
        }
        /// <summary>
        /// Shows the window.
        /// </summary>
        public async Task Initialize()
        {
            AppWindow NewWindow = await AppWindow.TryCreateAsync();
            NewWindow.Title     = Title;

            Root = new() 
            {
                Padding = new Thickness(10)
            };

            var PageElement = new Page
            {
                Background  = Colors.Black,
                Content     = Root
            };

            ElementCompositionPreview.SetAppWindowContent(NewWindow, PageElement);
            await NewWindow.TryShowAsync();
        }
    }
    /// <summary>
    /// TODO: 
    /// DT PopUp class. 
    /// Scope:
    ///   Text content (header or something);
    ///   Input field with publicly { setable } content;
    ///   Confirmation button;
    ///   Event to do something with Input field text content after confirmation button released. 
    ///   
    /// </summary>
    public class PopUp // <- i do really believe we can cook with built-in uwp popups in layoutmanager class, but its dangerous, but its yea, it is a popup.
    {

    }
}
