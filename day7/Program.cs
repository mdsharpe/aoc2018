using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace day7
{
    class Program
    {
        private static readonly int WorkerCount = 2;
        private static readonly int StepBaseDuration = 0;
        private static readonly Regex InputParser = new Regex("^Step (?<s1>[A-Z]) must be finished before step (?<s2>[A-Z]) can begin.$");

        static void Main(string[] args)
        {
            var dependencies = (from dependencyString in System.IO.File.ReadAllLines("./input.example.txt")
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
                         select new Step(l, StepBaseDuration))
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

                Console.WriteLine($"Step {step.Letter} ({step.Duration}s) is before: {string.Join(", ", step.IsBefore.Select(o => o.Letter))}");
            }

            var linearSequence = new List<Step>();
            var availableSteps = new HashSet<Step>(steps.Values);
            while (availableSteps.Any())
            {
                var candidateSteps = from s in availableSteps
                                     where !availableSteps.Any(o => o.IsBefore.Contains(s))
                                     orderby s.Letter
                                     select s;
                var selectedStep = candidateSteps.First();
                linearSequence.Add(selectedStep);
                availableSteps.Remove(selectedStep);
            }

            Console.WriteLine($"Linear order: {string.Concat(linearSequence.Select(o => o.Letter))}");

            var multiWorkerSequence = new List<Step>();
            var second = 0;
            availableSteps = new HashSet<Step>(steps.Values);
            var workerAssignments = Enumerable.Range(0, WorkerCount).ToDictionary(o => o, o => (WorkerAssignment)null);
            Console.WriteLine("Sec\t" + string.Join("\t", Enumerable.Range(0, WorkerCount).Select(o => $"W{o}")) + "\tDone");

            do
            {
                var candidateSteps = (from s in availableSteps
                                      where !availableSteps.Any(o => o.IsBefore.Contains(s))
                                      orderby s.Letter
                                      select s).ToArray();

                foreach (var candidateStep in candidateSteps)
                {
                    var worker = workerAssignments.Where(o => o.Value == null)
                        .Select(o => new { Id = o.Key })
                        .FirstOrDefault();

                    if (worker != null)
                    {
                        workerAssignments[worker.Id] = new WorkerAssignment
                        {
                            Step = candidateStep,
                            Ttl = candidateStep.Duration
                        };

                        // TODO Don't remove from available steps 'til done.
                        availableSteps.Remove(candidateStep);
                    }
                    else
                    {
                        break;
                    }
                }

                var output = new StringBuilder();
                output.Append($"{second}\t");
                foreach (var worker in workerAssignments.OrderBy(o => o.Key))
                {
                    output.Append(string.Concat(worker.Value != null ? worker.Value.Step.Letter : '.', '\t'));
                }
                output.Append($"{string.Concat(multiWorkerSequence.Select(o => o.Letter))}");
                Console.WriteLine(output.ToString());

                foreach (var worker in workerAssignments.Where(o => o.Value != null).ToArray())
                {
                    worker.Value.Ttl--;
                    if (worker.Value.Ttl == 0)
                    {
                        multiWorkerSequence.Add(worker.Value.Step);
                        workerAssignments[worker.Key] = null;
                    }
                }

                second++;
            } while (availableSteps.Any() || workerAssignments.Any(o => o.Value != null));
        }
    }
}
