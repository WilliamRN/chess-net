using ChessNet.Utilities.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNet.Resources
{
    public static class Images
    {
        public static byte[] B_Bishop = BinaryUtils.GetBinaryData(ImagePaths.B_Bishop);
        public static byte[] B_King = BinaryUtils.GetBinaryData(ImagePaths.B_King);
        public static byte[] B_Knight = BinaryUtils.GetBinaryData(ImagePaths.B_Knight);
        public static byte[] B_Pawn = BinaryUtils.GetBinaryData(ImagePaths.B_Pawn);
        public static byte[] B_Queen = BinaryUtils.GetBinaryData(ImagePaths.B_Queen);
        public static byte[] B_Rook = BinaryUtils.GetBinaryData(ImagePaths.B_Rook);
        public static byte[] W_Bishop = BinaryUtils.GetBinaryData(ImagePaths.W_Bishop);
        public static byte[] W_King = BinaryUtils.GetBinaryData(ImagePaths.W_King);
        public static byte[] W_Knight = BinaryUtils.GetBinaryData(ImagePaths.W_Knight);
        public static byte[] W_Pawn = BinaryUtils.GetBinaryData(ImagePaths.W_Pawn);
        public static byte[] W_Queen = BinaryUtils.GetBinaryData(ImagePaths.W_Queen);
        public static byte[] W_Rook = BinaryUtils.GetBinaryData(ImagePaths.W_Rook);
    }
}
