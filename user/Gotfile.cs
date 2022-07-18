using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Gotfile : Form
    {
        public Socket socks;
        public string ipaddr;

        public Gotfile()
        {
            InitializeComponent();
        }

        private void Gotfile_Load(object sender, EventArgs e)
        {
            label3.Text = ipaddr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string localpath = textBox2.Text;
                string gf = "F" + textBox1.Text;
                socks.Send(Encoding.UTF8.GetBytes(gf));

                FileStream fs = new FileStream(localpath, FileMode.Create);
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

                MessageBox.Show("操作成功结束", "完成");
            }
            catch
            {
                MessageBox.Show("操作失败", "完成");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socks.Send(Encoding.UTF8.GetBytes("U" + textBox3.Text));
            try
            {
                FileStream fs = new FileStream(textBox4.Text, FileMode.Open);
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
                MessageBox.Show("操作成功结束", "完成");
            }
            catch
            {
                MessageBox.Show("操作失败", "完成");
            }
        }
    }
}
