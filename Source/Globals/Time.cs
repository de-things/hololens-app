using System;

namespace DT.Source.Globals;

class Time
{
    public static string Current 
    { 
        get
        {
            var Hor = DateTime.Now.Hour.ToString();
            var Min = DateTime.Now.Minute.ToString();
            var Sec = DateTime.Now.Second.ToString();

            if (Hor.Length == 1) Hor = $"0{Hor}";
            if (Min.Length == 1) Min = $"0{Min}";
            if (Sec.Length == 1) Sec = $"0{Sec}";

            return $"{Hor}:{Min}:{Sec}";
        } 
    }
}
