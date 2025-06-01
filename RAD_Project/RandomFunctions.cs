using System.Security.Cryptography;
namespace RAD_Project
{
    public static class RandomFunctions
    {
        private const int q = 89;
        private static readonly UInt128 p = (UInt128.One << q) - 1;
        public static ulong GenerateOdd64Bit()
        {
            Span<byte> buffer = stackalloc byte[8];
            RandomNumberGenerator.Fill(buffer);

            ulong value = BitConverter.ToUInt64(buffer);

            value |= 1;

            return value;
        }

        public static UInt128 GenerateRandom89Bit()
        {
            Span<byte> bytes = stackalloc byte[16];
            RandomNumberGenerator.Fill(bytes);

            UInt128 value = BitConverter.ToUInt64(bytes[..8]) |
                        ((UInt128)BitConverter.ToUInt64(bytes[8..]) << 64);

            value &= (UInt128.One << q) - 1;

            // Rare: retry if it's all 1s (p)
            return value == p ? GenerateRandom89Bit() : value;
        }
    }
}