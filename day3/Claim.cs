using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace day3
{
    class Claim
    {
        public int Id { get; set; }

        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public static Claim Parse(string claimString)
        {
            var match = new Regex("^#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)$")
                .Match(claimString);

            var x1 = int.Parse(match.Groups[2].Value);
            var y1 = int.Parse(match.Groups[3].Value);

            return new Claim
            {
                Id = int.Parse(match.Groups[1].Value),
                X1 = x1,
                Y1 = y1,
                X2 = x1 + int.Parse(match.Groups[4].Value) - 1,
                Y2 = y1 + int.Parse(match.Groups[5].Value) - 1
            };
        }

        public bool Contains(int x, int y)
        {
            return x >= X1
                && x <= X2
                && y >= Y1
                && y <= Y2;
        }

        public bool Intersect(Claim o)
        {
            var nonIntersectX = this.X2 < o.X1 || this.X1 > o.X2;
            var nonIntersectY = this.Y2 < o.Y1 || this.Y1 > o.Y2;
            var nonIntersect = nonIntersectX || nonIntersectY;
            return !nonIntersect;
        }
    }
}
