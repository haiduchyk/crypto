namespace Lab3
{
    public class Lcg
    {
        public const long M = 4294967296; 
        private readonly long a;
        private readonly long c;
        private long last;

        public Lcg(long a, long c, long seed)
        {
            this.a = a;
            this.c = c;
            last = seed;
        }

        public int Next()
        {
            last = (a * last + c) % M;
            return (int) last;
        }
    }
}