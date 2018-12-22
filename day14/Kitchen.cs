using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day14
{
    internal class Kitchen
    {
        private readonly string _input;
        private readonly List<byte> _scores;
        private readonly int[] _elfAssignments;

        public Kitchen(string input, int elfCount)
        {
            _input = input;
            _scores = input.Select(o => (byte)Char.GetNumericValue(o)).ToList();
            _elfAssignments = Enumerable.Range(0, elfCount).ToArray();
        }

        public void FindDigitsAfter(int after, int count, bool verbose = false)
        {
            Run(() => _scores.Count < after + count, verbose);

            var result = string.Concat(_scores.Skip(after).Take(count).Select(o => o.ToString()));
            Console.WriteLine($"{_input}: After {after} recipes, the scores of the next {count} would be {result}.");
        }

        public void CountDigitsBefore(string sequenceString, bool verbose = false)
        {
            var targetSequence = sequenceString.Select(o => (byte)Char.GetNumericValue(o)).ToArray();

            Run(() =>
            {
                var scoresOffset = _scores.Count - targetSequence.Length;
                for (int i = 0; i < targetSequence.Length; i++)
                {
                    var si = scoresOffset + i;
                    if (si < 0) return true;

                    if (_scores[si] != targetSequence[i])
                    {
                        return true;
                    }
                }

                return false;
            }, verbose);

            var result = _scores.Count - targetSequence.Length;
            Console.WriteLine($"{_input}: {sequenceString} first appears after {result} recipes.");
        }

        private void Run(Func<bool> runWhile, bool verbose)
        {
            if (verbose) Print();

            do
            {
                CreateNewRecipes();

                for (var elfIndex = 0; elfIndex < _elfAssignments.Length; elfIndex++)
                {
                    PickNewRecipe(elfIndex);
                }

                if (verbose) Print();
            } while (runWhile());
        }

        private void CreateNewRecipes()
            => _scores.AddRange(
                _elfAssignments
                    .Select(o => (int)_scores[o])
                    .Sum()
                    .ToString()
                    .Select(o => (byte)Char.GetNumericValue(o)));

        private void PickNewRecipe(int elfIndex)
        {
            var assignment = _elfAssignments[elfIndex];
            _elfAssignments[elfIndex] = (assignment + _scores[assignment] + 1) % _scores.Count;
        }

        private void Print()
        {
            var output = new StringBuilder();

            for (var i = 0; i < _scores.Count; i++)
            {
                var surround =
                _elfAssignments[0] == i ? "()" :
                _elfAssignments[1] == i ? "[]" :
                "  ";

                output.Append(surround[0]);
                output.Append(_scores[i]);
                output.Append(surround[1]);
            }

            Console.WriteLine(output);
        }
    }
}