namespace day14
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Kitchen("37", 2).FindDigitsAfter(9, 10);
            new Kitchen("37", 2).FindDigitsAfter(5, 10);
            new Kitchen("37", 2).FindDigitsAfter(18, 10);
            new Kitchen("37", 2).FindDigitsAfter(2018, 10);
            new Kitchen("37", 2).FindDigitsAfter(939601, 10);
            new Kitchen("37", 2).CountDigitsBefore("51589");
            new Kitchen("37", 2).CountDigitsBefore("01245");
            new Kitchen("37", 2).CountDigitsBefore("92510");
            new Kitchen("37", 2).CountDigitsBefore("59414");
            new Kitchen("37", 2).CountDigitsBefore("939601");
        }
    }
}
