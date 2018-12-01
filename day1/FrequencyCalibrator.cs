using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace day1
{
    internal class FrequencyCalibrator
    {
        internal async Task<int> GetFrequency()
        {
            var inputLines = await System.IO.File.ReadAllLinesAsync("./input.txt");

            var freqChanges = inputLines.Select(o => int.Parse(o));

            var numbersSeen = new HashSet<int>();

            var frequency = 0;
            var sumsCount = 0;
            var duplicateFound = false;

            do
            {
                foreach (var fc in freqChanges)
                {
                    numbersSeen.Add(frequency);
                    // Console.WriteLine($"{frequency} + {fc} = {(frequency + fc)}");
                    frequency += fc;
                    sumsCount++;

                    if (numbersSeen.Contains(frequency))
                    {
                        Console.WriteLine("DUPLICATE DETECTED!");
                        Console.WriteLine($"Had to do {sumsCount} sums.");
                        Console.WriteLine($"The input has {inputLines.Length} numbers.");
                        Console.WriteLine($"Therefore, {(decimal)sumsCount / inputLines.Length} loops");
                        duplicateFound = true;
                        break;
                    }
                }
            } while (!duplicateFound);

            return frequency;
        }
    }
}
