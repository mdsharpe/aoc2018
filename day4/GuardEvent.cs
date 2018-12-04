using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace day4
{
    class GuardEvent
    {
        public DateTime When { get; set; }
        public int GuardId { get; set; }
        public GuardEventType Type { get; set; }

        public static GuardEvent Parse(string eventString)
        {
            var match = new Regex(@"^\[([0-9\- \:]+)\] (wakes up|falls asleep|Guard #[0-9]+ begins shift)$")
                .Match(eventString);

            var evt = new GuardEvent
            {
                When = DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };

            var desc = match.Groups[2].Value;

            if (string.Equals(desc, "wakes up", StringComparison.Ordinal))
            {
                evt.Type = GuardEventType.WakesUp;
                evt.GuardId = -1;
            }
            else if (string.Equals(desc, "falls asleep", StringComparison.Ordinal))
            {
                evt.Type = GuardEventType.FallsAsleep;
                evt.GuardId = -1;
            }
            else
            {
                evt.Type = GuardEventType.BeginsShift;

                var guardIdMatch = new Regex("^Guard #([0-9]+) begins shift$")
                    .Match(desc);

                evt.GuardId = int.Parse(guardIdMatch.Groups[1].Value);
            }

            return evt;
        }
    }
}