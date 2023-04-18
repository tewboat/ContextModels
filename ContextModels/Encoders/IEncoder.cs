namespace ContextModels.Encoders;

internal interface IEncoder<in TSource, out TResult>
{
    TResult Encode(TSource source);
}