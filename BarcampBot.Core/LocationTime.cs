using System;
using System.Collections.Generic;
using System.Text;

namespace BarcampBot.Core
{
    public class LocationTime
    {
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Location { get; set; }

        public LocationTime()
        {
            From = DateTime.Now;
            To = null;
            Location = null;
        }

        public override string ToString()
        {
            var ret = $"{From.ToString("d.MM.yyyy")}";

            if (To != null)
                ret += $" to {To?.ToString("d.MM.yyyy")}";

            if (!string.IsNullOrWhiteSpace(Location))
                ret += $" in {Location}";

            return ret;
        }

        public static LocationTime Parse(string raw)
        {
            var lt = new LocationTime();

            var index = raw.IndexOf('(');
            lt.From = DateTime.Parse(raw.Substring(0, index).Trim());

            index = raw.IndexOf("bis");

            if (index > 0)
            {
                index += 3;

                raw = raw.Substring(index, raw.Length - index).Trim();
                index = raw.IndexOf('(');
                lt.To = DateTime.Parse(raw.Substring(0, index).Trim());
            }

            index = raw.IndexOf("in");

            if (index > 0)
            {
                index += 2;

                raw = raw.Substring(index, raw.Length - index).Trim();
                lt.Location = raw;
            }

            return lt;
        }
    }
}
