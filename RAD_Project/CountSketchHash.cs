using System.Linq.Expressions;
using System.Numerics;

public class CountSketchHash
{
    private readonly PolynomialHash g;
    private readonly int t;
    private readonly ulong m;
    public int T => t;

    public CountSketchHash(PolynomialHash g, int t)
    {
        if (t < 1 || t > 64) throw new ArgumentOutOfRangeException(nameof(t));
        this.g = g;
        this.t = t;
        this.m = 1UL << t;
    }

    public (ulong h, int s) Hash(ulong x)
    {
        BigInteger gx = g.Evaluate(x);

        ulong h = (ulong)(gx & (m - 1));
        int bx = (int)(gx >> (89 - 1));
        int s = 1 - 2 * bx;

        return (h, s);
    }
}
