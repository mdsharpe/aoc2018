using System;

namespace day15
{
    internal abstract class Unit
    {
        public Coordinate Location { get; set ;}
        public abstract char Char { get; }
    }
}
