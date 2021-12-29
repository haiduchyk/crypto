namespace Lab3
{
    public static class LcgCracker
    {
        public static (long a, long c) GetCoefs(long[] numbers)
        {
            var x0 = numbers[0];
            var x1 = numbers[1];
            var x2 = numbers[2];

            var a = (x2 - x1) * ModInverse(x1 - x0, Lcg.M) % Lcg.M;
            var c = (x1 - a * x0) % Lcg.M;

            return (a, c);
        }

        // https://stackoverflow.com/questions/7483706/c-sharp-modinverse-function
        private static long ModInverse(long a, long n)
        {
            long i = n, v = 0, d = 1;
            while (a > 0)
            {
                long t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }

            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}