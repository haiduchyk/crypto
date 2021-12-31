namespace Lab3
{
    using System;

    internal static class Program
    {
        private const long MoneyToWin = 1_000_000;
        private const long BetAmount = 100;

        private static void Main()
        {
            // LcgTask();
            // MtTask();
            // MtBetter();
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

            while (CasinoRequestProvider.Money < MoneyToWin)
            {
                var next = lcg.Next();
                var result = CasinoRequestProvider.Play(BetAmount, next, Mode.Lcg);
                Console.WriteLine(result);
            }
        }

        private static void MtTask()
        {
            var mt19937 = GetTunedMt();
            while (CasinoRequestProvider.Money < MoneyToWin)
            {
                var currentNumber = (long) mt19937.genrand_int32();
                var result = CasinoRequestProvider.Play(BetAmount, currentNumber, Mode.Mt);
                Console.WriteLine(result);
            }
        }

        private static Mt19937 GetTunedMt()
        {
            var seed = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var mt19937 = new Mt19937();

            var currentNumber = (long) mt19937.genrand_int32();
            var targetNumber = CasinoRequestProvider.Play(BetAmount, 1, Mode.Mt).RealNumber;

            while (currentNumber != targetNumber)
            {
                seed++;
                mt19937.init_genrand((ulong) seed);
                currentNumber = (long) mt19937.genrand_int32();
            }

            return mt19937;
        }
        
        private static void MtBetter()
        {
            
        }

    }
}