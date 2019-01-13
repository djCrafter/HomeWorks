#pragma once
#include <iostream>
#include <string>
#include "WinSock2.h"
#include <ws2tcpip.h>
#pragma comment(lib, "Ws2_32.lib")

using namespace std;
const int MAXSTRLEN = 255;

class Socket 
{
protected:
	WSADATA wsaData;

	SOCKET _socket;
	SOCKET acceptSocket;

	sockaddr_in addr;

public:
	Socket();
	~Socket();
	bool SendData(char*);
	bool ReciveData(char*, int);
	void CloseConnection();
	void SendDataMessage();

};

class ServerSocket : public Socket 
{
public:
	void Listen();
	void Bind(int port);
	void StartHosting(int port);
};

 class ClientSocket : public Socket
{
public:
	void ConnectToServer(const char *ip, int port);
};