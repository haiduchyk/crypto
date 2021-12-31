namespace Lab3
{
    using System;
    using System.Collections;
    using System.Linq;

    public class MtCracker
    {
        public static ulong InverseOfXorRightShift(ulong value, int shifts)
        {
            while (shifts < 32)
            {
                value ^= value >> shifts;
                shifts *= 2;
            }
            return value;
        }
        
        // 
        // abcdefgh ^
        // 0abcdefg =
        // ijklmnop
        
        
        public static long UndoXorLeftShiftMask(long v, int shift, long mask2)
        {
            var y = new BitArray(BitConverter.GetBytes(v)).Cast<bool>().Take(32).ToArray();;
                
            var mask = new BitArray(BitConverter.GetBytes(mask2)).Cast<bool>().Take(32).ToArray();;
            

            y = y.Reverse().ToArray();
            mask = mask.Reverse().ToArray();
                
                
            var x = new bool[32];

            for (var n = 0; n < 32; n++)
            {
                if (n < shift) {
                    x[n] = y[n];
                } else {
                    x[n] = y[n] ^ mask[n] & x[n - shift];
                }
            }
            
            return BoolArrayToInt(x.Reverse().ToArray());
        }
        
        static long BoolArrayToInt(bool[] bits){
            if(bits.Length > 32) throw new ArgumentException("Can only fit 32 bits in a uint");
 
            long r = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    r |= (uint) (1 << ( bits.Length - i));
                }
            }

            return r;
        }
    }
}