using System;
using System.IO;
using System.Linq;

namespace day6
{
    class Program
    {
        static readonly string OutFileName = "./output.txt";

        static void Main(string[] args)
        {
            var letter = 'A';
            var points = File.ReadAllLines("./input.txt")
                .Select(o =>
                {
                    var p = Point.Parse(o, letter++);
                    if (letter > 'Z')
                        letter = 'A';
                    return p;
                })
                .ToArray();

            Console.WriteLine($"{points.Length} points in input.");

            var minX = points.Min(o => o.X);
            var minY = points.Min(o => o.Y);
            var maxX = points.Max(o => o.X);
            var maxY = points.Max(o => o.Y);

            Console.WriteLine($"Grid: ({minX}, {minY}) - ({maxX}, {maxY}).");

            var coords = (from y in Enumerable.Range(minY, maxY - minY)
                          from x in Enumerable.Range(minX, maxX - minX)
                          let coord = new Point(x, y)
                          let nearestPoints = (from point in points
                                               let distance = coord.GetManhattanDistanceTo(point)
                                               group point by distance into pointsByDistance
                                               select new
                                               {
                                                   Distance = pointsByDistance.Key,
                                                   Points = pointsByDistance.AsEnumerable()
                                               }).OrderBy(o => o.Distance).First()
                          select new
                          {
                              Point = coord,
                              IsInputPoint = points.Any(p => coord.Equals(p)),
                              NearestPoints = nearestPoints.Points.ToArray()
                          }).ToArray();

            if (File.Exists(OutFileName))
                File.Delete(OutFileName);

            using (var outFile = File.OpenWrite(OutFileName))
            using (var outWriter = new StreamWriter(outFile, System.Text.Encoding.UTF8))
            {
                var y = 0;
                foreach (var coord in coords)
                {
                    if (y != coord.Point.Y)
                    {
                        outWriter.WriteLine();
                        y = coord.Point.Y;
                    }

                    char outChar;

                    if (coord.NearestPoints.Length == 1)
                    {
                        outChar = coord.NearestPoints.Single().Letter;

                        if (!coord.IsInputPoint)
                        {
                            outChar = Char.ToLowerInvariant(outChar);
                        }
                    }
                    else
                    {
                        outChar = '.';
                    }

                    outWriter.Write(outChar);
                }
            }

            var areas = from coord in coords
                        where coord.NearestPoints.Count() == 1
                        group coord by coord.NearestPoints.Single() into area
                        select new
                        {
                            Center = area.Key,
                            Points = area.AsEnumerable(),
                            Size = area.Count()
                        };

            var areasNotInfinite = from a in areas
                                   where !a.Points.Any(o => o.Point.X == minX)
                                   where !a.Points.Any(o => o.Point.X == maxX)
                                   where !a.Points.Any(o => o.Point.Y == minY)
                                   where !a.Points.Any(o => o.Point.Y == maxY)
                                   select a;

            var largestArea = areasNotInfinite.OrderByDescending(o => o.Size).First();

            Console.WriteLine($"Largest area is of size {largestArea.Size}.");
        }
    }
}
