using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace day12
{
    class Program
    {
        private static readonly Regex RuleRegex = new Regex("^(?<seq>[#.]+) => (?<res>[#.])$");

        static void Main(string[] args)
        {
            var generations = 50000000000L;
            // var generations = 20;
            var expandIncrement = 100;
            var expandTotal = 0;

            var inputLines = System.IO.File.ReadAllLines("./input.txt");

            var buf1 = new BitArray(ParseInitialState(inputLines.First()).ToArray());
            var buf2 = new BitArray(buf1.Length);

            var rules = inputLines.Skip(2).Select(o => ParseRule(o)).ToArray();

            long gen = 0;
            BitArray populatedBuf = buf1;
            BitArray targetBuf = buf2;

            var outputTimer = new System.Threading.Timer(_ =>
            {
                Console.SetCursorPosition(0, Math.Max(Console.CursorTop - 1, 0));
                Console.WriteLine($"{gen} / {generations} ({gen / generations * 100}%); Length: {buf1.Length}");
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            for (gen = 1; gen <= generations; gen++)
            {
                if (GetRequiresExpansion(populatedBuf))
                {
                    Expand(ref buf1, expandIncrement);
                    Expand(ref buf2, expandIncrement);
                    expandTotal += expandIncrement;
                }

                for (var i = 0; i < populatedBuf.Length; i++)
                {
                    targetBuf[i] = rules
                        .Any(o => o.Evaluate(populatedBuf, i).GetValueOrDefault());
                }

                Swap(buf1, buf2, ref populatedBuf, ref targetBuf);
            }

            outputTimer.Dispose();
            Console.WriteLine("Done, counting...");

            long sum = 0;
            for (var i = 0; i < populatedBuf.Length; i++)
            {
                if (populatedBuf[i])
                {
                    sum += i - expandTotal;
                }
            }

            Console.WriteLine($"Sum of the numbers of all pots which contain a plant: {sum}");
        }

        private static bool GetRequiresExpansion(BitArray buf)
            => buf[0] || buf[1] || buf[buf.Length - 1] || buf[buf.Length - 2];

        private static void Expand(ref BitArray buf, int expandIncrement)
        {
            buf.Length += expandIncrement * 2;
            buf = buf.LeftShift(expandIncrement);
        }

        private static void Swap(BitArray buf1, BitArray buf2, ref BitArray populatedBuf, ref BitArray targetBuf)
        {
            populatedBuf = populatedBuf == buf1 ? buf2 : buf1;
            targetBuf = targetBuf == buf1 ? buf2 : buf1;
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
