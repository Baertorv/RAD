using System.Diagnostics;
using System.Numerics;
using System.IO;
using System.Globalization;

namespace RAD_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // TASK 1
            // int n = 100_000_000;
            // int l = 16;
            // Console.WriteLine($"Testing hashing functions with n=100.000.000 and l={l}");

            // var stream = StreamGenerator.CreateStream(n, l);

            // ulong a_shift = RandomFunctions.GenerateOdd64Bit();
            // var a_prime = RandomFunctions.GenerateRandom89Bit();
            // var b_prime = RandomFunctions.GenerateRandom89Bit();

            // ulong sumShift = 0;
            // var swShift = Stopwatch.StartNew();
            // foreach (var (key, _) in stream)
            // {
            //     sumShift += HashFunctions.MultiplyShift(key, a_shift, l);
            // }
            // swShift.Stop();
            // Console.WriteLine($"Multiply-Shift sum: {sumShift}");
            // Console.WriteLine($"Multiply-Shift time: {swShift.ElapsedMilliseconds} ms");

            // stream = StreamGenerator.CreateStream(n, l);

            // ulong sumPrime = 0;
            // var swPrime = Stopwatch.StartNew();
            // foreach (var (key, _) in stream)
            // {
            //     sumPrime += HashFunctions.MultiplyModPrime(key, a_prime, b_prime, l);
            // }
            // swPrime.Stop();
            // Console.WriteLine($"Multiply-Mod-Prime sum: {sumPrime}");
            // Console.WriteLine($"Multiply-Mod-Prime time: {swPrime.ElapsedMilliseconds} ms");

            //TASK 3
            // for (int l2 = 4; ; l2++)
            // {
            //     if ((1 << l2) > n)
            //     {
            //         Console.WriteLine($"l^2 now greater than {n}, terminating");
            //         break;
            //     }
            //     Console.WriteLine($"\nTesting l = {l2}, 2^{l2} keys:");

            //     var stream2 = StreamGenerator.CreateStream(n, l2);
            //     var table1 = new ChainedHashTable(l2, x => HashFunctions.MultiplyShift(x, a_shift, l2));
            //     var sw1 = Stopwatch.StartNew();
            //     var result1 = HashFunctions.ComputeSquareSum(stream2, table1);
            //     sw1.Stop();
            //     Console.WriteLine($"Multiply-Shift: S = {result1}, time = {sw1.ElapsedMilliseconds} ms");

            //     stream2 = StreamGenerator.CreateStream(n, l2);
            //     var table2 = new ChainedHashTable(l2, x => HashFunctions.MultiplyModPrime(x, a_prime, b_prime, l2));
            //     var sw2 = Stopwatch.StartNew();
            //     var result2 = HashFunctions.ComputeSquareSum(stream2, table2);
            //     sw2.Stop();
            //     Console.WriteLine($"Multiply-Mod-Prime: S = {result2}, time = {sw2.ElapsedMilliseconds} ms");
            // }

            //TASK 7
            int n = 10_000_000;
            int l = 23;
            int t = 14;

            Console.WriteLine("Generating stream...");
            var stream = StreamGenerator.CreateStream(n, l).ToList();

            Console.WriteLine("Computing exact S...");
            ulong aShift = RandomFunctions.GenerateOdd64Bit();
            var exactTable = new ChainedHashTable(l, x => HashFunctions.MultiplyShift(x, aShift, l));
            foreach (var (x, d) in stream)
                exactTable.Increment(x, d);

            long S = 0;
            foreach (var bucket in exactTable.GetAllBuckets())
                foreach (var (_, val) in bucket)
                    S += val * val;

            Console.WriteLine($"Exact S = {S}");

            List<long> estimates = new();

            Console.WriteLine("Running 100 Count-Sketch experiments...");
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Starting experiment {i + 1}");
                var g = PolynomialHash.GenerateRandom();
                var hashFunc = new CountSketchHash(g, t);
                var sketch = new CountSketch(hashFunc);

                foreach (var (x, d) in stream)
                    sketch.Update(x, d);

                long estimate = sketch.EstimateSecondMoment();
                estimates.Add(estimate);
            }

            estimates.Sort();
            double mse = estimates
                .Select(x => Math.Pow(x - S, 2))
                .Average();

            Console.WriteLine($"Mean squared error (MSE) = {mse}");

            List<double> medians = new();
            for (int i = 0; i < 9; i++)
            {
                var group = estimates.Skip(i * 11).Take(11).ToList();
                group.Sort();
                medians.Add(group[5]);
            }

            medians.Sort();

            Console.WriteLine("\nSorted estimates:");
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"{i + 1}, {estimates[i]}");

            Console.WriteLine("\nSorted medians of 9 groups:");
            for (int i = 0; i < 9; i++)
                Console.WriteLine($"{i + 1}, {medians[i]}");

            using (var writer = new StreamWriter("sorted_estimates.csv"))
            {
                writer.WriteLine("experiment_index,estimate");
                for (int i = 0; i < 100; i++)
                {
                    writer.WriteLine($"{i + 1},{estimates[i].ToString(CultureInfo.InvariantCulture)}");
                }
            }

            using (var writer = new StreamWriter("medians.csv"))
            {
                writer.WriteLine("group_index,median");
                for (int i = 0; i < 9; i++)
                {
                    writer.WriteLine($"{i + 1},{medians[i].ToString(CultureInfo.InvariantCulture)}");
                }
            }

            using (var writer = new StreamWriter("metadata.txt"))
            {
                writer.WriteLine($"True S: {S}");
                writer.WriteLine($"Mean Squared Error (MSE): {mse}");
            }

        }
    }
}