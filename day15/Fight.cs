using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day15
{
    internal class Fight
    {
        private const int DelayMs = 2000;
        private readonly bool[,] _walls;
        private readonly HashSet<Coordinate> _wallsSet;

        private readonly List<Unit> _units;

        public Fight(bool[,] walls, List<Unit> units)
        {
            _walls = walls;
            _units = units;

            var width = _walls.GetLength(0);
            var height = _walls.GetLength(1);

            _wallsSet = new HashSet<Coordinate>();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (walls[x, y])
                    {
                        _wallsSet.Add(new Coordinate(x, y));
                    }
                }
            }
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
                            units.Add(new Goblin { Location = new Coordinate(x, y) });
                            break;
                        case 'E':
                            units.Add(new Elf { Location = new Coordinate(x, y) });
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

        public async Task Run(bool verbose = false)
        {
            if (verbose)
            {
                Print();
                await Task.Delay(DelayMs);
            }

            Task delay = null;

            do
            {
                if (_units.GroupBy(o => o.GetType()).Count() < 2) break;

                if (verbose)
                {
                    delay = Task.Delay(DelayMs);
                }

                var unitsInOrder = GetUnitsInReadingOrder().ToArray();

                foreach (var unit in unitsInOrder)
                {
                    if (!_units.Contains(unit)) continue;
                    TakeTurn(unit);
                    if (verbose) {
                        Print();
                        await Task.Delay(200);
                    }
                }

                if (verbose)
                {
                    await delay;
                    Print();
                }
            } while (true);
        }

        private void TakeTurn(Unit unit)
        {
            var rf = new RouteFinder(
                unit.Location,
                _wallsSet.Union(_units.Where(o => o != unit).Select(o => o.Location)));

            var routes = (from u in _units
                          where u.GetType() != unit.GetType()
                          let r = rf.RouteTo(u.Location).Route
                          where r != null
                          select r)
                          .ToArray();

            if (routes.Any() && !routes.Any(o => o.Length == 0))
            {
                routes = routes
                    .OrderBy(o => o.Length)
                    .ThenBy(o => o[0].Y)
                    .ThenBy(o => o[0].X)
                    .ToArray();

                var route = routes
                    .FirstOrDefault();

                if (route != null && route.Any())
                {
                    unit.Location = route.First();
                }
            }
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
            => _units.OrderBy(o => o.Location.Y).ThenBy(o => o.Location.X);
    }
}