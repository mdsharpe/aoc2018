using System;
using System.Threading.Tasks;

namespace day15
{
    internal class Program
    {
        public static async Task Main()
        {
            // await Fight.Parse("input.example2.txt").Run(verbose: true);
            await Fight.Parse("input.txt").Run(verbose: true);
        }
    }
}
