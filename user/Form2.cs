using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Socket socks;
        public string hostinfo;
        public string ipaddr;
        public int buffersize = 512;

        public void reget()
        {
            while (true)
            {
                fuck();
                if (button3.Enabled == false)
                {
                    button1.Enabled = true;
                    break;
                }
            }
        }

        public Form2()
        {
            InitializeComponent();
        }

        int recivednum = 0; string filename;

        public void fuck() 
        {
            
            butto1.Enabled = false;
            butto1.Text = "正在获取图像，请稍后";

            try
            {
                recivednum = recivednum + 1;
                if (recivednum == 10)
                {
                    recivednum = 1; 
                }

                filename = recivednum.ToString() + ".jpg";

                socks.Send(Encoding.UTF8.GetBytes("DD"));

                FileStream fs = new FileStream(filename, FileMode.Create);
                byte[] data = new byte[8];
                socks.Receive(data, data.Length, SocketFlags.None);
                long contentLength = BitConverter.ToInt64(data, 0);
                int size = 0;

                butto1.Text = "开始接收文件";

                while (size < contentLength)
                {
                    byte[] bits = new byte[2048];
                    int r = socks.Receive(bits, bits.Length, SocketFlags.None);
                    if (r <= 0) break;
                    fs.Write(bits, 0, r);
                    size += r;
                }
                butto1.Text = "文件接收完成";

                fs.Flush();
                fs.Close();

                butto1.Text = "再来一张";
                butto1.Enabled = true;

                pictureBox1.Image = Image.FromFile(filename);

            }
            catch (Exception sad)
            {
                label7.Text =("出现故障" + sad.ToString());
            }

            GC.Collect();
        }
        private void butto1_Click(object sender, EventArgs e)
        {
            Thread threads = new Thread(fuck);
            threads.Start();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            butto1.Enabled = false;

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + "del /f /s /q *.jpg";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            label2.Text = hostinfo;
            label3.Text = ipaddr;

            butto1.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GC.Collect();

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + "del /f /s /q *.jpg";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = true;
            Thread thread = new Thread(reget);
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            button3.Enabled = false;
            pictureBox1.Image = null;
            GC.Collect();
            Thread.Sleep(500);
        }
    }
}
