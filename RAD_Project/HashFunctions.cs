using System.Numerics;

namespace RAD_Project
{
    public static class HashFunctions
    {
        private const int q = 89;
        private static readonly BigInteger p = (BigInteger.One << q) - 1;
        public static ulong MultiplyShift(ulong x, ulong a, int l)
        {
            if (l < 1 || l >= 64)
                throw new ArgumentOutOfRangeException(nameof(l), "l must be between 1 and 63");

            ulong result = (a * x) >> (64 - l);
            return result;
        }

        public static ulong MultiplyModPrime(ulong x, BigInteger a, BigInteger b, int l)
        {
            BigInteger y = a * x + b;
            y = (y & p) + (y >> q);
            if (y >= p) y -= p;
            return (ulong)(y & ((1UL << l) - 1));
        }

        public static long ComputeSquareSum(IEnumerable<Tuple<ulong, int>> stream, ChainedHashTable table)
        {
            foreach (var pair in stream)
                table.Increment(pair.Item1, pair.Item2);

            long total = 0;
            foreach (var bucket in table.GetAllBuckets())
            {
                foreach (var (key, value) in bucket)
                    total += value * value;
            }

            return total;
        }
    }
}