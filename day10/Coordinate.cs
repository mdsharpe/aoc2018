namespace day10
{
    class Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }

            if (!(obj is Coordinate other)) {
                return false;
            }

            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return $"X = {X}, Y = {Y}".GetHashCode();
        }
    }
}