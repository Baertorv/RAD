using System.Globalization;

namespace RAD_Project.Tasks
{
    public class TaskSeven
    {
        public static void start()
        {
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