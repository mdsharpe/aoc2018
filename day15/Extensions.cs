using System;
using System.Collections.Generic;
using System.Linq;

namespace day15
{
    internal static class Extensions
    {
        public static bool IsAt(this Unit @this, int x, int y)
            => @this.Location.X == x && @this.Location.Y == y;

        public static Coordinate GetAdjacent(this Coordinate @this, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Coordinate(@this.X, @this.Y - 1);
                case Direction.Right:
                    return new Coordinate(@this.X + 1, @this.Y);
                case Direction.Down:
                    return new Coordinate(@this.X, @this.Y + 1);
                case Direction.Left:
                    return new Coordinate(@this.X - 1, @this.Y);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static IEnumerable<Coordinate> EnumerateAdjacent(this Coordinate @this)
        {
            return from d in Enum.GetValues(typeof(Direction)).Cast<Direction>()
                   select GetAdjacent(@this, d);
        }

        public static bool GetIsWall(this bool[,] @this, Coordinate c)
            => @this[c.X, c.Y];
    }
}
