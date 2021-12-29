namespace Lab3
{
    using System;

    internal static class Program
    {
        private const long MoneyToWin = 1_000_000;
        private const long BetAmount = 100;

        private static void Main()
        {
            LcgTask();
        }

        private static void LcgTask()
        {
            var numbers = new long[3];
            for (var i = 0; i < 3; i++)
            {
                var result = CasinoRequestProvider.Play(BetAmount, 1, Mode.Lcg);
                numbers[i] = result.RealNumber;
            }

            var (a, c) = LcgCracker.GetCoefs(numbers);
            var lcg = new Lcg(a, c, numbers[2]);

            while (CasinoRequestProvider.account.Money < MoneyToWin)
            {
                var next = lcg.Next();
                var result = CasinoRequestProvider.Play(BetAmount, next, Mode.Lcg);
                Console.WriteLine(result);
            }
        }
    }
}