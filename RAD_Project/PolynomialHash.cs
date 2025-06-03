using System.Numerics;
using RAD_Project;

public class PolynomialHash
{
    private static readonly int B = 89;
    private static readonly BigInteger P = (BigInteger.One << B) - 1;

    private readonly BigInteger[] a;

    public PolynomialHash(BigInteger a0, BigInteger a1, BigInteger a2, BigInteger a3)
    {
        a = new[] { a0, a1, a2, a3 };
    }

    public static PolynomialHash GenerateRandom()
    {
        return new PolynomialHash(
            RandomFunctions.GenerateRandom89Bit(),
            RandomFunctions.GenerateRandom89Bit(),
            RandomFunctions.GenerateRandom89Bit(),
            RandomFunctions.GenerateRandom89Bit()
        );
    }

    public BigInteger Evaluate(ulong x)
    {
        BigInteger y = a[3];

        for (int i = 2; i >= 0; i--)
        {
            y = y * x + a[i];
            y = (y&P) + (y>>B);
        }
        if (y >= P)
            y = y - P;
        return y;
    }
}
