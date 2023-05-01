namespace ContextModels.Encoders;

using Models;

internal interface IEncoder
{
    byte[] Encode(byte[] source);
}