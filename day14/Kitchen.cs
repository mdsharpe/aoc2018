using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day14
{
    internal class Kitchen
    {
        private readonly string _input;
        private readonly List<int> _scores;
        private readonly int[] _elfAssignments;

        public Kitchen(string input, int elfCount)
        {
            _input = input;
            _scores = input.Select(o => (int)Char.GetNumericValue(o)).ToList();
            _elfAssignments = Enumerable.Range(0, elfCount).ToArray();
        }

        public void Find(int after, int count, bool verbose = false)
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
            } while (_scores.Count < after + count);

            var result = string.Concat(_scores.Skip(after).Take(count).Select(o => o.ToString()));
            Console.WriteLine($"{_input}: After {after} recipes, the scores of the next {count} would be {result}.");
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

        private void CreateNewRecipes()
        {
            var curRecSum = _elfAssignments.Select(o => _scores[o]).Sum();
            _scores.AddRange(curRecSum.ToString().Select(o => (int)Char.GetNumericValue(o)));
        }

        private void PickNewRecipe(int elfIndex)
        {
            var assignment = _elfAssignments[elfIndex];
            assignment += _scores[assignment] + 1;
            assignment %= _scores.Count;
            _elfAssignments[elfIndex] = assignment;
        }
    }
}