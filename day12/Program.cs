using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day12
{
    internal static class Program
    {
        private static readonly Regex RuleRegex = new Regex("^(?<seq>[#.]+) => (?<res>[#.])$");
        private static readonly object BufLockObject = new object();
        private static readonly ParallelOptions RuleEvaluationParallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        internal static void Main(string[] args)
        {
            var generations = 50000000000L;
            // var generations = 20;
            var expandIncrement = 500;
            var expansionOffset = 0;

            var inputLines = System.IO.File.ReadAllLines("./input.txt");

            bool[] populatedBuf = ParseInitialState(inputLines.First()).ToArray();
            bool[] targetBuf = new bool[populatedBuf.Length];
            bool[] bufSwapTemp;

            var rules = inputLines.Skip(2).Select(o => ParseRule(o)).ToArray();

            long gen = 0;
            var startTime = DateTimeOffset.Now;

            var outputTimer = new System.Threading.Timer(_ =>
            {
                long value;
                string plantStrOutput;
                lock (BufLockObject)
                {
                    plantStrOutput = populatedBuf.Cast<bool>().ToPlantString();
                    value = GetValue(populatedBuf, expansionOffset);
                }

                var ratio = (double)value / gen;
                var rate = Math.Round(gen / DateTimeOffset.Now.Subtract(startTime).TotalSeconds, 0);

                Console.Clear();
                Console.WriteLine(
                    $"{gen} / {generations} ({Math.Round((double)gen / generations * 100, 0)}%); Value: {value}; Gen-to-val ratio: {ratio}; Projected final value: {Math.Round(ratio * generations, 0)}; Rate: {rate}/sec");
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1500));

            for (gen = 1; gen <= generations; gen++)
            {
                lock (BufLockObject)
                {
                    if (GetRequiresExpansion(populatedBuf))
                    {
                        populatedBuf = Expand(populatedBuf, expandIncrement);
                        targetBuf = new bool[populatedBuf.Length];
                        expansionOffset += expandIncrement;
                    }
                    else
                    {
                        Array.Clear(targetBuf, 0, targetBuf.Length);
                    }

                    var firstPopulatedIndex = Math.Max(2, Array.IndexOf(populatedBuf, true));
                    var lastPopulatedIndex = Math.Min(populatedBuf.Length - 3, Array.LastIndexOf(populatedBuf, true));

                    Parallel.For(firstPopulatedIndex - 2, lastPopulatedIndex + 2, RuleEvaluationParallelOptions, i =>
                    {
                        targetBuf[i] = rules.Any(o => o.Evaluate(populatedBuf, i));
                    });

                    bufSwapTemp = populatedBuf;
                    populatedBuf = targetBuf;
                    targetBuf = bufSwapTemp;
                }
            }

            outputTimer.Dispose();

            Console.WriteLine($"Final value: {GetValue(populatedBuf, expansionOffset)}");
        }

        private static bool GetRequiresExpansion(bool[] buf)
            => buf[0] || buf[1] || buf[2] || buf[3]
            || buf[buf.Length - 1] || buf[buf.Length - 2] || buf[buf.Length - 3] || buf[buf.Length - 4];

        private static bool[] Expand(bool[] buf, int expandIncrement)
            => Enumerable.Repeat(false, expandIncrement)
            .Concat(buf)
            .Concat(Enumerable.Repeat(false, expandIncrement))
            .ToArray();

        private static long GetValue(bool[] buf, int expansionOffset)
        {
            long sum = 0;
            for (var i = 0; i < buf.Length; i++)
            {
                if (buf[i])
                {
                    sum += i - expansionOffset;
                }
            }

            return sum;
        }

        private static IEnumerable<bool> ParseInitialState(string s)
            => ParsePlantArray(s.Substring("initial state: ".Length));

        private static Rule ParseRule(string s)
        {
            var match = RuleRegex.Match(s);
            return new Rule(
                ParsePlantArray(match.Groups["seq"].Value).ToArray(),
                ParsePlantArray(match.Groups["res"].Value).Single());
        }

        private static IEnumerable<bool> ParsePlantArray(string s)
            => s.Select(o => o == '#');
    }
}
