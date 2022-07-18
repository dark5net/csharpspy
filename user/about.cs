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

namespace WindowsFormsApp1
{
    public partial class about : Form
    {
        public int clicktime = 0;

        public about()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            clicktime++;
            if(clicktime == 1) MessageBox.Show("本来想加个彩蛋的  想想还是算了", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(clicktime == 2) MessageBox.Show("本来想加个彩蛋的  想想还是算了", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(clicktime == 3) MessageBox.Show("你还瞎JB点什么（", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(clicktime == 4) MessageBox.Show("如果你打开源代码，，会发现程序写的像一堆狗屎", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(clicktime == 5) MessageBox.Show("变量随便命名 代码无可读性", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(clicktime == 6) MessageBox.Show("还请嘴下留情，，，毕竟我没参加过多人协作的项目开发，，自己看得懂就行了", "运行时错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ChangeLogColor()
        {
            Random ran = new Random();
            while (true)
            {
                label1.ForeColor = Color.FromArgb(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255));
                Thread.Sleep(150);
            }
        }

        private void about_Load(object sender, EventArgs e)
        {
            Thread CheckKaSi = new Thread(ChangeLogColor);
            CheckKaSi.Start();
        }
    }
}
