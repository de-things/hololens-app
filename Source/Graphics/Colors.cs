using Windows.UI;
using Windows.UI.Xaml.Media;
using DT.Source.Globals;

namespace DT.Source.Graphics;

class Colors
{
    // --- #192787 - hololens window topbar color btw ---

    public static SolidColorBrush White { 
        get
        {
            var bytes = "#ffffff".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        } 
    }
    public static SolidColorBrush Black
    {
        get
        {
            var bytes = "#000000".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush Blue
    {
        get
        {
            var bytes = "#283748".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }

    public static SolidColorBrush SoftGreen
    {
        get
        {
            var bytes = "#314828".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush Green
    {
        get
        {
            var bytes = "#202F1A".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush SoftYellow
    {
        get
        {
            var bytes = "#FFD752".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush Yellow
    {
        get
        {
            var bytes = "#FFF4D2".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush SoftGrey
    {
        get
        {
            var bytes = "#CCCCCC".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
    public static SolidColorBrush Grey
    {
        get
        {
            var bytes = "#999999".ToRgb();
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            return new SolidColorBrush(color);
        }
    }
}
