using System;
using System.Linq;
namespace day6
{
    class Point
    {
        public Point(int x, int y, char letter = default(char))
        {
            X = x;
            Y = y;
            Letter = letter;
        }
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Letter { get; private set; }

        public bool Equals(Point other) {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return X == other.X && Y == other.Y;
        }

        public static Point Parse(string s, char letter)
        {
            var parts = s.Split(',')
            .Select(o => o.Trim())
            .ToArray();

            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            return new Point(x, y, letter);
        }

        public int GetManhattanDistanceTo(Point p)
        {
            var distX = Math.Abs(X - p.X);
            var distY = Math.Abs(Y - p.Y);
            return distX + distY;
        }
    }
}