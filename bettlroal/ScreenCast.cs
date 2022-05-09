using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bettlroal
{
    class ScreenCast
    {
        private bool running;
        int screenId;
        byte[] previous;

        public ScreenCast(int id)
        {
            screenId = id;
            previous = new byte[0];
            running = true;
        }

        public void CaptureScreen()
        {
            while (running)
            {
                Rectangle bounds = Screen.AllScreens[screenId].Bounds;
                Bitmap b = new Bitmap(1920, 1080);
                Graphics graphics = Graphics.FromImage(b);
                graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);

                BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                IntPtr intPtr = data.Scan0;
                int bytes = Math.Abs(data.Stride) * b.Height;
                byte[] rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(intPtr, rgbValues, 0, bytes);
                b.UnlockBits(data);
                List<ImageChunk> chunks = new List<ImageChunk>();
                lock (previous)
                {
                    if (rgbValues.Length != previous.Length)
                    {
                        for (int i = 0; i < rgbValues.Length; i += 9)
                        {
                            ImageChunk chunk = new ImageChunk();
                            chunk.bytes = new byte[Math.Min(9, rgbValues.Length - i)];
                            for (int j = 0; j < chunk.bytes.Length; j++)
                            {
                                chunk.bytes[j] = rgbValues[i+ j];
                            }
                            chunks.Add(chunk);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < rgbValues.Length; i += data.Stride)
                        {
                            ImageChunk chunk = new ImageChunk();
                            chunk.bytes = new byte[data.Stride];
                            rgbValues.CopyTo(chunk.bytes, i);
                            byte[] oldChunk = new byte[data.Stride];
                            previous.CopyTo(oldChunk, i);
                            if (oldChunk != chunk.bytes)
                            {
                                chunks.Add(chunk);
                            }
                        }
                    }
                    previous = new byte[rgbValues.Length];
                    rgbValues.CopyTo(previous, 0);
                }

                NetworkData d = new NetworkData();
                d.imageSize = rgbValues.Length;
                d.type = NetworkData.DataType.ImageUpdate;
                d.chunks = chunks;
                InternetIO.instance.SendImageUpdate(d);
            }
        }
    }
}
