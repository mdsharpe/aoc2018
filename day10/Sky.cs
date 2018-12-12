using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day10
{
    class Sky
    {
        private readonly ICollection<Point> _points;

        private Sky(ICollection<Point> points)
        {
            _points = points;
        }

        public int Time { get; private set; }

        public static Sky Parse(string filename)
        {
            var points = System.IO.File.ReadAllLines(filename)
                .Select(o => Point.Parse(o))
                .ToArray();
            return new Sky(points);
        }

        public void MoveAll(int seconds = 1)
        {
            Parallel.ForEach(_points, o => o.Move(seconds));

            Time += seconds;
        }

        public void Print()
        {
            var topLeft = new Coordinate(
                _points.Min(o => o.Coordinate.X),
                _points.Min(o => o.Coordinate.Y)
            );
            var bottomRight = new Coordinate(
                _points.Max(o => o.Coordinate.X),
                _points.Max(o => o.Coordinate.Y)
            );

            Console.WriteLine($"({topLeft.X}, {topLeft.Y}) - ({bottomRight.X}, {bottomRight.Y})");

            var width = (bottomRight.X - topLeft.X) + 1;
            var height = (bottomRight.Y - topLeft.Y) + 1;
            var consoleWidth = Math.Min(width, Console.WindowWidth);
            var consoleHeight = Math.Min(height, Console.WindowHeight - 5);
            var ratioX = Convert.ToDecimal(consoleWidth) / width;
            var ratioY = Convert.ToDecimal(consoleHeight) / height;
            var chunkWidth = width / consoleWidth;
            var chunkHeight = height / consoleHeight;

            var coords = (from o in _points
                          where o.Coordinate.X >= topLeft.X
                          where o.Coordinate.X <= bottomRight.X
                          where o.Coordinate.Y >= topLeft.Y
                          where o.Coordinate.Y <= bottomRight.Y
                          select new Coordinate(
                              Convert.ToInt32((o.Coordinate.X - topLeft.X) * ratioX),
                              Convert.ToInt32((o.Coordinate.Y - topLeft.Y) * ratioY)))
                          .Distinct()
                          .GroupBy(o => o.Y)
                          .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(gi => gi.X)));

            foreach (var y in Enumerable.Range(0, consoleHeight))
            {
                var rowHasPoints = coords.TryGetValue(y, out var rowPoints);

                foreach (var x in Enumerable.Range(0, consoleWidth))
                {
                    Console.Write((rowHasPoints && rowPoints.Contains(x)) ? '#' : '.');
                }

                if (y < consoleHeight)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
