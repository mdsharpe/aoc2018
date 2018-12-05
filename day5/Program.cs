using System;
using System.Linq;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var polymer = System.IO.File.ReadAllText("./input.txt");

            Console.WriteLine($"Starting length: {polymer.Length}.");

            polymer = React(polymer);

            Console.WriteLine($"Reacted length: {polymer.Length}.");

            polymer = FindShortestLengthWithUnitRemoved(polymer, out var unitRemoved);

            Console.WriteLine($"Achieved shortest length with unit '{unitRemoved}' removed: {polymer.Length}.");
        }

        private static string React(string polymer)
        {
            int index;
            while (TryFindReaction(polymer, out index))
            {
                polymer = EliminateReaction(polymer, index);
            }

            return polymer;
        }

        private static bool TryFindReaction(string polymer, out int reactionIndex)
        {
            for (var i = 0; i < polymer.Length - 1; i++)
            {
                var currentChar = polymer[i];
                var nextChar = polymer[i + 1];

                if ((char.IsLower(currentChar) && char.IsUpper(nextChar))
                    || (char.IsUpper(currentChar) && char.IsLower(nextChar)))
                {
                    if (char.ToUpperInvariant(currentChar) == char.ToUpperInvariant(nextChar)){
                        reactionIndex = i;
                        return true;
                    }
                }
            }

            reactionIndex = 0;
            return false;
        }

        private static string EliminateReaction(string polymer, int reactionIndex)
            => polymer.Remove(reactionIndex, 2);

        private static string FindShortestLengthWithUnitRemoved(string polymer, out object unitRemoved)
        {
            unitRemoved = default(char);
            string shortenedPolymer = null;
            var minLength = polymer.Length;

            for (var c = 'A'; c <= 'Z'; c++) {
                var p = EliminateUnit(polymer, c);
                p = React(p);

                if (p.Length < minLength){
                    minLength = p.Length;
                    unitRemoved = c;
                    shortenedPolymer = p;
                }
            }

            return shortenedPolymer;
        }

        private static string EliminateUnit(string polymer, char unit)
            => polymer
                .Replace(char.ToLowerInvariant(unit).ToString(), string.Empty)
                .Replace(char.ToUpperInvariant(unit).ToString(), string.Empty);
    }
}
