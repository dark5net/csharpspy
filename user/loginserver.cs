using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class loginserver : Form
    {
        static Socket socket;
        const int BUFFER_SIZE = 1024;
        static byte[] readBuff = new byte[BUFFER_SIZE];
        public string backmsg;
        public loginserver()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int serverPort = int.Parse(textBox3.Text.ToString());
            try
            {
                //验证当前IP或端口是否可用
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse(textBox1.Text);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, serverPort);
                Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);
                listener.Listen(10);

                listener.Close();

                this.Visible = false;

                Form1 form1 = new Form1();
                form1.port = int.Parse(textBox3.Text);
                form1.raddr = textBox1.Text;
                form1.ShowDialog(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接到服务器时出现了错误,检测你的端口或IP"+ex.ToString() , "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void loginserver_Load(object sender, EventArgs e)
        {


            DialogResult MsgBoxResult = MessageBox.Show("运行前警告:\n请将程序放在一个单独的空文件夹,清理运行环境时会将程序目录下所有文件删除\n也可以选择不清理运行环境，但是可能会出现bug", "你要清理运行环境吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (MsgBoxResult.ToString() == "Yes")
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[1]; 

                textBox1.Text = ipAddress.ToString();
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c " + "del /f /s /q *";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                label3.Text = "运行环境清理成功";
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        
    }
}
