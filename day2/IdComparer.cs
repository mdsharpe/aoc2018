using System.Collections.Generic;

namespace day2
{
    class IdComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var differences = 0;

            for (var i = 0; i < x.Length; i++){
                var cx = x[i];

                if (i > y.Length - 1) {
                    differences++;
                    continue;
                }

                var cy = y[i];

                if (cx != cy) {
                    differences++;
                }
            }

            return differences;
        }
    }
}