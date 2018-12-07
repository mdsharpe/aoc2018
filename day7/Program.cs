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
            var dependencies = (from dependencyString in System.IO.File.ReadAllLines("./input.txt")
                                let match = InputParser.Match(dependencyString)
                                select new
                                {
                                    StepLetter = match.Groups["s2"].Value.Single(),
                                    DependsOn = match.Groups["s1"].Value.Single()
                                }).ToArray();

            var steps = (from l in Enumerable.Concat(
                            dependencies.Select(o => o.StepLetter),
                            dependencies.Select(o => o.DependsOn))
                            .Distinct()
                         select new Step(l))
                        .ToDictionary(o => o.Letter, o => o);

            Console.WriteLine($"Steps: {string.Concat(steps.Keys.OrderBy(o => o))}");

            foreach (var step in steps.Values.OrderBy(o => o.Letter))
            {
                foreach (var dep in dependencies.Where(o => o.StepLetter == step.Letter))
                {
                    step.Dependencies.Add(steps[dep.DependsOn]);
                }

                Console.WriteLine($"Step {step.Letter} depends on {string.Join(", ", step.Dependencies.Select(o => o.Letter))}");
            }

            var sequence = steps.Values
                .OrderBy(o => o, new StepComparer())
                .ThenBy(o => o.Letter);
                
            Console.WriteLine($"Steps ordered: {string.Concat(sequence.Select(o => o.Letter))}");
        }
    }
}
