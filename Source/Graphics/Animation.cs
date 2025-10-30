using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;

namespace DT.Source.Graphics;
class Animation
{
    const double CLICK_TIME     = .12;
    const double WAIT_TIME      = .5;
    const double DISOLVE_TIME   = .8;
    const double APPROACH_TIME  = 1.3;
    const double ROTATE_TIME    = 2;

    static readonly Duration ClickDuration = new(TimeSpan.FromSeconds(CLICK_TIME));
    static readonly Duration WaitDuration = new(TimeSpan.FromSeconds(WAIT_TIME));
    static readonly Duration ApproachDuration = new(TimeSpan.FromSeconds(APPROACH_TIME));
    static readonly Duration RotateDuration = new(TimeSpan.FromSeconds(ROTATE_TIME));
    static readonly Duration DisolveDuration = new(TimeSpan.FromSeconds(DISOLVE_TIME));

    static readonly CubicEase EaseInOut = new() { EasingMode = EasingMode.EaseInOut };
    static readonly CubicEase EaseOut = new() { EasingMode = EasingMode.EaseOut };
    public static void ButtonPressed(Border element)
    {
        Storyboard SB = new();

        var OpacityAnimaiton = new DoubleAnimation
        {
            From = element.Opacity,
            To = .6,
            Duration = ClickDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(OpacityAnimaiton);

        Storyboard.SetTarget(OpacityAnimaiton, element);
        Storyboard.SetTargetProperty(OpacityAnimaiton, "Opacity");

        SB.Begin();
    }
    public static void ButtonReleased(Border element)
    {
        Storyboard SB = new();

        var OpacityAnimaiton = new DoubleAnimation
        {
            From = element.Opacity,
            To = 1,
            Duration = ClickDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(OpacityAnimaiton);

        Storyboard.SetTarget(OpacityAnimaiton, element);
        Storyboard.SetTargetProperty(OpacityAnimaiton, "Opacity");

        SB.Begin();
    }
    public static void LogoRotate(RotateTransform logoRotator, SolidColorBrush logoBrush)
    {
        Storyboard SB = new();

        var RotateAnimaiton = new DoubleAnimation
        {
            From = 0,
            To = 45,
            Duration = RotateDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(RotateAnimaiton);

        RotateAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(RotateAnimaiton, logoRotator);
        Storyboard.SetTargetProperty(RotateAnimaiton, "Angle");

        var ColorAnimaiton = new ColorAnimation
        {
            From = Color.FromArgb(255, 255, 255, 255),
            To = Color.FromArgb(255, 72, 206, 255),
            Duration = RotateDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(ColorAnimaiton);

        ColorAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(ColorAnimaiton, logoBrush);
        Storyboard.SetTargetProperty(ColorAnimaiton, "Color");

        SB.Completed += (s, a) =>
        {
            Storyboard SB2 = new();

            var RotateAnimaiton2 = new DoubleAnimation
            {
                From = 0,
                To = 45,
                Duration = RotateDuration,
                EasingFunction = EaseInOut,
                RepeatBehavior = RepeatBehavior.Forever,
            };
            SB2.Children.Add(RotateAnimaiton2);

            RotateAnimaiton2.EnableDependentAnimation = true;

            Storyboard.SetTarget(RotateAnimaiton2, logoRotator);
            Storyboard.SetTargetProperty(RotateAnimaiton2, "Angle");

            var ColorAnimaiton2 = new ColorAnimation
            {
                From = Color.FromArgb(255, 72, 206, 255),
                To = Color.FromArgb(255, 255, 142, 72),
                Duration = RotateDuration,
                EasingFunction = EaseInOut,
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };
            SB2.Children.Add(ColorAnimaiton2);

            ColorAnimaiton.EnableDependentAnimation = true;

            Storyboard.SetTarget(ColorAnimaiton2, logoBrush);
            Storyboard.SetTargetProperty(ColorAnimaiton2, "Color");

            SB2.Begin();
        };

        SB.Begin();
    }
    public static void WelcomeApproach(Path logo, RotateTransform logoRotator, SolidColorBrush logoBrush, TextBlock title, TextBlock hint)
    {
        Storyboard SB = new();

        var WidthAnimaiton = new DoubleAnimation
        {
            From = 0,
            To = 120,
            Duration = ApproachDuration,
            EasingFunction = EaseInOut,
        };
        
        WidthAnimaiton.EnableDependentAnimation = true;

        SB.Children.Add(WidthAnimaiton);

        Storyboard.SetTarget(WidthAnimaiton, logo);
        Storyboard.SetTargetProperty(WidthAnimaiton, "Width");

        var HeightAnimaiton = new DoubleAnimation
        {
            From = 0,
            To = 120,
            Duration = ApproachDuration,
            EasingFunction = EaseInOut,
        };

        HeightAnimaiton.EnableDependentAnimation = true;

        SB.Children.Add(HeightAnimaiton);

        Storyboard.SetTarget(HeightAnimaiton, logo);
        Storyboard.SetTargetProperty(HeightAnimaiton, "Height");

        var OpacityAnimaiton = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = RotateDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(OpacityAnimaiton);

        Storyboard.SetTarget(OpacityAnimaiton, logo);
        Storyboard.SetTargetProperty(OpacityAnimaiton, "Opacity");


        SB.Completed += (s, a) =>
        {
            LogoRotate(logoRotator, logoBrush);

            Storyboard SB2 = new();

            var OpacityAnimaiton2 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = RotateDuration,
                EasingFunction = EaseInOut,
            };
            SB2.Children.Add(OpacityAnimaiton2);

            Storyboard.SetTarget(OpacityAnimaiton2, title);
            Storyboard.SetTargetProperty(OpacityAnimaiton2, "Opacity");

            SB2.Completed += (s, a) =>
            {
                Storyboard SB3 = new();

                var OpacityAnimaiton3 = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = RotateDuration,
                    EasingFunction = EaseInOut,
                };
                SB3.Children.Add(OpacityAnimaiton3);

                Storyboard.SetTarget(OpacityAnimaiton3, hint);
                Storyboard.SetTargetProperty(OpacityAnimaiton3, "Opacity");

                SB3.Begin();
            };
            SB2.Begin();
        };
        SB.Begin();
    }
    public static void Disolve(Border element)
    {
        Storyboard SB = new();

        var OpacityAnimaiton = new DoubleAnimation
        {
            From = element.Opacity,
            To = 0,
            Duration = DisolveDuration,
            EasingFunction = EaseOut,
        };
        SB.Children.Add(OpacityAnimaiton);

        Storyboard.SetTarget(OpacityAnimaiton, element);
        Storyboard.SetTargetProperty(OpacityAnimaiton, "Opacity");

        SB.Begin();

        SB.Completed += (s, a) =>
        {
            element.Visibility = Visibility.Collapsed;
        };
    }
    public static void Show(Border element)
    {
        element.Visibility = Visibility.Visible;

        Storyboard SB = new();

        var OpacityAnimaiton = new DoubleAnimation
        {
            From = element.Opacity,
            To = 1,
            Duration = DisolveDuration,
            EasingFunction = EaseOut,
        };
        SB.Children.Add(OpacityAnimaiton);

        Storyboard.SetTarget(OpacityAnimaiton, element);
        Storyboard.SetTargetProperty(OpacityAnimaiton, "Opacity");

        SB.Begin();
    }
    public static void ColorRamp(SolidColorBrush element)
    {
        Storyboard SB = new();

        var ColorAnimaiton = new ColorAnimation
        {
            From = Color.FromArgb(255, 255, 255, 255),
            To = Color.FromArgb(255, 255, 135, 98),
            Duration = WaitDuration,
            EasingFunction = EaseInOut,
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true
        };
        SB.Children.Add(ColorAnimaiton);

        ColorAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(ColorAnimaiton, element);
        Storyboard.SetTargetProperty(ColorAnimaiton, "Color");

        SB.Begin();
    }
    public static void Rotate(RotateTransform element)
    {
        Storyboard SB = new();

        var RotateAnimaiton = new DoubleAnimation
        {
            From = 0,
            To = 45,
            Duration = WaitDuration,
            EasingFunction = EaseInOut,
            RepeatBehavior = RepeatBehavior.Forever,
        };
        SB.Children.Add(RotateAnimaiton);

        RotateAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(RotateAnimaiton, element);
        Storyboard.SetTargetProperty(RotateAnimaiton, "Angle");

        SB.Begin();
    }
    public static void ErrorBlink(SolidColorBrush element, SolidColorBrush sourceColor)
    {
        Storyboard SB = new();

        var ColorAnimaiton = new ColorAnimation
        {
            From = Color.FromArgb(255, 255, 98, 98),
            To = Color.FromArgb(255, sourceColor.Color.R, sourceColor.Color.G, sourceColor.Color.B),
            Duration = ClickDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(ColorAnimaiton);

        ColorAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(ColorAnimaiton, element);
        Storyboard.SetTargetProperty(ColorAnimaiton, "Color");

        SB.Begin();
    }
    public static void Blink(SolidColorBrush element, SolidColorBrush sourceColor)
    {
        Storyboard SB = new();

        var ColorAnimaiton = new ColorAnimation
        {
            From = Color.FromArgb(255, 255, 255, 255),
            To = Color.FromArgb(255, sourceColor.Color.R, sourceColor.Color.G, sourceColor.Color.B),
            Duration = ClickDuration,
            EasingFunction = EaseInOut,
        };
        SB.Children.Add(ColorAnimaiton);

        ColorAnimaiton.EnableDependentAnimation = true;

        Storyboard.SetTarget(ColorAnimaiton, element);
        Storyboard.SetTargetProperty(ColorAnimaiton, "Color");

        SB.Begin();
    }
}
