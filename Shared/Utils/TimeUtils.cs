using System;
using System.Collections.Generic;

namespace yDevs.Shared.Utils
{
    public static class Utils 
    {
        public static TimeSpan ConvertToTimeSpan(this string timeSpan)
        {
            List<string> timeUnits = new List<string>() { "d", "h", "m", "s", "f", "z" };
            
            var type = "s";
            var value = timeSpan;        
            if (timeUnits.Contains(timeSpan.Substring(timeSpan.Length-1)))
            {
                var l = timeSpan.Length - 1;
                value = timeSpan.Substring(0, l);
                type = timeSpan.Substring(l, 1);
            }

            switch (type)
            {
                case "d": return TimeSpan.FromDays(double.Parse(value));
                case "h": return TimeSpan.FromHours(double.Parse(value));
                case "m": return TimeSpan.FromMinutes(double.Parse(value));
                case "s": return TimeSpan.FromSeconds(double.Parse(value));
                case "f": return TimeSpan.FromMilliseconds(double.Parse(value));
                case "z": return TimeSpan.FromTicks(long.Parse(value));
                default: return TimeSpan.FromDays(double.Parse(value));
            }
        }
    }
}