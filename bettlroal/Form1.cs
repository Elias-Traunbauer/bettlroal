using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bettlroal
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer t;
        public static string name;

        public Form1()
        {
            InitializeComponent();
            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Enabled = true;
            t.Tick += T_Tick;
            InternetIO.instance.MessagesUpdate += Instance_MessagesUpdate;
            InitializeForm f = new InitializeForm();

            while (f.ShowDialog() != DialogResult.OK)
            {
                name = f.name;
            }
            name = f.name;
        }

        private delegate void MessageUpdate(object sender, NetworkData d);

        private void Instance_MessagesUpdate(object sender, NetworkData e)
        {
            MessageUpdate d = (object senderr, NetworkData ee) => {
                foreach (var msg in e.msgs)
                {
                    Label l = new Label();
                    l.AutoSize = true;
                    l.Text = msg.date.ToString() + " " + msg.sender + ": " + msg.content;
                    flpChat.Controls.Add(l);
                    flpChat.ScrollControlIntoView(l);
                }
            };
            Invoke(d, new object[] { sender, e });
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (InternetIO.instance.mode == InternetIO.IOType.Server)
            {
                InternetIO.instance.FlushMsgs();
            }
        }

        private void btnHost_Click(object sender, EventArgs e)
        {
            gbConn.Enabled = false;
            lblPublicIP.Text = InternetIO.instance.StartServer(true);
            textBox1.Enabled = true;
            btnSend.Enabled = true;
            if (lblPublicIP.Text == "error")
            {
                var res = MessageBox.Show("Router not found. Portforwarding required. Please contact your system administrator. Proceed without portforwarding?", "Error", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                {
                    lblPublicIP.Text = InternetIO.instance.StartServer(false);
                    return;
                }
                gbConn.Enabled = true;
                textBox1.Enabled = false;
                btnSend.Enabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                gbConn.Enabled = false;
                InternetIO.instance.Connect(System.Net.IPAddress.Parse(tbIP.Text), int.Parse(tbPort.Text));
                textBox1.Enabled = true;
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while connection to host: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
                gbConn.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            Message msg = new Message();
            msg.content = textBox1.Text;
            if (msg.content != "")
            {
                textBox1.Text = "";

                InternetIO.instance.SendMessage(msg);
            }
        }

        public void ShowMsg(string content)
        {
            MessageBox.Show(content);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
                e.Handled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            InternetIO.instance.DeleteMapping();
        }

        ScreenCast sc;

        private void btnStream_Click(object sender, EventArgs e)
        {
            if (sc == null)
            {
                ChooseScreen cs = new ChooseScreen();
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    sc = new ScreenCast(cs.selectedId);
                    sc.minQuality = cs.quality;
                    for (int i = 0; i < 3; i++)
                    {
                        Thread d = new Thread(sc.CaptureScreen);
                        d.IsBackground = true;
                        d.Name = "Screen Cast Thread";
                        d.Start();
                    }

                }
                btnStream.Text = "End Stream";
            }
            else
            {
                sc.running = false;
                sc = null;
                btnStream.Text = "Stream";
            }
        }

        private void btnOpenStream_Click(object sender, EventArgs e)
        {
            Stream s = new Stream();
            s.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ScreenCast.changeThreshold = (int)numericUpDown3.Value;
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ScreenCast.chunkDivider = (int)numericUpDown2.Value;
        }
    }
}
