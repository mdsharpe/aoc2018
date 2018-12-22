namespace day14
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Kitchen("37", 2).Find(9, 10);
            new Kitchen("37", 2).Find(5, 10);
            new Kitchen("37", 2).Find(18, 10);
            new Kitchen("37", 2).Find(2018, 10);
            new Kitchen("37", 2).Find(939601, 10);
        }
    }
}
