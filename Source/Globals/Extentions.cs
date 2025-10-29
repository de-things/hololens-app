using System;
using System.Linq;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace DT.Source.Globals;
static class Extentions
{
    public static byte[] ToRgb(this string HEXColor)
    {
        HEXColor = HEXColor.Substring(1);
        byte R = byte.Parse($"{HEXColor[0]}{HEXColor[1]}", System.Globalization.NumberStyles.HexNumber);
        byte G = byte.Parse($"{HEXColor[2]}{HEXColor[3]}", System.Globalization.NumberStyles.HexNumber);
        byte B = byte.Parse($"{HEXColor[4]}{HEXColor[5]}", System.Globalization.NumberStyles.HexNumber);
        return new byte[3] { R, G, B };
    }
    public static Geometry ToGeometry(this string Path)
    {
        var Value = Geometry.Empty;
        try
        {
            Value = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), Path);
        }
        catch { }
        return Value;
    }
}
