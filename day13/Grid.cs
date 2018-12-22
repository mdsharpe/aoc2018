using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day13
{
    internal class Grid
    {
        private readonly CellType[,] _cells;
        private readonly ICollection<Truck> _trucks;

        private Grid(CellType[,] cells, IEnumerable<Truck> trucks)
        {
            _cells = cells;
            _trucks = trucks.ToList();
        }

        public static Grid Parse(string filePath)
        {
            var inputLines = System.IO.File.ReadAllLines(filePath);
            var width = inputLines.Max(o => o.Length);
            var height = inputLines.Length;

            var cells = new CellType[width, height];

            Direction? d;
            var trucks = new List<Truck>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    cells[x, y] = inputLines[y][x].ToCellType(out d);
                    if (d.HasValue)
                    {
                        trucks.Add(new Truck
                        {
                            X = x,
                            Y = y,
                            Direction = d.Value
                        });
                    }
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (cells[x, y] == CellType.Unknown)
                    {
                        cells[x, y] = ResolveUnknownCell(cells, x, y);
                    }
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (cells[x, y] == CellType.TLBR || cells[x, y] == CellType.TRBL)
                    {
                        cells[x, y] = ResolveCornerCell(cells, x, y);
                    }
                }
            }

            return new Grid(cells, trucks);
        }

        public async Task Run(bool verbose = false, bool writeMapToFile = false, bool removeCollidedTrucks = false)
        {
            if (writeMapToFile)
            {
                System.IO.File.WriteAllText(
                    $"output_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt",
                    GetTrackString(trackOnly: true, fitToConsole: false));
            }

            if (verbose)
            {
                Console.Clear();
                Console.Write(GetTrackString(fitToConsole: true));
            }

            Task delay = null;

            var completed = false;

            do
            {
                if (verbose)
                {
                    delay = Task.Delay(50);
                }

                var truckSequence = _trucks
                    .OrderBy(o => o.Y)
                    .ThenBy(o => o.X)
                    .ToArray();

                foreach (var truck in truckSequence)
                {
                    if (!_trucks.Contains(truck)) continue;

                    MoveTruck(truck);

                    if (removeCollidedTrucks)
                    {
                        var collidedWith = GetCollidedWith(truck);
                        if (collidedWith != null)
                        {
                            _trucks.Remove(truck);
                            _trucks.Remove(collidedWith);
                        }
                    }
                    else
                    {
                        if (GetIsCollided(truck))
                        {
                            completed = true;
                            Console.Clear();
                            Console.WriteLine($"Collision at {truck.X}, {truck.Y}.");
                            break;
                        }
                    }
                }

                if (removeCollidedTrucks && _trucks.Count == 1)
                {
                    completed = true;
                    Console.Clear();
                    var remainingTruck = _trucks.Single();
                    Console.WriteLine($"Single remaining truck at at {remainingTruck.X}, {remainingTruck.Y}.");
                }

                if (verbose && !completed)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write(GetTrackString(fitToConsole: true));
                    await delay;
                }
            } while (!completed);
        }

        private void MoveTruck(Truck truck)
        {
            var currentCellType = _cells[truck.X, truck.Y];

            var possibleDirections = currentCellType.GetPossibleExits();
            possibleDirections.Remove(truck.Direction.GetOpposite());

            if (possibleDirections.Count == 1)
            {
                truck.Direction = possibleDirections.Single();
            }
            else if (possibleDirections.Count == 3)
            {
                truck.Direction = truck.GetNextTurnDirection();
            }
            else
            {
                throw new InvalidOperationException($"Unexpected count of possible directions ({possibleDirections.Count}).");
            }

            var nextLocation = truck.GetNextLocation();
            truck.X = nextLocation.x;
            truck.Y = nextLocation.y;
        }

        private bool GetIsCollided(Truck truck)
            => _trucks.Any(o => o != truck && o.X == truck.X && o.Y == truck.Y);

        private Truck GetCollidedWith(Truck truck)
            => _trucks.FirstOrDefault(o => o != truck && o.X == truck.X && o.Y == truck.Y);

        private string GetTrackString(bool trackOnly = false, bool fitToConsole = false)
        {
            var width = GetWidth();
            var height = GetHeight();

            if (fitToConsole)
            {
                width = Math.Min(width, Console.WindowWidth - 1);
                height = Math.Min(height, Console.WindowHeight - 1);
            }

            var output = new StringBuilder();

            Truck truck;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    truck = _trucks.FirstOrDefault(o => o.X == x && o.Y == y);
                    if (truck != null && !trackOnly)
                    {
                        output.Append(truck.ToChar());
                    }
                    else
                    {
                        output.Append(_cells[x, y].ToChar());
                    }
                }

                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }

        private static CellType ResolveUnknownCell(CellType[,] cells, int x, int y)
        {
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);

            var above = y > 0 ? cells[x, y - 1] : CellType.Empty;
            var toRight = x < width - 1 ? cells[x + 1, y] : CellType.Empty;
            var below = y < height - 1 ? cells[x, y + 1] : CellType.Empty;
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

            throw new Exception("Failed to resolve unknown cell.");
        }

        private static CellType ResolveCornerCell(CellType[,] cells, int x, int y)
        {
            CellType? ResolveCornerConnection((bool a, bool b, bool r, bool l) c)
            {
                if (c.a && !c.b && c.r && !c.l) return CellType.BL;
                if (c.a && !c.b && !c.r && c.l) return CellType.BR;
                if (!c.a && c.b && c.r && !c.l) return CellType.TL;
                if (!c.a && c.b && !c.r && c.l) return CellType.TR;
                return null;
            }

            var width = cells.GetLength(0);
            var height = cells.GetLength(1);

            var above = y > 0 ? cells[x, y - 1] : CellType.Empty;
            var toRight = x < width - 1 ? cells[x + 1, y] : CellType.Empty;
            var below = y < height - 1 ? cells[x, y + 1] : CellType.Empty;
            var toLeft = x > 0 ? cells[x - 1, y] : CellType.Empty;

            (bool a, bool b, bool r, bool l) GetConnections(bool allowAmbiguousAdjacentCorners)
            {
                var connectsAbove = above == CellType.Vertical || above == CellType.TL || above == CellType.TR || above == CellType.Cross
                    || (allowAmbiguousAdjacentCorners && (above == CellType.TLBR || above == CellType.TRBL));
                var connectsBelow = below == CellType.Vertical || above == CellType.BL || below == CellType.BR || below == CellType.Cross
                    || (allowAmbiguousAdjacentCorners && (below == CellType.TLBR || below == CellType.TRBL));
                var connectsRight = toRight == CellType.Horizontal || toRight == CellType.TR || toRight == CellType.BR || toRight == CellType.Cross
                    || (allowAmbiguousAdjacentCorners && (toRight == CellType.TLBR || toRight == CellType.TRBL));
                var connectsLeft = toLeft == CellType.Horizontal || toLeft == CellType.TL || toLeft == CellType.BL || toLeft == CellType.Cross
                    || (allowAmbiguousAdjacentCorners && (toLeft == CellType.TLBR || toLeft == CellType.TRBL));
                return (connectsAbove, connectsBelow, connectsRight, connectsLeft);
            }

            var result = ResolveCornerConnection(GetConnections(false));
            if (result == null)
            {
                result = ResolveCornerConnection(GetConnections(true));
            }

            if (result != null)
            {
                return result.Value;
            }

            throw new Exception($"Failed to resolve corner cell at {x}, {y}.");
        }

        private int GetWidth()
        {
            return _cells.GetLength(0);
        }

        private int GetHeight()
        {
            return _cells.GetLength(1);
        }
    }
}
