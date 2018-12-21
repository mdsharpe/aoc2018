namespace day13
{
    internal enum CellType : byte
    {
        Unknown = 0,
        Empty = 1,
        Vertical = 2,
        Horizontal = 3,
        TLBR = 4,
        TRBL = 5,
        Cross = 6
    }
}