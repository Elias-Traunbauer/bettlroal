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

        byte[] previous = new byte[0];

        public void UpdateImage(NetworkData d)
        {
            if (previous.Length != d.imageSize)
            {
                previous = new byte[d.imageSize];
            }
            foreach (var chunk in d.chunks)
            {
                byte[] prevChunk = new byte[chunk.bytes.Length];
                previous.CopyTo(prevChunk, chunk.start);
                if (prevChunk != chunk.bytes)
                {
                    chunk.bytes.CopyTo(previous, chunk.start);
                }
            }
            Bitmap b = new Bitmap(1920, 1080);
            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;
            Marshal.Copy(previous, 0, ptr, previous.Length);
            b.UnlockBits(data);
            pbStream.Image = b;
        }

        private void Stream_Load(object sender, EventArgs e)
        {

        }
    }
}
