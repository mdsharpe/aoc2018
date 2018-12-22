using System;

namespace day15
{
    internal class Coordinate : IEquatable<Coordinate>
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public bool Equals(Coordinate other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object o)
        {
            if (Object.ReferenceEquals(this, o)) return true;
            return Equals(o as Coordinate);
        }

        public override int GetHashCode()
        {
            return $"X = {X}, Y = {Y}".GetHashCode();
        }
    }
}