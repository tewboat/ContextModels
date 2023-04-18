namespace ContextModels.Encoders;

using System.Text;

internal sealed class GammaEncoder : IEncoder<int, string>
{
    public string Encode(int source)
    {
        var stringBuilder = new StringBuilder();
        var section = (int)Math.Log2(source);
        for (var i = 0; i < section; i++)
            stringBuilder.Append('0');
        var binarySource = Convert.ToString(source, 2);
        stringBuilder.Append(binarySource);
        return stringBuilder.ToString();
    }
}