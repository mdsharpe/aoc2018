using System;
using System.Collections.Generic;
using System.Linq;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var claims = System.IO.File.ReadAllLines("./input.txt")
                .Select(o => Claim.Parse(o))
                .ToList();

            // FindClaimOverlapSquares(claims);

            FindNonOverlappedClaim(claims);
        }

        private static void FindClaimOverlapSquares(ICollection<Claim> claims) {
            var width = claims.Max(o => o.X2);
            var height = claims.Max(o => o.Y2);

            Console.WriteLine($"Size: {width} x {height}");

            var getOverlaps = from y in Enumerable.Range(1, height)
                                  .AsParallel()
                              from x in Enumerable.Range(1, width)
                              from c in claims
                              where c.Contains(x, y)
                              group c by new { x, y } into g
                              select new
                              {
                                  X = g.Key.x,
                                  Y = g.Key.y,
                                  Count = g.Count()
                              };

            var countSquaresTwoOrMoreClaims = getOverlaps
                .Where(o => o.Count > 1)
                .Count();

            Console.WriteLine($"{countSquaresTwoOrMoreClaims} squares have two or more claims.");
        }

        private static void FindNonOverlappedClaim(List<Claim> claims)
        {
            var nonOverlappedClaim = claims
                .Where(o => !claims.Any(c => c != o && o.Intersect(c)))
                .Single();

            Console.WriteLine($"Claim #{nonOverlappedClaim.Id} does not overlap.");
        }
    }
}
