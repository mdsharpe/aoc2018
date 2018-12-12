using System;
using System.Text.RegularExpressions;

namespace day10
{
    class Point
    {
        private static readonly Regex ParseRegex = new Regex(
            @"^position=<(?<px>[\s\-0-9]+),(?<py>[\s\-0-9]+)> velocity=<(?<vx>[\s\-0-9]+),(?<vy>[\s\-0-9]+)>$");

        private Point(Coordinate coordinate, Velocity velocity)
        {
            Coordinate = coordinate;
            Velocity = velocity;
        }

        public Coordinate Coordinate { get; }
        public Velocity Velocity { get; }

        public static Point Parse(string s)
        {
            var match = ParseRegex.Match(s);

            return new Point(
                new Coordinate(
                    int.Parse(match.Groups["px"].Value),
                    int.Parse(match.Groups["py"].Value)),
                new Velocity(
                    int.Parse(match.Groups["vx"].Value),
                    int.Parse(match.Groups["vy"].Value)));
        }

        public void Move(int seconds = 1)
        {
            Coordinate.X += (Velocity.X * seconds);
            Coordinate.Y += (Velocity.Y * seconds);
        }
    }
}
