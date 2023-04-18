namespace ContextModels.Encoders;

internal sealed class BinaryStringToIntegersEncoder : IEncoder<string, uint[]>
{
    private const int IntegerSize = 4 * 8;

    public uint[] Encode(string source)
    {
        var integers = (int)Math.Ceiling(source.Length / (double)IntegerSize);
        var result = new uint[integers];
        for (var i = 0; i < integers; i++)
        {
            var substring = source.Substring(i, IntegerSize);
            if (substring.Length < IntegerSize)
                substring += new string('0', IntegerSize - substring.Length);
            result[i] = Convert.ToUInt32(substring, 2);
        }

        return result;
    }
}