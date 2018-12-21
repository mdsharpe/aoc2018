using System;

namespace day13
{
    internal static class Extensions
    {
        public static CellType ToCellType(this char @this, out Direction? truckDirection)
        {
            truckDirection = null;

            switch (@this)
            {
                case '|':
                    return CellType.Vertical;
                case '-':
                    return CellType.Horizontal;
                case '/':
                    return CellType.TLBR;
                case '\\':
                    return CellType.TRBL;
                case '+':
                    return CellType.Cross;
                case ' ':
                    return CellType.Empty;
                case '>':
                    truckDirection = Direction.Right;
                    return CellType.Unknown;
                case '<':
                    truckDirection = Direction.Left;
                    return CellType.Unknown;
                case '^':
                    truckDirection = Direction.Up;
                    return CellType.Unknown;
                case 'v':
                    truckDirection = Direction.Down;
                    return CellType.Unknown;
                default:
                    throw new InvalidOperationException($"'{@this}' is not a recognized cell type.");
            }
        }
    }
}