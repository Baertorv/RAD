using System.Diagnostics;

namespace RAD_Project.Tasks
{
    public class TaskThree
    {
        public static void start()
        {
            for (int l2 = 4; ; l2++)
            {
                int requiredN = 50_000_000;

                if (1 << l2 > requiredN)
                {
                    Console.WriteLine($"2^{l2} exceeds upper n limit, terminating...");
                    break;
                }

                Console.WriteLine($"\nTesting l = {l2}, n = {requiredN} (i.e., 2^{l2} keys)");

                // Generate random hash parameters once
                var a_shift = RandomFunctions.GenerateOdd64Bit();
                var a_prime = RandomFunctions.GenerateRandom89Bit();
                var b_prime = RandomFunctions.GenerateRandom89Bit();

                var stream2 = StreamGenerator.CreateStream(requiredN, l2);

                // Multiply-Shift
                var table1 = new ChainedHashTable(l2, x => HashFunctions.MultiplyShift(x, a_shift, l2));
                var sw1 = Stopwatch.StartNew();
                var result1 = HashFunctions.ComputeSquareSum(stream2, table1);
                sw1.Stop();
                Console.WriteLine($"Multiply-Shift: S = {result1}, time = {sw1.ElapsedMilliseconds} ms");

                // Multiply-Mod-Prime
                var table2 = new ChainedHashTable(l2, x => HashFunctions.MultiplyModPrime(x, a_prime, b_prime, l2));
                var sw2 = Stopwatch.StartNew();
                var result2 = HashFunctions.ComputeSquareSum(stream2, table2);
                sw2.Stop();
                Console.WriteLine($"Multiply-Mod-Prime: S = {result2}, time = {sw2.ElapsedMilliseconds} ms");
            }
        }
    }
}