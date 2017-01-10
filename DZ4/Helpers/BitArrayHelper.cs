using System;
using System.Collections;
using System.Text;

namespace APR.DZ4
{
    public static class BitArrayHelper
    {
        public static BitArray CreateRandom(int length)
        {
            bool[] bits = new bool[length];

            for (int j = 0; j < bits.Length; j++)
            {
                bits[j] = RandomNumberGeneratorProvider.Instance.NextDouble() > 0.5;
            }

            return new BitArray(bits);
        }

        public static string ToBinaryString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Length; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}