namespace ContextModels.Encoders;

internal sealed class AriphmeticEncoder : IEncoder
{
    public byte[] Encode(byte[] source)
    {
        ulong l = 0u;
        ulong r = uint.MaxValue;
        var counter = new int[256];
        for (var i = 0; i < counter.Length; i++)
            counter[i] = 1;

        foreach (var bt in source)
        {
            var (a, b) = GetBorders(bt, counter);
            var nl = (ulong)(l + Math.Ceiling(a * (r - l + 1) / (double)uint.MaxValue));
            var nr = (ulong)(l + Math.Ceiling((b + 1) * (r - l + 1) / (double)uint.MaxValue) - 1);
            
            
        }

        return null;
    }

    private static (uint, uint) GetBorders(byte b, int[] counter)
    {
        var c = 0;
        for (var i = 0; i < b; i++)
            c += counter[i];
        var alpha = c / (double)counter.Length;
        var beta = c / (double)counter.Length;
        return ((uint)Math.Ceiling(alpha * uint.MaxValue), (uint)Math.Ceiling(beta * uint.MaxValue) - 1);
    }
}