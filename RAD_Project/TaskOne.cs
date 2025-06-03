using System.Diagnostics;

namespace RAD_Project.Tasks
{
    public class TaskOne
    {
        public static void start()
        {
            int n = 100_000_000;
            int l = 26;
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

            ulong sumPrime = 0;
            var swPrime = Stopwatch.StartNew();
            foreach (var (key, _) in stream)
            {
                sumPrime += HashFunctions.MultiplyModPrime(key, a_prime, b_prime, l);
            }
            swPrime.Stop();
            Console.WriteLine($"Multiply-Mod-Prime sum: {sumPrime}");
            Console.WriteLine($"Multiply-Mod-Prime time: {swPrime.ElapsedMilliseconds} ms");
        }
    }
}