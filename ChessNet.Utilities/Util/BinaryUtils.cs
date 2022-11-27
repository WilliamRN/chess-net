using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNet.Utilities.Util
{
    public static class BinaryUtils
    {
        public static byte[] GetBinaryData(string filepath)
        {
            try
            {
                FileStream fileStream = File.OpenRead(filepath);
                using MemoryStream ms = ReadBinaryData(fileStream);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static MemoryStream ReadBinaryData(Stream stream)
        {
            MemoryStream returnData = new();
            byte[] buffer = new byte[1000];

            using (Stream input = stream)
            {
                int size = input.Read(buffer, 0, buffer.Length);

                while (size > 0)
                {
                    returnData.Write(buffer, 0, size);
                    size = input.Read(buffer, 0, buffer.Length);
                }
            }

            returnData.Position = 0;
            return returnData;
        }
    }
}
