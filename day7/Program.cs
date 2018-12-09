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
                        step.IsBefore.Add(steps[dep.IsBefore]);
                    }
                }

                Console.WriteLine($"Step {step.Letter} is before: {string.Join(", ", step.IsBefore.Select(o => o.Letter))}");
            }

            var sequence = new List<Step>();
            var availableSteps = new HashSet<Step>(steps.Values);
            while (availableSteps.Any())
            {
                var candidateSteps = from s in availableSteps
                                     where !availableSteps.Any(o => o.IsBefore.Contains(s))
                                     select s;
                var selectedStep = candidateSteps.OrderBy(o => o.Letter).First();
                sequence.Add(selectedStep);
                availableSteps.Remove(selectedStep);
            }

            Console.WriteLine($"The order is: {string.Concat(sequence.Select(o => o.Letter))}");
        }
    }
}
