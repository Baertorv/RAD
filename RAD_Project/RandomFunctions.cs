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
            Span<byte> bytes = stackalloc byte[16];
            RandomNumberGenerator.Fill(bytes);

            BigInteger value = BitConverter.ToUInt64(bytes[..8]) |
                        ((BigInteger)BitConverter.ToUInt64(bytes[8..]) << 64);

            value &= p;

            // Rare: retry if it's all 1s (p)
            return value == p ? GenerateRandom89Bit() : value;
        }
    }
}