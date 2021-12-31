namespace Lab3
{
    public static class MtCracker
    {
        public static ulong[] CalulateCurrentState(ulong[] sequence)
        {
            var state = new ulong[Mt19937.N];
            for (var i = 0; i < state.Length; i++)
            {
                state[i] = Distempering(sequence[i]);
            }
            return state;
        }
        
        // https://www.maths.tcd.ie/~fionn/misc/mt/
        private static ulong Distempering(ulong value)
        {
            var y = value;

            // # Inverse of y = y ^ (y >> 18)
            y ^= (y >> 18);

            // # Inverse of y = y ^ ((y << 15) & 0xefc60000)
            y ^= ((y & 0x1df8c) << 15);

            // # Inverse of y = y ^ ((y << 7) & 0x9d2c5680)
            var t = y;
            t = ((t & 0x0000002d) << 7) ^ y;
            t = ((t & 0x000018ad) << 7) ^ y;
            t = ((t & 0x001a58ad) << 7) ^ y;
            y = ((t & 0x013a58ad) << 7) ^ y;

            // Inverse of y = y ^ (y >> 11)
            var top = y & 0xffe00000;
            var mid = y & 0x001ffc00;
            var low = y & 0x000003ff;

            y = top | ((top >> 11) ^ mid) | ((((top >> 11) ^ mid) >> 11) ^ low);

            return y;
        }
    }
}