using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bettlroal
{
    public partial class Stream : Form
    {
        public static Stream instance;

        public Stream()
        {
            InitializeComponent();
            instance = this;
        }

        int time = 0;
        int frames = 0;

        byte[] previous = new byte[0];

        delegate void UpdateImg(NetworkData d);

        public void UpdateImage(NetworkData asd)
        {
            UpdateImg ds = (NetworkData d) =>
            {
                if (Environment.TickCount - time >= 1000)
                {
                    int fps = frames / ((Environment.TickCount - time) / 1000);
                    label2.Text = "fps: " + fps;
                    time = Environment.TickCount;
                    frames = 0;
                }
                frames++;
                if (previous.Length != d.imageSize)
                {
                    previous = new byte[d.imageSize];
                }
                foreach (var chunk in d.chunks)
                {
                    chunk.bytes.CopyTo(previous, chunk.start);
                }
                Bitmap b = new Bitmap(d.Stride, d.imageSize / 4 / d.Stride);
                BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                IntPtr ptr = data.Scan0;
                Marshal.Copy(previous, 0, ptr, previous.Length);
                b.UnlockBits(data);
                pbStream.Image = b;
                pbStream.Update();
                label1.Text = d.chunks.Count + " Chunks";
            };
            try
            {
                Invoke(ds, asd);
            }
            catch (Exception)
            {
                instance = null;
            }
        }

        private void Stream_Load(object sender, EventArgs e)
        {

        }
    }
}
