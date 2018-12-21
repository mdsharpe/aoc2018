using System;
using System.Collections.Generic;

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

        public static char ToChar(this CellType @this)
        {
            switch (@this)
            {
                case CellType.Empty:
                    return ' ';
                case CellType.Vertical:
                    return '║';
                case CellType.Horizontal:
                    return '═';
                case CellType.Cross:
                    return '╬';
                case CellType.TLBR:
                    return '/';
                case CellType.TRBL:
                    return '\\';
                case CellType.TL:
                    return '╔';
                case CellType.TR:
                    return '╗';
                case CellType.BR:
                    return '╝';
                case CellType.BL:
                    return '╚';
                default:
                case CellType.Unknown:
                    return '?';
            }
        }

        public static char ToChar(this Direction @this)
        {
            switch (@this)
            {
                case Direction.Up:
                    return '▵';
                case Direction.Right:
                    return '▹';
                case Direction.Down:
                    return '▿';
                case Direction.Left:
                    return '◃';
                default:
                    return '▫';
            }
        }

        public static char ToChar(this Truck @this)
        {
            return @this.Direction.ToChar();
        }

        public static HashSet<Direction> GetPossibleExits(this CellType @this)
        {
            var dirs = new HashSet<Direction>();

            switch (@this)
            {
                case CellType.Vertical:
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Down);
                    break;
                case CellType.Horizontal:
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Right);
                    break;
                case CellType.Cross:
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Right);
                    break;
                case CellType.TL:
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Right);
                    break;
                case CellType.TR:
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Left);
                    break;
                case CellType.BR:
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Left);
                    break;
                case CellType.BL:
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Right);
                    break;
                default:
                    break;
            }

            return dirs;
        }

        public static Direction GetOpposite(this Direction @this)
        {
            switch (@this)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return @this;
            }
        }

        public static Direction GetTurnDirection(this Direction @this, bool right)
        {
            switch (@this)
            {
                case Direction.Up:
                    return right ? Direction.Right : Direction.Left;
                case Direction.Right:
                    return right ? Direction.Down : Direction.Up;
                case Direction.Down:
                    return right ? Direction.Left : Direction.Right;
                case Direction.Left:
                    return right ? Direction.Up : Direction.Down;
                default:
                    return @this;
            }
        }
    }
}
