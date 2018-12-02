using System;
using System.Linq;
using System.Text;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var ids = System.IO.File.ReadAllLines("./input.txt").ToList();

            var idsWithCounts = from id in ids
                                select new
                                {
                                    Id = id,
                                    LetterCounts = id.GroupBy(o => o)
                                      .Select(o => new
                                      {
                                          Letter = o.Key,
                                          Count = o.Count()
                                      })
                                      .Where(o => o.Count > 1)
                                      .ToDictionary(o => o.Letter, o => o.Count)
                                };

            foreach (var id in idsWithCounts)
            {
                var output = new StringBuilder();
                output.Append(id.Id);
                foreach (var lc in id.LetterCounts)
                {
                    output.Append("\t");
                    output.Append($"{lc.Key}:{lc.Value}");
                }
                Console.WriteLine(output.ToString());
            }

            var countWithTwo = idsWithCounts.Where(o => o.LetterCounts.Any(lc => lc.Value == 2)).Count();
            var countWithThree = idsWithCounts.Where(o => o.LetterCounts.Any(lc => lc.Value == 3)).Count();

            Console.WriteLine();
            Console.WriteLine($"Count with two: {countWithTwo}");
            Console.WriteLine($"Count with three: {countWithThree}");

            var comparer = new IdComparer();

            var comparisons = from id1 in ids
                              from id2 in ids
                              where id1 != id2
                              select new
                              {
                                  Id1 = id1,
                                  Id2 = id2,
                                  Diff = comparer.Compare(id1, id2)
                              };

            foreach (var comparison in comparisons)
            {
                if (comparison.Diff == 1)
                {
                    Console.WriteLine($"{comparison.Id1} : {comparison.Id2} - {comparison.Diff} difference");
                }
            }
        }
    }
}
