namespace ContextModels.Encoders;

using System.Text;

internal sealed class DeltaEncoder : IEncoder<int, string>
{
    private readonly GammaEncoder gammaEncoder;
    
    public DeltaEncoder(GammaEncoder gammaEncoder)
    {
        this.gammaEncoder = gammaEncoder;
    }
    
    public string Encode(int source)
    {
        var stringBuilder = new StringBuilder();
        var section = (int)Math.Log2(source);
        var encodedSection = gammaEncoder.Encode(section + 1);
        stringBuilder.Append(encodedSection);
        var encodedSource = Convert.ToString(source, 2);
        stringBuilder.Append(encodedSource[1..]);
        return stringBuilder.ToString();
    }
}