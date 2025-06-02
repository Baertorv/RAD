using System.Security.Cryptography;
using System.Numerics;
namespace RAD_Project
{
    public static class RandomFunctions
    {
        private const int q = 89;
        private static readonly BigInteger p = (BigInteger.One << q) - 1;
        public static ulong GenerateOdd64Bit()
        {
            Span<byte> buffer = stackalloc byte[8];
            RandomNumberGenerator.Fill(buffer);

            ulong value = BitConverter.ToUInt64(buffer);

            value |= 1;

            return value;
        }

        public static BigInteger GenerateRandom89Bit()
        {
            Span<byte> buffer = stackalloc byte[12];
            BigInteger value;

            do
            {
                RandomNumberGenerator.Fill(buffer);
                buffer[^1] &= 0x1F;

                value = new BigInteger(buffer, isUnsigned: true, isBigEndian: true);
            }
            while (value >= p);

            return value;
        }
    }
}