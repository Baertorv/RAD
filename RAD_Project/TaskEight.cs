using System.Diagnostics;
using System.Globalization;

namespace RAD_Project.Tasks
{
    public class TaskEight
    {
        public static void start()
        {
            int n = 10_000_000;
            int l = 23;
            int[] m_values = new[] { 1 << 8, 1 << 14, 1 << 20 };

            var stream = StreamGenerator.CreateStream(n, l);

            ulong aShift = RandomFunctions.GenerateOdd64Bit();
            var exactTable = new ChainedHashTable(l, x => HashFunctions.MultiplyShift(x, aShift, l));
            long trueS = HashFunctions.ComputeSquareSum(stream, exactTable);

            Console.WriteLine($"True S: {trueS}");
            File.WriteAllText("true_s.txt", trueS.ToString());

            foreach (int m in m_values)
            {
                int t = (int)Math.Log2(m);
                List<long> estimates = new();
                List<long> runtimes = new();

                Console.WriteLine($"Running Count-Sketch experiments with m={m}...");
                Stopwatch totalSw = Stopwatch.StartNew();

                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine($"Running Count-Sketch experiment {i + 1}...");
                    var g = PolynomialHash.GenerateRandom();
                    var hasher = new CountSketchHash(g, t);
                    var sketch = new CountSketch(hasher);
                    Console.WriteLine($"Set up the CountSketch...");
                    Stopwatch sw = Stopwatch.StartNew();
                    foreach (var (x, d) in stream)
                        sketch.Update(x, d);
                    long estimate = sketch.EstimateSecondMoment();
                    Console.WriteLine($"Estimated second moment...");
                    sw.Stop();

                    estimates.Add(estimate);
                    runtimes.Add(sw.ElapsedMilliseconds);
                }

                totalSw.Stop();

                estimates.Sort();
                double mse = estimates.Select(e => Math.Pow(e - trueS, 2)).Average();

                using (var writer = new StreamWriter($"sorted_estimates_m{m}.csv"))
                {
                    writer.WriteLine("experiment_index,estimate");
                    for (int i = 0; i < estimates.Count; i++)
                        writer.WriteLine($"{i + 1},{estimates[i].ToString(CultureInfo.InvariantCulture)}");
                }

                using (var writer = new StreamWriter($"runtimes_m{m}.csv"))
                {
                    writer.WriteLine("experiment_index,runtime_ms");
                    for (int i = 0; i < runtimes.Count; i++)
                        writer.WriteLine($"{i + 1},{runtimes[i]}");
                }

                List<double> medians = new();
                for (int i = 0; i < 9; i++)
                {
                    var group = estimates.Skip(i * 11).Take(11).ToList();
                    group.Sort();
                    medians.Add(group[5]);
                }

                medians.Sort();
                using (var writer = new StreamWriter($"medians_m{m}.csv"))
                {
                    writer.WriteLine("group_index,median");
                    for (int i = 0; i < medians.Count; i++)
                        writer.WriteLine($"{i + 1},{medians[i].ToString(CultureInfo.InvariantCulture)}");
                }

                using (var writer = new StreamWriter($"metadata_m{m}.txt"))
                {
                    writer.WriteLine($"m: {m}");
                    writer.WriteLine($"True S: {trueS}");
                    writer.WriteLine($"Mean Squared Error (MSE): {mse}");
                    writer.WriteLine($"Total Runtime (ms): {totalSw.ElapsedMilliseconds}");
                    writer.WriteLine($"Average Runtime per Experiment (ms): {runtimes.Average()}");
                }

                Console.WriteLine($"Done with m={m}. MSE={mse}, Avg Runtime={runtimes.Average():F2}ms");
            }
        }
    }
}