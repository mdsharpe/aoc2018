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
            var expandIncrement = 3;
            var expansion = 0;

            var inputLines = System.IO.File.ReadAllLines("./input.txt");

            bool[] populatedBuf = ParseInitialState(inputLines.First()).ToArray();
            bool[] targetBuf = new bool[populatedBuf.Length];
            bool[] bufSwapTemp;

            var rules = inputLines.Skip(2).Select(o => ParseRule(o)).ToArray();

            long gen = 0;
            var startTime = DateTimeOffset.Now;
            bool[] repeatedPattern = new bool[0];
            var growthOnlyPhase = false;
            var growthMeasurementStartIndent = -1;
            var growthMeasurementStartExpansion = -1;
            long growthMeasurementStartGen = -1;

            var outputTimer = new System.Threading.Timer(_ =>
            {
                long value;
                int indent;
                bool[] pattern;
                lock (BufLockObject)
                {
                    indent = populatedBuf.GetIndent();
                    pattern = populatedBuf.ToPattern();
                    value = GetValue(indent - expansion, pattern);

                    if (!growthOnlyPhase)
                    {
                        if (pattern.SequenceEqual(repeatedPattern))
                        {
                            growthOnlyPhase = true;
                            growthMeasurementStartIndent = indent;
                            growthMeasurementStartGen = gen;
                            growthMeasurementStartExpansion = expansion;
                        }

                        repeatedPattern = pattern;
                    }
                }

                var indentRate = (double)(indent - growthMeasurementStartIndent) / (gen - growthMeasurementStartGen);
                var expansionRate = (double)(expansion - growthMeasurementStartExpansion) / (gen - growthMeasurementStartGen);

                var projectedIndent = growthMeasurementStartIndent + (long)Math.Round(indentRate * (generations - growthMeasurementStartGen), 0);
                var projectedExpansion = growthMeasurementStartExpansion + (long)Math.Round(expansionRate * (generations - growthMeasurementStartGen), 0);

                var projectedValue = GetValue(projectedIndent - projectedExpansion, repeatedPattern);

                var rate = Math.Round(gen / DateTimeOffset.Now.Subtract(startTime).TotalSeconds, 0);

                Console.Clear();
                Console.WriteLine($"#{gen}; Rate: {rate}/sec; Val: {value}; Width: {populatedBuf.Length}; Indent: {indent}; Expansion: {expansion}");
                Console.WriteLine($"Indent rate: {indentRate}; Expansion rate: {expansionRate}; Projected value: {projectedValue}");
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));

            for (gen = 1; gen <= generations; gen++)
            {
                lock (BufLockObject)
                {
                    if (GetRequiresExpansion(populatedBuf))
                    {
                        populatedBuf = Enumerable.Repeat(false, expandIncrement)
                            .Concat(populatedBuf)
                            .Concat(Enumerable.Repeat(false, expandIncrement))
                            .ToArray();
                        targetBuf = new bool[populatedBuf.Length];
                        expansion += expandIncrement;
                    }
                    else
                    {
                        Array.Clear(targetBuf, 0, targetBuf.Length);
                    }

                    var firstPopulatedIndex = Math.Max(2, populatedBuf.GetIndent());
                    var lastPopulatedIndex = Math.Min(populatedBuf.Length - 3, populatedBuf.GetIndent(true));

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

            Console.WriteLine($"Final value: {GetValue(populatedBuf.GetIndent() - expansion, populatedBuf.ToPattern())}");
        }

        private static bool GetRequiresExpansion(bool[] buf)
            => buf[0] || buf[1] || buf[2] || buf[3]
            || buf[buf.Length - 1] || buf[buf.Length - 2] || buf[buf.Length - 3] || buf[buf.Length - 4];

        private static long GetValue(long indent, IEnumerable<bool> pattern)
            => pattern.Select((o, i) => o ? i + indent : 0).Sum();

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
