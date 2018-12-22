using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day15
{
    internal class Fight
    {
        private readonly bool[,] _walls;

        private readonly List<Unit> _units;

        public Fight(bool[,] walls, List<Unit> units)
        {
            _walls = walls;
            _units = units;
        }

        public static Fight Parse(string filePath)
        {
            var inputLines = System.IO.File.ReadAllLines(filePath);
            var height = inputLines.Length;
            var width = inputLines.Max(o => o.Length);

            var units = new List<Unit>();
            var walls = new bool[width, height];

            for (var y = 0; y < height; y++)
            {
                var line = inputLines[y];

                for (var x = 0; x < width; x++)
                {
                    var c = line[x];

                    switch (c)
                    {
                        case '#':
                            walls[x, y] = true;
                            break;
                        case 'G':
                            units.Add(new Goblin { X = x, Y = y });
                            break;
                        case 'E':
                            units.Add(new Elf { X = x, Y = y });
                            break;
                        case '.':
                            walls[x, y] = false;
                            break;
                        default:
                            throw new Exception($"Unexpected input character '{c}'.");
                    }
                }
            }

            return new Fight(walls, units);
        }

        public void Run(bool verbose = false)
        {
            if (verbose) Print();

            do
            {
                if (_units.GroupBy(o => o.GetType()).Count() < 2) break;

                var unitsInOrder = GetUnitsInReadingOrder().ToArray();

                foreach (var unit in unitsInOrder)
                {
                    if (!_units.Contains(unit)) continue;

                    TakeTurn(unit);
                }
            } while (true);
        }

        private void TakeTurn(Unit unit)
        {
            var routesToTargets = from u in _units
                                  where u.GetType() != unit.GetType()
                                  from c in u.Location.EnumerateAdjacent()
                                  let r = new RouteFinder(_walls).FindRoute(unit.Location, c)
                                  where r != null
                                  select u;

            var route = routesToTargets.OrderByDescending(o => o).FirstOrDefault();
        }

        private void Print()
        {
            Console.Clear();

            var output = new StringBuilder();

            var width = _walls.GetLength(0);
            var height = _walls.GetLength(1);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = _walls[x, y] ? '#' : '.';
                    var u = _units.FirstOrDefault(o => o.IsAt(x, y));
                    if (u != null) c = u.Char;
                    output.Append(c);
                }

                output.Append(Environment.NewLine);
            }

            Console.Write(output);
        }

        private IEnumerable<Unit> GetUnitsInReadingOrder()
            => _units.OrderBy(o => o.Y).ThenBy(o => o.X);
    }
}