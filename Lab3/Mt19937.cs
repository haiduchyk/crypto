﻿namespace Lab3
{
    public class Mt19937
    {
        // Period parameters
        public const ulong N = 624;
        private const ulong M = 397;
        private const ulong MATRIX_A = 0x9908B0DFUL; // constant vector a 
        private const ulong UPPER_MASK = 0x80000000UL; // most significant w-r bits
        private const ulong LOWER_MASK = 0X7FFFFFFFUL; // least significant r bits

        private static ulong[] mt = new ulong[N]; // the array for the state vector
        private static ulong mti = N + 1; // mti==N+1 means mt[N] is not initialized

        public Mt19937()
        {
            var init = new ulong[] {0x123, 0x234, 0x345, 0x456};
            ulong length = 4;
            init_by_array(init, length);
        }
        
        public void init_genrand(ulong[] states)
        {
            mt = states;
        }

        // initializes mt[N] with a seed
        public void init_genrand(ulong s)
        {
            mt[0] = s & 0xffffffffUL;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
                /* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
                /* In the previous versions, MSBs of the seed affect   */
                /* only MSBs of the array mt[].                        */
                /* 2002/01/09 modified by Makoto Matsumoto             */
                mt[mti] &= 0xffffffffUL;
                /* for >32 bit machines */
            }
        }

        // initialize by an array with array-length
        // init_key is the array for initializing keys
        // key_length is its length
        public void init_by_array(ulong[] init_key, ulong key_length)
        {
            ulong i, j, k;
            this.init_genrand(19650218UL);
            i = 1;
            j = 0;
            k = (N > key_length ? N : key_length);
            for (; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525UL))
                        + init_key[j] + j; // non linear 
                mt[i] &= 0xffffffffUL; // for WORDSIZE > 32 machines
                i++;
                j++;
                if (i >= N)
                {
                    mt[0] = mt[N - 1];
                    i = 1;
                }

                if (j >= key_length) j = 0;
            }

            for (k = N - 1; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941UL))
                        - i; // non linear
                mt[i] &= 0xffffffffUL; // for WORDSIZE > 32 machines
                i++;
                if (i >= N)
                {
                    mt[0] = mt[N - 1];
                    i = 1;
                }
            }

            mt[0] = 0x80000000UL; // MSB is 1; assuring non-zero initial array
        }

        // generates a random number on [0,0xffffffff]-interval
        public ulong genrand_int32()
        {
            ulong y = 0;
            ulong[] mag01 = new ulong[2];
            mag01[0] = 0x0UL;
            mag01[1] = MATRIX_A;
            /* mag01[x] = x * MATRIX_A  for x=0,1 */

            if (mti >= N)
            {
                // generate N words at one time
                ulong kk;

                if (mti == N + 1) /* if init_genrand() has not been called, */
                    init_genrand(5489UL); /* a default initial seed is used */

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }

                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    //mt[kk] = mt[kk+(M-N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                    mt[kk] = mt[kk - 227] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }

                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y = tempering(y);

            return y;
        }

        private ulong tempering(ulong y)
        {
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            return y;
        }
    }
}