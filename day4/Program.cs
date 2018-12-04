using System;
using System.Collections.Generic;
using System.Linq;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var events = System.IO.File.ReadAllLines("./input.txt")
                .Select(o => GuardEvent.Parse(o))
                .OrderBy(o => o.When)
                .ToList();

            var guards = new Dictionary<int, Guard>();

            var id = 0;
            var fellAsleep = default(DateTime);

            foreach (var e in events)
            {
                if (e.Type == GuardEventType.BeginsShift)
                {
                    id = e.GuardId;
                    continue;
                }

                if (e.Type == GuardEventType.FallsAsleep)
                {
                    fellAsleep = e.When;
                    continue;
                }

                if (!guards.TryGetValue(id, out var guard))
                {
                    guard = new Guard { Id = id };
                    guards.Add(guard.Id, guard);
                }

                for (var minute = fellAsleep.Minute; minute < e.When.Minute; minute++)
                {
                    if (!guard.SleepingMinutes.TryGetValue(minute, out int count))
                    {
                        count = 0;
                    }

                    count++;
                    guard.SleepingMinutes[minute] = count;
                }
            }

            var guardMostAsleep = guards.Values
                .OrderByDescending(o => o.TotalMinutesAsleep)
                .First();

            Console.WriteLine(
                $"Guard #{guardMostAsleep.Id} was asleep the most, and slept most on minute {guardMostAsleep.MinuteMostAsleep}.");

            var guardMostAsleepOnSameMinute = (from g in guards.Values
                                               where g.MinuteMostAsleep.HasValue
                                               let minuteMostAsleep = g.MinuteMostAsleep.Value
                                               let timesAsleep = g.SleepingMinutes[minuteMostAsleep]
                                               orderby timesAsleep descending
                                               select new
                                               {
                                                   Id = g.Id,
                                                   MinuteMostAsleep = minuteMostAsleep,
                                                   TimesAsleep = timesAsleep
                                               }).First();

            Console.WriteLine(
                $"Guard #{guardMostAsleepOnSameMinute.Id} was asleep the most on the same minute, sleeping {guardMostAsleepOnSameMinute.TimesAsleep} times on minute {guardMostAsleepOnSameMinute.MinuteMostAsleep}.");
        }
    }
}
