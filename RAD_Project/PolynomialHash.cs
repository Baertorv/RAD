using System.Numerics;
using RAD_Project;

public class PolynomialHash
{
    private static readonly int B = 89;
    private static readonly BigInteger P = (BigInteger.One << B) - 1;

    private readonly BigInteger[] a; // a[0] = a₀, ..., a[3] = a₃

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
            y = FastModP(y);
        }

        return y;
    }

    private static BigInteger FastModP(BigInteger y)
    {
        BigInteger low = y & P;       // y mod 2^89
        BigInteger high = y >> B;     // y / 2^89
        BigInteger result = low + high;

        if (result >= P)
            result -= P;

        return result;
    }
}
