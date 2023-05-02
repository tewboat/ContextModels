namespace ContextModels.Encoders;

internal sealed class DeltaEncoder : IEncoder
{
    public byte[] Encode(byte[] source)
    {
        var offset = 0;
        var byteOffset = 7;
        var result = new List<byte>{0};
        var totalBits = 0;

        void DecrementByteOffset()
        {
            byteOffset = (byteOffset + 7) % 8;
            totalBits++;
            if (byteOffset != 7) return;
            offset++;
            result.Add(0);
        }
        
        foreach (var b in source)
        {
            var section = (int)Math.Log2(b + 1);

            var sectionOfSection = (int)Math.Log2(section + 1);
            for (var i = 0; i < sectionOfSection; i++)
                DecrementByteOffset();
            for (var i = sectionOfSection; i >= 0; i--)
            {
                var v = (byte)(((1 << i) & (section + 1)) > 0 ? 1 << byteOffset : 0);
                result[offset] |= v;
                DecrementByteOffset();
            }
                
            for (var i = section - 1; i >= 0; i--)
            {
                var v = (byte)(((1 << i) & (b + 1)) > 0 ? 1 << byteOffset : 0);
                result[offset] |= v;
                DecrementByteOffset();
            }
        }
        
        Console.WriteLine(totalBits);
        return result.ToArray();
    }
    
}