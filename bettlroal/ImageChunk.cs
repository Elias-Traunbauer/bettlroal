using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bettlroal
{
    [Serializable]
    public class ImageChunk
    {
        public byte[] compressedBytes;
        
        public byte[] GetBytes()
        {
            MemoryStream input = new MemoryStream(compressedBytes);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public void SetBytes(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            DeflateStream deflate = new DeflateStream(stream, CompressionLevel.Fastest);
            deflate.Write(bytes, 0, bytes.Length);
            deflate.Dispose();
            compressedBytes = stream.ToArray();
            deflate.Close();
            stream.Close();
        }

        public int start;
        public int width;
        public int height;
    }
}
