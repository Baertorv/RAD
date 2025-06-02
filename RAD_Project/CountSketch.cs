public class CountSketch
{
    private readonly long[] C;
    private readonly CountSketchHash hasher;

    public CountSketch(CountSketchHash hasher)
    {
        this.hasher = hasher;
        int m = 1 << hasher.T;
        C = new long[m];
    }

    public void Update(ulong x, int d)
    {
        var (h, s) = hasher.Hash(x);
        C[h] += s * d;
    }

    public long EstimateSecondMoment()
    {
        long sum = 0;
        foreach (long count in C)
            sum += count * count;

        return sum;
    }
}
