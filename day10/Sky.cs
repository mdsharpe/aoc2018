using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day10
{
    class Sky
    {
        private readonly ICollection<Point> _points;
        private readonly Coordinate _topLeft;
        private readonly Coordinate _bottomRight;

        private Sky(ICollection<Point> points)
        {
            _points = points;
            _topLeft = new Coordinate(
                points.Min(o => o.Coordinate.X),
                points.Min(o => o.Coordinate.Y)
            );
            _bottomRight = new Coordinate(
                points.Max(o => o.Coordinate.X),
                points.Max(o => o.Coordinate.Y)
            );
        }

        public int Time { get; private set; }
        public Coordinate TopLeft => _topLeft;
        public Coordinate BottomRight => _bottomRight;

        public static Sky Parse(string filename)
        {
            var points = System.IO.File.ReadAllLines(filename)
                .Select(o => Point.Parse(o))
                .ToArray();
            return new Sky(points);
        }

        public void Print()
        {
            var width = (_bottomRight.X - _topLeft.X) + 1;
            var height = (_bottomRight.Y - _topLeft.Y) + 1;
            var consoleWidth = Math.Min(width, Console.WindowWidth);
            var consoleHeight = Math.Min(height, Console.WindowHeight - 5);

            var consoleOutput = new StringBuilder(consoleWidth * consoleHeight);

            var indexOffsetX = _topLeft.X;
            var indexOffsetY = _topLeft.Y;

            var coordsByY = _points
                .Where(o => o.Coordinate.X >= _topLeft.X)
                .Where(o => o.Coordinate.X <= _bottomRight.X)
                .Where(o => o.Coordinate.Y >= _topLeft.Y)
                .Where(o => o.Coordinate.Y <= _bottomRight.Y)
                .ToLookup(
                    o => o.Coordinate.Y,
                    o => o.Coordinate.X);

            for (var y = _topLeft.Y; y <= _bottomRight.Y; y++)
            {
                if (y - indexOffsetY < consoleHeight)
                {
                    var consoleRow = Enumerable.Repeat('.', consoleWidth).ToArray();

                    var points = coordsByY[y];
                    if (points != null)
                    {
                        foreach (var x in points)
                        {
                            if (x - indexOffsetX < consoleRow.Length - 1)
                            {
                                consoleRow[x - indexOffsetX] = '#';
                            }
                        }
                    }

                    consoleOutput.Append(consoleRow);
                    consoleOutput.Append(Environment.NewLine);
                }
            }

            Console.WriteLine(consoleOutput.ToString());
        }

        public void MoveAll()
        {
            foreach (var point in _points)
            {
                point.Move();
            }

            Time++;
        }
    }
}