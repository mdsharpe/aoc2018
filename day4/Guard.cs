using System.Collections.Generic;
using System.Linq;

namespace day4
{
    class Guard
    {
        public int Id { get; set; }

        public Dictionary<int, int> SleepingMinutes { get; private set; } = new Dictionary<int, int>();

        public int TotalMinutesAsleep
        {
            get
            {
                return SleepingMinutes.Values.Sum();
            }
        }

        public int? MinuteMostAsleep {
            get {
                return SleepingMinutes
                    .OrderByDescending(o => o.Value)
                    .Select(o => o.Key)
                    .Cast<int?>()
                    .FirstOrDefault();
            }
        }
    }
}