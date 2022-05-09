using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bettlroal
{
    public partial class ChooseScreen : Form
    {
        Bitmap[] screens;
        GroupBox[] gbs;
        public int selectedId = -1;

        public ChooseScreen()
        {
            InitializeComponent();
            screens = new Bitmap[Screen.AllScreens.Length];
            gbs = new GroupBox[Screen.AllScreens.Length];
            for (int i = 0; i < screens.Length; i++)
            {
                Rectangle bounds = Screen.AllScreens[i].Bounds;
                screens[i] = new Bitmap(1024, 768);
                Graphics graphics = Graphics.FromImage(screens[i]);
                graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
            }
        }

        private void ChooseScreen_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < screens.Length; i++)
            {
                gbs[i] = new GroupBox();
                gbs[i].Text = "Screen " + i;
                PictureBox b = new PictureBox();
                b.Image = screens[i];
                b.Name = i + "";
                gbs[i].Width = (int)(this.Width * 0.9 / 2);
                gbs[i].Height = (int)(gbs[i].Width * 0.75);
                b.Top = 20;
                b.Left = 15;
                b.MouseClick += B_MouseClick;
                b.Width = (gbs[i].Width - 40);
                b.Height = gbs[i].Height - 30;
                b.SizeMode = PictureBoxSizeMode.Zoom;
                b.BorderStyle = BorderStyle.Fixed3D;
                gbs[i].BackColor = Color.White;
                gbs[i].Controls.Add(b);
                flowLayoutPanel1.Controls.Add(gbs[i]);
            }
        }

        private void B_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedId != -1)
            {
                gbs[selectedId].BackColor = Color.White;
            }
            selectedId = int.Parse(((PictureBox)sender).Name);
            gbs[selectedId].BackColor = Color.LightGray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedId != -1)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }
    }
}
