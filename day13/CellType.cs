namespace day13
{
    internal enum CellType : byte
    {
        Unknown = 0,
        Empty = 1,
        Vertical = 2,
        Horizontal = 3,
        Cross = 4,
        TLBR = 5,
        TRBL = 6,
        TL = 7,
        TR = 8,
        BR = 9,
        BL = 10
    }
}