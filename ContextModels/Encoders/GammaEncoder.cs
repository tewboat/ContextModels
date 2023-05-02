// namespace ContextModels.Encoders;
//
// using Models;
//
// internal sealed class GammaEncoder : IEncoder
// {
//     public byte[] Encode(byte[] source)
//     {
//         var list = new List<byte>();
//         foreach (var b in source)
//         {
//             var section = (int)Math.Log2(b);
//             for (var i = 0; i < section; i++)
//                 bits.Append(false);
//             var startIndex = section;
//             while (startIndex >= 0)
//                 bits.Append((source & (1 << startIndex--)) > 0);
//         }
//
//         return list.ToArray();
//     }
// }