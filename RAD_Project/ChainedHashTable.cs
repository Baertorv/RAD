using System;
using System.Collections.Generic;

public class ChainedHashTable
{
    private readonly int tableSize;
    private readonly List<(ulong key, long value)>[] buckets;
    private readonly Func<ulong, ulong> hashFunc;

    public ChainedHashTable(int l, Func<ulong, ulong> hashFunc)
    {
        this.tableSize = 1 << l;
        this.hashFunc = hashFunc;
        buckets = new List<(ulong, long)>[tableSize];
        for (int i = 0; i < tableSize; i++)
            buckets[i] = new List<(ulong, long)>();
    }

    public long Get(ulong x)
    {
        ulong h = hashFunc(x);
        var bucket = buckets[h];
        foreach (var (key, value) in bucket)
        {
            if (key == x)
                return value;
        }
        return 0;
    }

    public void Set(ulong x, long v)
    {
        ulong h = hashFunc(x);
        var bucket = buckets[h];
        for (int i = 0; i < bucket.Count; i++)
        {
            if (bucket[i].key == x)
            {
                bucket[i] = (x, v);
                return;
            }
        }
        bucket.Add((x, v));
    }

    public void Increment(ulong x, long d)
    {
        ulong h = hashFunc(x);
        var bucket = buckets[h];
        for (int i = 0; i < bucket.Count; i++)
        {
            if (bucket[i].key == x)
            {
                bucket[i] = (x, bucket[i].value + d);
                return;
            }
        }
        bucket.Add((x, d));
    }

    public IEnumerable<List<(ulong key, long value)>> GetAllBuckets()
    {
        return buckets;
    }
}