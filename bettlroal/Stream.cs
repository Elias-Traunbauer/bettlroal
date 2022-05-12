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
        int bytes = 0;

        byte[] previous = new byte[0];

        delegate void UpdateImg(NetworkData d);

        public void UpdateImage(NetworkData asd)
        {
            UpdateImg ds = (NetworkData d) =>
            {
                if (Environment.TickCount - time >= 1000)
                {
                    double fps = frames / ((Environment.TickCount - time) / 1000);
                    double bps = bytes / ((Environment.TickCount - time) / 1000);
                    double bpss = bps * 8;
                    string calling = "b";
                    string callingg = "bit";
                    if (bps > 1000)
                    {
                        bps /= 1000;
                        calling = "kb";
                    }
                    if (bps > 1000)
                    {
                        bps /= 1000;
                        calling = "mb";
                    }

                    if (bpss > 1000)
                    {
                        bpss /= 1000;
                        callingg = "kbp";
                    }
                    if (bpss > 1000)
                    {
                        bpss /= 1000;
                        callingg = "mbp";
                    }
                    label2.Text = "fps: " + fps;
                    lblBytes.Text = "transfer rate: " + bps + calling + "/s   " + bpss +  callingg + "/s";
                    time = Environment.TickCount;
                    frames = 0;
                    bytes = 0;
                }
                frames++;
                if (previous.Length != d.imageSize || cbDebug.Checked)
                {
                    previous = new byte[d.imageSize];
                }
                foreach (var chunk in d.chunks)
                {
                    byte[] chunkbytes = chunk.GetBytes();
                    bytes += chunkbytes.Length;
                    for (int y = 0; y < chunk.height; y++)
                    {
                        for (int x = 0; x < chunk.width; x++)
                        {
                            previous[chunk.start + y * d.Stride + x] = chunkbytes[y * chunk.width + x];
                        }
                    }
                }
                Bitmap b = new Bitmap(d.Stride, previous.Length / (d.Stride));
                BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
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
