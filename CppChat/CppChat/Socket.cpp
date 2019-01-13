#include "Socket.h"

Socket::Socket() 
{
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != NO_ERROR) 
	{
		cout << "WSAStartup error\n";
		system("pause");
		WSACleanup();
		exit(10);
    }

	_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (_socket == INVALID_SOCKET)
	{
		cout << "Socket create error." << endl;
		system("pause");
		WSACleanup();
		exit(11);
	}
}

Socket::~Socket()
{
	WSACleanup();
}

bool Socket::SendData(char *buffer)
{
	send(_socket, buffer, strlen(buffer), 0);
	return true;
}

bool Socket::ReciveData(char *buffer, int size)
{
	int i = recv(_socket, buffer, size, 0);
	buffer[i] = '\0';
	return true;
}

void Socket::CloseConnection()
{
	closesocket(_socket);
}

void Socket::SendDataMessage() 
{
	char message[MAXSTRLEN];	
	cin.ignore();

	cout << "Input message: ";
	cin.get(message, MAXSTRLEN);
	SendData(message);
}


void ServerSocket::StartHosting(int port)
{
	Bind(port);
	Listen();
}

void ServerSocket::Listen()
{
	cout << "Waiting for client..." << endl;

	if (listen(_socket, 1) == SOCKET_ERROR)
	{
		cout << "Listening Error\n";
		system("pause");
		WSACleanup();
		exit(15);
	}

	acceptSocket = accept(_socket, NULL, NULL);

	while (acceptSocket == SOCKET_ERROR) 
	{
		acceptSocket = accept(_socket, NULL, NULL);
	}

	_socket = acceptSocket;
}

void ServerSocket::Bind(int port)
{
	addr.sin_family = AF_INET;
	inet_pton(AF_INET, "0.0.0.0", &addr.sin_addr);
	addr.sin_port = htons(port);

	cout << "Binding to port: " << port << endl;

	if (bind(_socket, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR)
	{
		cout << "Failed to bind to port\n";
		system("pause");
		WSACleanup();
		exit(14);
	}
}

void ClientSocket::ConnectToServer(const char *ipAddress, int port)
{
	addr.sin_family = AF_INET;
	inet_pton(AF_INET, ipAddress, &addr.sin_addr);
	addr.sin_port = htons(port);

	if (connect(_socket, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR)
	{
		cout << "Failed to connect to server\n";
		system("pause");
		WSACleanup();
		exit(13);			
	}
}