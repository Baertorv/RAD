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
            // int l = 26;
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

            // ulong sumPrime = 0;
            // var swPrime = Stopwatch.StartNew();
            // foreach (var (key, _) in stream)
            // {
            //     sumPrime += HashFunctions.MultiplyModPrime(key, a_prime, b_prime, l);
            // }
            // swPrime.Stop();
            // Console.WriteLine($"Multiply-Mod-Prime sum: {sumPrime}");
            // Console.WriteLine($"Multiply-Mod-Prime time: {swPrime.ElapsedMilliseconds} ms");
            //_________________________________________________________________________________
            //
            //TASK 3
            // for (int l2 = 4; ; l2++)
            // {
            //     int requiredN = 50_000_000;

            //     if (1 << l2 > requiredN)
            //     {
            //         Console.WriteLine($"2^{l2} exceeds upper n limit, terminating...");
            //         break;
            //     }

            //     Console.WriteLine($"\nTesting l = {l2}, n = {requiredN} (i.e., 2^{l2} keys)");

            //     // Generate random hash parameters once
            //     var a_shift = RandomFunctions.GenerateOdd64Bit();
            //     var a_prime = RandomFunctions.GenerateRandom89Bit();
            //     var b_prime = RandomFunctions.GenerateRandom89Bit();

            //     var stream2 = StreamGenerator.CreateStream(requiredN, l2);

            //     // Multiply-Shift
            //     var table1 = new ChainedHashTable(l2, x => HashFunctions.MultiplyShift(x, a_shift, l2));
            //     var sw1 = Stopwatch.StartNew();
            //     var result1 = HashFunctions.ComputeSquareSum(stream2, table1);
            //     sw1.Stop();
            //     Console.WriteLine($"Multiply-Shift: S = {result1}, time = {sw1.ElapsedMilliseconds} ms");

            //     // Multiply-Mod-Prime
            //     var table2 = new ChainedHashTable(l2, x => HashFunctions.MultiplyModPrime(x, a_prime, b_prime, l2));
            //     var sw2 = Stopwatch.StartNew();
            //     var result2 = HashFunctions.ComputeSquareSum(stream2, table2);
            //     sw2.Stop();
            //     Console.WriteLine($"Multiply-Mod-Prime: S = {result2}, time = {sw2.ElapsedMilliseconds} ms");
            // }
            //_________________________________________________________________________________
            //
            //TASK 7
            // int n = 10_000_000;
            // int l = 23;
            // int t = 14;

            // Console.WriteLine("Generating stream...");
            // var stream = StreamGenerator.CreateStream(n, l).ToList();

            // Console.WriteLine("Computing exact S...");
            // ulong aShift = RandomFunctions.GenerateOdd64Bit();
            // var exactTable = new ChainedHashTable(l, x => HashFunctions.MultiplyShift(x, aShift, l));
            // foreach (var (x, d) in stream)
            //     exactTable.Increment(x, d);

            // long S = 0;
            // foreach (var bucket in exactTable.GetAllBuckets())
            //     foreach (var (_, val) in bucket)
            //         S += val * val;

            // Console.WriteLine($"Exact S = {S}");

            // List<long> estimates = new();

            // Console.WriteLine("Running 100 Count-Sketch experiments...");
            // for (int i = 0; i < 100; i++)
            // {
            //     var g = PolynomialHash.GenerateRandom();
            //     var hashFunc = new CountSketchHash(g, t);
            //     var sketch = new CountSketch(hashFunc);

            //     foreach (var (x, d) in stream)
            //         sketch.Update(x, d);

            //     long estimate = sketch.EstimateSecondMoment();
            //     estimates.Add(estimate);
            // }

            // estimates.Sort();
            // double mse = estimates
            //     .Select(x => Math.Pow(x - S, 2))
            //     .Average();

            // Console.WriteLine($"Mean squared error (MSE) = {mse}");

            // List<double> medians = new();
            // for (int i = 0; i < 9; i++)
            // {
            //     var group = estimates.Skip(i * 11).Take(11).ToList();
            //     group.Sort();
            //     medians.Add(group[5]);
            // }

            // medians.Sort();

            // Console.WriteLine("\nSorted estimates:");
            // for (int i = 0; i < 100; i++)
            //     Console.WriteLine($"{i + 1}, {estimates[i]}");

            // Console.WriteLine("\nSorted medians of 9 groups:");
            // for (int i = 0; i < 9; i++)
            //     Console.WriteLine($"{i + 1}, {medians[i]}");

            // using (var writer = new StreamWriter("sorted_estimates.csv"))
            // {
            //     writer.WriteLine("experiment_index,estimate");
            //     for (int i = 0; i < 100; i++)
            //     {
            //         writer.WriteLine($"{i + 1},{estimates[i].ToString(CultureInfo.InvariantCulture)}");
            //     }
            // }

            // using (var writer = new StreamWriter("medians.csv"))
            // {
            //     writer.WriteLine("group_index,median");
            //     for (int i = 0; i < 9; i++)
            //     {
            //         writer.WriteLine($"{i + 1},{medians[i].ToString(CultureInfo.InvariantCulture)}");
            //     }
            // }

            // using (var writer = new StreamWriter("metadata.txt"))
            // {
            //     writer.WriteLine($"True S: {S}");
            //     writer.WriteLine($"Mean Squared Error (MSE): {mse}");
            // }
            //_________________________________________________________________________________
            // TASK 8
            // int n = 10_000_000;
            // int l = 23;
            // int[] m_values = new[] { 1 << 8, 1 << 14, 1 << 20 };

            // var stream = StreamGenerator.CreateStream(n, l);

            // // Exact S using MultiplyShift hashing and chaining
            // ulong aShift = RandomFunctions.GenerateOdd64Bit();
            // var exactTable = new ChainedHashTable(l, x => HashFunctions.MultiplyShift(x, aShift, l));
            // long trueS = HashFunctions.ComputeSquareSum(stream, exactTable);

            // Console.WriteLine($"True S: {trueS}");
            // File.WriteAllText("true_s.txt", trueS.ToString());

            // foreach (int m in m_values)
            // {
            //     int t = (int)Math.Log2(m);
            //     List<long> estimates = new();
            //     List<long> runtimes = new();

            //     Console.WriteLine($"Running Count-Sketch experiments with m={m}...");
            //     Stopwatch totalSw = Stopwatch.StartNew();

            //     for (int i = 0; i < 100; i++)
            //     {
            //         Console.WriteLine($"Running Count-Sketch experiment {i+1}...");
            //         var g = PolynomialHash.GenerateRandom();
            //         var hasher = new CountSketchHash(g, t);
            //         var sketch = new CountSketch(hasher);
            //         Console.WriteLine($"Set up the CountSketch...");
            //         Stopwatch sw = Stopwatch.StartNew();
            //         foreach (var (x, d) in stream)
            //             sketch.Update(x, d);
            //         long estimate = sketch.EstimateSecondMoment();
            //         Console.WriteLine($"Estimated second moment...");
            //         sw.Stop();

            //         estimates.Add(estimate);
            //         runtimes.Add(sw.ElapsedMilliseconds);
            //     }

            //     totalSw.Stop();

            //     estimates.Sort();
            //     double mse = estimates.Select(e => Math.Pow(e - trueS, 2)).Average();

            //     // Write sorted estimates
            //     using (var writer = new StreamWriter($"sorted_estimates_m{m}.csv"))
            //     {
            //         writer.WriteLine("experiment_index,estimate");
            //         for (int i = 0; i < estimates.Count; i++)
            //             writer.WriteLine($"{i + 1},{estimates[i].ToString(CultureInfo.InvariantCulture)}");
            //     }

            //     // Write runtime per experiment
            //     using (var writer = new StreamWriter($"runtimes_m{m}.csv"))
            //     {
            //         writer.WriteLine("experiment_index,runtime_ms");
            //         for (int i = 0; i < runtimes.Count; i++)
            //             writer.WriteLine($"{i + 1},{runtimes[i]}");
            //     }

            //     // Compute medians from 9 groups of 11 values each (X100 left out)
            //     List<double> medians = new();
            //     for (int i = 0; i < 9; i++)
            //     {
            //         var group = estimates.Skip(i * 11).Take(11).ToList();
            //         group.Sort();
            //         medians.Add(group[5]);
            //     }

            //     medians.Sort();
            //     using (var writer = new StreamWriter($"medians_m{m}.csv"))
            //     {
            //         writer.WriteLine("group_index,median");
            //         for (int i = 0; i < medians.Count; i++)
            //             writer.WriteLine($"{i + 1},{medians[i].ToString(CultureInfo.InvariantCulture)}");
            //     }

            //     using (var writer = new StreamWriter($"metadata_m{m}.txt"))
            //     {
            //         writer.WriteLine($"m: {m}");
            //         writer.WriteLine($"True S: {trueS}");
            //         writer.WriteLine($"Mean Squared Error (MSE): {mse}");
            //         writer.WriteLine($"Total Runtime (ms): {totalSw.ElapsedMilliseconds}");
            //         writer.WriteLine($"Average Runtime per Experiment (ms): {runtimes.Average()}");
            //     }

            //     Console.WriteLine($"Done with m={m}. MSE={mse}, Avg Runtime={runtimes.Average():F2}ms");
            // }
        }
    }
}