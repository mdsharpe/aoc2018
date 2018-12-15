using System;
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
            var inputLines = System.IO.File.ReadAllLines("./input.example.txt");

            var state = ParseInitialState(inputLines.First()).ToArray();
            var rules = inputLines.Skip(2).Select(o => ParseRule(o)).ToArray();

            PrintState(state, 0);

            for (var gen = 1; gen <= 20; gen++)
            {
                state = Generate(state, rules);
                PrintState(state, gen);
            }
        }

        private static bool[] Generate(
            bool[] state,
            ICollection<Rule> rules)
        {
            var result = new bool[state.Length];

            for (var i = 0; i < state.Length; i++)
            {
                result[i] = rules
                    .Select(o => o.Evaluate(state, i))
                    .Any(o => o.GetValueOrDefault());
            }

            return result;
        }

        private static void PrintState(bool[] state, int generation)
        {
            var output = new StringBuilder(generation.ToString("00") + ": ");

            foreach (var pot in state)
            {
                output.Append(pot ? '#' : '.');
            }

            Console.WriteLine(output);
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
