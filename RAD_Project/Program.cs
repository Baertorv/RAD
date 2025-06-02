using System.Diagnostics;
using System.Numerics;

namespace RAD_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 100_000_000;
            int l = 16;
            Console.WriteLine($"Testing hashing functions with n=100.000.000 and l={l}");

            var stream = StreamGenerator.CreateStream(n, l);

            ulong a_shift = RandomFunctions.GenerateOdd64Bit();
            var a_prime = RandomFunctions.GenerateRandom89Bit();
            var b_prime = RandomFunctions.GenerateRandom89Bit();

            ulong sumShift = 0;
            var swShift = Stopwatch.StartNew();
            foreach (var (key, _) in stream)
            {
                sumShift += HashFunctions.MultiplyShift(key, a_shift, l);
            }
            swShift.Stop();
            Console.WriteLine($"Multiply-Shift sum: {sumShift}");
            Console.WriteLine($"Multiply-Shift time: {swShift.ElapsedMilliseconds} ms");

            stream = StreamGenerator.CreateStream(n, l);

            ulong sumPrime = 0;
            var swPrime = Stopwatch.StartNew();
            foreach (var (key, _) in stream)
            {
                sumPrime += HashFunctions.MultiplyModPrime(key, a_prime, b_prime, l);
            }
            swPrime.Stop();
            Console.WriteLine($"Multiply-Mod-Prime sum: {sumPrime}");
            Console.WriteLine($"Multiply-Mod-Prime time: {swPrime.ElapsedMilliseconds} ms");
            
            for (int l2 = 4; ; l2++)
            {
                if ((1 << l2) > n)
                {
                    Console.WriteLine($"l^2 now greater than {n}, terminating");
                    break;
                }
                Console.WriteLine($"\nTesting l = {l2}, 2^{l2} keys:");

                var stream2 = StreamGenerator.CreateStream(n, l2);
                var table1 = new ChainedHashTable(l2, x => HashFunctions.MultiplyShift(x, a_shift, l2));
                var sw1 = Stopwatch.StartNew();
                var result1 = HashFunctions.ComputeSquareSum(stream2, table1);
                sw1.Stop();
                Console.WriteLine($"Multiply-Shift: S = {result1}, time = {sw1.ElapsedMilliseconds} ms");

                stream2 = StreamGenerator.CreateStream(n, l2);
                var table2 = new ChainedHashTable(l2, x => HashFunctions.MultiplyModPrime(x, a_prime, b_prime, l2));
                var sw2 = Stopwatch.StartNew();
                var result2 = HashFunctions.ComputeSquareSum(stream2, table2);
                sw2.Stop();
                Console.WriteLine($"Multiply-Mod-Prime: S = {result2}, time = {sw2.ElapsedMilliseconds} ms");
            }
        }
    }
}