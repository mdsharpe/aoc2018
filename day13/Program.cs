using System;
using System.Threading.Tasks;

namespace day13
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // await Grid.Parse("input.example.txt").Run(verbose: true);
            // await Grid.Parse("input.txt").Run(verbose: false, writeMapToFile: true);
            // await Grid.Parse("input.example2.txt").Run(verbose: true, removeCollidedTrucks: true);
            await Grid.Parse("input.txt").Run(removeCollidedTrucks: true);
        }
    }
}
