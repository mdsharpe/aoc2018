using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day7
{
    class Program
    {
        private static readonly Regex InputParser = new Regex("^Step (?<s1>[A-Z]) must be finished before step (?<s2>[A-Z]) can begin.$");

        static void Main(string[] args)
        {
            var dependencies = (from dependencyString in System.IO.File.ReadAllLines("./input.1.txt")
                                let match = InputParser.Match(dependencyString)
                                select new
                                {
                                    StepLetter = match.Groups["s1"].Value.Single(),
                                    IsBefore = match.Groups["s2"].Value.Single()
                                })
                                .GroupBy(o => o.StepLetter)
                                .ToDictionary(o => o.Key, o => o.ToArray());

            var steps = (from l in Enumerable.Concat(
                            dependencies.SelectMany(o => o.Value).Select(o => o.StepLetter),
                            dependencies.SelectMany(o => o.Value).Select(o => o.IsBefore))
                            .Distinct()
                         select new Step(l))
                         .ToDictionary(o => o.Letter);

            foreach (var step in steps.Values.OrderBy(o => o.Letter))
            {
                if (dependencies.TryGetValue(step.Letter, out var deps))
                {
                    foreach (var dep in deps)
                    {
                        if (steps.TryGetValue(dep.IsBefore, out var depStep))
                        {
                            step.IsBefore.Add(depStep);
                        }
                    }
                }

                Console.WriteLine($"Step {step.Letter} is before: {string.Join(", ", step.IsBefore.Select(o => o.Letter))}");
            }

            var sequence = steps.Values
                .OrderBy(o => o, new StepComparer())
                .ThenBy(o => o.Letter);

            Console.WriteLine($"The order is: {string.Concat(sequence.Select(o => o.Letter))}");
        }
    }
}
