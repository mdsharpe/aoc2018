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

            if (GetIsBefore(x, y, 1, out diff))
            {
                Console.WriteLine($"[Comparison] {x.Letter} is before {y.Letter} by {diff}.");
                diff *= -1;
            }
            else if (GetIsBefore(y, x, 1, out diff))
            {
                Console.WriteLine($"[Comparison] {x.Letter} is after {y.Letter} by {diff}.");
            }
            else
            {
                diff = 0;
                Console.WriteLine($"[Comparison] No dependency found for {x.Letter} and {y.Letter}.");
            }

            return diff;
        }

        private bool GetIsBefore(Step x, Step y, int curDepth, out int depth)
        {
            depth = curDepth;

            if (x.IsBefore.Contains(y))
            {
                return true;
            }

            foreach (var dep in x.IsBefore)
            {
                if (GetIsBefore(dep, y, curDepth + 1, out int innerDepth))
                {
                    depth++;
                    return true;
                }
            }

            return false;
        }
    }
}