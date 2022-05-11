using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bettlroal
{
    class ScreenCast
    {
        public bool running;
        int screenId;
        byte[] previous;
        int frames = 0;
        int timeStart = 0;
        int qualityModifier = 1;
        public int minQuality = 2;
        int lastUpdateSent = 0;
        int lastScreen = 0;
        int pauseBetweenScreens = 50;

        public static int chunkDivider = 10;
        public static int changeThreshold = 30;

        public ScreenCast(int id)
        {
            screenId = id;
            previous = new byte[0];
            running = true;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void CaptureScreen()
        {
            while (running)
            {
                int screenTaken = 0;
                if (Environment.TickCount - timeStart >= 1000)
                {
                    int fps = frames / (Environment.TickCount - timeStart) * 1000;
                    frames = 0;
                    timeStart = Environment.TickCount;
                    if (fps < 20)
                    {
                        qualityModifier--;
                        if (qualityModifier < minQuality)
                        {
                            qualityModifier = minQuality;
                        }
                    }
                    else if (fps > 25)
                    {
                        qualityModifier++;
                        if (qualityModifier > 10)
                        {
                            qualityModifier = 10;
                        }
                    }
                }

                Rectangle bounds = Screen.AllScreens[screenId].Bounds;
                Bitmap b = new Bitmap(bounds.Width, bounds.Height);
                Graphics graphics = Graphics.FromImage(b);
                int wait = lastScreen - Environment.TickCount;
                wait += pauseBetweenScreens;
                if (wait < 0)
                {
                    wait = 0;
                }
                lastScreen = Environment.TickCount + wait;
                Debug.WriteLine("Last screen: " + lastScreen);
                Debug.WriteLine("Wait: " + wait);
                Thread.Sleep(wait);
                graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
                screenTaken = Environment.TickCount;
                b = ResizeImage(b, (int)(b.Width * qualityModifier / 10), (int)(b.Height * qualityModifier / 10));
                BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
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
                        int chunkWidth = data.Stride / chunkDivider;
                        int chunkHeight = data.Height / chunkDivider;

                        for (int y = 0; y < data.Height; y += chunkHeight)
                        {
                            for (int x = 0; x < data.Stride; x += chunkWidth)
                            {
                                int currentChunkWidth = Math.Min(data.Stride - x, chunkWidth);
                                int currentChunkHeight = Math.Min(data.Height - y, chunkHeight);
                                ImageChunk chunk = new ImageChunk();
                                chunk.start = y * data.Stride + x;
                                chunk.width = currentChunkWidth;
                                chunk.height = currentChunkHeight;
                                byte[] buffer = new byte[currentChunkWidth * currentChunkHeight];
                                for (int chY = 0; chY < currentChunkHeight; chY++)
                                {
                                    Array.Copy(rgbValues, y * data.Stride + x + chY * data.Stride, buffer, chY * currentChunkWidth, currentChunkWidth);
                                }
                                chunk.SetBytes(buffer);
                                chunks.Add(chunk);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < rgbValues.Length; i += data.Stride)
                        {
                            int length = Math.Min(data.Stride, rgbValues.Length - i);
                            ImageChunk chunk = new ImageChunk();
                            chunk.bytes = new byte[length];
                            chunk.start = i;
                            for (int j = 0; j < chunk.bytes.Length; j++)
                            {
                                chunk.bytes[j] = rgbValues[i + j];
                            }
                            
                            byte[] oldChunk = new byte[length];
                            for (int j = 0; j < oldChunk.Length; j++)
                            {
                                oldChunk[j] = previous[i + j];
                            }
                            if (!oldChunk.SequenceEqual(chunk.bytes))
                            {
                                chunks.Add(chunk);
                            }
                        }
                    }
                    previous = new byte[rgbValues.Length];
                    rgbValues.CopyTo(previous, 0);
                }

                NetworkData d = new NetworkData();
                d.Stride = data.Stride;
                d.imageSize = rgbValues.Length;
                d.type = NetworkData.DataType.ImageUpdate;
                d.chunks = chunks;
                if (lastUpdateSent < screenTaken)
                {
                    lastUpdateSent = screenTaken;
                    InternetIO.instance.SendImageUpdate(d);
                    frames++;
                }
            }
        }
    }
}
