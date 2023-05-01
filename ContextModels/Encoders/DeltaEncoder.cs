// namespace ContextModels.Encoders;
//
// using Models;
//
// internal sealed class DeltaEncoder : IEncoder
// {
//     public byte[] Encode(byte[] source)
//     {
//         var pointer = 0;
//         var result = new List<byte>();
//         foreach (var b in source)
//         {
//             var section = (int)Math.Log2(b);
//             gammaEncoder.Encode((byte)(section + 1), bits);
//             var startIndex = section - 1;
//             while (startIndex >= 0)
//                 bits.Append((source & (ulong)(1 << startIndex--)) > 0);
//         }
//         
//         return result.ToArray();
//     }
//     
// }