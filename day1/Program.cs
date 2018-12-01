using System;
using System.Threading.Tasks;

namespace day1
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var o = new FrequencyCalibrator();
            var frequency = await o.GetFrequency();
            Console.WriteLine(frequency);
        }
    }
}
