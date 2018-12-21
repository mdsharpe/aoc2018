using System;
using System.Collections.Generic;
using System.Linq;

namespace day13
{
    internal class Grid
    {
        private readonly CellType[,] _cells;

        private Grid(CellType[,] cells)
        {
            _cells = cells;
        }

        public static Grid Parse(string filePath)
        {
            var inputLines = System.IO.File.ReadAllLines(filePath);
            var width = inputLines.Max(o => o.Length);
            var height = inputLines.Length;

            var cells = new CellType[width, height];

            Direction? d;
            var trucks = new List<Direction>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    cells[x, y] = inputLines[y][x].ToCellType(out d);
                    if (d.HasValue)
                    {
                        trucks.Add(d.Value);
                    }
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (cells[x, y] == CellType.Unknown)
                    {
                        cells[x, y] = DetermineCellType(cells, x, y);
                    }
                }
            }

            return new Grid(cells);
        }

        private static CellType DetermineCellType(CellType[,] cells, int x, int y)
        {
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);
            
            var above = y > 0 ? cells[x, y - 1] : CellType.Empty;
            var toRight = x < width ? cells[x + 1, y] : CellType.Empty;
            var below = y < height ? cells[x, y + 1] : CellType.Empty;
            var toLeft = x > 0 ? cells[x - 1, y] : CellType.Empty;

            var connectsAbove = above == CellType.Vertical || above == CellType.TLBR || above == CellType.TRBL || above == CellType.Cross;
            var connectsBelow = below == CellType.Vertical || below == CellType.TLBR || below == CellType.TRBL || below == CellType.Cross;
            var connectsToRight = toRight == CellType.Horizontal || toRight == CellType.TLBR || toRight == CellType.TRBL || toRight == CellType.Cross;
            var connectsToLeft = toLeft == CellType.Horizontal || toLeft == CellType.TLBR || toLeft == CellType.TRBL || toLeft == CellType.Cross;

            if (connectsToRight && connectsToLeft && !connectsAbove && !connectsBelow)
            {
                return CellType.Horizontal;
            }

            if (connectsAbove && connectsBelow && !connectsToRight && !connectsToLeft)
            {
                return CellType.Vertical;
            }

            if (connectsAbove && connectsBelow && connectsToRight && connectsToLeft)
            {
                return CellType.Cross;
            }

            if ((connectsAbove && !connectsBelow && !connectsToRight && connectsToLeft)
                || (!connectsAbove && connectsBelow && connectsToRight && !connectsToLeft))
            {
                return CellType.TLBR;
            }

            if ((connectsAbove && !connectsBelow && connectsToRight && !connectsToLeft)
                || (!connectsAbove && connectsBelow && !connectsToRight && connectsToLeft))
            {
                return CellType.TRBL;
            }

            throw new Exception("Failed to determine unknown cell type.");
        }
    }
}