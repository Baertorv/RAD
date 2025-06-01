namespace RAD_Project
{
    public static class HashFunctions
    {
        private const int q = 89;
        private static readonly UInt128 p = (UInt128.One << q) - 1;
        public static ulong MultiplyShift(ulong x, ulong a, int l)
        {
            if (l < 1 || l >= 64)
                throw new ArgumentOutOfRangeException(nameof(l), "l must be between 1 and 63");

            ulong result = (a * x) >> (64 - l);
            return result;
        }

        public static ulong MultiplyModPrime(ulong x, UInt128 a, UInt128 b, int l)
        {
            UInt128 y = a * x + b;
            y = ModP(y);
            return (ulong)(y & ((1UL << l) - 1));
        }

        private static UInt128 ModP(UInt128 x)
        {
            var y = (x & p) + (x >> q);
            if (y >= p) y -= p;
            return y;
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