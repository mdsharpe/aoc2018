using System;
using System.Collections.Generic;
using System.Text;

namespace day7
{
    class StepComparer : IComparer<Step>
    {
        public int Compare(Step x, Step y)
        {
            int diff;

            if (FindDependency(x, y, 1, out diff))
            {
                diff *= -1;
                Console.WriteLine($"[Comparison] {x.Letter} comes before {y.Letter} by {diff}.");
            }
            else if (FindDependency(y, x, 1, out diff))
            {
                Console.WriteLine($"[Comparison] {x.Letter} comes after {y.Letter} by {diff}.");
            }
            else
            {
                diff = 0;
                Console.WriteLine($"[Comparison] No dependency found for {x.Letter} and {y.Letter}.");
            }

            return diff;
        }

        private bool FindDependency(Step x, Step y, int curDepth, out int depth)
        {
            depth = curDepth;

            if (y.Dependencies.Contains(x))
            {
                return true;
            }

            foreach (var dep in y.Dependencies)
            {
                if (FindDependency(x, dep, curDepth + 1, out int innerDepth))
                {
                    depth++;
                    return true;
                }
            }

            return false;
        }
    }
}