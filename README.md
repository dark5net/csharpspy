# csharpspy
一个用C#写的功能简单的Windows远控程序
下载源码使用Visual Studio生成即可,默认使用的是.NET4.0框架,自行更改
注意:部分功能会从 colafox.top 下载组件,第一次使用相关功能时需要保持能连接到 https://colafox.top/

### 运行前注意
为了避免奇形怪状的bug,程序运行前会删除目录下的所有文件.会有提示询问一次<br>
![l6z6C.png](https://s1.328888.xyz/2022/07/19/l6z6C.png)

## 生成一个被控端
程序提供了两种生成方式 使用csc.exe生成 或使用 msbuild.exe 生成<br>
![image.png](https://img1.imgtp.com/2022/07/19/YDVe7co8.png)

**小白如何操作:** 将仓库中 `client` 文件夹中的 `Program.cs` 拖入文本框, 或者点击下载源码，输入你的IP地址和端口号, 点击设置即可替换源代码中的地址. 选择勾选隐藏黑框选项 , 点击开始编译即可生成被控端

### 使用csc.exe生成
默认填写了.NET2.0 的路径,但是注意 .NET2.0 在Windows 10系统中是不会自带的,需要自行切换csc.exe的地址<br>
举几个例子:<br>
.NET 2.0 `C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe`<br>
.NET 3.5 `C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe`<br>
.NET 4.0 `C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe`<br>
使用csc.exe可以直接将被控端源代码生成出exe程序,简单且快捷<br>
但是我不推荐，这样生成出来的程序容易被杀软查杀，不隐藏黑框还不会被杀<br>

### 使用msbuild.exe生成
这就和直接用Visual Studio没啥区别了

## 我推荐的方式
创建一个 `Windows窗体应用`, 将`Program.cs`中的相关代码放进去
在窗口加载事件的函数里加载`Program.cs`的main函数，再加一句 `this.Visible = false` 来隐藏窗口
这样同样能隐藏窗口 还能免杀 
![QQ截图20220719004849.png](https://img1.imgtp.com/2022/07/19/bOJ0TMy8.png)<br>
举一反三，也可以找到开源的C#项目，重复以上操作
