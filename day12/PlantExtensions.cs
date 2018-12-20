using System;
using System.Collections.Generic;
using System.Linq;

namespace day12
{
    static class PlantExtensions
    {
        public static string ToPlantString(this IEnumerable<bool> @this)
            => string.Concat(
                @this.Select(o => o ? '#' : '.')
            );

        public static int GetIndent(this bool[] @this, bool back = false)
            => !back ? Array.IndexOf(@this, true) : Array.LastIndexOf(@this, true);

        public static bool[] ToPattern(this bool[] @this)
        {
            var frontIndent = @this.GetIndent();
            var backIndent = @this.GetIndent(true);

            return @this
                   .Skip(@this.GetIndent())
                   .TakeWhile((o, i) => i <= backIndent)
                   .ToArray();
        }
    }
}